using System;
using System.Text;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// using System.Net.Mqtt;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using Mqtt.Client.AspNetCore.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;


// Modelos para Base de datos
using AspStudio.Models;
using AspStudio.Data;
using System.Net.Http;

namespace AspStudio.Controllers
{
    public class ConsultaLenelController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<ConsultaLenel> consultaslenel = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:64189/api/CardHolder/ObtenerEmpleado/11770");
                //HTTP GET
                var responseTask = client.GetAsync("consultalenel");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<IList<ConsultaLenel>>();
                    //readTask.Wait();

                    //consultaslenel = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    consultaslenel = Enumerable.Empty<ConsultaLenel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(consultaslenel);
        }
    }
}
