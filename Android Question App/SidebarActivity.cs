using System;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Xamarin.Essentials;

namespace Android_Question_App
{
    [Activity(Label = "@string/sidebar")]
    public class SidebarActivity : AppCompatActivity
    {
        private static readonly string TAG = typeof(SidebarActivity).Name;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var subredditName = Intent.Extras.GetString(Constants.SUBREDDIT_NAME);
            string sidebarHtml = null;
            try
            {
                Task<string> getSidebarHtmlTask = new HttpClient().GetStringAsync(Constants.BASE_URL + subredditName + Constants.ABOUT_SIDEBAR);
                sidebarHtml = await getSidebarHtmlTask;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, Resource.String.network_exception_tips, ToastLength.Short).Show();
                Log.Debug(TAG, ex.ToString());
                return;
            }

            if (null == sidebarHtml)
            {
                Toast.MakeText(this, Resource.String.fail_to_get_sidebar_content, ToastLength.Short).Show();
                return;
            }

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            int width = (int)mainDisplayInfo.Width;
            int height = (int)mainDisplayInfo.Height;
            var webView = new WebView(this);
            AddContentView(webView, new ViewGroup.LayoutParams(width / 2, height));
            webView.LoadData(sidebarHtml, Constants.TEXT_HTML, Constants.UTF_8);
        }
    }
}