using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models.Klassement
{
    public class KlassementEtappe
    {
        public int Plaats { get; set; }
        public Guid GebruikersId { get; set; }
        public string GebruikersNaam { get; set; }
        public Guid EtappeId { get; set; }
        public int TotaalTijd { get; set; }
    }
}
