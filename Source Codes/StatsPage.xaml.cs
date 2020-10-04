using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GuessTheWordGame.Helpers;

namespace GuessTheWordGame
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatsPage : ContentPage
    {
        FirebasePlayerHelper firebasePlayerHelper = new FirebasePlayerHelper();
        Player player = new Player();
        public StatsPage (Player currentplayer)
		{
			InitializeComponent ();
            player = currentplayer;
            Update();
		}

        private void BackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Update()
        {
            avatarbox.Source = player.Avatar;
            Namebox.Text = player.Username;
            IDbox.Text = player.UserID.ToString();
            Goldbox.Text = player.HighestGold.ToString();
            Roundbox.Text = player.HighestRound.ToString();
            Revealbox.Text = player.Reveals.ToString();
            TotalRoundsbox.Text = player.TotalRounds.ToString();
        }
    }
}