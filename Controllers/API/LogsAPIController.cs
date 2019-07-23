using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LogsTFWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LogsTFWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsAPIController : ControllerBase
    {
        private const string BASE_LOGSTF_URL = "http://logs.tf/json/";
        private const string BASE_LOGSTF_API_URL = "http://logs.tf/api/v1/log";
        private JToken _token;
        private JToken _playerToken;
        private JToken _nameToken;
        private JToken _logsToken;

        public ActionResult GetDataForPlayerFromLog(string steamId, string logNumber)
        {
            PlayerViewModel data = new PlayerViewModel
            {
                SteamId = steamId
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_LOGSTF_URL);
                //HTTP GET
                var responseTask = client.GetAsync(logNumber);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var json = result.Content.ReadAsStringAsync();
                    json.Wait();

                    this._token = JObject.Parse(json.Result);

                    this._playerToken = this._token.SelectToken("players");
                    this._playerToken[$"[{steamId}]"].ToString();

                    this._nameToken = this._token.SelectToken("names");

                    data.Kills = this._playerToken[$"[{steamId}]"]["kills"].Value<int>();
                    data.Assists = this._playerToken[$"[{steamId}]"]["assists"].Value<int>();
                    data.Deaths = this._playerToken[$"[{steamId}]"]["deaths"].Value<int>();
                    data.DamagePerMinute = this._playerToken[$"[{steamId}]"]["dapm"].Value<int>();
                    data.Playername = this._nameToken[$"[{steamId}]"].Value<string>();
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // GET: api/LogsAPI/5
        [HttpGet("{id}")]
        public ActionResult GetLogListForPlayer(string steamId)
        {
            PlayerViewModel data = new PlayerViewModel
            {
                SteamId = steamId
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_LOGSTF_API_URL);
                var task = client.GetAsync($"?player={steamId}");
                task.Wait();
                var result = task.Result;
                if (result.IsSuccessStatusCode)
                {
                    var json = result.Content.ReadAsStringAsync();
                    json.Wait();
                    this._token = JObject.Parse(json.Result);
                    this._logsToken = this._token.SelectToken("logs");
                    data.LogList = this._logsToken.ToObject<List<LogViewModel>>();
                    return Ok(data);
                }
                else
                {
                    return NotFound("Couldn't return any results");
                }
            }
        }
    }
}