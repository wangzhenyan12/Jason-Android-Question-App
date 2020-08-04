using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Xamarin.Essentials;

namespace Android_Question_App
{
    [Activity(Label = "SidebarActivity")]
    public class SidebarActivity : AppCompatActivity
    {
        private static readonly string TAG = typeof(SidebarActivity).Name;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var subredditName = Intent.Extras.GetString("subredditName");
            string sidebarHtml = null;
            try
            {
                Task<string> getSidebarHtmlTask = new HttpClient().GetStringAsync("http://www.reddit.com/" + subredditName + "/about/sidebar");
                sidebarHtml = await getSidebarHtmlTask;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Network is not available!", ToastLength.Short).Show();
                Log.Debug(TAG, ex.ToString());
                return;
            }

            if (null == sidebarHtml)
            {
                Toast.MakeText(this, "Fail to Get Sidebar Content!", ToastLength.Short).Show();
                return;
            }

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            int width = (int)mainDisplayInfo.Width;
            int height = (int)mainDisplayInfo.Height;
            var webView = new WebView(this);
            AddContentView(webView, new ViewGroup.LayoutParams(width / 2, height));
            webView.LoadData(sidebarHtml, "text/html", "utf-8");
        }
    }
}