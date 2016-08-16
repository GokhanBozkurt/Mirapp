using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mirapp.Data;

namespace Mirapp 
{
    class DictonaryListAdapter : BaseAdapter<DictonaryWords>
    {
        readonly IList<DictonaryWords> items;
        readonly Activity context;
        //private Button DictionaryDeleteButton;
        public DictonaryListAdapter(Activity context, IList<DictonaryWords> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DictonaryWords this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.DictionaryListRow, null);
            view.FindViewById<TextView>(Resource.Id.DictonaryRowWord).Text = items[position].ToString();
            
            //DictionaryDeleteButton = view.FindViewById<Button>(Mirapp.Resource.Id.DictionaryDeleteButton);
            //DictionaryDeleteButton.Click += DictionaryDeleteButton_Click;
            return view;
        }


        private void DictionaryDeleteButton_Click(object sender, EventArgs e)
        {
           

        }
    }
}