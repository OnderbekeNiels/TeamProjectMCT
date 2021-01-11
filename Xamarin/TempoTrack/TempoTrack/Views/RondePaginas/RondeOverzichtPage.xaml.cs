using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.RondePaginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RondeOverzichtPage : ContentPage
    {
        public static GebruikerV2 GebruikersInfo { get; set; }
        public RondeOverzichtPage(GebruikerV2 gebruikersInfo)
        {
            GebruikersInfo = gebruikersInfo;
            InitializeComponent();
            LoadRondesAsync(GebruikersInfo.GebruikerId, lvwRondes);
        }

        //De rondes ophalen van een bepaalde gebruiker
        private static async Task LoadRondesAsync(Guid gebruikerId, Xamarin.Forms.ListView lvw)
        {
            List<RondesGebruiker> rondesGebruiker = await RondeRepository.GetRondesGebruiker(gebruikerId);

            foreach (RondesGebruiker item in rondesGebruiker)
            {
                Debug.WriteLine(item.ToString());
                //LoadKlassementAsync(item, lvw);
            }

            lvw.ItemsSource = rondesGebruiker;
        }

        //Het klassement ophalen van de rondes waarin de gebruiker meedeed.
        private static async Task LoadKlassementAsync(RondesGebruiker ronde, Xamarin.Forms.ListView lvw)
        {
            List<RondeKlassement> klassementen = await RondeRepository.GetRondeKlassement(ronde.RondeId);

            List<RondesGebruiker> output = new List<RondesGebruiker>();

            foreach (RondeKlassement item in klassementen)
            {
                Debug.WriteLine(item.ToString());
                if (item.GebruikersId == GebruikersInfo.GebruikerId)
                {
                    ronde.Ranking = item.Plaats;
                    output.Add(ronde);
                }
            }

            foreach(RondesGebruiker item in output)
            {
                Debug.WriteLine(item.ToString());
            }

            lvw.ItemsSource = output;

        }
    }
}