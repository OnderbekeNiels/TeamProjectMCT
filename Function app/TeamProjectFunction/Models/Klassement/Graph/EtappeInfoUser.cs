using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models.Klassement.Graph
{
    public class EtappeInfoUser
    {
        public string GebruikersNaam { get; set; }
        public Guid GebruikerId { get; set; }
        public string RondeNaam { get; set; }
        public Guid RondeId { get; set; }
        public Guid EtappeId { get; set; }
        public int TotaalTijd { get; set; }
        public double GemiddeldeLaptijd { get; set; }
        public int SnelsteLapTijd { get; set; }
        public int TraagsteLapTijd { get; set; }
    }
}
