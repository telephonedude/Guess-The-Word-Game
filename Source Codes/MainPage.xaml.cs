using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using GuessTheWordGame.Helpers;

namespace GuessTheWordGame
{
    public partial class MainPage : ContentPage
    {
        FirebasePlayerHelper firebasePlayerHelper = new FirebasePlayerHelper();
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                loginBtn.IsEnabled = false;
                var player = await firebasePlayerHelper.GetPlayer(userbox.Text);
                if (player.Password == passbox.Text)
                {
                    await DisplayAlert("Success!", "Player found, now logging in", "OK");
                    loginBtn.IsEnabled = true;
                    await Navigation.PushAsync(new MainMenu(player.Username));
                }
                else
                {
                    await DisplayAlert("Error!", "Wrong password", "OK");
                    loginBtn.IsEnabled = true;
                }
            }
            catch
            {
                await DisplayAlert("Error!", "Player not found, please try again", "OK");
                loginBtn.IsEnabled = true;
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }
    }
}
