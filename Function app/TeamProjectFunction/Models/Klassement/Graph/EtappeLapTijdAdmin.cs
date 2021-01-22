using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models.Klassement.Graph
{
    public class EtappeLapTijdAdmin
    {
        public Guid EtappeId { get; set; }
        public Guid GebruikersId { get; set; }
        public string GebruikersNaam { get; set; }
        public int Plaats { get; set; }
        public List<EtappeLapTijdUser> LapTijden { get; set; }

    }
}
