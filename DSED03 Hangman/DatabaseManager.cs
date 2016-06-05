using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;

using Android.Runtime;
using Android.Views;
using Android.Widget;

using SQLite;
using Environment = Android.OS.Environment;

namespace DSED03_Hangman
{
    public class Player
    {
        [PrimaryKey, AutoIncrement]
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
        public Player() { }
    }
    class DatabaseManager
    {
        public SQLiteConnection db { get; set; }

        public DatabaseManager()
        {
            string databaseFileName = "PlayerDatabase.sqlite";
            string databasePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), databaseFileName);
            db = new SQLiteConnection(databasePath);
            db.CreateTable<Player>();

            if (db.Table<Player>().Count() == 0)
            {
                PopulateDatabase();
            }
        }

        public List<Player> ViewAll()
        {
            try
            {
               return db.Query<Player>("SELECT * FROM Player ORDER BY PlayerName");
            }catch (Exception e)
            {
                Console.WriteLine("Error:"+e.Message);
                return null;
            }
        }

        internal void AddPlayer(string playerName)
        {
            Player newPlayer = new Player();
            newPlayer.PlayerName = playerName;
            
            db.Insert(newPlayer);
        }
       
        void UpdatePlayerData()
        {
            
        }
        private void PopulateDatabase()
        {
            Player newPlayer = new Player();
            newPlayer.PlayerName = "Hamster";
            newPlayer.BestScore = 0;
            newPlayer.HardestWord = "Fandangle";
            newPlayer.HardestWordScore = 14;
            newPlayer.BestScoreDate = new DateTime(2015, 4, 1);
            newPlayer.HardestWordDate = new DateTime(2015, 3, 1);
            newPlayer.LastPlayedDate = new DateTime(2015, 5, 1);
            newPlayer.BestStreak = 5;
            newPlayer.CurrentStreak = 3;

            db.Insert(newPlayer);

            newPlayer = new Player();
            newPlayer.PlayerName = "Bob";
            newPlayer.BestScore = 0;

            db.Insert(newPlayer);

            newPlayer = new Player();
            newPlayer.PlayerName = "Pete";
            newPlayer.BestScore = 0;

            db.Insert(newPlayer);

            newPlayer = new Player();
            newPlayer.PlayerName = "Jane";
            newPlayer.BestScore = 0;

            db.Insert(newPlayer);
        }

    }
}