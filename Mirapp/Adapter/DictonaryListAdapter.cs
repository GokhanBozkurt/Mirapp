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
namespace Mirapp 
{
    class DictonaryListAdapter : BaseAdapter<DictonaryWords>
    {
        private readonly IList<DictonaryWords> items;
        private readonly Activity context;

        public DictonaryListAdapter(Activity context, IList<DictonaryWords> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DictonaryWords this[int position] => items[position];

        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.DictionaryListRow, null); 
            view.FindViewById<TextView>(Resource.Id.DictonaryRowWordID).Text = items[position].ID.ToString();
            view.FindViewById<TextView>(Resource.Id.DictonaryRowWord).Text = items[position].Word.ToUpper();
            view.FindViewById<TextView>(Resource.Id.DictonaryRowToWord).Text = items[position].TranslatedWord.ToUpper();
            
            return view;
        }

        public void Remove(long id)
        {
        }
    }
}