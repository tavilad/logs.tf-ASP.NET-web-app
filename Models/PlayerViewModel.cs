using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogsTFWebApp.Models
{
    public class PlayerViewModel
    {
        public string Playername { get; set; }

        public string SteamId { get; set; }

        public int Kills { get; set; }

        public int Assists { get; set; }

        public int Deaths { get; set; }

        public int DamagePerMinute { get; set; }

        public List<LogViewModel> LogList { get; set; }
    }
}
