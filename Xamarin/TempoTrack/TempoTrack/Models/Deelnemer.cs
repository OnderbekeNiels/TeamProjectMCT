using System;
using System.Collections.Generic;
using System.Text;

namespace TempoTrack.Models
{
    public class Deelnemer
    {
        public Guid GebruikerId { get; set; }
        public Guid RondeId { get; set; }
        public Guid DeelnemerId { get; set; }
        public string InviteCode { get; set; }
    }
}
