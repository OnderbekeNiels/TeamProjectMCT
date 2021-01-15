using System;
using System.Collections.Generic;
using System.Text;

namespace TempoTrack.Models
{
    public class RondesGebruiker
    {

        public DateTime StartDatum { get; set; }

        public string StartDate
        {
            get
            {
                DateTime belgie = new DateTime();
                belgie = StartDatum.ToUniversalTime();
                return belgie.ToString("dd/MM/yyyy");
                //return belgie.ToShortDateString();
            }
        }
        public string RondeNaam { get; set; }
        public Guid RondeId { get; set; }

        public int Plaats { get; set; }

        public string Ranking
        {
            get
            {
                return $"#{Plaats}";
            }
        }

        public Guid GebruikersId { get; set; }

        public string InviteCode { get; set; }
        public int TotaalTijd { get; set; }
        public Guid Admin { get; set; }
        public int AantalEtappes { get; set; }
        public int AantalDeelnemers { get; set; }

        public string IsAdmin { get; set; }

        public override string ToString()
        {
            return $"Startdatum: {StartDatum}, RondeNaam: {RondeNaam}, RondeId: {RondeId}, Ranking: {Ranking}";
        }
    }
}
