using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models.Klassement
{
    public class EtappeInfo
    {
        public Guid RondeId { get; set; }
        public string RondeNaam { get; set; }
        public Guid EtappeId { get; set; }
        public int TotaalTijd { get; set; }
        public double GemiddeldeLapTijd { get; set; }
        public double Afstand { get; set; }
        public int Laps { get; set; }
        public int AantalDeelnemers  { get; set; }
    }
}
