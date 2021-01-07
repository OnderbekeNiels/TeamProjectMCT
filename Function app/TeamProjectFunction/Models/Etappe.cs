using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models
{
    public class Etappe
    {
        public Guid EtappeId { get; set; }
        public int Laps { get; set; }
        public Guid RondeId { get; set; }
        public float LapAfstand { get; set; }
        public DateTime StartTijd { get; set; }
    }
}
