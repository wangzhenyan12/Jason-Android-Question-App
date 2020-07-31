using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace Android_Question_App
{
    [Activity(Label = "SidebarActivity")]
    public class SidebarActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var sidebarHtml = Intent.Extras.GetString("sidebarHtml");
            var webView = new WebView(this);
            AddContentView(webView, new ViewGroup.LayoutParams(800, 1600));
            webView.LoadData(sidebarHtml, "text/html", "utf-8");
        }
    }
}