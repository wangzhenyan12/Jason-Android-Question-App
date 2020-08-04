﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Android_Question_App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        private IList<string> subredditNames = null;
        private ListView subredditList = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            Button searchButton = FindViewById<Button>(Resource.Id.search_button);
            searchButton.Click += SearchButton_Click;

            subredditList = FindViewById<ListView>(Resource.Id.subreddit__list);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            var json = new WebClient().DownloadString("http://www.reddit.com/subreddits/search.json?q=" + FindViewById<TextInputEditText>(Resource.Id.textInput1).Text);
            var subreddits = JsonConvert.DeserializeObject<JObject>(json);
            subredditNames = new List<string>();

            foreach (var subreddit in subreddits["data"]["children"] as JArray)
            {
                var name = subreddit["data"]["display_name_prefixed"].ToString();
                subredditNames.Add(name);
            }

            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, subredditNames);
            subredditList.Adapter = adapter;
            subredditList.ItemClick += NewListItem_Click;
        }

        private void NewListItem_Click(object sender, AdapterView.ItemClickEventArgs e)
        {
            var subredditName = subredditNames[e.Position];
            var sidebarHtml = new WebClient().DownloadString("http://www.reddit.com/" + subredditName + "/about/sidebar");

            var intent = new Intent(this, typeof(SidebarActivity));
            intent.PutExtra("sidebarHtml", sidebarHtml);
            this.StartActivity(intent);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}

