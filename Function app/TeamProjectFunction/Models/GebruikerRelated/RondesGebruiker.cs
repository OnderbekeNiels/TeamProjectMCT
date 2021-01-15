﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TeamProjectFunction.Models.GebruikerRelated
{
    class RondesGebruiker
    {
        public Guid GebruikersId { get; set; }
        public DateTime StartDatum { get; set; }
        public string RondeNaam { get; set; }
        public Guid RondeId { get; set; }
        public int Plaats { get; set; }
        public string InviteCode { get; set; }
        public int TotaalTijd { get; set; }
        public int AantalEtappes { get; set; }
        public int AantalDeelnemers { get; set; }
        public Guid Admin { get; set; }
        public bool DeelnemerActief { get; set; }
        public bool EtappeActief { get; set; }
    }
}
