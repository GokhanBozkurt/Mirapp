using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Mirapp
{
    [Activity(Label = "CallHistory", MainLauncher = false)]
    public class CallHistoryActivity : ListActivity
    {
        IList<string> phoneNumbers ;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //if ( Intent.Extras.GetStringArrayList("phone_numbers")!=null)
            //{
            //    phoneNumbers = Intent.Extras.GetStringArrayList("phone_numbers") ?? new string[0];
            //}
            //else
            {
                phoneNumbers = new List<string>();
                for (int i = 0; i < 100; i++)
                {
                    phoneNumbers.Add(String.Format("{0} number", i));
                }
            }
            
            ListAdapter = new HomeScreenAdapter(this,phoneNumbers);
            ListView.VerticalScrollBarEnabled = true;
            ListView.FastScrollEnabled = true;
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var t = phoneNumbers[position];
           Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
        }
    }

    public class HomeScreenAdapter : BaseAdapter<string>
    {
        IList<string> items;
        Activity context;
        public HomeScreenAdapter(Activity context, IList<string> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Resource.Layout.CallDetail, null);
            view.FindViewById<TextView>(Resource.Id.CallNumber).Text = items[position];
            view.FindViewById<TextView>(Resource.Id.TotalCall).Text = String.Format("Total: {0}", items.Count);
            return view;
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