using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models
{
    public class Ronde
    {
        public Guid RondeId { get; set; }
        public string Naam { get; set; }
        public Guid Admin { get; set; }
        public string InviteCode { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
    }
}
