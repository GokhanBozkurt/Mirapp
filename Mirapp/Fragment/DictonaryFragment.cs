using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mirapp.Data;

namespace Mirapp 
{
    public class DictonaryFragment : BaseFragment
    {
        private EditText WordText;
        private Button DictionaryAddButton;
        private EditText TranslatedWordText;
        private Repository<DictonaryWords> repository;
        private Spinner spinner;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();

            SetRepository();

            LoadViews();
            
        }

        private void SetRepository()
        {
            repository = new Repository<DictonaryWords>();
            repository.Open();
            //repository.DeleteTable();
            repository.CreateTable();
        }

        protected override void FindViews()
        {
            WordText = View.FindViewById<EditText>(Mirapp.Resource.Id.WordText);
            TranslatedWordText = View.FindViewById<EditText>(Mirapp.Resource.Id.TranslatedWordText);
            spinner = View.FindViewById<Spinner>(Mirapp.Resource.Id.spinner);
            DictionaryAddButton = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryAddButton);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Dictonary, container, false);
            return view;
        }

        public override void OnStop()
        {
            repository.Close();
            base.OnStop();
        }

        protected override void HandleEvents()
        {
            DictionaryAddButton.Click += DictionaryAddButton_Click;
        }


        private void DictionaryAddButton_Click(object sender, EventArgs e)
        {
            DictonaryWords dictonaryWords=new DictonaryWords()
            {
                Language = spinner.SelectedItem.ToString(),
                Word = WordText.Text,
                TranslatedWord = TranslatedWordText.Text
            };
            repository.Insert(dictonaryWords);

            ClearForm();
        }

        private void ClearForm()
        {
            var toast = Toast.MakeText(this.Activity, "Word added", ToastLength.Short);
            toast.Show();

            WordText.Text = "";
            TranslatedWordText.Text = "";
            WordText.RequestFocus();
        }

        private void LoadViews()
        {
            var adapter = ArrayAdapter.CreateFromResource(this.Activity, Resource.Array.Languages, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }
    }
}