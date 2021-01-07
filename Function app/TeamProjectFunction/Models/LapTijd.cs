using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models
{
    public class LapTijd
    {
        public Guid LapTijdId { get; set; }
        public Guid EtappeId { get; set; }
        public Guid GebruikerId { get; set; }
        public int TijdLap { get; set; }
        public int LapNummer { get; set; }
    }
}
