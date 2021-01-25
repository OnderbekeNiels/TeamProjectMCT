using System;
using System.Collections.Generic;
using System.Text;

namespace TempoTrack.Models
{
    public class DeelnemersRonde
    {
        public Guid GebruikersId { get; set; }
        public string GebruikersNaam { get; set; }

        public int Nummer { get; set; }

        public string Kleur { get; set; }
    }
}
