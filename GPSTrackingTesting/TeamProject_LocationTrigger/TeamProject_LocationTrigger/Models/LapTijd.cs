using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProject_LocationTrigger.Models
{
    public class LapTijd
    {
        public Guid EtappeId { get; set; }
        public Guid GebruikerId { get; set; }
        public int TijdLap { get; set; }
        public int LapNummer { get; set; }

        public LapTijd(Guid par_etappeId, Guid par_gebruikerId, int par_tijdLap, int par_lapnummer)
        {
            EtappeId = par_etappeId;
            GebruikerId = par_gebruikerId;
            TijdLap = par_tijdLap;
            LapNummer = par_lapnummer;
        }
    }
}
