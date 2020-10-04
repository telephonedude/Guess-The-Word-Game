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
	public partial class GameStart : ContentPage
    {
        List<Button> ButtonList = new List<Button>();
        List<Label> LabelList = new List<Label>();
        FirebaseWordHelper firebaseWordHelper = new FirebaseWordHelper();
        FirebasePlayerHelper firebasePlayerHelper = new FirebasePlayerHelper();
        Random generator = new Random();
        int currentWord;
        int reveals = 0;
        string currentuser;
        int rounds=0;
        int currentGold = 500;

        public GameStart (string username)
		{
			InitializeComponent ();
            ButtonList = new List<Button>
            {
                ABtn, BBtn, CBtn, DBtn, EBtn,
                FBtn, GBtn, HBtn, IBtn, JBtn,
                KBtn, LBtn, MBtn, NBtn, OBtn,
                PBtn, QBtn, RBtn, SBtn, TBtn,
                UBtn, VBtn, WBtn, XBtn, YBtn,
                ZBtn
            };
            LabelList = new List<Label>
            {
                Box0, Box1, Box2, Box3, Box4,
                Box5, Box6, Box7
            };
            currentuser = username;
            GenerateNewWord();
            goldBox.Text = currentGold + " Gold";
            roundBox.Text = rounds.ToString();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            int l = 0;
            bool checking = false;
            foreach(Button x in ButtonList)
            {
                if(x==sender)
                {
                    foreach(Label y in LabelList)
                    {
                        if (x.Text == y.Text)
                        {
                            y.TextColor = Color.FromHex("#000000");
                            checking = true;
                            x.IsEnabled = false;
                        }
                        else
                        {
                            x.IsEnabled = false;
                        }
                    }
                }
            }
            if (checking == false)
            {
                foreach (Label z in LabelList)
                {
                    if (z.IsVisible == true)
                    {
                        l++;
                    }
                }
                double b = hpBar.Progress;
                double n = 100 / l;
                b = b * 100;
                b = b - n;
                b = b / 100;
                hpBar.Progress = b;
                l = 0;
                if(hpBar.Progress <= 0)
                {
                    DisplayAlert("You have lost.", "You have run out of hit points, please restart game if you want to play again", "OK");
                    TurnOffButtons();
                    foreach (Label x in LabelList)
                    {
                        x.TextColor = Color.FromHex("#000000");
                    }
                    exitBtn.IsEnabled = true;
                    return;
                }
            }
            checking = false;
            CheckWin();
        }

        private async void GenerateNewWord()
        {
            TurnOffButtons();
            rounds++;
            roundBox.Text = rounds.ToString();
            hpBar.Progress = 1;
            var player = await firebasePlayerHelper.GetPlayer(currentuser);
            usernameBox.Text = player.Username;
            int y = 0;
            currentWord = generator.Next(0, 59);
            var allPersons = await firebaseWordHelper.GetAllWords();
            foreach (Label x in LabelList)
            {
                LabelList[y].Text = "";
                y++;
            }
            y = 0;
            foreach (char x in allPersons[currentWord].Word)
            {
                LabelList[y].TextColor = Color.FromHex("#b8d4a2");
                LabelList[y].Text = x.ToString();
                y++;
            }
            y = 0;
            foreach(Label x in LabelList)
            {
                if (LabelList[y].Text == "")
                {
                    LabelList[y].IsVisible = false;
                }
                else
                {
                    LabelList[y].IsVisible = true;
                }
                y++;
            }
            synonymBox.Text = allPersons[currentWord].Hint;
            definitionBox.Text = allPersons[currentWord].Definition;
            definitionBox.IsVisible = false;
            y = 0;
            TurnOnButtons();
        }

        private void WordRevealBtn_Clicked(object sender, EventArgs e)
        {
            if (currentGold < 100)
            {
                DisplayAlert("Alert", "Sorry, you do not have enough gold, you need 100 to reveal word", "OK");
                return;
            }
            else
            {
                reveals++;
                foreach (Label x in LabelList)
                {
                    x.TextColor = Color.FromHex("#000000");
                }
                currentGold -= 100;
                continueBtn.IsVisible = true;
                continueBtn.IsEnabled = true;
                goldBox.Text = currentGold + " gold";
            }
        }

        private void DefRevealBtn_Clicked(object sender, EventArgs e)
        {
            if (currentGold < 10)
            {
                DisplayAlert("Alert", "Sorry, you do not have enough gold, you need 10 to reveal word", "OK");
                return;
            }
            else
            {
                definitionBox.IsVisible = true;
                currentGold -= 10;
                goldBox.Text = currentGold + " gold";
            }
            SaveProgress();
        }

        private void ContinueBtn_Clicked(object sender, EventArgs e)
        {
            continueBtn.IsVisible = false;
            continueBtn.IsEnabled = false;
            GenerateNewWord();
        }

        private void TurnOffButtons()
        {
            foreach (Button x in ButtonList)
            {
                x.IsEnabled = false;
            }
            wordRevealBtn.IsEnabled = false;
            defRevealBtn.IsEnabled = false;
            exitBtn.IsEnabled = false;
        }

        private void TurnOnButtons()
        {
            foreach (Button x in ButtonList)
            {
                x.IsEnabled = true;
            }
            wordRevealBtn.IsEnabled = true;
            defRevealBtn.IsEnabled = true;
            exitBtn.IsEnabled = true;
        }

        private void CheckWin()
        {
            int l = 0;
            int p = 0;
            int goldGained = 0;
            foreach(Label x in LabelList)
            {
                if(x.IsVisible == true)
                {
                    l++;
                    if(x.TextColor == Color.FromHex("#000000"))
                    {
                        p++;
                    }
                }
            }
            if(l==p)
            {
                goldGained = l * 10;
                DisplayAlert("Congratualations!", "You got the word right! You get " + goldGained + " gold", "OK");
                continueBtn.IsVisible = true;
                continueBtn.IsEnabled = true;
                currentGold += goldGained;
                goldBox.Text = currentGold + " gold";
                SaveProgress();
            }
        }

        private async void SaveProgress()
        {
            var player = await firebasePlayerHelper.GetPlayer(currentuser);
            Player newdude = new Player();
            newdude.UserID = player.UserID;
            newdude.Username = player.Username;
            newdude.Password = player.Password;
            newdude.Avatar = player.Avatar;
            newdude.Info = player.Info;
            newdude.TotalRounds = player.TotalRounds + rounds;
            newdude.Reveals = player.Reveals + reveals;
            if(currentGold > player.HighestGold)
            {
                newdude.HighestGold = currentGold;
            }
            if(rounds > player.HighestRound)
            {
                newdude.HighestRound = rounds;
            }
            await firebasePlayerHelper.UpdatePlayer(newdude);
        }

        private void ExitBtn_Clicked(object sender, EventArgs e)
        {
            SaveProgress();
            Navigation.PopAsync();
        }
    }
}