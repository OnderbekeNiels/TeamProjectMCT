using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models.GebruikerRelated
{
    public class RondesAdmin
    {
        public Guid RondeId { get; set; }
        public string RondeNaam { get; set; }
        public DateTime StartDatum { get; set; }
        public int AantalEtappes { get; set; }
        public Guid Admin { get; set; }
    }
}
