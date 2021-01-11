using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models.Klassement
{
    class KlassementRonde
    {
        public int Plaats { get; set; }
        public Guid GebruikersId { get; set; }
        public string GebruikersNaam { get; set; }
        public Guid RondeId { get; set; }
        public string RondeNaam { get; set; }
        public int TotaalTijd { get; set; }
    }
}
