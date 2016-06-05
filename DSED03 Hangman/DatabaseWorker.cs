using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace DSED03_Hangman
{
    [Activity(Label = "DatabaseWorker")]
    public class DatabaseWorker : Activity
    {
        public List<Player> playerList;
        private DatabaseManager myDbManager;
        private TextView lblDbName;
        private TextView lblDbLastPlayedDate;
        private TextView lblDbHardWordLabel;
        private TextView lblDbHardestWord;
        private TextView lblDbHardestWordDate;
        private TextView lblDbHardestWordScore;
        private TextView lblDbBestScoreLabel;
        private TextView lblDbBestScoreDate;
        private TextView lblDbBestScore;
        private TextView lblDbBestStreak;
        private TextView lblDbCurrentStreak;
        private EditText txtDbName;
        private ListView listViewDb1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DatabaseWorker);
            lblDbName = FindViewById<TextView>(Resource.Id.lblDbName);
            lblDbLastPlayedDate = FindViewById<TextView>(Resource.Id.lblDbLastPlayedDate);
            lblDbHardWordLabel = FindViewById<TextView>(Resource.Id.lblDbHardWordLabel);
            lblDbHardestWord = FindViewById<TextView>(Resource.Id.lblDbHardestWord);
            lblDbHardestWordDate = FindViewById<TextView>(Resource.Id.lblDbHardestWordDate);
            lblDbHardestWordScore = FindViewById<TextView>(Resource.Id.lblDbHardestWordScore);
            lblDbBestScoreLabel = FindViewById<TextView>(Resource.Id.lblDbBestScoreLabel);
            lblDbBestScoreDate = FindViewById<TextView>(Resource.Id.lblDbBestScoreDate);
            lblDbBestScore = FindViewById<TextView>(Resource.Id.lblDbBestScore);
            lblDbBestStreak = FindViewById<TextView>(Resource.Id.lblDbBestStreak);
            lblDbCurrentStreak = FindViewById<TextView>(Resource.Id.lblDbCurrentStreak);
            txtDbName = FindViewById<EditText>(Resource.Id.txtDbName);
            listViewDb1 = FindViewById<ListView>(Resource.Id.listViewDb1);
            listViewDb1.ItemClick += ListViewDb1_ItemClick;
            Button btnDbUpdate = FindViewById<Button>(Resource.Id.btnDbUpdate);
            btnDbUpdate.Click += BtnDbUpdate_Click;
            Button btnDbDelete = FindViewById<Button>(Resource.Id.btnDbDelete);
            btnDbDelete.Click += BtnDbDelete_Click;
            //Log.Debug(GameInfo.logTag, "myDbManager");
            myDbManager = new DatabaseManager();
            // Log.Debug(GameInfo.logTag, "playerList");

            UpdatePlayerList();
            PlayerDataAdapter myDataAdapter = new PlayerDataAdapter(this, playerList);
            listViewDb1.Adapter = myDataAdapter;
        }

        void UpdatePlayerList()
        {
            playerList = myDbManager.ViewAll();
        }

        private void ListViewDb1_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            Player myPlayer = playerList[e.Position];
            GameInfo.CurrentPlayer = myPlayer;
            txtDbName.Text = myPlayer.PlayerName;
            lblDbLastPlayedDate.Text = "Last Played: " + myPlayer.LastPlayedDate.ToShortDateString();
            lblDbHardestWord.Text = myPlayer.HardestWord;
            lblDbHardestWordDate.Text = myPlayer.HardestWordDate.ToShortDateString();
            lblDbHardestWordScore.Text = "Score: " + myPlayer.HardestWordScore.ToString();
            lblDbBestScoreDate.Text = "Date: " + myPlayer.BestScoreDate.ToShortDateString();
            lblDbBestScore.Text = myPlayer.BestScore.ToString();
            lblDbBestStreak.Text = "Best Streak: " + myPlayer.BestStreak.ToString();
            lblDbCurrentStreak.Text = "Current Streak: " + myPlayer.CurrentStreak.ToString();
            
            txtDbName.ClearFocus();

            InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            var currentFocus = CurrentFocus;
            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
        }

        private void BtnDbDelete_Click(object sender, EventArgs e)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage("Delete Player?");
            builder.SetPositiveButton("OK", (s, ev) =>
            {
                /* do something on OK click */
                myDbManager.db.Delete<Player>(GameInfo.CurrentPlayer.PlayerId);
            });
            builder.SetNegativeButton("Cancel", (s, ev) =>
            {
                /* do something on Cancel click */
            });
            builder.Create().Show();

           UpdatePlayerList();

        }

        private void BtnDbUpdate_Click(object sender, EventArgs e)
        {
            GameInfo.CurrentPlayer.PlayerName = txtDbName.Text;
            myDbManager.db.Update(GameInfo.CurrentPlayer);
        UpdatePlayerList();
        }
    }
}