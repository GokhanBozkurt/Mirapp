using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mirapp.Data;

namespace Mirapp
{
    public class DictionaryGameFragment : BaseFragment
    {
        private Button DictionaryGameFROMButton;
        private Button DictionaryGameStartButton;
        private Button DictionaryGameToButton1;
        private Button DictionaryGameToButton2;
        private Button DictionaryGameToButton3;
        private Button DictionaryGameToButton4;
        private Repository<DictonaryWords> repository;
        private Spinner DictionaryGameSpinner;
        private readonly Random random = new Random();
        private DictonaryWords TrueWord;
        private List<int> ListButton;
        private List<DictonaryWords> ListDictonaryWords;        
        private readonly List<DictonaryWords> ListWrongyWords=new List<DictonaryWords>();        
        private TextView DictionaryGameResult;
        private int TryCount=0;
        private int SuccessCount=0;
        private int randomNumber;
        private int TrueRandomNumber;
        private Stopwatch ElapsedStropWatch;


        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();

            SetRepository();

            LoadViews();

            InvisibleForm();

            TryCount = 0;
            SuccessCount = 0;
            ListDictonaryWords = null;
            ElapsedStropWatch = new Stopwatch();

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.DictonaryGame, container, false);
            ListDictonaryWords = null;

            return view;
        }

        //public override void OnStop()
        //{
        //    repository.Close();
        //    TryCount = 0;
        //    SuccessCount = 0;
        //    base.OnStop();
        //}

        //public override void OnPause()
        //{
        //    ListDictonaryWords = null;
        //    base.OnPause();
        //}

        private TextView DictionaryGameTime;

        private void SetRepository()
        {
            repository = new Repository<DictonaryWords>();
            repository.Open();
            //repository.DeleteTable();
            repository.CreateTable();
        }

        protected override void FindViews()
        {
            DictionaryGameFROMButton = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameFROMButton);
            DictionaryGameSpinner = View.FindViewById<Spinner>(Mirapp.Resource.Id.DictionaryGameSpinner);
            DictionaryGameStartButton = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameStartButton);
            DictionaryGameToButton1 = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameToButton1);
            DictionaryGameToButton2 = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameToButton2);
            DictionaryGameToButton3 = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameToButton3);
            DictionaryGameToButton4 = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameToButton4);
            DictionaryGameResult= View.FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameResult);
            DictionaryGameTime = View.FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameTime);
            
        }

        protected override void HandleEvents()
        {
            DictionaryGameStartButton.Click += DictionaryGameStartButton_Click;
            DictionaryGameToButton1.Click += DictionaryGameToButton1_Click;
            DictionaryGameToButton2.Click += DictionaryGameToButton2_Click;
            DictionaryGameToButton3.Click += DictionaryGameToButton3_Click;
            DictionaryGameToButton4.Click += DictionaryGameToButton4_Click;

        }
 
        private void DictionaryGameToButton4_Click(object sender, EventArgs e)
        {
            CheckWord((Button) (sender));
        }

        private void DictionaryGameToButton3_Click(object sender, EventArgs e)
        {
            CheckWord((Button) (sender));
        }

        private void DictionaryGameToButton2_Click(object sender, EventArgs e)
        {
            CheckWord((Button) (sender));
        }

        private void DictionaryGameToButton1_Click(object sender, EventArgs e)
        {
            CheckWord((Button) (sender));
        }

        private void DictionaryGameStartButton_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            try
            {
                var dictonaryWords = new DictonaryWords()
                {
                    Language = DictionaryGameSpinner.SelectedItem.ToString()
                };
                
                if (ListDictonaryWords == null)
                {
                    ListDictonaryWords = PrepareWordList(repository.GetRecords(),dictonaryWords);
                    if (ListDictonaryWords.Count < 4)
                    {
                        var toast = Toast.MakeText(this.Activity, "Please enter word. There is not enough word for starting game.",ToastLength.Short);
                        toast.Show();
                        return;
                    }
                    ElapsedStropWatch.Start();
                    var toast1 = Toast.MakeText(this.Activity, String.Format( "Game started!!! {0}  Word",ListDictonaryWords.Count), ToastLength.Short);
                    toast1.Show();
                }

                VisibleClearForm();

                var RandomWordses = PrepareWordList(repository.GetRecords(), dictonaryWords);
                ListButton = GenerateRandom(4);

                TrueRandomNumber = random.Next(ListDictonaryWords.Count);
                TrueWord = ListDictonaryWords[TrueRandomNumber];
                DictionaryGameFROMButton.Text = TrueWord.Word;

                Button randomButton = GetRandomButton(ListButton[0]);
                randomButton.Text = TrueWord.TranslatedWord;
                var TrueWordindx = GetTrueWordindxIndx(RandomWordses);
                RandomWordses.RemoveAt(TrueWordindx);

                randomNumber = random.Next(RandomWordses.Count);
                randomButton = GetRandomButton(ListButton[1]);
                randomButton.Text = RandomWordses[randomNumber].TranslatedWord;
                RandomWordses.RemoveAt(randomNumber);

                randomNumber = random.Next(RandomWordses.Count);
                randomButton = GetRandomButton(ListButton[2]);
                randomButton.Text = RandomWordses[randomNumber].TranslatedWord;
                RandomWordses.RemoveAt(randomNumber);

                randomNumber = random.Next(RandomWordses.Count);
                randomButton = GetRandomButton(ListButton[3]);
                randomButton.Text = RandomWordses[randomNumber].TranslatedWord;
                RandomWordses.RemoveAt(randomNumber);
            }
            catch (Exception ex)
            {
                var toast = Toast.MakeText(this.Activity,ex.Message,ToastLength.Short);
                toast.Show();
            }
        }

        
        private void CheckWord(Button button)
        {
            TryCount++;
            DictionaryGameStartButton.Visibility = ViewStates.Invisible;
            DictionaryGameStartButton.Text = "";

            if (button.Text == TrueWord.TranslatedWord)
            {
                SuccesAction();

                if (ListDictonaryWords.Count == 0)
                {
                    GameOverAction();
                }
                else
                {
                    StartGame();
                    DictionaryGameResult.Text = String.Format("%{0} Success  {1} Words Remained", SuccessPercentage, ListDictonaryWords.Count); 
                }
            }
            else
            {
                AddWrongWord(TrueWord);
                DictionaryGameStartButton.Visibility=ViewStates.Visible;
                DictionaryGameStartButton.Text = "Wrong Word";
                DictionaryGameResult.Text = String.Format("%{0} Success  {1} Words Remained", SuccessPercentage, ListDictonaryWords.Count); ; ;
            }
        }

        private void AddWrongWord(DictonaryWords translatedWord)
        {
            if (!ListWrongyWords.Contains(translatedWord))
            {
                ListWrongyWords.Add(translatedWord);
            }
        }

        private decimal SuccessPercentage
        {
            get
            {
                try
                {
                    if (TryCount == 0)
                    {
                        return 0;
                    }
                    return 100 * Math.Round(Convert.ToDecimal(SuccessCount) / Convert.ToDecimal(TryCount), 2);
                }
                catch (Exception)
                {

                    return 0;
                }
            }
        }

        private List<DictonaryWords> PrepareWordList(List<DictonaryWords> listWords,DictonaryWords words)
        {
            var lst=new List<DictonaryWords>();
            foreach (DictonaryWords item in listWords)
            {
                if (item.Language!=words.Language)
                {
                    lst.Add(new DictonaryWords()
                    {
                        ID = item.ID,
                        Word = item.TranslatedWord,
                        TranslatedWord = item.Word,
                        Language = item.Language
                    });
                }
                else
                {
                    lst.Add(item);
                }
            }

            return lst;
        }

        private void SuccesAction()
        {

            SuccessCount++;
            ListDictonaryWords.RemoveAt(TrueRandomNumber);
            InvisibleForm();
        }
        private void GameOverAction()
        {
            DictionaryGameResult.Text = String.Format("Try : {0} Second:{1}  Sucess: %{2} ", TryCount, ElapsedStropWatch.ElapsedMilliseconds / 1000, SuccessPercentage);

            ElapsedStropWatch.Stop();
            ListDictonaryWords = null;
            TryCount = 0;
            SuccessCount = 0;

            DictionaryGameStartButton.Text = "Start Again";
            DictionaryGameStartButton.Visibility = ViewStates.Visible;
            DictionaryGameSpinner.Visibility=ViewStates.Visible;
        }

        private void InvisibleForm()
        {
            DictionaryGameFROMButton.Visibility = ViewStates.Invisible;
            DictionaryGameToButton1.Visibility = ViewStates.Invisible;
            DictionaryGameToButton2.Visibility = ViewStates.Invisible;
            DictionaryGameToButton3.Visibility = ViewStates.Invisible;
            DictionaryGameToButton4.Visibility = ViewStates.Invisible;
        }

        private void VisibleClearForm()
        {
            DictionaryGameFROMButton.Visibility = ViewStates.Visible;
            DictionaryGameToButton1.Visibility = ViewStates.Visible;
            DictionaryGameToButton2.Visibility = ViewStates.Visible;
            DictionaryGameToButton3.Visibility = ViewStates.Visible;
            DictionaryGameToButton4.Visibility = ViewStates.Visible;
            DictionaryGameStartButton.Visibility = ViewStates.Invisible;
            DictionaryGameSpinner.Visibility = ViewStates.Invisible;
            DictionaryGameFROMButton.Text = "";
            DictionaryGameToButton1.Text = "";
            DictionaryGameToButton2.Text = "";
            DictionaryGameToButton3.Text = "";
            DictionaryGameToButton4.Text = "";
            DictionaryGameResult.Text = "";
        }

        private int GetTrueWordindxIndx(IEnumerable<DictonaryWords> RandomWordses)
        {
            int indx = 0;
            int TrueWordindx = 0;
            foreach (DictonaryWords dictonaryWordse in RandomWordses)
            {
                if ((dictonaryWordse.TranslatedWord == TrueWord.TranslatedWord)
                    && (dictonaryWordse.Word == TrueWord.Word)
                    && (dictonaryWordse.Language == TrueWord.Language))
                {
                    TrueWordindx = indx;
                }
                indx++;
            }
            return TrueWordindx;
        }


        private List<DictonaryWords> CopyList(List<DictonaryWords> list)
        {
            List<DictonaryWords> returnListlis=new List<DictonaryWords>();
            foreach (var VARIABLE in list)
            {
                returnListlis.Add(VARIABLE);
            }
            return returnListlis;
        }

        public  List<int> GenerateRandom(int count)
        {
            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();
            while (candidates.Count < count)
            {
                // May strike a duplicate.
                candidates.Add(random.Next(count));
            }

            // load them in to a list.
            List<int> result = new List<int>();
            result.AddRange(candidates);

            // shuffle the results:
            int i = result.Count;
            while (i > 1)
            {
                i--;
                int k = random.Next(i + 1);
                int value = result[k];
                result[k] = result[i];
                result[i] = value;
            }
            return result;
        }

        private Button GetRandomButton(int next)
        {
            switch (next)
            {
                case 0:
                    return DictionaryGameToButton1;
                case 1:
                    return DictionaryGameToButton2;
                case 2:
                    return DictionaryGameToButton3;
                case 3:
                    return DictionaryGameToButton4;
                default:
                    return DictionaryGameToButton1;
            }
        }

        private void LoadViews()
        {
            var adapter = ArrayAdapter.CreateFromResource(this.Activity, Resource.Array.Languages, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            DictionaryGameSpinner.Adapter = adapter;
        }
    }
}