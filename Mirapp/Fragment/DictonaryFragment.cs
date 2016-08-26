using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Mirapp 
{
    public class DictonaryFragment : BaseFragment
    {
        private AutoCompleteTextView WordText;
        //private EditText WordText;
        private Button DictionaryAddButton;
        private Button TranslateButton;
        private EditText TranslatedWordText;
        private Repository<DictonaryWords> repository;
        private Spinner spinner;
        private List<DictonaryWords> wordList;

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

            wordList = repository.GetRecords();
        }

        protected override void FindViews()
        {
            //WordText = View.FindViewById<EditText>(Mirapp.Resource.Id.WordText);
            TranslatedWordText = View.FindViewById<EditText>(Mirapp.Resource.Id.TranslatedWordText);
            spinner = View.FindViewById<Spinner>(Mirapp.Resource.Id.spinner);
            DictionaryAddButton = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryAddButton);
            TranslateButton = View.FindViewById<Button>(Mirapp.Resource.Id.TranslateButton);
            WordText = View.FindViewById<AutoCompleteTextView>(Resource.Id.WordTextAutoComplete);
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
            TranslateButton.Click += TranslateButton_Click;
            WordText.ItemClick += WordTextAutoComplete_ItemClick;
        }



        private void WordTextAutoComplete_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var count = wordList.Where(a => a.Word == WordText.Text).Select(b => b.TranslatedWord).Count();
            if (count>0)
            {
                TranslatedWordText.Text = wordList.Where(a => a.Word == WordText.Text).Select(b => b.TranslatedWord).First();
                DictionaryAddButton.Enabled = false;
            }
        }

        private void TranslateButton_Click(object sender, EventArgs e)
        {
            Translate();
        }

        private void DictionaryAddButton_Click(object sender, EventArgs e)
        {

            if (CheckForm())
            {
                DictonaryWords dictonaryWords = new DictonaryWords()
                {
                    Language = spinner.SelectedItem.ToString(),
                    Word = WordText.Text,
                    TranslatedWord = TranslatedWordText.Text
                };
                repository.Insert(dictonaryWords);

                ClearForm(); 
            }
        }

        private bool CheckForm()
        {
            if (WordText.Text=="" || TranslatedWordText.Text=="")
            {
                var toast = Toast.MakeText(this.Activity, "Please fill the form", ToastLength.Short);
                toast.Show();
                return false;
            }

            var count = wordList.Where(a => a.Word.Trim() == WordText.Text.Trim()).Select(b => b.TranslatedWord).Count();
            if (count > 0)
            {
                var toast = Toast.MakeText(this.Activity, "This word avaliable in dictonary", ToastLength.Short);
                toast.Show();
                return false;
            }
            return true;
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

            adapter = new ArrayAdapter<String>(this.Activity, Resource.Layout.WordListItem,wordList.Select(a=>a.Word).ToList());
            WordText.Adapter = adapter;
        }
        private void Translate()
        {
            try
            {
                var translateResult = Translater.TranslateFromRestService(WordText.Text);
                TranslatedWordText.Text = translateResult.Result.TranslatedWordList[0].desc;
            }
            catch (Exception ex)
            {
                var toast = Toast.MakeText(this.Activity, "Hata : " + ex.Message, ToastLength.Short);
                toast.Show();

            }
        }


    }
}