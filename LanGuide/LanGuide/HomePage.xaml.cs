using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LanGuide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public string WebAPIkey = "AIzaSyDct_nPvd8-FQwP990H8xUixiLu8b6pA7M";
        public HomePage()
        {
            InitializeComponent();

            GetProfileInformationAndRefreshToken();
        }

        private async void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            try
            {
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alert", "Oh no! Token expired.", "OK");
            }
        }

        private void UsersFrame_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UsersPage());
        }
        private async void ResultsFrame_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ResultsPage());
        }
        private async void LanguagesFrame_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LanguagesPage());
        }

        private void logoutButton_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            App.Current.MainPage = new NavigationPage(new MainPage());
        }

    }
}