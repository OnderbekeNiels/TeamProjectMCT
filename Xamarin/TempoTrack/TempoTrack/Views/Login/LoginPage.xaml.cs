using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.Net.Http;
using TempoTrack.Models;
using TempoTrack.Repositories;
using TempoTrack.Views.RondePaginas;
using TempoTrack.Views.EtappePaginas;
using TempoTrack.Views.Activity;

namespace TempoTrack.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private static string token = "";
        public LoginPage()
        {
            InitializeComponent();
            imgLogo.Source = ImageSource.FromResource("TempoTrack.Assets.Images.LogoOnboarding.png");
            imgGoogle.Source = ImageSource.FromResource("TempoTrack.Assets.Images.GoogleLogo.png");

            TapGestureRecognizer recog = new TapGestureRecognizer();
            recog.Tapped += BtnLogin_Clicked;
            btnLogin.GestureRecognizers.Add(recog);
        }

        private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            /*
            var authenticator = new OAuth2Authenticator
                         (
                           "623999391750-8cep3ajgrnh26gdnmlbjq3376im2gcui.apps.googleusercontent.com",
                           "email profile",
                            new System.Uri("https://accounts.google.com/o/oauth2/auth"),
                            new System.Uri("https://localhost:44312/signin-google")
                          );

            authenticator.AllowCancel = true;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);

            authenticator.Completed += async (senders, obj) =>
            {
                if (obj.IsAuthenticated)
                {
                    Debug.WriteLine("Dit doet iets");
                    var clientData = new HttpClient();

                    //call google api to fetch logged in user profile info
                    token = obj.Account.Properties["access_token"];
                    var resData = await clientData.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo?access_token=" + obj.Account.Properties["access_token"]);
                    var jsonData = await resData.Content.ReadAsStringAsync();

                    //deserlize the jsondata and intilize in GoogleAuthClass
                    GoogleLogin googleObject = JsonConvert.DeserializeObject<GoogleLogin>(jsonData);

                    //you can access following property after login
                    //string email = googleObject.email;
                    //string name = googleObject.name;

                    GebruikerV2 gebruikerInfo = await LoginRepository.CheckLogin(googleObject);

                    if(gebruikerInfo != null)
                    {
                        //Navigation.PushAsync();
                        Debug.WriteLine("-----------------------------------------------------------------------------------------");
                        Debug.WriteLine($"Email:{gebruikerInfo.Email}, Gebruikersnaam: {gebruikerInfo.name}, GebruikersId: {gebruikerInfo.GebruikerId}");
                        Debug.WriteLine("-----------------------------------------------------------------------------------------");

                    }
                    else
                    {
                        Navigation.PopAsync();
                        Debug.WriteLine("-----------------------------------------------------------------------------------------");
                        Debug.WriteLine("Failed for some reason");
                        Debug.WriteLine("-----------------------------------------------------------------------------------------");
                    }
                }
                else
                {
                    //Authentication fail
                    // write the code to handle when auth failed
                }
            };
            authenticator.Error += onAuthError;*/
            GebruikerV2 gebruikerInfo = new GebruikerV2();
            //gebruikerInfo.GebruikerId = Guid.Parse("547f309b-8596-4dbe-9439-333a7c9e79de");
            gebruikerInfo.GebruikerId = Guid.Parse("7B002679-1EE4-44DF-B5B7-54B7C76C3C73");
            gebruikerInfo.Email = "niels@email.com";
            gebruikerInfo.name = "Niels";
            Navigation.PushAsync(new RondeOverzichtPage(gebruikerInfo));
            //Navigation.PushAsync(new CreateRondePage(gebruikerInfo));
            //Navigation.PushAsync(new ActivityPage());

            //Ronde ronde = new Ronde();
            //ronde.RondeId = Guid.Parse("3A8CC923-EEAA-49CA-9E95-07687F7ADC3E");
            //Navigation.PushAsync(new CreateEtappePage(ronde.RondeId));
        }

        private void onAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            DisplayAlert("Google Authentication Error", e.Message, "OK");
        }
    }
}