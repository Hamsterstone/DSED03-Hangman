using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DSED03_Hangman
{
    [Activity(Label = "Gameplay")]
    public class Gameplay : Activity
    {
        private static string logTag = "aaaaaLogTag";
        
        public TextView TxtDisplay;
        public ImageView ImgHangman;
      
        


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GameScreen);
            InitializeButtons();
            TxtDisplay= FindViewById<TextView>(Resource.Id.txtDisplay);
            ImgHangman = FindViewById<ImageView>(Resource.Id.imgHangman);
            GenerateWordList();
            PickRandomWord();
            SetGameWordCharArray();
            CreateDisplayCharArray();
            DisplayCharArrayOnScreen();
            SetWordScore();
           GameInfo.Attempt = 1;
            ImgHangman.SetImageResource(GameInfo.ImageLookup[GameInfo.Attempt]);
           // ImgHangman.Background=Resources.GetDrawable(Resource.Drawable)       ImageLookup[Attempt]
        }
        private void InitializeButtons()
        {
            Button btnA = FindViewById<Button>(Resource.Id.btnA);
            Button btnB = FindViewById<Button>(Resource.Id.btnB);
            Button btnC = FindViewById<Button>(Resource.Id.btnC);
            Button btnD = FindViewById<Button>(Resource.Id.btnD);
            Button btnE = FindViewById<Button>(Resource.Id.btnE);
            Button btnF = FindViewById<Button>(Resource.Id.btnF);
            Button btnG = FindViewById<Button>(Resource.Id.btnG);
            Button btnH = FindViewById<Button>(Resource.Id.btnH);
            Button btnI = FindViewById<Button>(Resource.Id.btnI);
            Button btnJ = FindViewById<Button>(Resource.Id.btnJ);
            Button btnK = FindViewById<Button>(Resource.Id.btnK);
            Button btnL = FindViewById<Button>(Resource.Id.btnL);
            Button btnM = FindViewById<Button>(Resource.Id.btnM);
            Button btnN = FindViewById<Button>(Resource.Id.btnN);
            Button btnO = FindViewById<Button>(Resource.Id.btnO);
            Button btnP = FindViewById<Button>(Resource.Id.btnP);
            Button btnQ = FindViewById<Button>(Resource.Id.btnQ);
            Button btnR = FindViewById<Button>(Resource.Id.btnR);
            Button btnS = FindViewById<Button>(Resource.Id.btnS);
            Button btnT = FindViewById<Button>(Resource.Id.btnT);
            Button btnU = FindViewById<Button>(Resource.Id.btnU);
            Button btnV = FindViewById<Button>(Resource.Id.btnV);
            Button btnW = FindViewById<Button>(Resource.Id.btnW);
            Button btnX = FindViewById<Button>(Resource.Id.btnX);
            Button btnY = FindViewById<Button>(Resource.Id.btnY);
            Button btnZ = FindViewById<Button>(Resource.Id.btnZ);
            btnA.Click += OnButtonClick;
            btnB.Click += OnButtonClick;
            btnC.Click += OnButtonClick;
            btnD.Click += OnButtonClick;
            btnE.Click += OnButtonClick;
            btnF.Click += OnButtonClick;
            btnG.Click += OnButtonClick;
            btnH.Click += OnButtonClick;
            btnI.Click += OnButtonClick;
            btnJ.Click += OnButtonClick;
            btnK.Click += OnButtonClick;
            btnL.Click += OnButtonClick;
            btnM.Click += OnButtonClick;
            btnN.Click += OnButtonClick;
            btnO.Click += OnButtonClick;
            btnP.Click += OnButtonClick;
            btnQ.Click += OnButtonClick;
            btnR.Click += OnButtonClick;
            btnS.Click += OnButtonClick;
            btnT.Click += OnButtonClick;
            btnU.Click += OnButtonClick;
            btnV.Click += OnButtonClick;
            btnW.Click += OnButtonClick;
            btnX.Click += OnButtonClick;
            btnY.Click += OnButtonClick;
            btnZ.Click += OnButtonClick;
        }

        public int LetterScoreValue(char letter)
        {
            Log.Debug(logTag, "LetterScoreValue");
            switch (letter)
            {
                case 'A':case 'E': case 'I': case 'O': case 'U': case 'L': case 'N': case 'S': case 'T': case 'R':
                    return 1;
                 
                case 'D': case 'G':
                    return 2;
                  
                case 'B': case 'C': case 'M': case 'P':
                    return 3;
                 
                case 'F': case 'H': case 'V': case 'W': case 'Y':
                    return 4;
                  
                case 'K':
                    return 5;
                 
                case 'J': case 'X':
                    return 8;
                  
                case 'Q': case 'Z':
                    return 10;
                   

            }
            Log.Debug(logTag, "Something went wrong in LetterScoreValue, should never get here");
            return 0;
        }

        public void OnButtonClick(object sender, EventArgs e)
        {
            Log.Debug(logTag, "OnButtonClick");
            Button fakeButton = sender as Button;
            fakeButton.Enabled = false;
            CheckLetter(Convert.ToChar(fakeButton.Text.ToUpper()));
            DisplayCharArrayOnScreen();
            ImgHangman.SetImageResource(GameInfo.ImageLookup[GameInfo.Attempt]);
            TestForEndGame();
        }
        private void TestForEndGame() {
            Log.Debug(logTag, "TestForEndGame");
            if (IsTooManyGuesses())
            {
                GameInfo.WordGuessed = false;
                StartActivity(typeof(EndGame));
                Finish();
            }
            if (HasGuessedWord())
            {
                GameInfo.WordGuessed = true;
                StartActivity(typeof(EndGame));
                Finish();
            }
        }

       
        public void GenerateWordList()
        {
            
            Log.Debug(logTag, "GenerateWordList");
            if (GameInfo.WordList.Count == 0)
            {
                int counter = 0;
                try
                {
                    var assets = Assets;
                    using (StreamReader streamReader = new StreamReader(assets.Open("WordList.txt")))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            string dictionaryLine = streamReader.ReadLine();


                            counter++;
                            GameInfo.WordList.Add(dictionaryLine);
                            Log.Debug(logTag, "WordDone " + counter);

                        }
                    }
                }
                catch (Exception)
                {
                    Toast.MakeText(this, "Word list didn't load", ToastLength.Short).Show();
                }
            }
        }

        private void PickRandomWord()
        {
            Log.Debug(logTag, "PickRandomWord");
            GameInfo.GameWord = GameInfo.WordList[RandomNumberGenerator(GameInfo.WordList.Count)];

           
        }
        private int RandomNumberGenerator(int maxNumber)
        {
            Log.Debug(logTag, "RandomNumberGenerator");
            int rndNum;
            Random rndCompChoice = new Random(Guid.NewGuid().GetHashCode());
            rndNum = rndCompChoice.Next(1, maxNumber);
            return rndNum;
        }

        private char[] CreateWordCharArray(string word)
        {
            Log.Debug(logTag, "CreateWordCharArray");
            return word.ToCharArray();
        }

        private void SetGameWordCharArray()
        {
            Log.Debug(logTag, "SetGameWordCharArray");
            GameInfo.GameWordCharArray = GameInfo.GameWord.ToCharArray();
               // CreateWordCharArray(gameWord);
        }

        private void CreateDisplayCharArray()
        {
            Log.Debug(logTag, "CreateDisplayCharArray");
            GameInfo.DisplayCharArray = GameInfo.GameWord.ToCharArray();
            for (int i = 0; i < GameInfo.DisplayCharArray.Length; i++)
            {
                if (GameInfo.DisplayCharArray[i] != '-')
                {
                    GameInfo.DisplayCharArray[i] = '_';
                }
            }
        }

        private void DisplayCharArrayOnScreen()
        {Log.Debug(logTag, "DisplayCharArrayOnScreen");
             GameInfo.DisplayString="";
            foreach (char letter in GameInfo.DisplayCharArray)
            {
                GameInfo.DisplayString += letter+" ";
            }
            
            TxtDisplay.Text = GameInfo.DisplayString;
        }

        private void CheckLetter(char testLetter)
        {
            if (IsLetterInWord(testLetter))
            {
                ChangeDisplay(testLetter);
            }
            else GameInfo.Attempt++;
        }
        private bool IsLetterInWord(char testLetter)
        {
            Log.Debug(logTag, "CheckLetter");
            int counter = 0;
            foreach (char wordLetter in GameInfo.GameWordCharArray)
            {
                if (testLetter == wordLetter)
                {
                    return true;

                }
                counter++;
            }
            return false;
        }

        private void ChangeDisplay(char letter)
        {
            int counter = 0;
            foreach (char wordLetter in GameInfo.GameWordCharArray)
            {
                if (letter == wordLetter)
                {
                    GameInfo.DisplayCharArray[counter] = wordLetter;
                }
                counter++;
            }
        }

        private bool IsTooManyGuesses()
        {
            if (GameInfo.Attempt == 14)
            {
                return true;
            }
            return false;
        }

        private bool HasGuessedWord() 
        {
            foreach (char letter in GameInfo.DisplayCharArray)
            {
                if (letter == '_')
                {
                    return false;
                }
                
            }
            return true;
        }
        private void EndGameFail()
        {
            GameInfo.WordGuessed = false;
            StartActivity(typeof(EndGame));
            //TODO new screen with hung man, play again y/n buttons. no goes to start screen, Yes to new instance.
        }

        private void EndGameSuccess()
        {
            GameInfo.WordGuessed = true;
            StartActivity(typeof(EndGame));
            //todo new screen (with safe man?) play again y/n. add WordScore() to TotalScore
        }
        public void SetWordScore()
        {
            GameInfo.WordScore=0;
            foreach (char letter in GameInfo.GameWord)
            {
                GameInfo.WordScore += LetterScoreValue(letter);
            }
           
        }

    }
}

