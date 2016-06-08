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
    [Activity(Label = "DSED03_Hangman", MainLauncher = true, Icon = "@drawable/icon",ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden)]
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
            cbxEasy.CheckedChange += CbxCheckChanged;
            cbxMedium.CheckedChange += CbxCheckChanged;
            cbxHard.CheckedChange += CbxCheckChanged;
            btnAddPlayer.LongClick += BtnAddPlayer_LongClick;

            playersListView.ItemClick += OnListItemClick;
            Log.Debug(GameInfo.logTag, "myDbManager");
            myDbManager=new DatabaseManager();
            Log.Debug(GameInfo.logTag, "playerList");
            
            UpdatePlayerList();
            updatedTableItems =playerList;



            PlayerDataAdapter myDataAdapter=new PlayerDataAdapter(this,playerList);
            playersListView.Adapter=myDataAdapter;
           // HideKeyboard();

            txtName.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                // filter on text changed
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
            //HideKeyboard();
           //txtName.Text = GameInfo.CurrentPlayer.PlayerName;
            //InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            //var currentFocus = CurrentFocus;
            //inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
        }

        public void HideKeyboard()
        {
            //txtName.ClearFocus();
            InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            var currentFocus = CurrentFocus;
            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
        }
        private void CbxCheckChanged(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            GameInfo.WordList.Clear();
        }

        void UpdatePlayerList()
        {
            playerList = myDbManager.ViewAll();
            PlayerDataAdapter myDataAdapter = new PlayerDataAdapter(this, playerList);
            playersListView.Adapter = myDataAdapter;
        }
        private void BtnAddPlayer_LongClick(object sender, View.LongClickEventArgs e)
        {
            StartActivity(typeof(DatabaseWorker));
        }

        private void BtnAddPlayer_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "")
            {
                string playerName = txtName.Text;
                myDbManager.AddPlayer(playerName);
                //GameInfo.CurrentPlayer=
                UpdatePlayerList();
                txtName.Text = "";
                //txtName.Text = GameInfo.CurrentPlayer.PlayerName;
            }
            else
            {
                Toast.MakeText(this, "Enter a player name to add", ToastLength.Long).Show();
            }
        }

        public void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            GameInfo.CurrentPlayer = updatedTableItems[e.Position];
            txtName.Text = GameInfo.CurrentPlayer.PlayerName;
            HideKeyboard();
            //txtName.ClearFocus();
            //InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            //var currentFocus = CurrentFocus;
            //inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
        }



        private void BtnPlay_Click(object sender, EventArgs e)
        {

            GameInfo.Easy = cbxEasy.Checked;
            GameInfo.Medium = cbxMedium.Checked;
            GameInfo.Hard = cbxHard.Checked;
            if (cbxEasy.Checked == false && cbxMedium.Checked == false && cbxHard.Checked == false)
            {
                Toast.MakeText(this, "Select Difficulty", ToastLength.Long).Show();
            }else if (GameInfo.CurrentPlayer==null || GameInfo.CurrentPlayer.PlayerName != txtName.Text)
            {
                Toast.MakeText(this, "Select Player", ToastLength.Long).Show();
            }
            else
            {
                StartActivity(typeof (Gameplay));
            }
        }


    }

    public class PlayerDataAdapter : BaseAdapter<Player>
    {
        private readonly Activity context;
        private readonly List<Player> items; 
        public PlayerDataAdapter(Activity context, List<Player> items)
        {
            Log.Debug(GameInfo.logTag, "PlayerDataAdapter");
            this.context = context;
            this.items = items;
        }
        public override Player this[int position]
        {
            get
            {
                Log.Debug(GameInfo.logTag, "position");
                return items[position] ;
            }
        }

        public override int Count
        {
            get
            {
                Log.Debug(GameInfo.logTag, "Count");
                return items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            Log.Debug(GameInfo.logTag, "GetItemId");
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
                view.FindViewById<TextView>(Resource.Id.lblHardestWordScore).Text = "Score: " + item.HardestWordScore.ToString();
                view.FindViewById<TextView>(Resource.Id.lblHardestWordDate).Text = item.HardestWordDate.ToShortDateString();
                view.FindViewById<TextView>(Resource.Id.lblBestStreak).Text = "Best: " + item.BestStreak.ToString();
                view.FindViewById<TextView>(Resource.Id.lblCurrentStreak).Text = "Streak: Current: " + item.CurrentStreak.ToString();

            
            return view;
        }
        
    }

}
