using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Mirapp
{
    [Activity(Label = "CallHistory", MainLauncher = false)]
    public class CallHistoryListActivity : ListActivity
    {
        IList<string> phoneNumbers ;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Intent.Extras.GetStringArrayList("phone_numbers") != null)
            {
                phoneNumbers = Intent.Extras.GetStringArrayList("phone_numbers") ?? new string[0];
            }
            else
            {
                phoneNumbers = new List<string>();
                for (int i = 0; i < 100; i++)
                {
                    phoneNumbers.Add(String.Format("{0} number", i));
                }
            }
            
            ListAdapter = new ListAdapter(this,phoneNumbers);
            ListView.VerticalScrollBarEnabled = true;
            ListView.FastScrollEnabled = true;
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var t = phoneNumbers[position];
           Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
        }
    }


    /*
     
     public class NonConfigInstanceActivity : ListActivity
{
  protected override void OnCreate (Bundle bundle)
  {
    base.OnCreate (bundle);
    SearchTwitter ("xamarin");
  }

  public void SearchTwitter (string text)
  {
    string searchUrl = String.Format("http://search.twitter.com/search.json?" + "q={0}&rpp=10&include_entities=false&" + "result_type=mixed", text);

    var httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri (searchUrl));
    httpReq.BeginGetResponse (new AsyncCallback (ResponseCallback), httpReq);
  }

  void ResponseCallback (IAsyncResult ar)
  {
    var httpReq = (HttpWebRequest)ar.AsyncState;

    using (var httpRes = (HttpWebResponse)httpReq.EndGetResponse (ar)) {
      ParseResults (httpRes);
    }
  }

  void ParseResults (HttpWebResponse httpRes)
  {
    var s = httpRes.GetResponseStream ();
    var j = (JsonObject)JsonObject.Load (s);

    var results = (from result in (JsonArray)j ["results"] let jResult = result as JsonObject select jResult ["text"].ToString ()).ToArray ();

    RunOnUiThread (() => {
      PopulateTweetList (results);
    });
  }

  void PopulateTweetList (string[] results)
  {
    ListAdapter = new ArrayAdapter<string> (this, Resource.Layout.ItemView, results);
  }
}
*/
}