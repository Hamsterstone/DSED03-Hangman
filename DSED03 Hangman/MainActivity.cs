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
    [Activity(Label = "DSED03_Hangman", MainLauncher = true, Icon = "@drawable/icon",ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
       
        public List<Player> playerList;
        //= new List<Player>()
        //{
        //    new Player() {PlayerId=1,PlayerName = "Bob", BestScore = 0,HardestWord = "Fandangle",HardestWordScore = 14,BestScoreDate = new DateTime(2015,4,1), HardestWordDate = new DateTime(2015,3,1),LastPlayedDate = new DateTime(2015,5,1),BestStreak=5,CurrentStreak = 3},
        //    new Player() {PlayerId=2,PlayerName = "Mary", BestScore = 0},
        //    new Player() {PlayerId=3,PlayerName = "Pete", BestScore = 0},
        //    new Player() {PlayerId=4,PlayerName = "Jane", BestScore = 0}
        //};

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
        }

        private void CbxCheckChanged(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            GameInfo.WordList.Clear();
        }

        void UpdatePlayerList()
        {
            playerList = myDbManager.ViewAll();
            //todo get playerlist updating when new player added
        }
        private void BtnAddPlayer_LongClick(object sender, View.LongClickEventArgs e)
        {
            //todo add long click to edit player info.(lingclick on button or player listview?
            StartActivity(typeof(DatabaseWorker));
        }

        private void BtnAddPlayer_Click(object sender, EventArgs e)
        {
            string playerName = txtName.Text;
            myDbManager.AddPlayer(playerName);
            UpdatePlayerList();
        }

        public void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            //todo fix 
            var listView = sender as ListView;
            GameInfo.CurrentPlayer = updatedTableItems[e.Position];
            txtName.Text = GameInfo.CurrentPlayer.PlayerName;
            txtName.ClearFocus();
            InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            var currentFocus = CurrentFocus;
            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
        }



        private void BtnPlay_Click(object sender, EventArgs e)
        {

            GameInfo.Easy = cbxEasy.Checked;
            GameInfo.Medium = cbxMedium.Checked;
            GameInfo.Hard = cbxHard.Checked;
            if (cbxEasy.Checked == false && cbxMedium.Checked == false && cbxHard.Checked == false)
            {
                Toast.MakeText(this, "Select Difficulty", ToastLength.Long).Show();
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
                view.FindViewById<TextView>(Resource.Id.lblBestScore).Text = "Best Score: "+item.BestScore.ToString();
                view.FindViewById<TextView>(Resource.Id.lblBestScoreDate).Text = item.BestScoreDate.ToShortDateString();
                view.FindViewById<TextView>(Resource.Id.lblHardestWord).Text = item.HardestWord;
                view.FindViewById<TextView>(Resource.Id.lblHardestWordScore).Text = "Score: " + item.HardestWordScore.ToString();
                view.FindViewById<TextView>(Resource.Id.lblHardestWordDate).Text = item.HardestWordDate.ToShortDateString();
                view.FindViewById<TextView>(Resource.Id.lblBestStreak).Text = "Best: " + item.BestStreak.ToString();
                view.FindViewById<TextView>(Resource.Id.lblCurrentStreak).Text = "Streak: Current: " + item.CurrentStreak.ToString();

            
            return view;
        }
        
    }

}

/*
 
    SetContentView(Resource.Layout.HomeScreen) ;

tableItems = new List<TableItem>();

                var client = new RestClient("http://azurewebsites.net/");
var request = new RestRequest("Service/regionSearch", Method.POST);
request.RequestFormat = DataFormat.Json;
                tableItems = client.Execute<List<TableItem>>(request).Data;

                listView.Adapter = new HomeScreenAdapter(this, tableItems);
region = FindViewById<TextView> (Resource.Id.viewtext);
     area= FindViewById<TextView> (Resource.Id.viewtext2);
                _filterText = FindViewById<EditText>(Resource.Id.search);
                listView = FindViewById<ListView>(Resource.Id.listView);
 _filterText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                // filter on text changed
                var searchTerm = _filterText.Text;
};    


        listView.ItemClick += OnListItemClick;
    }

    protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
{
    var listView = sender as ListView;
    var t = tableItems[e.Position];
    //   var clickedTableItem = listView.Adapter[e.Position];
    Android.Widget.Toast.MakeText(this, clickedTableItem.DDLValue, Android.Widget.ToastLength.Short).Show();

}


public class HomeScreenAdapter : BaseAdapter<TableItem>
{
    List<TableItem> items;
    Activity context;
    public HomeScreenAdapter(Activity context, List<TableItem> items)
        : base()
    {

        this.context = context;
        this.items = items;
    }
    public override long GetItemId(int position)
    {
        return position;
    }
    public override TableItem this[int position]
    {
        get { return items[position]; }
    }
    public override int Count
    {
        get { return items.Count; }
    }
    public override View GetView(int position, View convertView, ViewGroup parent)
    {
        var item = items[position];

        //  TableItem item = items[position];


        View view = convertView;
        if (view == null) // no view to re-use, create new
            view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
        view.FindViewById<TextView>(Resource.Id.Text1).Text = item.DDLValue;
        view.FindViewById<TextView>(Resource.Id.Text2).Text = item.areaMsg;
        return view;
    }
}



_filterText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
            // filter on text changed
            var searchTerm = _filterText.Text;
var updatedTableItems = tableItems.Where(
    // TODO Fill in your search, for example:
    tableItem => tableItem.Msg.Contains(searchTerm) ||
                 tableItem.DDLValue.Contains(searchTerm)
).ToList();
var filteredResultsAdapter = new HomeScreenAdapter(this, updatedTableItems);
listView.Adapter = filteredResultsAdapter;
 };    
 
     
     */