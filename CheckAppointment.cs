using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using System.Web;

namespace CheckAppointment
{
    public static class CheckAppointment
    {
        [FunctionName("CheckAppointment")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var client = new RestClient("https://sosmakeanappointment.as.me/schedule.php?action=showCalendar&fulldate=1&owner=17667501&template=monthly&location=Dearborn");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cookie", "AWSALB=P2JydspPpa6g4YbHOLs264NwtL00u8is+K+NdVlpZdhTXbPg+ORRxCcLs3BC3Ke9H6YKxsqhrn2qnvHYdGM+iLxcVT9fOhKaT/Ov3Fmn2tQY8Jzp6pnu7z6t8bSg; AWSALBCORS=P2JydspPpa6g4YbHOLs264NwtL00u8is+K+NdVlpZdhTXbPg+ORRxCcLs3BC3Ke9H6YKxsqhrn2qnvHYdGM+iLxcVT9fOhKaT/Ov3Fmn2tQY8Jzp6pnu7z6t8bSg; PHPSESSID=lph94f4d2o5h4l0ifrkpa72e23");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("type", "13535563");
            request.AddParameter("calendar", "2979607");
            request.AddParameter("skip", "true");
            request.AddParameter("options[qty]", "1");
            request.AddParameter("options[numDays]", "5");
            IRestResponse response = client.Execute(request);
            var x = response.Content;
            Console.WriteLine(response.Content);
            if (x.Contains("selected=\"selected\">December 2020"))
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                const string accountSid = "**********";
                const string authToken = "*************";
                TwilioClient.Init(accountSid, authToken);
                var to = new PhoneNumber("+1*******");
                var from = new PhoneNumber("+1*******");
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["msg"] = "Hello, go apply fast";
                string queryString = query.ToString();
                var uri = new Uri("https://handler.twilio.com/twiml/*************?" + queryString);
                var call = CallResource.Create(
                    to,
                    from,
                    url: uri
                    );
            }
        }
    }
}
