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
using Xamarin.Essentials;
using TempoTrack.Views.InternetConnectivity;

namespace TempoTrack.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private static string token = "";
        public LoginPage()
        {
            InitializeComponent();
            checkConnectivity();
            imgLogo.Source = ImageSource.FromResource("TempoTrack.Assets.Images.LogoOnboarding.png");
            imgGoogle.Source = ImageSource.FromResource("TempoTrack.Assets.Images.GoogleLogo.png");

            TapGestureRecognizer recog = new TapGestureRecognizer();
            recog.Tapped += BtnLogin_Clicked;
            btnLogin.GestureRecognizers.Add(recog);
        }

        private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            checkConnectivity();

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
                    string email = googleObject.email;
                    string name = googleObject.name;

                    GebruikerV2 gebruikerInfo = await LoginRepository.CheckLogin(googleObject);

                    if (gebruikerInfo != null)
                    {
                        Navigation.PushAsync(new RondeOverzichtPage(gebruikerInfo));
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
                    //write the code to handle when auth failed
                }
            };
            authenticator.Error += onAuthError;

        }

        private void onAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            DisplayAlert("Google Authentication Error", e.Message, "OK");
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void checkConnectivity()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                Navigation.PushModalAsync(new NoConnection());
            }
        }
    }
}