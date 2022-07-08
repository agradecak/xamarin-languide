using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LanGuide
{
    public partial class MainPage : ContentPage
    {
        public string WebAPIkey = "AIzaSyDct_nPvd8-FQwP990H8xUixiLu8b6pA7M";
        public MainPage()
        {
            InitializeComponent();
        }
        private async void loginButton_Clicked(object sender, EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(userEntry.Text, passEntry.Text);
                var content = await auth.GetFreshAuthAsync();
                var serializedcontent = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serializedcontent);
                await Navigation.PushAsync(new HomePage());
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid email and password combination.", "OK");
            }
        }
    }
}
