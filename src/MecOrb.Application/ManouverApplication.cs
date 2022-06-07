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

        private Dictionary<string, Orbit> _manouverOrbits;

        private Planet _earth;

        public ManouverApplication(IMapper mapper, IPlanetRepository planetRepository, ISimulationApplication simulationApplication)
        {
            _mapper = mapper;
            _planetRepository = planetRepository;
            _simulationApplication = simulationApplication;

            _manouverOrbits = new Dictionary<string, Orbit>();
        }

        #region[METHODS]

        public ManouverResult Hohmann(ManouverConfigModel manouverConfigModel)
        {
            _manouverResult = new ManouverResult();

            _manouverConfig = _mapper.Map<ManouverConfigModel, ManouverConfig>(manouverConfigModel);

            SetupInitialOrbits();
            SetupHohmannTransferOrbit();
            CalculateHohmannManouver();

            SimulateManouver();

            return _manouverResult;
        }
        public ManouverResult BiElliptic(ManouverConfigModel manouverConfigModel)
        {
            _manouverResult = new ManouverResult();

            _manouverConfig = _mapper.Map<ManouverConfigModel, ManouverConfig>(manouverConfigModel);

            SetupInitialOrbits();
            SetupBiEllipticTransferOrbit();
            CalculateBiEllipticManouver();

            SimulateManouver();

            return _manouverResult;
        }
        public ManouverResult CompareHohmannAndBiElliptic(ManouverConfigModel manouverConfigModel)
        {
            _manouverResult = new ManouverResult();

            _manouverConfig = _mapper.Map<ManouverConfigModel, ManouverConfig>(manouverConfigModel);

            SetupInitialOrbits();

            SetupHohmannTransferOrbit();
            CalculateHohmannManouver();

            _manouverResult.DeltaV.Add("", 0);

            SetupBiEllipticTransferOrbit();
            CalculateBiEllipticManouver();

            SimulateManouver();

            return _manouverResult;
        }


        #endregion[METHODS]

        private void SetupInitialOrbits()
        {
            _manouverResult.DeltaV = new Dictionary<string, double>();

            Orbit initialOrbit = GetOrbitParameters(_manouverConfig.InitialOrbit, "Orbita Inicial");
            Orbit finalOrbit = GetOrbitParameters(_manouverConfig.FinalOrbit, "Orbita Final");

            _manouverOrbits.Add("initialOrbit", initialOrbit);
            _manouverOrbits.Add("finalOrbit", finalOrbit);
        }

        #region[HOHMANN]

        private void SetupHohmannTransferOrbit()
        {
            Orbit hohmannTransferOrbit = new Orbit();

            hohmannTransferOrbit.PerigeeRadius = _manouverOrbits["initialOrbit"].PerigeeRadius;
            hohmannTransferOrbit.ApogeeRadius = _manouverOrbits["finalOrbit"].ApogeeRadius;

            hohmannTransferOrbit = GetOrbitParameters(hohmannTransferOrbit, "Orbita de Transferência - Hohmann", true);

            _manouverOrbits.Add("hohmannTransferOrbit", hohmannTransferOrbit);
        }

        private void CalculateHohmannManouver()
        {
            double firstDeltaV = _manouverOrbits["hohmannTransferOrbit"].PerigeeVelocity - _manouverOrbits["initialOrbit"].PerigeeVelocity;

            double secondDeltaV = _manouverOrbits["finalOrbit"].ApogeeVelocity - _manouverOrbits["hohmannTransferOrbit"].ApogeeVelocity;

            double totalDeltaV = Math.Abs(firstDeltaV) + Math.Abs(secondDeltaV);

            _manouverResult.DeltaV.Add("ΔV_H_1", firstDeltaV);
            _manouverResult.DeltaV.Add("ΔV_H_2", secondDeltaV);
            _manouverResult.DeltaV.Add("ΔV_H", totalDeltaV);

            CalculateDeltaMassPropellant("ΔM_H", totalDeltaV);
        }

        #endregion[HOHMANN]

        #region[BI ELLIPTIC]
        private void SetupBiEllipticTransferOrbit()
        {
            SetupFirstBiEllipticOrbit();
            SetupSecondBiEllipticOrbit();
        }

        private void SetupFirstBiEllipticOrbit()
        {
            Orbit firstTransferOrbit = new Orbit();

            firstTransferOrbit.PerigeeRadius = _manouverOrbits["initialOrbit"].PerigeeRadius;
            firstTransferOrbit.ApogeeRadius = _manouverConfig.FirstBiEllipseApogge.Value;

            firstTransferOrbit = GetOrbitParameters(firstTransferOrbit, "Orbita de Transferência 1 - Bi Eliptica", true);

            _manouverOrbits.Add("biEllipticFirstTransferOrbit", firstTransferOrbit);
        }

        private void SetupSecondBiEllipticOrbit()
        {
            Orbit secondTransferOrbit = new Orbit();

            secondTransferOrbit.PerigeeRadius = _manouverOrbits["finalOrbit"].PerigeeRadius;
            secondTransferOrbit.ApogeeRadius = _manouverConfig.FirstBiEllipseApogge.Value;

            secondTransferOrbit = GetOrbitParameters(secondTransferOrbit, "Orbita de Transferência 2 - Bi Eliptica", true);

            _manouverOrbits.Add("biEllipticSecondTransferOrbit", secondTransferOrbit);
        }

        private void CalculateBiEllipticManouver()
        {
            double firstDeltaV = _manouverOrbits["biEllipticFirstTransferOrbit"].PerigeeVelocity - _manouverOrbits["initialOrbit"].PerigeeVelocity;

            double secondDeltaV = _manouverOrbits["biEllipticSecondTransferOrbit"].ApogeeVelocity - _manouverOrbits["biEllipticFirstTransferOrbit"].ApogeeVelocity;

            double thirdDeltaV = _manouverOrbits["finalOrbit"].PerigeeVelocity - _manouverOrbits["biEllipticSecondTransferOrbit"].PerigeeVelocity;

            double totalDeltaV = Math.Abs(firstDeltaV) + Math.Abs(secondDeltaV) + Math.Abs(thirdDeltaV);

            _manouverResult.DeltaV.Add("ΔV_BE_1", firstDeltaV);
            _manouverResult.DeltaV.Add("ΔV_BE_2", secondDeltaV);
            _manouverResult.DeltaV.Add("ΔV_BE_3", thirdDeltaV);
            _manouverResult.DeltaV.Add("ΔV_BE", totalDeltaV);


            CalculateDeltaMassPropellant("ΔM_BE_1", totalDeltaV);
        }

        #endregion[BI ELLIPTIC]

        #region[MANOUVER]

        private void SimulateManouver()
        {
            _earth = _planetRepository.GetByNasaBodyId(399);

            _manouverResult.Planets = new List<Planet>() { new Planet() };
            _manouverResult.Collision = new List<string>();
            _manouverResult.Time = new List<double>();

            foreach (Orbit orbit in _manouverOrbits.Values)
            {
                List<Planet> simulationPlanets = GetSimulationPlanets(orbit);
                SimulationConfig simulationConfig = GetSimulationConfig(orbit, simulationPlanets);

                SimulationResult simulationResult = _simulationApplication.SimulateForManouver(simulationConfig);

                _manouverResult.Planets[0] = simulationResult.Planets.FirstOrDefault(planet => planet.NasaHorizonBodyId == 399);
                _manouverResult.Planets.Add(simulationResult.Planets.FirstOrDefault(planet => planet.NasaHorizonBodyId == -1));
                _manouverResult.TrajectoryPoints = simulationResult.TrajectoryPoints;
                _manouverResult.Time = simulationResult.Time;

                if (simulationResult.Collision != null && simulationResult.Collision.Any())
                    _manouverResult.Collision.AddRange(simulationResult.Collision);
            }
        }

        private List<Planet> GetSimulationPlanets(Orbit orbit)
        {
            List<Planet> simulationPlanets = new List<Planet>();

            Planet equivalentPlanet = CreateEquivalentPlanet(orbit);
            simulationPlanets.Add(_earth);
            simulationPlanets.Add(equivalentPlanet);

            return simulationPlanets;
        }

        private Planet CreateEquivalentPlanet(Orbit orbit)
        {
            Planet planet = new Planet
            {
                Name = orbit.Name,
                NamePTBR = orbit.Name,

                NasaHorizonBodyId = -1,
                Mass = 2_000,
                Radius = 0.001,

                CurrentVelocity = GetOrbitCurrentVelocity(orbit),
                CurrentPosition = GetOrbitCurrentPosition(orbit)
            };

            return planet;
        }

        private Vector3 GetOrbitCurrentPosition(Orbit orbit)
        {
            double currentPosition = orbit.PerigeeRadius;

            bool inTransfer = orbit.Name.Contains("Transferência");

            if (inTransfer)
            {
                currentPosition = orbit.ApogeeRadius * -1;
            }

            return new Vector3(currentPosition, 0, 0);
        }

        private Vector3 GetOrbitCurrentVelocity(Orbit orbit)
        {
            double currentVelocity = orbit.PerigeeVelocity;

            bool inTransfer = orbit.Name.Contains("Transferência");

            if (inTransfer)
            {
                currentVelocity = orbit.ApogeeVelocity;
            }


            bool isSecondManouver = orbit.Name.Contains("2");

            if (isSecondManouver)
            {
                currentVelocity *= -1;
            }

            return new Vector3(0, currentVelocity, 0);
        }

        private SimulationConfig GetSimulationConfig(Orbit orbit, List<Planet> simulationPlanets)
        {
            SimulationConfig simulationConfig = new SimulationConfig();
            simulationConfig.InitialDate = DateTime.Now;
            simulationConfig.Planets = simulationPlanets;
            simulationConfig.SimulationInSeconds = orbit.PeriodInSeconds;
            simulationConfig.SimulationSteps = 10_000;

            return simulationConfig;
        }

        #endregion[MANOUVER]

        #region[ORBIT PARAMETERS]

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
            GetOrbitPeriod(orbit, transferOrbit);

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

        private void CalculateDeltaMassPropellant(string orbitName, double totalDeltaV)
        {
            double Isp = _manouverConfig.Spacecraft.Isp;
            double g0 = 9.81;
            double bodyMass = _manouverConfig.Spacecraft.Mass;


            double exponentialPower = -( (totalDeltaV * 1000) / (Isp * g0));

            double massRatio = 1 - Math.Exp(exponentialPower);

            double deltaMassPropellant = massRatio * bodyMass;


            _manouverResult.DeltaV.Add(orbitName, deltaMassPropellant);
        }

        #endregion[ORBIT PARAMETERS]
    }
}