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
using Java.Lang;

namespace Mirapp
{
    public class DictonaryListFragment : BaseFragment
    {
        private Repository<DictonaryWords> repository = new Repository<DictonaryWords>();
        private List<DictonaryWords> ListDictonaryWords;
        private DictonaryListAdapter _dictonaryListAdapter;

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
            e.View.Animate()
            .SetDuration(500)
            .Alpha(0)
            .WithEndAction(new Runnable(() => {
                e.View.Alpha = 1f;

                var LinearLayoutDictionaryListRowButtonDelete = e.View.FindViewById<LinearLayout>(Resource.Id.LinearLayoutDictionaryListRowButtonDelete);
                var LinearLayoutDictionaryListRowButtonEdit = e.View.FindViewById<LinearLayout>(Resource.Id.LinearLayoutDictionaryListRowButtonEdit);
                var DictonaryRowWord = e.View.FindViewById<TextView>(Resource.Id.DictonaryRowWord);
                var DictionaryListRowDelete = e.View.FindViewById<ImageView>(Resource.Id.DictionaryListRowDelete);
                var DictionaryListRowEdit = e.View.FindViewById<ImageView>(Resource.Id.DictionaryListRowEdit);
                LinearLayoutDictionaryListRowButtonDelete.Visibility = ViewStates.Visible;
                LinearLayoutDictionaryListRowButtonEdit.Visibility = ViewStates.Visible;
                                                  DictionaryListRowDelete.Click += (o, args) =>
                                                  {
                                                      var callDialog = new AlertDialog.Builder(this.Activity);
                                                      callDialog.SetMessage("Are you sure to delete " + DictonaryRowWord.Text + "?");
                                                      callDialog.SetNegativeButton("Cancel", delegate { });
                                                      callDialog.SetNeutralButton("Delete",
                                                                                            delegate
                                                                                            {
                                                                                                var DictonaryRowWordID = e.View.FindViewById<TextView>(Resource.Id.DictonaryRowWordID);
                                                                                                DictonaryWords item = new DictonaryWords() { ID = Convert.ToInt32(DictonaryRowWordID.Text) };
                                                                                                repository.Delete(item);
                                                                                                LoadList();
                                                                                            }
                                                                                        );
                                                      callDialog.Show();

                                                     
                                                  };

                DictionaryListRowEdit.Click += (o, args) =>
                {
                    var DictonaryRowWordID = e.View.FindViewById<TextView>(Resource.Id.DictonaryRowWordID);
                    var intent = new Intent();
                    intent.SetClass(this.Activity, typeof(DictionaryActivity));
                    intent.PutExtra("wordId", Convert.ToInt32(DictonaryRowWordID.Text));
                    StartActivityForResult(intent, 100);
                };




            }));

            //var DictonaryRowWordID = e.View.FindViewById<TextView>(Resource.Id.DictonaryRowWordID);
            //var intent = new Intent();
            //intent.SetClass(this.Activity, typeof(DictionaryActivity));
            //intent.PutExtra("wordId", Convert.ToInt32(DictonaryRowWordID.Text));

            //StartActivityForResult(intent, 100);

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
                _dictonaryListAdapter = new DictonaryListAdapter(this.Activity, ListDictonaryWords.OrderBy(a=>a.Language).ThenBy(b=>b.Word).ThenBy(c=>c.TranslatedWord).ToList());
                listView.Adapter = _dictonaryListAdapter;
            }
        }
    }
}