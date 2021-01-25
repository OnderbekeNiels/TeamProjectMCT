using System;
using System.Collections.Generic;
using System.Text;

namespace TempoTrack.Models
{
    public class RondeKlassement
    {
        public int Plaats { get; set; }
        public Guid GebruikersId { get; set; }
        public string GebruikersNaam { get; set; }
        public Guid RondeId { get; set; }
        public string RondeNaam { get; set; }
        public int TotaalTijd { get; set; }

        public override string ToString()
        {
            return $"Plaats: {Plaats}, GebruikersNaam: {GebruikersNaam}, RondeNaam: {RondeNaam}";
        }
    }
}
