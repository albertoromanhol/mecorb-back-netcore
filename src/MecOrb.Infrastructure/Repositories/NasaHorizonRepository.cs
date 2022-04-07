using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MecOrb.Infrastructure.Repositories
{
    public class NasaHorizonRepository : INasaHorizonRepository
    {
        public async Task<Dictionary<string, VectorXYZ>> GetEphemerities(int bodyId, DateTime? simulationDate = null)
        {
            (string startDate, string endDate) = GetStartDate(simulationDate);

            string nasaHorizonResponse = await GetNasaHorizonResponse(bodyId, startDate, endDate);
            Dictionary<string, VectorXYZ> ephemerities = CreateNasaEphemeritiesVector(nasaHorizonResponse);

            return ephemerities;
        }

        private async Task<string> GetNasaHorizonResponse(int bodyId, string startDate, string endDate)
        {
            string nasaHorizonURI = @$"https://ssd.jpl.nasa.gov/api/horizons.api?format=text
                                        &COMMAND='{bodyId}'
                                        &OBJ_DATA='NO'
                                        &MAKE_EPHEM='YES'
                                        &EPHEM_TYPE='VECTOR'
                                        &CENTER='500@0'
                                        &START_TIME='{startDate}'
                                        &STOP_TIME='{endDate}'
                                        &STEP_SIZE='1d'
                                        &VEC_LABELS='NO'";

            HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(nasaHorizonURI);

            return response;
        }

        private Dictionary<string, VectorXYZ> CreateNasaEphemeritiesVector(string nasaHorizonResponse)
        {
            Dictionary<string, VectorXYZ> ephemerities = new Dictionary<string, VectorXYZ>();

            List<string> reponseString = nasaHorizonResponse
                .Split("$$SOE")[1]
                .Split("$$EOE")[0]
                .Split("\\n  ")
                .ToList();

            string positionString = reponseString[1];
            string velocityString = reponseString[2];

            VectorXYZ position = CreateVectorXYZ(positionString);
            VectorXYZ velocity = CreateVectorXYZ(velocityString);

            ephemerities.Add("position", position);
            ephemerities.Add("velocity", velocity);

            return ephemerities;
        }

        private VectorXYZ CreateVectorXYZ(string fullString)
        {
            List<string> clearedStrings = GetCleanStringInList(fullString);

            double xRef = ParaseStringToDouble(clearedStrings[0]);
            double yRef = ParaseStringToDouble(clearedStrings[1]);
            double zRef = ParaseStringToDouble(clearedStrings[2]);

            VectorXYZ vector = new VectorXYZ(xRef, yRef, zRef);

            return vector;
        }

        private List<string> GetCleanStringInList(string trashedString)
        {
            List<string> clearString = new List<string>();

            foreach (var position in trashedString.Split(" "))
            {
                if (!String.IsNullOrEmpty(position.Trim()))
                {
                    clearString.Add(position.Trim());
                }
            }

            return clearString;
        }

        private double ParaseStringToDouble(string input)
        {
            return Double.Parse(input, NumberStyles.Float, CultureInfo.CreateSpecificCulture("en-US"));
        }

        private (string, string) GetStartDate(DateTime? simulationDate = null)
        {
            DateTime startDate = simulationDate != null
                ? simulationDate.Value
                : DateTime.Now;

            DateTime endDate = startDate.AddDays(1);

            return (GetDateString(startDate), GetDateString(endDate));
        }

        private string GetDateString(DateTime date)
        {
            string dateFormat = "yyyy-MM-dd";

            return date.ToString(dateFormat, CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}
