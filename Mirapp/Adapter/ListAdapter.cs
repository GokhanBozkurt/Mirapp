using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace Mirapp
{
    public class ListAdapter : BaseAdapter<string>
    {
        IList<string> items;
        Activity context;
        public ListAdapter(Activity context, IList<string> items) : base()
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
}