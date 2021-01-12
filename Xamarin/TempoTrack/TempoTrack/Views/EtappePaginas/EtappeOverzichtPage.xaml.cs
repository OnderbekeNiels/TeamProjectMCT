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

namespace TempoTrack.Views.EtappePaginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EtappeOverzichtPage : ContentPage
    {
        private static RondesGebruiker RondeInfo { get; set; }
        public EtappeOverzichtPage(RondesGebruiker rondeInfo)
        {
            RondeInfo = rondeInfo;
            InitializeComponent();

            btnInvite.Clicked += BtnInvite_Clicked;
            btnStoppen.Clicked += BtnStoppen_Clicked;

            LoadEtappesAsync(RondeInfo.RondeId, RondeInfo.GebruikersId, lvwEtappes, lblRondePlaats, lblRondeTijd);
            Title = RondeInfo.RondeNaam;
        }

        private static async Task LoadEtappesAsync(Guid rondeId, Guid gebruikersId, Xamarin.Forms.ListView lvw, Xamarin.Forms.Label lblRondePlaats, Xamarin.Forms.Label lblRondeTijd)
        {
           List<EtappesRonde> etappes = await EtappeRepository.GetEtappesRonde(rondeId, gebruikersId);

            int teller = 1;
            foreach (EtappesRonde item in etappes)
            {
                item.EtappeNaam = $"Etappe {teller}";
                teller += 1;
                //item.TotaalRondeTijd = RondeInfo.TotaalTijd;
                //Debug.WriteLine("-------------------------------------------------------");
                //Debug.WriteLine(item.ToString());
                //Debug.WriteLine("-------------------------------------------------------");
            }

            lvw.ItemsSource = etappes;
            lblRondePlaats.Text = RondeInfo.Plaats;
            lblRondeTijd.Text = TimeSpan.FromSeconds(RondeInfo.TotaalTijd).ToString();
        }

        private void BtnStoppen_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnInvite_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new Invite)
        }
    }
}