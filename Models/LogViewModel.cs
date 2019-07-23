using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogsTFWebApp.Models
{
    public class LogViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Map { get; set; }
        public string Date { get; set; }
        public int Views { get; set; }
        public int Players { get; set; }

    }
}
