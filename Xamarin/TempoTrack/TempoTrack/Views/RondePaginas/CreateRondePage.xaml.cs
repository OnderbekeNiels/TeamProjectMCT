using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.RondePaginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateRondePage : ContentPage
    {
        public CreateRondePage(GebruikerV2 gebruikersInfo)
        {
            InitializeComponent();
        }
    }
}