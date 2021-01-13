using System;
using System.Collections.Generic;
using System.Text;

namespace TempoTrack.Models
{
    public class EtappesRonde
    {
        public Guid GebruikersId { get; set; }
        public Guid RondId { get; set; }
        public Guid EtappeId { get; set; }
        public int Laps { get; set; }
        public DateTime StartTijd { get; set; }
        public double LapAfstand { get; set; }
        public int TotaalTijd { get; set; }
        public int SnelsteTijd { get; set; }
        public int Plaats { get; set; }

        public string Ranking
        {
            get
            {
                return $"#{Plaats}";
            }
        }
        public string EtappeNaam { get; set; }
        public string VerschilTijd
        {
            get
            {
                string secondenTekst = "";
                string minutenTekst = "";

                int verschil = TotaalTijd - SnelsteTijd;

                int seconden = verschil % 60;
                if (seconden < 10)
                {
                    secondenTekst = $"0{seconden}";
                }
                else
                {
                    secondenTekst = Convert.ToString(seconden);
                }

                int minuten = verschil / 60;
                if (minuten < 10)
                {
                    minutenTekst = $"0{minuten}";
                }
                else
                {
                    minutenTekst = Convert.ToString(minuten);
                }
                return $"+{minutenTekst}:{secondenTekst}";
            }
        }

        public string EtappeTijd
        {
            get
            {
                return TimeSpan.FromSeconds(TotaalTijd).ToString();
            }
        }

        public override string ToString()
        {
            return $"EtappeId: {EtappeId}, GebruikersId: {GebruikersId}, TotaalTijd: {TotaalTijd}, Plaats: {Plaats}";
        }
    }
}
