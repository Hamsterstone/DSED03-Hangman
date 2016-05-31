using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DSED03_Hangman
{
    static class GameInfo
    {
        public static List<string> WordList = new List<string>();
        public static string GameWord { get; set; }
        public static char[] GameWordCharArray { get; set; }
        public static char[] DisplayCharArray { get; set; }
        public static int Attempt { get; set; }
        public static bool WordGuessed { get; set; }
        public static string DisplayString { get; set; }
        public static int WordScore { get; set; }
        public static Player CurrentPlayer { get; set; }

        public static Dictionary<int, int> ImageLookup = new Dictionary<int, int>()
        {
            {1, Resource.Drawable.HangmanMine01},{2, Resource.Drawable.HangmanMine02},{3, Resource.Drawable.HangmanMine03},{4, Resource.Drawable.HangmanMine04},{5, Resource.Drawable.HangmanMine05},{6, Resource.Drawable.HangmanMine06},{7, Resource.Drawable.HangmanMine07},{8, Resource.Drawable.HangmanMine08},{9, Resource.Drawable.HangmanMine09},{10, Resource.Drawable.HangmanMine10},{11, Resource.Drawable.HangmanMine11},{12, Resource.Drawable.HangmanMine12},{13, Resource.Drawable.HangmanMine13},{14, Resource.Drawable.HangmanMine14}
        };
    }

    public class Player
    {
         public  int ID { get; set; }
         public string PlayerName { get; set; }
         public DateTime LastPlayedDate { get; set; }
         public int BestStreak { get; set; }
         public int BestScore { get; set; }
         public DateTime BestScoreDate { get; set; }
         public string HardestWord { get; set; }
         public int HardestWordScore { get; set; }
         public DateTime HardestWordDate { get; set; }
         public int CurrentStreak { get; set; }
    }

   
}