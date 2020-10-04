using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using GuessTheWordGame.Helpers;

namespace GuessTheWordGame
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignUpPage : ContentPage
    {
        FirebasePlayerHelper firebasePlayerHelper = new FirebasePlayerHelper();
        string Imagepath;
        Random generator = new Random();
        public SignUpPage ()
		{
			InitializeComponent ();
            idbox.Text = generator.Next(10000000, 99999999).ToString();
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
{
            if (userbox.Text == "" || passbox.Text == "" || infobox.Text == "")
            {
                await DisplayAlert("Alert", "Please fill up all available areas, thank you", "OK");
            return;
            }
            if (check_boxes() == false)
            {
                return;
            }
            Player newdude = new Player();
            newdude.UserID = Convert.ToInt32(idbox.Text);
            newdude.Username = userbox.Text;
            newdude.Password = passbox.Text;
            newdude.Avatar = Imagepath;
            newdude.Info = infobox.Text;
            var player = await firebasePlayerHelper.GetPlayer(userbox.Text);
            var playertest = await firebasePlayerHelper.GetAllPersons();
            foreach (Player k in playertest)
            {
                if(k.UserID == Convert.ToInt32(idbox.Text))
                {
                    idbox.Text = generator.Next(10000000, 99999999).ToString();
                }
            }
            if (player == null)
            {
                await firebasePlayerHelper.AddPlayer(newdude);
                await DisplayAlert("Success!", "Successfully added player", "OK");
                await Navigation.PopAsync();
            return;
            }
            else
            {
                await DisplayAlert("Alert!", "Username already taken", "OK");
            return;
            }

        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void Upload_Btn_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Not supported", "Your device does not support this", "OK");
                    return;
                }

                var mediaOptions = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };

                var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                if (SelectedImage == null)
                {
                    await DisplayAlert("Error", "Could not get image.", "OK");
                    return;
                }

                SelectedImage.Source = selectedImageFile.Path;
                Imagepath = selectedImageFile.Path;
            }
            catch (MediaPermissionException)
            {
                await DisplayAlert("Error, no permission to access", "Can't access gallery.", "Ok");
            }
            catch
            {
            }

        }

        private bool check_boxes()
        {
            int x = 0;
            var player = firebasePlayerHelper.GetPlayer(userbox.Text);
            foreach (char c in passbox.Text)
            {
                if (!Char.IsNumber(c))
                {
                    DisplayAlert("Alert", "Please only input numbers in the password box, thanks!", "OK");
                    return false;
                }
            }
            foreach (char c in passbox.Text)
            {
                x++;
            }
            if (x != 6)
            {
                DisplayAlert("Alert", "Please only input 6 digits in the password box, thanks!", "OK");
                x = 0;
                return false;
            }
            if (Imagepath == null)
            {
                DisplayAlert("Alert", "Please choose an avatar before proceeding, thanks!", "OK");
                x = 0;
                return false;
            }
            if(passbox.Text != passbox2.Text)
            {
                DisplayAlert("Alert", "Passwords do not match!", "OK");
                return false;
            }
            x = 0;
            return true;
        }

    }
}