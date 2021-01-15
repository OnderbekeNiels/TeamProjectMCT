using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models
{
    public class EtappesRonde
    {
        public Guid GebruikersId { get; set; }
        public Guid RondId { get; set; }
        public string RondeNaam { get; set; }
        public Guid EtappeId { get; set; }
        public int Laps { get; set; }
        public DateTime StartTijd { get; set; }
        public double LapAfstand { get; set; }
        public int TotaalTijd { get; set; }
        public int SnelsteTijd { get; set; }
        public int Plaats { get; set; }
        public Guid Admin { get; set; }
        public int EtappeActief { get; set; }

    }
}
