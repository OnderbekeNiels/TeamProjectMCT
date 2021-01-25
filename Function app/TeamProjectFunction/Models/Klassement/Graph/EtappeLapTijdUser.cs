using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models.Klassement.Graph
{
    public class EtappeLapTijdUser
    {
        public Guid EtappeId { get; set; }

        public Guid GebruikerId { get; set; }
        public int Laps { get; set; }
        public int  LapNummer { get; set; }
        public int TijdLap { get; set; }
    }
}
