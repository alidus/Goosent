using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Android.Views;
using Android.Support.V7.AppCompat;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Graphics.Drawables;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Goosent.Adapters;
using Android.Views.Animations;
using Android.Content.Res;
using Android.Graphics;
using System.IO;
using Android.Runtime;
using System.Threading.Tasks;

namespace Goosent
{
    [Activity(Label = "Goosent", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Android.Support.V7.App.AppCompatActivity
    {
        private ViewPager viewPager;
        private Android.Support.V7.Widget.Toolbar toolbar;
        private Android.Support.V4.Widget.DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private CompoundButton switchActionView;
        private FloatingActionButton fab;
        private TabLayout tabLayout;
        private TabsPagerFragmentAdapter fragmentPagerAdapter;
        public ServerManager sm;
        ActionBarDrawerToggle mDrawerToggle;
        Android.Support.V7.App.ActionBar actionBar;
        Spinner spinner;
        ChatsSpinnerArrayAdapter newSpinnerAdapter;
        DBHandler dbHandler;
        private bool isAuthorized = false;

        // Внутренние переменные для работы с сетами
        private int SELECTED_SET_INDEX = -1;
        private ChannelsSetsList SETS_LIST;
        private Channel SELECTED_CHANNEL;

        // Временные переменные
        const string email = "test@test.com";
        const string password = "testtest";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            dbHandler = new DBHandler(this);
            sm = new ServerManager();

            //dbHandler.ClearDatabase();
            SetContentView(Resource.Layout.Main);

            InitChannelsSets();
            InitProgram();
            InitToolbar();
            InitNavigationView();
            InitTabs();
            InitFAB();

            if (AuthStatus == false)
            {
                Intent i = new Intent(this, typeof(AuthActivity));
                StartActivityForResult(i, 1);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            sm.UID = data.GetStringExtra("uid");
            AuthStatus = true;
            Console.WriteLine("Установлен uid: " + sm.UID);
        }

        private void InitProgram()
        {
            // Если существуют сеты - сделать выбранным сетом по умолчанию первый
            if (SetsList.Count > 0)
            {
                SelectedSetIndex = 0;
            }
        }

        private void InitChannelsSets()
        {
            SETS_LIST = new ChannelsSetsList();
            SETS_LIST = dbHandler.GetDataFromDB();
        }

        private void InitFAB()
        {
            fab = (FloatingActionButton)FindViewById(Resource.Id.floatingActionButton);
            fab.Hide();

            fab.Click += Fab_Click;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            Fragments.AddSetDialogFragment addSetFragmentDialogFragment = Fragments.AddSetDialogFragment.GetInstance();
            addSetFragmentDialogFragment.Show(FragmentManager, "add set dialog");
        }

        private void InitToolbar()
        {
            toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            toolbar.SetTitle(Resource.String.app_name);
            //toolbar.InflateMenu(Resource.Menu.menu);
            InitActionBarSpinner();
        }


        private void InitActionBarSpinner()
        {
            spinner = (Spinner)FindViewById(Resource.Id.spinner);
            spinner.ItemSelected += Spinner_ItemSelected;
            UpdateActionBarSpinner();
        }

        private void UpdateActionBarSpinner()
        {
            ChannelsSet selectedSet;
            if (SelectedSetIndex != -1)
            {
                selectedSet = SetsList.GetSetsList[SELECTED_SET_INDEX];
            } else
            {
                selectedSet = null;
            }
            if (SetsList.Count == 0 || selectedSet.Channels.Count == 0 )
            {
                spinner.Visibility = ViewStates.Invisible;
                return;
            }
            else
            {
                spinner.Visibility = ViewStates.Visible;
            }
            newSpinnerAdapter = new ChatsSpinnerArrayAdapter(this, selectedSet.Channels);
            spinner.Adapter = newSpinnerAdapter;
            spinner.ItemSelected += Spinner_ItemSelected;
            newSpinnerAdapter.NotifyDataSetChanged();
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            SelectedChannel = SelectedSet.Channels[e.Position];
        }

        public Channel SelectedChannel
        {
            set {
                SELECTED_CHANNEL = value;
                SwitchChatChannel();
            }
            get
            {
                return SELECTED_CHANNEL;
            }
        }

        private async Task SwitchChatChannel()
        {
            await sm.DeleteListeningChat();
            ServerManager.AddChatToListeningResponseContainer addChatResponse = await sm.AddChatToListening(SELECTED_CHANNEL.Name);
            switch (addChatResponse.response.answer)
            {
                case "channel_not_found":
                    Toast.MakeText(this, "Выбранный канал не найден", ToastLength.Short).Show();
                    break;
                case "add_successful":
                    Toast.MakeText(this, "Выбранный канал успешно добавлен", ToastLength.Short).Show();
                    break;
                default:
                    break;
            }
        }


        private void InitNavigationView()
        {
            drawerLayout = (Android.Support.V4.Widget.DrawerLayout)FindViewById(Resource.Id.drawer_layout);
            navigationView = (NavigationView)FindViewById(Resource.Id.navigation_view);
            IMenuItem switchItem = navigationView.Menu.FindItem(Resource.Id.nav_menu_item_keep_awake);
            switchActionView = (CompoundButton)MenuItemCompat.GetActionView(switchItem);
            switchActionView.CheckedChange += SwitchView_CheckedChange;
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.nav_menu_item_keep_awake:
                    switchActionView.Checked = !switchActionView.Checked;
                    break;
                default:
                    break;
            }
        }

        private void SwitchView_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                // Keep screen awake
                Console.WriteLine("Keeping screen awake");
                Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            }
            else
            {
                Console.WriteLine("Not keeping screen awake");
                Window.ClearFlags(WindowManagerFlags.KeepScreenOn);
            }
        }

        private void InitTabs()
        {
            viewPager = (ViewPager)FindViewById(Resource.Id.view_pager);
            fragmentPagerAdapter = new TabsPagerFragmentAdapter(SupportFragmentManager, this);
            viewPager.Adapter = fragmentPagerAdapter;
            viewPager.PageSelected += ViewPager_PageSelected;
            tabLayout = (TabLayout)FindViewById(Resource.Id.tab_layout);
            tabLayout.SetupWithViewPager(viewPager);

            tabLayout.GetTabAt(0).SetIcon(Resource.Drawable.ic_tab_chat);
            tabLayout.GetTabAt(1).SetIcon(Resource.Drawable.ic_tab_edit_set);
        }

        // Выполняется каждый раз, когда меняется страница в ViewPager
        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            // Показать/скрыть спиннер при смене страницы
            switch (e.Position)
            {
                case 0:               
                    SupportActionBar.SetTitle(Resource.String.tab_item_chat_actionbar);
                    fab.Hide();
                    break;

                case 1:
                    SupportActionBar.SetTitle(Resource.String.tab_item_select_set);
                    fab.Show();
                    break;

                case 2:
                    SupportActionBar.SetTitle(Resource.String.tab_item_edit_set);
                    fab.Show();
                    break;

                default:

                    break;
            }
        }

        void AddTab(TabLayout tabLayout, string label, Drawable icon, bool iconOnly = false)
        {
            var newTab = tabLayout.NewTab();
            if (!iconOnly)
            {
                newTab.SetText(label);
            }
            newTab.SetIcon(icon);
            tabLayout.AddTab(newTab);
        }

        public bool AuthStatus
        {
            get { return isAuthorized; }
            set { isAuthorized = value; }
        }

        public int SelectedSetIndex
        {
            get { return SELECTED_SET_INDEX; }
            set { SELECTED_SET_INDEX = value; if (spinner != null) { UpdateActionBarSpinner(); } }
        }

        public ChannelsSet SelectedSet
        {
            get { return SetsList.GetSetsList[SELECTED_SET_INDEX]; }
        }

        public ChannelsSetsList SetsList
        {
            get { return SETS_LIST; }
        }

        public void AddSet(ChannelsSet set)
        {
            SetsList.AddSet(set);
            dbHandler.AddSet(set);
            if (SetsList.Count == 1)
            {
                SelectedSetIndex = 0;
            }
            ((FragmentEditSets)fragmentPagerAdapter.tabs[1]).UpdateEditSetListView();
        }

        public void AddChannel(int setIndex, Channel channel)
        {
            SetsList.GetSetsList[setIndex].AddChannel(channel);
            dbHandler.AddChannel(channel, setIndex);
            ((FragmentEditSets)fragmentPagerAdapter.tabs[1]).UpdateEditSetListView();
            UpdateActionBarSpinner();
        }

        public void DeleteChannel (int setIndex, string channelName, DATA.Platforms channelPlatform)
        {
            SetsList.DeleteChannel(setIndex, channelName, channelPlatform);
            dbHandler.DeleteChannel(setIndex, channelName, channelPlatform);
            ((FragmentEditSets)fragmentPagerAdapter.tabs[1]).UpdateEditSetListView();
            UpdateActionBarSpinner();
        }

        public void DeleteSet (int setIndex)
        {
            dbHandler.DeleteSet(SetsList.GetSetsList[setIndex].Name);
            SetsList.DeleteSet(setIndex);

            // Больше не осталось сетов
            if (SetsList.Count == 0)
            {
                SelectedSetIndex = -1;
            } else if (SelectedSetIndex == setIndex && setIndex != 0)
            {
                SelectedSetIndex = setIndex - 1;
            }

            ((FragmentEditSets)fragmentPagerAdapter.tabs[1]).UpdateEditSetListView();
            UpdateActionBarSpinner();
        }

        public bool IsSetExist(string name)
        {
            foreach (ChannelsSet set in SetsList.GetSetsList)
            {
                if (set.Name == name)
                {

                    return true;
                }
            }

            return false;
        }

        public bool AreSetsExist
        {
            get {
                if (SETS_LIST.Count == 0)
                {
                    return false;
                }

                return true;
            }
        }

        //void SetupDB()
        //{
        //    string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "goosentDB.db");
        //}
    }
}

