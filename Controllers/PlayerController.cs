using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LogsTFWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogsTFWebApp.Controllers
{
    public class PlayerController : Controller
    {
        public IActionResult Index()
        {
            PlayerViewModel data = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/api/");
                //HTTP GET
                var responseTask = client.GetAsync("LogsAPI?steamId=U:1:36224460&logNumber=2304504");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<PlayerViewModel>();
                    readTask.Wait();

                    data = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(data);
        }
    }
}