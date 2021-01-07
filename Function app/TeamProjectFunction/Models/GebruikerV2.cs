using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models
{
    public class GebruikerV2
    {
        public Guid GebruikerId { get; set; }
        public string Email { get; set; }
        public string GebruikersNaam { get; set; }
    }
}
