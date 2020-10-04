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
	public partial class MainStatPage : ContentPage
	{
        FirebasePlayerHelper firebasePlayerHelper = new FirebasePlayerHelper();
		public MainStatPage ()
		{
			InitializeComponent ();
            UpdateList();
		}

        private async void UpdateList()
        {
            var allPersons = await firebasePlayerHelper.GetAllPersons();
            PlayerListView.ItemsSource = allPersons;
        }

        private void PlayerListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Player selectedItem = e.SelectedItem as Player;
            Navigation.PushAsync(new StatsPage(selectedItem));
            return;
        }

        private void BackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}