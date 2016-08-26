using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Mirapp
{
    public class DictonaryListFragment : BaseFragment
    {
        private Repository<DictonaryWords> repository = new Repository<DictonaryWords>();
        private List<DictonaryWords> ListDictonaryWords;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();

            SetRepository();

            LoadList();

            if (listView != null)
            {
                listView.ItemLongClick += listView_ItemLongClick;
            }
        }

        private void listView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var DictonaryRowWordID = e.View.FindViewById<TextView>(Resource.Id.DictonaryRowWordID);
            var intent=new Intent();
            intent.SetClass(this.Activity, typeof(DictionaryActivity));
            intent.PutExtra("wordId", Convert.ToInt32(DictonaryRowWordID.Text));

            StartActivityForResult(intent,100);
        }

        private void SetRepository()
        {
            repository = new Repository<DictonaryWords>();
            repository.Open();
            repository.CreateTable();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.List, container, false);
            return view;
        }

        private void LoadList()
        {
            ListDictonaryWords = repository.GetRecords();
            if (ListDictonaryWords != null)
            {
                listView.Adapter = new DictonaryListAdapter(this.Activity, ListDictonaryWords.OrderBy(a=>a.Language).ThenBy(b=>b.Word).ThenBy(c=>c.TranslatedWord).ToList());
            }
        }
    }
}