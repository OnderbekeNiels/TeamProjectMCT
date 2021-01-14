using System;
using System.Collections.Generic;
using System.Text;

namespace TempoTrack.Models
{
    public class DeelnemerResponse
    {
        public Guid GebruikerId { get; set; }
        public Guid RondeId { get; set; }
        public Guid DeelnemerId { get; set; }
        public string InviteCode { get; set; }
        public bool Succes { get; set; }
        public string Message { get; set; }
    }
}
