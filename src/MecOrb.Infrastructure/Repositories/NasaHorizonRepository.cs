using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;
using MecOrb.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace MecOrb.Infrastructure.Repositories
{
    public class NasaHorizonRepository : INasaHorizonRepository
    {
        public async Task<Dictionary<string, VectorXYZ>> GetEphemerities(int bodyId, long? simulationDate = null)
        {
            Dictionary<string, VectorXYZ> ephemerities = new Dictionary<string, VectorXYZ>();

            string tcResponse = await GetTCResponse(bodyId, simulationDate);

            if (!String.IsNullOrEmpty(tcResponse))
                CreateEphemritiesVector(bodyId, tcResponse, ephemerities);

            return ephemerities;
        }

        // maybe we can use the jpl API.
        // trying to now how it works
        // https://ssd-api.jpl.nasa.gov/doc/horizons.html
        private async Task<string> GetTCResponse(int bodyId, long? simulationDate = null)
        {
            string tcResponse = string.Empty;

            try
            {
                DateTime simulationStartDate =
                    simulationDate.HasValue
                    ? Methods.UnixTimeToDateTime(simulationDate.Value)
                    : DateTime.Now.AddDays(-1);

                DateTime simulationEndDate = simulationStartDate.AddDays(1);
                TelnetConnection tc = new TelnetConnection("horizons.jpl.nasa.gov", 6775);

                Thread.Sleep(2000);
                await WriteTelnetInput(tc, bodyId.ToString());
                await WriteTelnetInput(tc, "e");
                await WriteTelnetInput(tc, "v");
                await WriteTelnetInput(tc, "@0");
                await WriteTelnetInput(tc, "eclip");
                await WriteTelnetInput(tc, GetDateString(simulationStartDate));
                await WriteTelnetInput(tc, GetDateString(simulationEndDate));
                await WriteTelnetInput(tc, "1d");
                await WriteTelnetInput(tc, "n");
                await WriteTelnetInput(tc, "J2000");
                await WriteTelnetInput(tc, "1");
                await WriteTelnetInput(tc, "1");
                await WriteTelnetInput(tc, "YES");
                await WriteTelnetInput(tc, "YES");
                await WriteFinalTelnetInput(tc, "2");
                Thread.Sleep(2000);

                tcResponse = tc.Read();
            }
            catch (Exception e)
            {
                Console.Write(bodyId.ToString() + e.Message + Environment.NewLine + e.StackTrace);
            }

            return tcResponse;
        }

        private void CreateEphemritiesVector(int bodyId, string tcResponse, Dictionary<string, VectorXYZ> ephemerities)
        {
            try
            {
                string response = tcResponse;
                response = response.Split("$$SOE")[1];
                response = response.Split("$$EOE")[0];
                response = response.Split("\\r\\n")[0];

                string[] fields = response.Split(",");
                var x = ParseTelnetDouble(fields[3]);
                var y = ParseTelnetDouble(fields[4]);
                var z = ParseTelnetDouble(fields[5]);
                var vx = ParseTelnetDouble(fields[6]);
                var vy = ParseTelnetDouble(fields[7]);
                var vz = ParseTelnetDouble(fields[8]);

                var position = new VectorXYZ(x, y, z);
                var velocity = new VectorXYZ(vx, vy, vz);

                ephemerities.Add("position", position);
                ephemerities.Add("velocity", velocity);
            }
            catch (Exception e)
            {
                Console.Write(bodyId.ToString() + e.Message + Environment.NewLine + e.StackTrace);
            }
        }
        private double ParseTelnetDouble(string input)
        {
            string fixedString = input.Trim();
            return Double.Parse(fixedString, System.Globalization.NumberStyles.Float, CultureInfo.CreateSpecificCulture("en-US"));
        }

        private async Task WriteTelnetInput(TelnetConnection tc, string input)
        {
            Thread.Sleep(500);
            tc.Read();
            await tc.WriteLine(input);
            tc.Read();
        }

        private async Task WriteFinalTelnetInput(TelnetConnection tc, string input)
        {
            Thread.Sleep(500);
            tc.Read();
            await tc.WriteLine(input);
        }

        private string GetDateString(DateTime date)
        {
            string dateFormat = "yyyy-MMM-dd HH:mm";

            return date.ToString(dateFormat, CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}
