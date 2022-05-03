using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Application.Models;
using MecOrb.Application.Utils;
using MecOrb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MecOrb.Application
{
    public class SimulationApplication : ISimulationApplication
    {
        private readonly IMapper _mapper;
        private readonly IPlanetApplication _planetApplication;

        private List<Planet> _planets;
        private double _timeStep;
        private double _simulationTimeSeconds;
        private SimulationConfig _simulationConfig;
        private SimulationResult _simulationResult;

        private readonly int DAY_IN_HOURS = 24;
        private readonly int HOUR_IN_MINUTES = 60;
        private readonly int MINUTE_IN_SECONDS = 60;

        public SimulationApplication(IMapper mapper, IPlanetApplication planetApplication)
        {
            _mapper = mapper;
            _planetApplication = planetApplication;
        }

        public SimulationResult Simulate(SimulationConfigModel simulationConfigModel)
        {
            _simulationResult = new SimulationResult();

            _simulationConfig = _mapper.Map<SimulationConfigModel, SimulationConfig>(simulationConfigModel);

            SetupSimulation();
            GetPlanetsAcceleration();
            ReduceTrajectories();

            _simulationResult.Planets = _planets;

            return _simulationResult;
        }

        public SimulationResult SimulateForManouver(SimulationConfig simulationConfig)
        {

            _simulationResult = new SimulationResult();

            _simulationConfig = simulationConfig;

            SetupSimulation(false);
            GetPlanetsAcceleration();
            ReduceTrajectories();

            _simulationResult.Planets = _planets;

            return _simulationResult;
        }

        #region[SETUP SIMULATOR]
        private void SetupSimulation(bool withEphemerities = true)
        {
            SetupSimulationTimeParams(_simulationConfig.SimulationDays, _simulationConfig.SimulationSteps);
            SetupSimulationPlanets(_simulationConfig.Planets, _simulationConfig.InitialDate, withEphemerities);
        }

        private void SetupSimulationTimeParams(int simulationDays = 365, int? simulationSteps = 1_000_000)
        {
            int finalNumberSteps = simulationSteps.HasValue ? simulationSteps.Value : 1_000_000;
            int finalSimulationDays = simulationDays;

            double simulationTimeSeconds = 0.0;

            if (_simulationConfig.SimulationInSeconds.HasValue)
            {
                simulationTimeSeconds = _simulationConfig.SimulationInSeconds.Value;
            }
            else
            {
                simulationTimeSeconds = finalSimulationDays
                                            * DAY_IN_HOURS
                                            * HOUR_IN_MINUTES
                                            * MINUTE_IN_SECONDS;
            }

            double timeStep = simulationTimeSeconds / finalNumberSteps;

            _simulationTimeSeconds = simulationTimeSeconds;
            _timeStep = timeStep;
        }

        private void SetupSimulationPlanets(List<Planet> planets, DateTime initialDate, bool withEphemerities = true)
        {
            if (withEphemerities)
                planets = _planetApplication.GetEphemerits(planets, initialDate).Result;

            foreach (Planet planet in planets)
            {
                (Vector3 bodyVelocity, Vector3 bodyPosition) = GetBodyVelocityAndPosition(planet);
                planet.CurrentPosition = bodyPosition;
                planet.CurrentVelocity = bodyVelocity;

                // TODO: AddReferenceBodyId? -> maybe when use another body instead planets

                planet.StartTrajectory();
            }

            _planets = planets;
        }

        #endregion[SETUP SIMULATOR]

        #region[ACCELERATION]
        private void GetPlanetsAcceleration()
        {
            try
            {
                double currentTime = 0;
                // we can do it better
                while (currentTime <= _simulationTimeSeconds)
                {
                    // we can do it better
                    foreach (Planet planet in _planets)
                    {
                        UpdatePlanetTrajectory(planet);
                    }

                    currentTime += _timeStep;
                }
            }
            catch (CollisionException collisionException)
            {
                // write in _result
                Console.WriteLine(collisionException.Message);
            }

        }

        private void UpdatePlanetTrajectory(Planet planet)
        {
            // implement dict -> really necessary?
            planet.CurrentAcceleration = IntegrateAccelerationRangeKutta(planet);

            // TODO: improve to allow manouvers
            // we have to know WHEN use the new vec velocity
            planet.CurrentVelocity += planet.CurrentAcceleration * _timeStep;
            planet.CurrentPosition += planet.CurrentVelocity * _timeStep;
            planet.Trajectory.AddVector(planet.CurrentPosition);
        }

        private Vector3 GetAccelearation(Planet planet, Vector3 position)
        {
            Vector3 acceleration = new Vector3();

            foreach (Planet secondPlanet in _planets.Where((p) => p.Id != planet.Id))
            {
                Vector3 distance = CalculateDistance(planet, secondPlanet, position);
                Vector3 gravitationalAcceleration = CalculateGravitationalAcceleration(distance, secondPlanet);

                acceleration += gravitationalAcceleration;
            }

            return acceleration;
        }

        private Vector3 CalculateDistance(Planet planet, Planet secondPlanet, Vector3 position)
        {
            Vector3 distance = secondPlanet.CurrentPosition - position; //verify
            double distanceNorm = distance.Norm();

            if (distanceNorm <= (secondPlanet.Radius + planet.Radius))
            {
                throw new CollisionException(planet.Name, secondPlanet.Name);
            }

            return distance;
        }

        private Vector3 CalculateGravitationalAcceleration(Vector3 distance, Planet secondPlanet)
        {
            return distance * (Constants.G * 1e-9 * secondPlanet.Mass / Math.Pow(distance.Norm(), 3));
        }

        private void ReduceTrajectories()
        {
            int reductionSize = _simulationConfig.SimulationSteps.HasValue
                ? _simulationConfig.SimulationSteps.Value / 5000
                : 2;

            foreach (Planet planet in _planets)
            {
                planet.Trajectory.X = Methods.ReduceArray(planet.Trajectory.X, reductionSize);
                planet.Trajectory.Y = Methods.ReduceArray(planet.Trajectory.Y, reductionSize);
                planet.Trajectory.Z = Methods.ReduceArray(planet.Trajectory.Z, reductionSize);
            }
        }

        #endregion[ACCELERATION]

        #region[INTEGRATOR]
        private Vector3 IntegrateAccelerationRangeKutta(Planet planet)
        {
            (Vector3 bodyVelocity, Vector3 bodyPosition) = GetBodyVelocityAndPosition(planet);

            Vector3 accelerationk1 = CalculateK(planet, bodyVelocity, bodyPosition);
            Vector3 accelerationk2 = CalculateK(planet, bodyVelocity, bodyPosition, accelerationk1, 0.5 * _timeStep);
            Vector3 accelerationk3 = CalculateK(planet, bodyVelocity, bodyPosition, accelerationk2, 0.5 * _timeStep);
            Vector3 accelerationk4 = CalculateK(planet, bodyVelocity, bodyPosition, accelerationk3, _timeStep);

            return (accelerationk1 + (accelerationk2 * 2) + (accelerationk3 * 2) + accelerationk4) / 6;
        }

        private (Vector3, Vector3) GetBodyVelocityAndPosition(Planet planet)
        {
            // change to have deafult values, in UA or AU
            // we can do it better
            Vector3 bodyVelocity = planet.CurrentVelocity;
            Vector3 bodyPosition = planet.CurrentPosition;

            if (bodyVelocity == null && planet.Ephemerities != null && !planet.Ephemerities.TryGetValue("velocity", out bodyVelocity))
            {
                bodyVelocity = planet.BaseVelocity;
            }

            if (bodyPosition == null && planet.Ephemerities != null && !planet.Ephemerities.TryGetValue("position", out bodyPosition))
            {
                bodyPosition = planet.BasePosition;
            };

            if (bodyVelocity == null)
            {
                bodyVelocity = new Vector3();
            }

            if (bodyPosition == null)
            {
                bodyPosition = new Vector3();
            }

            return (bodyVelocity, bodyPosition);
        }

        private Vector3 CalculateK(
            Planet planet,
            Vector3 velocity,
            Vector3 position,
            Vector3 baseAcceleration = null,
            double? timeStep = 0)
        {
            if (baseAcceleration != null && timeStep != null)
            {
                velocity = PartialStep(velocity, baseAcceleration, timeStep.Value);
                position = PartialStep(position, velocity, timeStep.Value);
            }

            Vector3 acceleration = GetAccelearation(planet, position);

            return acceleration;
        }

        private Vector3 PartialStep(Vector3 vector1, Vector3 vector2, double partialStepTime)
        {
            return vector1 + (vector2 * partialStepTime);
        }

        #endregion[INTEGRATOR]
    }
}
