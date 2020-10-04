using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuessTheWordGame
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMenu : ContentPage
	{
        string currentplayer;
		public MainMenu (string username)
		{
			InitializeComponent ();
            currentplayer = username;
		}

        private void StatBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainStatPage());
        }

        private void ExitBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void AboutBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }

        private void HelpBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HelpPage());
        }

        private void StartBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GameStart(currentplayer));
        }
    }
}