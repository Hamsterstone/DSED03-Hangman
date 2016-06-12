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
using SQLite;

namespace DSED03_Hangman
{
    //Static class used to pass information between the different activities
    static class GameInfo
    {
        public static string logTag = "aaaaaLogTag";
        public static List<string> WordList = new List<string>();
        public static string GameWord { get; set; }
        public static char[] GameWordCharArray { get; set; }
        public static char[] DisplayCharArray { get; set; }
        public static int Attempt { get; set; }
        public static bool WordGuessed { get; set; }
        public static string DisplayString { get; set; }
        public static int WordScore { get; set; }
        public static Player CurrentPlayer { get; set; }
        public static bool Easy { get; set; }
        public static bool Medium { get; set; }
        public static bool Hard { get; set; }
        public static int EasyWordScoreCap { get; set; } = 10;
        public static int MediumWordScoreCap { get; set; } = 16;
        public static Dictionary<int, int> ImageLookup = new Dictionary<int, int>()
        {
            {1, Resource.Drawable.HangmanMine01},{2, Resource.Drawable.HangmanMine02},{3, Resource.Drawable.HangmanMine03},{4, Resource.Drawable.HangmanMine04},{5, Resource.Drawable.HangmanMine05},{6, Resource.Drawable.HangmanMine06},{7, Resource.Drawable.HangmanMine07},{8, Resource.Drawable.HangmanMine08},{9, Resource.Drawable.HangmanMine09},{10, Resource.Drawable.HangmanMine10},{11, Resource.Drawable.HangmanMine11},{12, Resource.Drawable.HangmanMine12},{13, Resource.Drawable.HangmanMine13},{14, Resource.Drawable.HangmanMine14}
        };
    }

   

   
}