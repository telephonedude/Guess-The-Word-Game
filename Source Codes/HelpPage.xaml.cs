﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuessTheWordGame
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HelpPage : ContentPage
	{
		public HelpPage ()
		{
			InitializeComponent ();
		}

        private void BackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}