using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VoyageApi.Models;


namespace VoyageApi.Controller
{
    [Route("api/voyage")]
    [ApiController]
    public class VoyageController : ControllerBase
    {

        [HttpGet("time-difference")]
        public async Task<IActionResult> GetTimeDifference()
        {
            var json = await System.IO.File.ReadAllTextAsync("Z388-25061.json");
            var voyageData = JsonConvert.DeserializeObject<VoyageData>(json);

            if (voyageData?.PortCalls == null)
            return NotFound("No port calls found in the data.");

            var loadingPort = voyageData.PortCalls.FirstOrDefault(p => p.Function == "L");
            if (loadingPort == null)
                return NotFound("No loading port found.");

            var departureActivity = loadingPort.Activities?.FirstOrDefault(a =>
                a.Name == "ACTUAL TIME DEPARTURE / SAILED");
            var departure = departureActivity?.Time;
            if (departure == null)
                return NotFound("No departure time found for loading port.");

            var arrivalActivity = loadingPort.Activities?.FirstOrDefault(a =>
                a.Name == "ACTUAL TIME ARRIVED (EOSV - END OF SEA VOYAGE)");
            var arrived = arrivalActivity?.Time;
            if (arrived == null)
                return NotFound("No arrival time found for loading port.");

            var departureTime = DateTime.Parse(departure);
            var arrivalTime = DateTime.Parse(arrived);

            var timeDifference = departureTime - arrivalTime;

            Console.WriteLine(timeDifference);
            return Ok(new { TimeDifference = timeDifference.ToString() });

        }

    }
}