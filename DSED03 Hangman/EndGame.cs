using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DSED03_Hangman
{
    [Activity(Label = "EndGame", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EndGame : Activity
    {
        private DatabaseManager myDbManager;
        public TextView TxtEndGameStatus;
        public TextView TxtEndGameWord;
        public ImageView ImgEndGame;
        private Button btnPlayAgain;
        private Button btnQuit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EndScreen);
            // Create your application here
            TxtEndGameStatus = FindViewById<TextView>(Resource.Id.txtEndGameStatus);
            TxtEndGameWord = FindViewById<TextView>(Resource.Id.txtEndGameWord);
            ImgEndGame = FindViewById<ImageView>(Resource.Id.imgEndGame);
            ImgEndGame.SetImageResource(GameInfo.ImageLookup[GameInfo.Attempt]);
            btnPlayAgain = FindViewById<Button>(Resource.Id.btnEndGamePlayAgain);
            btnQuit = FindViewById<Button>(Resource.Id.btnEndGameQuit);
            btnPlayAgain.Click += BtnPlayAgainClick;
            btnQuit.Click += BtnQuitClick;
            myDbManager = new DatabaseManager();
            switch (GameInfo.WordGuessed)
            {
                case true:
                    GameWon();
                    break;
                case false:
                    GameLost();
                    break;
            }
            FindViewById<TextView>(Resource.Id.txtEndGameWord).SetText(Html.FromHtml(GameInfo.DisplayString), TextView.BufferType.Normal);
        }

        private void BtnQuitClick(object sender, EventArgs e)
        {
           Finish();
        }

        private void BtnPlayAgainClick(object sender, EventArgs e)
        {
            StartActivity(typeof(Gameplay));
            Finish();
        }

        private void GameLost()
        {
            SetDisplayString(false);
            TxtEndGameStatus.Text = "Sorry, you lost.";
            UpdatePlayerOnLoss();


            // DisplayCharArrayOnScreen();
        }

        private void GameWon()
        {
            SetDisplayString(true);
            TxtEndGameStatus.Text = "You Won. Word score: " + GameInfo.WordScore;
            UpdatePlayerOnWin();
            //DisplayCharArrayOnScreen();
        }

        public void UpdatePlayerOnWin()
        {
            //  update lastplayeddate
            GameInfo.CurrentPlayer.LastPlayedDate=DateTime.Today;
//add to currentstreak
            GameInfo.CurrentPlayer.CurrentStreak += GameInfo.WordScore;

            //update beststreak+
            if (GameInfo.CurrentPlayer.BestStreak < GameInfo.CurrentPlayer.CurrentStreak)
            {
                GameInfo.CurrentPlayer.BestStreak = GameInfo.CurrentPlayer.CurrentStreak;
            }
            //update hardestword and score
            if (GameInfo.WordScore > GameInfo.CurrentPlayer.HardestWordScore)
            {
                GameInfo.CurrentPlayer.HardestWordScore = GameInfo.WordScore;
                GameInfo.CurrentPlayer.HardestWord = GameInfo.GameWord;
                GameInfo.CurrentPlayer.HardestWordDate=DateTime.Today;
            }
            //update best streak if necessary


            myDbManager.db.Update(GameInfo.CurrentPlayer);
        }

        public void UpdatePlayerOnLoss()
        {
            //update lastplayeddate
            GameInfo.CurrentPlayer.LastPlayedDate = DateTime.Today;
            //close beststreak
            //close currentstreak
            if (GameInfo.CurrentPlayer.BestStreak < GameInfo.CurrentPlayer.CurrentStreak)
            {
                GameInfo.CurrentPlayer.BestStreak = GameInfo.CurrentPlayer.CurrentStreak;
            }
            GameInfo.CurrentPlayer.CurrentStreak = 0;
            myDbManager.db.Update(GameInfo.CurrentPlayer);
        }

        public bool IsWordWorthMore()
        {
            if (GameInfo.WordScore > GameInfo.CurrentPlayer.HardestWordScore)
            {
                return true;
            }
            return false;
        }

        /*
          
         any
         update lastplayeddate


            win




            lost




             public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public DateTime LastPlayedDate { get; set; }
        public int BestStreak { get; set; }
        public int BestScore { get; set; }
        public DateTime BestScoreDate { get; set; }
        public string HardestWord { get; set; }
        public int HardestWordScore { get; set; }
        public DateTime HardestWordDate { get; set; }
        public int CurrentStreak { get; set; }
             
             */





        private void DisplayCharArrayOnScreen()
        {
           // Log.Debug(logTag, "DisplayCharArrayOnScreen");
            GameInfo.DisplayString = "";
            foreach (char letter in GameInfo.DisplayCharArray)
            {
                GameInfo.DisplayString += letter + " ";
            }

            TxtEndGameWord.Text = GameInfo.DisplayString;
        }
        private void SetDisplayString(bool wonStatus)
        {
            GameInfo.DisplayString="";
            
            string FontRed = "<font color='#ff3333'>";
            string FontGreen = "<font color='#00cc00'>";
            string EndFont = "</font>";

            int counter = 0;
           switch (wonStatus)
            {
                case false:
                foreach (char letter in GameInfo.DisplayCharArray)
                {
                    if (letter == '_')
                    {
                        GameInfo.DisplayString += FontRed + GameInfo.GameWordCharArray[counter] + EndFont + " ";
                    }
                    else GameInfo.DisplayString += letter + " ";
                    counter++;
                }
                    break;
                case true:
                    GameInfo.DisplayString += FontGreen;
                    foreach (char letter in GameInfo.GameWordCharArray)
                    {
                        GameInfo.DisplayString += letter + " ";
                    }
                    GameInfo.DisplayString += EndFont;
                    break;

            }
        



        }
    }
}

