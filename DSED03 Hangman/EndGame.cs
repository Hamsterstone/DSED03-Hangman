using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DSED03_Hangman
{
    [Activity(Label = "EndGame")]
    public class EndGame : Activity
    {
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
            // DisplayCharArrayOnScreen();
        }

        private void GameWon()
        {
            SetDisplayString(true);
            TxtEndGameStatus.Text = "You Won. Word score: " + GameInfo.WordScore;
            //DisplayCharArrayOnScreen();
        }

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

