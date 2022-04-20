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
        public async Task<Dictionary<string, Vector3>> GetEphemerities(int bodyId, DateTime? simulationDate = null)
        {
            (string startDate, string endDate) = GetStartDate(simulationDate);

            string nasaHorizonResponse = await GetNasaHorizonResponse(bodyId, startDate, endDate);
            Dictionary<string, Vector3> ephemerities = CreateNasaEphemeritiesVector(nasaHorizonResponse);

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

        private Dictionary<string, Vector3> CreateNasaEphemeritiesVector(string nasaHorizonResponse)
        {
            Dictionary<string, Vector3> ephemerities = new Dictionary<string, Vector3>();

            List<string> reponseString = nasaHorizonResponse
                .Split("$$SOE")[1]
                .Split("$$EOE")[0]
                .Split("\\n  ")
                .ToList();

            string positionString = reponseString[1];
            string velocityString = reponseString[2];

            Vector3 position = CreateVectorXYZ(positionString);
            Vector3 velocity = CreateVectorXYZ(velocityString);

            ephemerities.Add("position", position);
            ephemerities.Add("velocity", velocity);

            return ephemerities;
        }

        private Vector3 CreateVectorXYZ(string fullString)
        {
            List<string> clearedStrings = GetCleanStringInList(fullString);

            double xRef = ParaseStringToDouble(clearedStrings[0]);
            double yRef = ParaseStringToDouble(clearedStrings[1]);
            double zRef = ParaseStringToDouble(clearedStrings[2]);

            Vector3 vector = new Vector3(xRef, yRef, zRef);

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
