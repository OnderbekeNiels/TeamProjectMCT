using System;
using System.Collections.Generic;
using System.Text;

namespace TempoTrack.Models
{
    public class RondesGebruiker
    {
        public DateTime StartDatum { get; set; }
        public string RondeNaam { get; set; }
        public Guid RondeId { get; set; }
        public int Ranking { get; set; }

        public override string ToString()
        {
            return $"Startdatum: {StartDatum}, RondeNaam: {RondeNaam}, RondeId: {RondeId}, Ranking: {Ranking}";
        }
    }
}
