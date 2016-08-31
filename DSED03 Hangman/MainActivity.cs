using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Views.InputMethods;
using Javax.Security.Auth;

namespace DSED03_Hangman
{
    [Activity(Label = "Hangman", MainLauncher = true, Icon = "@drawable/HangmanIcon",ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden)]
    public class MainActivity : Activity
    {
       
        public List<Player> playerList;
        
        private List<Player> updatedTableItems;
        private TextView txtName;
        private ListView playersListView;
        private DatabaseManager myDbManager;
        private CheckBox cbxEasy;
        private CheckBox cbxMedium;
        private CheckBox cbxHard;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Button btnPlay = FindViewById<Button>(Resource.Id.btnPlay);
            btnPlay.Click += BtnPlay_Click;
            Button btnAddPlayer = FindViewById<Button>(Resource.Id.btnAddPlayer);
            btnAddPlayer.Click += BtnAddPlayer_Click;
            txtName = FindViewById<TextView>(Resource.Id.txtName);
            cbxEasy = FindViewById<CheckBox>(Resource.Id.cbxEasy);
            cbxMedium = FindViewById<CheckBox>(Resource.Id.cbxMedium);
            cbxHard = FindViewById<CheckBox>(Resource.Id.cbxHard);
            playersListView = FindViewById<ListView>(Resource.Id.listView1);
            btnAddPlayer.LongClick += BtnAddPlayer_LongClick;
            playersListView.ItemClick += OnListItemClick;
            Log.Debug(GameInfo.logTag, "myDbManager");
            myDbManager=new DatabaseManager();
            Log.Debug(GameInfo.logTag, "playerList");
            
            UpdatePlayerList();
            updatedTableItems =playerList;



            PlayerDataAdapter myDataAdapter=new PlayerDataAdapter(this,playerList);
            playersListView.Adapter=myDataAdapter;
           
            txtName.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                // filter Playerlist on text changed
                Log.Debug(GameInfo.logTag, "TextChanged");
                var searchTerm = txtName.Text;
                 updatedTableItems = playerList.Where(player=>player.PlayerName.ToLower().Contains(searchTerm.ToLower())
                ).ToList();
                var filteredResultsAdapter = new PlayerDataAdapter(this, updatedTableItems);
                playersListView.Adapter = filteredResultsAdapter;
            };



        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdatePlayerList();
            txtName.Text = "";
            
        }

        public void HideKeyboard()
        {
            InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            var currentFocus = CurrentFocus;
            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
        }
        
        void UpdatePlayerList()
        {
            playerList = myDbManager.ViewAllSortByName();
            PlayerDataAdapter myDataAdapter = new PlayerDataAdapter(this, playerList);
            playersListView.Adapter = myDataAdapter;
        }
        private void BtnAddPlayer_LongClick(object sender, View.LongClickEventArgs e)
        {
            StartActivity(typeof(DatabaseWorker));
        }

        private void BtnAddPlayer_Click(object sender, EventArgs e)
        {
            //Check there is a name to add
            if (txtName.Text != "")
            {
                string playerName = txtName.Text;
                //Add the name
                myDbManager.AddPlayer(playerName);
                //Update the displayed players
                UpdatePlayerList();
                txtName.Text = "";
            }
            else
            {
                Toast.MakeText(this, "Enter a player name to add", ToastLength.Long).Show();
            }
        }

        public void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            //set CurrentPlayer to the selected player
            GameInfo.CurrentPlayer = updatedTableItems[e.Position];
            txtName.Text = GameInfo.CurrentPlayer.PlayerName;
            HideKeyboard();
        }



        private void BtnPlay_Click(object sender, EventArgs e)
        {

            
            if (cbxEasy.Checked == false && cbxMedium.Checked == false && cbxHard.Checked == false)
            {
                Toast.MakeText(this, "Select Difficulty", ToastLength.Long).Show();
            }else if (GameInfo.CurrentPlayer==null || GameInfo.CurrentPlayer.PlayerName != txtName.Text)
            {
                Toast.MakeText(this, "Select Player", ToastLength.Long).Show();
            }
            else
            {
                //    //Clears Wordlist on difficulty change so it will be rebuilt.
                //    if (GameInfo.Easy != cbxEasy.Checked || GameInfo.Medium != cbxMedium.Checked ||
                //        GameInfo.Hard != cbxHard.Checked)
                {
                    GameInfo.Easy = cbxEasy.Checked;
                    GameInfo.Medium = cbxMedium.Checked;
                    GameInfo.Hard = cbxHard.Checked;
                    //        GameInfo.WordList.Clear();
                }
                StartActivity(typeof (Gameplay));
            }
        }


    }

    //Listview adapter for player data
    public class PlayerDataAdapter : BaseAdapter<Player>
    {
        private readonly Activity context;
        private readonly List<Player> items; 
        public PlayerDataAdapter(Activity context, List<Player> items)
        {
            Log.Debug(GameInfo.logTag, "PlayerDataAdapter Constructor");
            this.context = context;
            this.items = items;
        }
        public override Player this[int position]
        {
            get
            {
                Log.Debug(GameInfo.logTag, "PlayerDataAdapter Position Override");
                return items[position] ;
            }
        }

        public override int Count
        {
            get
            {
                Log.Debug(GameInfo.logTag, "PlayerDataAdapter Count Override");
                return items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            Log.Debug(GameInfo.logTag, "PlayerDataAdapter GetItemId Override");
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Log.Debug(GameInfo.logTag, "GetView");
            var item = items[position];
            var view = convertView;
            if (view == null)

            {
              view=  context.LayoutInflater.Inflate(Resource.Layout.ListViewAdapter, null);
            }
                view.FindViewById<TextView>(Resource.Id.lblName).Text= item.PlayerName;
                view.FindViewById<TextView>(Resource.Id.lblLastPlayed).Text = "LastPlayed: "+item.LastPlayedDate.ToShortDateString();
                view.FindViewById<TextView>(Resource.Id.lblHardestWord).Text = item.HardestWord;
                view.FindViewById<TextView>(Resource.Id.lblHardestWordScore).Text = "Score: " + item.HardestWordScore;
                view.FindViewById<TextView>(Resource.Id.lblHardestWordDate).Text = item.HardestWordDate.ToShortDateString();
                view.FindViewById<TextView>(Resource.Id.lblBestStreak).Text = "Best: " + item.BestStreak;
                view.FindViewById<TextView>(Resource.Id.lblCurrentStreak).Text = "Streak: Current: " + item.CurrentStreak;

            
            return view;
        }
        
    }

}
