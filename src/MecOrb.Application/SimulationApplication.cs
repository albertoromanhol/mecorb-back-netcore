using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Application.Models;
using MecOrb.Application.Utils;
using MecOrb.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MecOrb.Application
{
    public class SimulationApplication : ISimulationApplication
    {
        private readonly IMapper _mapper;
        private readonly IPlanetApplication _planetApplication;

        private List<Planet> _planets;
        private double _timeStep;

        public SimulationApplication(IMapper mapper, IPlanetApplication planetApplication)
        {
            _mapper = mapper;
            _planetApplication = planetApplication;
        }

        public Simulation SimulateTwoBodies(SimulationConfigModel simulationConfigModel)
        {
            Simulation simulation = new Simulation();

            SimulationConfig simulationConfig = _mapper.Map<SimulationConfigModel, SimulationConfig>(simulationConfigModel);

            return simulation;
        }

        public List<Planet> GetPlanetsAcceleration()
        {
            // we can do it better
            _planets = GetPlanets(); // it will be pass as props

            int finalNumberSteps = 10000; //numberSteps param
            int finalSimulationDays = 365; //simulationDays param

            double simulationTime = finalSimulationDays * 24 * 60 * 60;
            double timeStep = simulationTime / finalNumberSteps;

            _timeStep = timeStep;

            double currentTime = 0;
            // we can do it better
            while (currentTime <= simulationTime)
            {
                // we can do it better
                foreach (var planet in _planets)
                {
                    // implement dict
                    planet.CurrentAcceleration = IntegrateAccelerationRangeKutta(planet);
                    planet.CurrentVelocity += planet.CurrentAcceleration * _timeStep;
                    planet.CurrentPosition += planet.CurrentVelocity * _timeStep;
                    planet.BodyTrajectory.AddVector(planet.CurrentPosition);
                }

                currentTime += timeStep;
            }

            // ReduceTrajectories(listBodies, simulationParameters.NumberSteps);

            return _planets;
        }

        private List<Planet> GetPlanets()
        {
            // we can do it better
            List<Planet> planets = _planetApplication.GetAllWithEphemerits().Result;

            foreach (Planet planet in planets)
            {
                (VectorXYZ bodyVelocity, VectorXYZ bodyPosition) = GetBodyVelocityAndPosition(planet);
                planet.CurrentPosition = bodyPosition;
                planet.CurrentVelocity = bodyVelocity;

                // AddReferenceBodyPosition
                planet.StartTrajectory();
            }

            return planets;
        }

        private double GetTimeStep()
        {
            // melhorar
            // we can do it better
            int finalNumberSteps = 10000; //numberSteps param
            int finalSimulationDays = 365; //simulationDays param

            double simulationTime = finalSimulationDays * 24 * 60 * 60;
            double timeStep = simulationTime / finalNumberSteps;

            return timeStep;
        }

        private VectorXYZ IntegrateAccelerationRangeKutta(Planet planet)
        {
            (VectorXYZ bodyVelocity, VectorXYZ bodyPosition) = GetBodyVelocityAndPosition(planet);

            VectorXYZ accelerationk1 = CalculateK(planet, bodyVelocity, bodyPosition);
            VectorXYZ accelerationk2 = CalculateK(planet, bodyVelocity, bodyPosition, accelerationk1, 0.5 * _timeStep);
            VectorXYZ accelerationk3 = CalculateK(planet, bodyVelocity, bodyPosition, accelerationk2, 0.5 * _timeStep);
            VectorXYZ accelerationk4 = CalculateK(planet, bodyVelocity, bodyPosition, accelerationk3, 0.5 * _timeStep);

            return (accelerationk1 + accelerationk2 * 2 + accelerationk3 * 2 + accelerationk4) / 6;
        }

        private (VectorXYZ, VectorXYZ) GetBodyVelocityAndPosition(Planet planet)
        {
            // MELHORAR PARA TER VALORES PADRAO
            // we can do it better
            VectorXYZ bodyVelocity = planet.CurrentVelocity;
            VectorXYZ bodyPosition = planet.CurrentPosition;

            if (bodyVelocity == null && !planet.Ephemerities.TryGetValue("velocity", out bodyVelocity))
            {
                bodyVelocity = planet.BaseVelocity;
            }

            if (bodyPosition == null && !planet.Ephemerities.TryGetValue("position", out bodyPosition))
            {
                bodyPosition = planet.BasePosition;
            };

            return (bodyVelocity, bodyPosition);
        }

        private VectorXYZ CalculateK(
            Planet planet,
            VectorXYZ velocity,
            VectorXYZ position,
            VectorXYZ baseAcceleration = null,
            double? timeStep = 0)
        {
            VectorXYZ acceleration;

            if (baseAcceleration != null && timeStep != null)
            {
                velocity = PartialStep(velocity, baseAcceleration, timeStep.Value);
                position = PartialStep(position, velocity, timeStep.Value);
            }

            acceleration = GetAccelearation(planet, position);

            return acceleration;
        }

        private VectorXYZ GetAccelearation(Planet planet, VectorXYZ position)
        {
            // we can do it better
            VectorXYZ acceleration = new VectorXYZ();

            foreach (Planet secondPlanet in _planets)
            {
                if (planet.Id != secondPlanet.Id)
                {
                    VectorXYZ distance = secondPlanet.CurrentPosition - position;

                    if (distance.Norm() < secondPlanet.Radius + planet.Radius)
                    {
                        //throw new CollisionException(targetBody.Name, externalBody.Name);
                    }

                    VectorXYZ gravitationalAcceleration = distance * (Constants.G * 1e-9 * secondPlanet.Mass / Math.Pow(distance.Norm(), 3));

                    acceleration += gravitationalAcceleration;
                }
            }

            return acceleration;
        }

        private VectorXYZ PartialStep(VectorXYZ vector1, VectorXYZ vector2, double partialStepTime)
        {
            return vector1 + vector2 * partialStepTime;
        }
    }
}
