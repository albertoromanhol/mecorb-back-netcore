using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Application.Models;
using MecOrb.Application.Utils;
using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MecOrb.Application
{
    public class ManouverApplication : IManouverApplication
    {
        private readonly IMapper _mapper;
        private readonly IPlanetRepository _planetRepository;
        private readonly ISimulationApplication _simulationApplication;

        private ManouverResult _manouverResult;
        private ManouverConfig _manouverConfig;
        private Orbit _initialOrbit;
        private Orbit _transferOrbit;
        private Orbit _finalOrbit;

        private Planet _earth;

        public ManouverApplication(IMapper mapper, IPlanetRepository planetRepository, ISimulationApplication simulationApplication)
        {
            _mapper = mapper;
            _planetRepository = planetRepository;
            _simulationApplication = simulationApplication;
        }

        public ManouverResult Simulate(ManouverConfigModel manouverConfigModel)
        {
            _manouverResult = new ManouverResult();

            _manouverConfig = _mapper.Map<ManouverConfigModel, ManouverConfig>(manouverConfigModel);

            SetupOrbits();
            SetupTransferOrbit();
            CalculateManouver();

            SimulateManouver();

            return _manouverResult;
        }

        private void SetupOrbits()
        {
            _initialOrbit = GetOrbitParameters(_manouverConfig.InitialOrbit, "Orbita Inicial");
            _finalOrbit = GetOrbitParameters(_manouverConfig.FinalOrbit, "Orbita Final");
        }

        private void SetupTransferOrbit()
        {
            _transferOrbit = new Orbit();

            _transferOrbit.PerigeeRadius = _initialOrbit.PerigeeRadius;
            _transferOrbit.ApogeeRadius = _finalOrbit.ApogeeRadius;

            _transferOrbit = GetOrbitParameters(_transferOrbit, "Orbita de Transferencia", true);
        }

        private void CalculateManouver()
        {

            double firstDeltaV = _transferOrbit.PerigeeVelocity - _initialOrbit.PerigeeVelocity;

            double secondDeltaV = _finalOrbit.ApogeeVelocity - _transferOrbit.ApogeeVelocity;

            double totalDeltaV = firstDeltaV + secondDeltaV;

            _manouverResult.FirstDeltaV = firstDeltaV;
            _manouverResult.SecondDeltaV = secondDeltaV;
            _manouverResult.TotalDeltaV = totalDeltaV;
        }

        private void SimulateManouver()
        {
            _earth = _planetRepository.GetByNasaBodyId(399);


            _manouverResult.Planets = new List<Planet>() { new Planet() };


            List<Orbit> orbits = new List<Orbit>
            {
                _initialOrbit,
                _transferOrbit,
                _finalOrbit
            };

            // CAN DO IT ASYNC
            foreach (Orbit orbit in orbits)
            {
                Planet equivalentPlanet = CreateEquivalentPlanet(orbit);

                List<Planet> simulationPlanets = new List<Planet>();

                simulationPlanets.Add(_earth);
                simulationPlanets.Add(equivalentPlanet);

                SimulationConfig simulationConfig = new SimulationConfig();
                simulationConfig.InitialDate = DateTime.Now;
                simulationConfig.Planets = simulationPlanets;
                simulationConfig.SimulationInSeconds = orbit.PeriodInSeconds;
                simulationConfig.SimulationSteps = 10_000;

                SimulationResult simulationResult = _simulationApplication.SimulateForManouver(simulationConfig);

                _manouverResult.Planets[0] = simulationResult.Planets.FirstOrDefault(planet => planet.NasaHorizonBodyId == 399);
                _manouverResult.Planets.Add(simulationResult.Planets.FirstOrDefault(planet => planet.NasaHorizonBodyId == -1));
            }
        }

        private List<Planet> GetEquivalentPlanets()
        {
            List<Planet> equivalentPlanets = new List<Planet>();

            List<Orbit> orbits = new List<Orbit>
            {
                _initialOrbit,
                _transferOrbit,
                _finalOrbit
            };

            foreach (Orbit orbit in orbits)
            {
                Planet planet = CreateEquivalentPlanet(orbit);
                equivalentPlanets.Add(planet);
            }

            return equivalentPlanets;
        }

        private Planet CreateEquivalentPlanet(Orbit orbit)
        {
            Planet planet = new Planet();

            planet.Name = orbit.Name;
            planet.NamePTBR = orbit.Name;

            planet.NasaHorizonBodyId = -1;
            planet.Mass = 2_000; // kg, calculate?
            planet.Radius = 0.01; // km, calculate?

            planet.CurrentVelocity = new Vector3(0, orbit.PerigeeVelocity, 0);
            planet.CurrentPosition = new Vector3(orbit.PerigeeRadius, 0, 0);

            return planet;
        }

        private Orbit GetOrbitParameters(Orbit orbit, string orbitName, bool transferOrbit = false)
        {
            if (!transferOrbit)
            {
                GetPerigeeRadius(orbit);
                GetApogeeRadius(orbit);
            }

            GetAngularMoment(orbit);
            GetPerigeeVelocity(orbit);
            GetApogeeVelocity(orbit);
            GetOrbitPeriod(orbit);

            orbit.Name = orbitName;

            return orbit;
        }

        private void GetPerigeeRadius(Orbit orbit)
        {
            double perigeeRadius = orbit.MajorSemiAxis * (1 - orbit.Excentricity);

            orbit.PerigeeRadius = perigeeRadius;
        }

        private void GetApogeeRadius(Orbit orbit)
        {
            double apogeeRadius = 2 * orbit.MajorSemiAxis - orbit.PerigeeRadius;

            orbit.ApogeeRadius = apogeeRadius;
        }

        private void GetAngularMoment(Orbit orbit)
        {
            double ratioBetweenRadius = (orbit.ApogeeRadius * orbit.PerigeeRadius) / (orbit.ApogeeRadius + orbit.PerigeeRadius);
            double angularMoment = Math.Sqrt(2 * Constants.MuEarth) * Math.Sqrt(ratioBetweenRadius);

            orbit.AngularMoment = angularMoment;
        }

        private void GetPerigeeVelocity(Orbit orbit)
        {
            double perigeeVelocity = orbit.AngularMoment / orbit.PerigeeRadius;

            orbit.PerigeeVelocity = perigeeVelocity;
        }

        private void GetApogeeVelocity(Orbit orbit)
        {
            double apogeeVelocity = orbit.AngularMoment / orbit.ApogeeRadius;

            orbit.ApogeeVelocity = apogeeVelocity;
        }

        private void GetOrbitPeriod(Orbit orbit, bool transferOrbit = false)
        {
            double majorSemiAxis = (orbit.PerigeeRadius + orbit.ApogeeRadius) / 2.0;
            double majorSemiAxisParam = Math.Pow(majorSemiAxis, 1.5);
            double periodInSeconds = ((2.0 * Math.PI) / Math.Sqrt(Constants.MuEarth)) * majorSemiAxisParam;

            if (transferOrbit)
            {
                periodInSeconds /= 2;
            }

            orbit.PeriodInSeconds = periodInSeconds;
        }
    }
}