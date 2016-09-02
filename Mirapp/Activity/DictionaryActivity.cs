using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Mirapp 
{
    [Activity(Label = "Word Edit", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class DictionaryActivity : Activity
    {
        private AutoCompleteTextView WordText;
        private Button DictionaryAddButton;
        private EditText TranslatedWordText;
        private Spinner spinner;
        private DictonaryWords item = new DictonaryWords();
        private Repository<DictonaryWords> repository;
        private Button DictionaryDeleteButton;
        private List<DictonaryWords> wordList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Dictonary);

            FindViews();

            HandleEvents();

            SetRepository();

            LoadViews();

            SetWord();
        }

        private void SetWord()
        {
            if (Intent.Extras.GetInt("wordId") != 0)
            {
                item = new DictonaryWords() {ID = Intent.Extras.GetInt("wordId")};
                item = repository.GetRecord(item);
                WordText.Text = item.Word;
                TranslatedWordText.Text = item.TranslatedWord;
                DictionaryAddButton.Text = "UPDATE";
                spinner.SetSelection(1);
                DictionaryDeleteButton.Visibility = ViewStates.Visible;
            }
        }

        private void SetRepository()
        {
            repository = new Repository<DictonaryWords>();
            repository.Open();
            repository.CreateTable();
            wordList = repository.GetRecords();
        }


        protected void FindViews()
        {
            WordText = FindViewById<AutoCompleteTextView>(Mirapp.Resource.Id.WordTextAutoComplete);
            TranslatedWordText = FindViewById<EditText>(Mirapp.Resource.Id.TranslatedWordText);
            spinner = FindViewById<Spinner>(Mirapp.Resource.Id.spinner);
            DictionaryAddButton = FindViewById<Button>(Mirapp.Resource.Id.DictionaryAddButton);
            DictionaryDeleteButton = FindViewById<Button>(Mirapp.Resource.Id.DictionaryDeleteButton);
        }

        protected void HandleEvents()
        {
            DictionaryAddButton.Click += DictionaryAddButton_Click;
            DictionaryDeleteButton.Click += DictionaryDeleteButton_Click;
        }

        private void LoadViews()
        {
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Languages, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            adapter = new ArrayAdapter<String>(this, Resource.Layout.WordListItem, wordList.Select(a => a.Word).ToList());
            WordText.Adapter = adapter;
        }
        private void DictionaryDeleteButton_Click(object sender, EventArgs e)
        {
            if (repository.Delete(item))
            {
                LoadMain();
            }
            else
            {
                ShowErrorMessage();
            }
        }

        private void ShowErrorMessage()
        {
            var toast1 = Toast.MakeText(this, String.Format("Hata :{0}  ", repository.RepositoryException.Message), ToastLength.Short);
            toast1.Show();
        }

        private void LoadMain()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(MainActivity));
            StartActivityForResult(intent, 100);
        }

        private void DictionaryAddButton_Click(object sender, EventArgs e)
        {
            item.Word = WordText.Text;
            item.TranslatedWord = TranslatedWordText.Text;
            if (repository.Update(item))
            {
                LoadMain();
            }
            else
            {
                ShowErrorMessage();
            }
        }
    }
}