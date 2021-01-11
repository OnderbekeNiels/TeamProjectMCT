using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models
{
    class EtappesRonde
    {
        public string RondeNaam { get; set; }
        public int EtappeTijd { get; set; }
        public Guid EtappeId { get; set; }
        public DateTime StartTijd { get; set; }
    }
}
