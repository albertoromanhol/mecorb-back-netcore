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
            TimeSpan startSimulation = DateTime.Now.TimeOfDay;

            _simulationResult = new SimulationResult();

            _simulationConfig = _mapper.Map<SimulationConfigModel, SimulationConfig>(simulationConfigModel);

            TimeSpan startSetup = DateTime.Now.TimeOfDay;

            SetupSimulation();

            TimeSpan endSetup = DateTime.Now.TimeOfDay;

            TimeSpan startGetAcceleration = DateTime.Now.TimeOfDay;

            GetPlanetsAcceleration();

            TimeSpan endGetAcceleration = DateTime.Now.TimeOfDay;

            TimeSpan startReduce = DateTime.Now.TimeOfDay;

            ReduceTrajectories();

            TimeSpan endReduce = DateTime.Now.TimeOfDay;

            _simulationResult.Planets = _planets;
            _simulationResult.TrajectoryPoints = _planets.First().Trajectory.X.Count;

            TimeSpan endSimulation = DateTime.Now.TimeOfDay;

            LogResults(startSimulation, startSetup, endSetup, startGetAcceleration, endGetAcceleration, startReduce, endReduce, endSimulation);

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
            _simulationResult.TrajectoryPoints = _planets.First().Trajectory.X.Count;

            return _simulationResult;
        }

        private void LogResults(TimeSpan startSimulation, TimeSpan startSetup, TimeSpan endSetup, TimeSpan startGetAcceleration, TimeSpan endGetAcceleration, TimeSpan startReduce, TimeSpan endReduce, TimeSpan endSimulation)
        {
            Console.WriteLine();
            Console.WriteLine("############################################");
            Console.WriteLine("Análise PNC");
            Console.Write("Planetas: ");
            Console.WriteLine(String.Join(", ", _simulationResult.Planets.Select(p => p.Name)));

            Console.Write("Simulation Days: ");
            Console.WriteLine(_simulationConfig.SimulationDays);

            Console.Write("Steps qtd: ");
            Console.WriteLine(_simulationConfig.SimulationSteps);

            Console.Write("Time Step (seconds): ");
            Console.WriteLine(_timeStep);

            WriteResult("Delta Setup", startSetup, endSetup);
            WriteResult("Delta Acceleration", startGetAcceleration, endGetAcceleration);
            WriteResult("Delta Reduce", startReduce, endReduce);
            WriteResult("Delta Simulation", startSimulation, endSimulation);

            Console.WriteLine("############################################");
        }

        private void WriteResult(String description, TimeSpan start, TimeSpan end)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine();
            Console.Write(description + ": ");
            Console.Write(GetMS(start, end));
            Console.WriteLine(" seconds");
            Console.WriteLine();
        }

        private double GetMS(TimeSpan start, TimeSpan end)
        {
            return (end - start).TotalSeconds;
        }


        #region[SETUP SIMULATOR]
        private void SetupSimulation(bool withEphemerities = true)
        {
            SetupSimulationTimeParams(_simulationConfig.SimulationDays, _simulationConfig.SimulationSteps);
            SetupSimulationPlanets(_simulationConfig.Planets, _simulationConfig.InitialDate, withEphemerities);
        }

        private void SetupSimulationTimeParams(int simulationDays = 365, int? simulationSteps = 100_000)
        {
            int finalNumberSteps = simulationSteps.HasValue ? simulationSteps.Value : 100_000;

            if (_simulationConfig.SimulationInSeconds.HasValue)
            {
                _simulationTimeSeconds = _simulationConfig.SimulationInSeconds.Value;
            }
            else
            {
                _simulationTimeSeconds = simulationDays
                                            * DAY_IN_HOURS
                                            * HOUR_IN_MINUTES
                                            * MINUTE_IN_SECONDS;
            }

            _timeStep = _simulationTimeSeconds / finalNumberSteps;
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

                planet.StartTrajectory();
            }

            _planets = planets;
        }

        #endregion[SETUP SIMULATOR]

        #region[ACCELERATION]
        private void GetPlanetsAcceleration()
        {
            double currentTime = 0;

            try
            {
                _simulationResult.Time = new List<double>();


                while (currentTime <= _simulationTimeSeconds)
                {
                    foreach (Planet planet in _planets)
                    {
                        UpdatePlanetTrajectory(planet);
                    }

                    _simulationResult.Time.Add(currentTime);
                    currentTime += _timeStep;
                }

                _simulationResult.Time.Add(currentTime);
            }
            catch (CollisionException collisionException)
            {
                if (_simulationResult.Collision == null)
                    _simulationResult.Collision = new List<string>();

                _simulationResult.Collision.Add(collisionException.Message + $". Decorridos {Math.Round(currentTime/(60*60*24), 1)} dias.");
            }

        }

        private void UpdatePlanetTrajectory(Planet planet)
        {
            planet.CurrentAcceleration = IntegrateAccelerationRangeKutta(planet);

            Vector3 deltaVelocity = planet.CurrentAcceleration * _timeStep;
            planet.CurrentVelocity = planet.CurrentVelocity + deltaVelocity;

            Vector3 deltaPosition = planet.CurrentVelocity * _timeStep;
            planet.CurrentPosition = planet.CurrentPosition + deltaPosition;

            planet.Trajectory.AddVector(planet.CurrentPosition);
        }

        private Vector3 GetAccelearation(Planet planet, Vector3 position)
        {
            Vector3 acceleration = new Vector3();

            foreach (Planet secondPlanet in _planets.Where((p) => p.Id != planet.Id))
            {
                Vector3 distance = CalculateDistance(planet, secondPlanet, position);
                Vector3 gravitationalAcceleration = CalculateGravitationalAcceleration(distance, secondPlanet);

                Vector3 planetInfluenceAcceleration = acceleration + gravitationalAcceleration;
                acceleration = planetInfluenceAcceleration;
            }

            return acceleration;
        }

        private Vector3 CalculateDistance(Planet planet, Planet secondPlanet, Vector3 position)
        {
            Vector3 distance = secondPlanet.CurrentPosition - position;
            double distanceNorm = distance.Norm();

            if (distanceNorm <= (secondPlanet.Radius + planet.Radius))
            {
                throw new CollisionException(planet.NamePTBR, secondPlanet.NamePTBR);
            }

            return distance;
        }

        private Vector3 CalculateGravitationalAcceleration(Vector3 distance, Planet secondPlanet)
        {
            return distance * (Constants.G * 1e-9 * secondPlanet.Mass) / Math.Pow(distance.Norm(), 3);
        }

        private void ReduceTrajectories()
        {
            int reductionSize = (_simulationConfig.SimulationSteps.HasValue && _simulationConfig.SimulationSteps.Value > 5000)
                ? _simulationConfig.SimulationSteps.Value / 5000
                : 2;

            foreach (Planet planet in _planets)
            {
                planet.Trajectory.X = Methods.ReduceArray(planet.Trajectory.X, reductionSize);
                planet.Trajectory.Y = Methods.ReduceArray(planet.Trajectory.Y, reductionSize);
                planet.Trajectory.Z = Methods.ReduceArray(planet.Trajectory.Z, reductionSize);
            }

            _simulationResult.Time = Methods.ReduceArray(_simulationResult.Time, reductionSize);
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

            Vector3 k1 = accelerationk1;
            Vector3 k2 = accelerationk2 * 2;
            Vector3 k3 = accelerationk3 * 2;
            Vector3 k4 = accelerationk4;

            Vector3 accelerationSum = k1 + k2 + k3 + k4;

            return accelerationSum / 6;
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
            if (baseAcceleration != null && timeStep != 0)
            {
                velocity = PartialStep(velocity, baseAcceleration, timeStep.Value);
                position = PartialStep(position, velocity, timeStep.Value);
            }

            Vector3 acceleration = GetAccelearation(planet, position);

            return acceleration;
        }

        private Vector3 PartialStep(Vector3 vector1, Vector3 vector2, double partialStepTime)
        {
            Vector3 deltaStep = vector2 * partialStepTime;
            return vector1 + deltaStep;
        }

        #endregion[INTEGRATOR]
    }
}
