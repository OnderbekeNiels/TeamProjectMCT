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
        RondesGebruiker RondeInfo { get; set; }
        public EtappeOverzichtPage(RondesGebruiker rondeInfo)
        {
            RondeInfo = rondeInfo;
            InitializeComponent();

            LoadEtappesAsync(RondeInfo.RondeId, RondeInfo.GebruikersId);
        }

        private static async Task LoadEtappesAsync(Guid rondeId, Guid gebruikersId)
        {
           List<EtappesRonde> etappes = await EtappeRepository.GetEtappesRonde(rondeId, gebruikersId);

            //foreach(EtappesRonde item in etappes)
            //{
            //    Debug.WriteLine("-------------------------------------------------------");
            //    Debug.WriteLine(item.ToString());
            //    Debug.WriteLine("-------------------------------------------------------");
            //}
        }
    }
}