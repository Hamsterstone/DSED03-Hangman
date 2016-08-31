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
        private Button btnDefinition;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EndScreen);
            TxtEndGameStatus = FindViewById<TextView>(Resource.Id.txtEndGameStatus);
            TxtEndGameWord = FindViewById<TextView>(Resource.Id.txtEndGameWord);
            ImgEndGame = FindViewById<ImageView>(Resource.Id.imgEndGame);
            ImgEndGame.SetImageResource(GameInfo.ImageLookup[GameInfo.Attempt]);
            btnPlayAgain = FindViewById<Button>(Resource.Id.btnEndGamePlayAgain);
            btnQuit = FindViewById<Button>(Resource.Id.btnEndGameQuit);
            btnDefinition = FindViewById<Button>(Resource.Id.btnDefinition);
            btnPlayAgain.Click += BtnPlayAgainClick;
            btnQuit.Click += BtnQuitClick;
            btnDefinition.Click += BtnDefinitionClick;
            myDbManager = new DatabaseManager();
            // Did the activity enter on a won or lost game
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

        private void BtnDefinitionClick(object sender, EventArgs e)
        {
            //todo Set up Definition button with API call.
            Toast.MakeText(this, "Button not yet implimented", ToastLength.Long).Show();
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
            
        }

        private void GameWon()
        {
            SetDisplayString(true);
            TxtEndGameStatus.Text = "You Won. Word score: " + GameInfo.WordScore;
            UpdatePlayerOnWin();
            
        }

        public void UpdatePlayerOnWin()
        {
            //  update lastplayeddate
            GameInfo.CurrentPlayer.LastPlayedDate=DateTime.Today;
            //add to wordscore to currentstreak
            GameInfo.CurrentPlayer.CurrentStreak += GameInfo.WordScore;

            //update beststreak if currentstreak is bigger
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
            

            //save the updated info to the database
            myDbManager.db.Update(GameInfo.CurrentPlayer);
        }

        public void UpdatePlayerOnLoss()
        {
            //update lastplayeddate
            GameInfo.CurrentPlayer.LastPlayedDate = DateTime.Today;
            //close beststreak
            
            if (GameInfo.CurrentPlayer.BestStreak < GameInfo.CurrentPlayer.CurrentStreak)
            {
                GameInfo.CurrentPlayer.BestStreak = GameInfo.CurrentPlayer.CurrentStreak;
            }
            //close currentstreak
            GameInfo.CurrentPlayer.CurrentStreak = 0;
            //save the updated info to the database
            myDbManager.db.Update(GameInfo.CurrentPlayer);
        }

       
        private void SetDisplayString(bool wonStatus)
        {
            GameInfo.DisplayString="";
            //Sets up HTML font encoding inserts
            string FontRed = "<font color='#ff3333'>";
            string FontGreen = "<font color='#00cc00'>";
            string EndFont = "</font>";

            int counter = 0;
            switch (wonStatus)
            {
                case false:
                    foreach (char letter in GameInfo.DisplayCharArray)
                    {
                        //Sets all unguessed letters to red for display
                        if (letter == '_')
                        {
                            GameInfo.DisplayString += FontRed + GameInfo.GameWordCharArray[counter] + EndFont + " ";
                        }
                        else GameInfo.DisplayString += letter + " ";
                        counter++;
                    }
                    break;
                case true:
                    //Sets entire word to green
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

