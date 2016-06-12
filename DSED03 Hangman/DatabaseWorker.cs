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
    [Activity(Label = "Player Management", WindowSoftInputMode = SoftInput.StateHidden)]
    public class DatabaseWorker : Activity
    {

        public List<Player> dbPlayerList;
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
           // PlayerDataAdapter myDataAdapter = new PlayerDataAdapter(this, dbPlayerList);
            //listViewDb1.Adapter = myDataAdapter;
        }
        public void HideKeyboard()
        {
            //txtDbName.ClearFocus();
            InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            var currentFocus = CurrentFocus;
            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
        }
        void UpdatePlayerList()
        {
            dbPlayerList = myDbManager.ViewAllSortByName();
            PlayerDataAdapter myDataAdapter = new PlayerDataAdapter(this, dbPlayerList);
            listViewDb1.Adapter = myDataAdapter;
        }

        private void ListViewDb1_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Log.Debug(GameInfo.logTag, "ListItemClick");
            var listView = sender as ListView;
            GameInfo.CurrentPlayer = dbPlayerList[e.Position];
            UpdateDisplayFields();
            HideKeyboard();
            Log.Debug(GameInfo.logTag, "CurrentPlayer Changed to " + GameInfo.CurrentPlayer.PlayerName);

        }

        private void UpdateDisplayFields() {
            if (GameInfo.CurrentPlayer != null)
            {
                txtDbName.Text = GameInfo.CurrentPlayer.PlayerName;
                lblDbLastPlayedDate.Text = "Last Played: " + GameInfo.CurrentPlayer.LastPlayedDate.ToShortDateString();
                lblDbHardestWord.Text = GameInfo.CurrentPlayer.HardestWord;
                lblDbHardestWordDate.Text = GameInfo.CurrentPlayer.HardestWordDate.ToShortDateString();
                lblDbHardestWordScore.Text = "Score: " + GameInfo.CurrentPlayer.HardestWordScore.ToString();
                lblDbBestScoreDate.Text = "Date: " + GameInfo.CurrentPlayer.BestScoreDate.ToShortDateString();
                lblDbBestScore.Text = GameInfo.CurrentPlayer.BestScore.ToString();
                lblDbBestStreak.Text = "Best Streak: " + GameInfo.CurrentPlayer.BestStreak.ToString();
                lblDbCurrentStreak.Text = "Current Streak: " + GameInfo.CurrentPlayer.CurrentStreak.ToString();
            }
            else
            {
                txtDbName.Text = "";
                lblDbLastPlayedDate.Text = "Last Played: ";
                lblDbHardestWord.Text = "";
                lblDbHardestWordDate.Text = "";
                lblDbHardestWordScore.Text = "Score: " ;
                lblDbBestScoreDate.Text = "Date: " ;
                lblDbBestScore.Text = "";
                lblDbBestStreak.Text = "Best Streak: ";
                lblDbCurrentStreak.Text = "Current Streak: " ;
            }
            
        }

        private void BtnDbDelete_Click(object sender, EventArgs e)
        {
            //Creates a popup OK/Cancel dialog to confirm player delete
            //NOTE: AlertDialog is async, place all needed calls inside button events or they will run too early.
            if (GameInfo.CurrentPlayer != null)
            {
                var builder = new AlertDialog.Builder(this);
                string deletePlayerString = "Delete " + GameInfo.CurrentPlayer.PlayerName + "?";
                builder.SetMessage(deletePlayerString);
                builder.SetPositiveButton("OK", (s, ev) =>
                {
                    /* do something on OK click */
                    myDbManager.db.Delete<Player>(GameInfo.CurrentPlayer.PlayerId);
                    //Reset the current player to null to avoid trying to delete a Player twice
                    GameInfo.CurrentPlayer = null;
                    UpdatePlayerList();
                    UpdateDisplayFields();
                });
                builder.SetNegativeButton("Cancel", (s, ev) =>
                {
                    /* do something on Cancel click */
                });
                builder.Create().Show();
               
            }
            else
            {
                Toast.MakeText(this, "No Player Selected", ToastLength.Long).Show();
            }

        }

        private void BtnDbUpdate_Click(object sender, EventArgs e)
        {
            GameInfo.CurrentPlayer.PlayerName = txtDbName.Text;
            myDbManager.db.Update(GameInfo.CurrentPlayer);
            UpdatePlayerList();
        }
    }
}