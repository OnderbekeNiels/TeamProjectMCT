using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models
{
    public class Gebruiker
    {
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Email { get; set; }
        public string Wachtwoord { get; set; }
        public Guid GebruikerId { get; set; }
    }
}
