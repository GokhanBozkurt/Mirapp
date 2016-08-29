using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [Activity(Label = "Game", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class DictonaryGameActivity : Activity
    {
        private Button DictionaryGameFROMButton;
        private Button DictionaryGameToButton1;
        private Button DictionaryGameToButton2;
        private Button DictionaryGameToButton3;
        private Button DictionaryGameToButton4;
        private Button DictionaryGameToButton5;
        private Button DictionaryGameToButton6;
        private Button DictionaryGameEndButton;
        
        private Button DictionaryGameError;
        private TextView DictionaryGameTime;
        private readonly Random random = new Random();
        private DictonaryWords TrueWord;
        private List<int> ListButton;
        private List<DictonaryWords> ListDictonaryWords;
        private readonly List<DictonaryWords> ListWrongyWords = new List<DictonaryWords>();
        private TextView DictionaryGameResult;
        //private int TryCount = 0;
        //private int SuccessCount = 0;
        private int randomNumber;
        private int TrueRandomNumber;
        private Repository<DictonaryWords> repository;

        public string Language { get; set; }

        public GameLevels GameLevel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DictonaryGame);

            FindViews();

            HandleEvents();

            SetRepository();

            InvisibleForm();

            GameResultCalculation.TryCount = 0;
            GameResultCalculation.SuccessCount = 0;
            ListDictonaryWords = null;
            GameResultCalculation.ElapsedStropWatch.Reset();

            ReadIntentExtras();

            StartGame();

        }

        private void ReadIntentExtras()
        {
            GameLevel = GameLevelOperation.GetGameLevel(Intent.Extras.GetString("GameLevel"));
            Language = Intent.Extras.GetString("Language");
        }




        private void SetRepository()
        {
            repository = new Repository<DictonaryWords>();
            repository.Open();
            repository.CreateTable();
        }

        private void FindViews()
        {
            DictionaryGameFROMButton = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameFROMButton);
            DictionaryGameToButton1 = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameToButton1);
            DictionaryGameToButton2 = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameToButton2);
            DictionaryGameToButton3 = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameToButton3);
            DictionaryGameToButton4 = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameToButton4);
            DictionaryGameToButton5 = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameToButton5);
            DictionaryGameToButton6 = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameToButton6);
            DictionaryGameEndButton = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameEndButton);
            
            DictionaryGameError = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameError);
            DictionaryGameResult = FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameResult);
            DictionaryGameTime = FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameTime);

        }

        private void HandleEvents()
        {
            DictionaryGameToButton1.Click += DictionaryGameToButton1_Click;
            DictionaryGameToButton2.Click += DictionaryGameToButton2_Click;
            DictionaryGameToButton3.Click += DictionaryGameToButton3_Click;
            DictionaryGameToButton4.Click += DictionaryGameToButton4_Click;
            DictionaryGameToButton5.Click += DictionaryGameToButton5_Click;
            DictionaryGameToButton6.Click += DictionaryGameToButton6_Click;
            DictionaryGameEndButton.Click += DictionaryGameEndButton_Click;

        }

        private void DictionaryGameEndButton_Click(object sender, EventArgs e)
        {
            var callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage("Are you sure to end game?");
            callDialog.SetNeutralButton("End Game",
                                               delegate
                                               {
                                                    GameOverAction();
                                               }
                                         );
            callDialog.SetNegativeButton("Cancel", delegate { });
            callDialog.Show();
        }

        private void DictionaryGameToButton6_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }
        private void DictionaryGameToButton5_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }
        private void DictionaryGameToButton4_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }

        private void DictionaryGameToButton3_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }

        private void DictionaryGameToButton2_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }

        private void DictionaryGameToButton1_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }
        private void StartGame()
        {
            try
            {
                var dictonaryWords = new DictonaryWords()
                {
                    Language =Language
                };

                if (ListDictonaryWords == null)
                {
                    ListDictonaryWords = PrepareWordList(repository.GetRecords(), dictonaryWords);
                    if (ListDictonaryWords.Count < 4)
                    {
                        var toast = Toast.MakeText(this, "Please enter word. There is not enough WordList for starting game.", ToastLength.Short);
                        toast.Show();
                        LoadMain();
                        return;
                    }
                    GameResultCalculation.ElapsedStropWatch.Start();
                    var toast1 = Toast.MakeText(this, String.Format("Game started!!! {0}  Word", ListDictonaryWords.Count), ToastLength.Short);
                    toast1.Show();
                }

                VisibleClearForm(GameLevel);

                var RandomWords = PrepareWordList(repository.GetRecords(), dictonaryWords);

                switch (GameLevel)
                {
                    case GameLevels.Easy:
                        ListButton = GenerateRandom(4);
                        break;
                    case GameLevels.Middle:
                        ListButton = GenerateRandom(5);
                        break;
                    case GameLevels.Hard:
                        ListButton = GenerateRandom(6);
                        break;
                }
                

                SetGuessWordButtons(RandomWords);
            }
            catch (Exception ex)
            {
                var toast = Toast.MakeText(this, ex.Message, ToastLength.Short);
                toast.Show();
            }
        }

        private void LoadMain()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(MainActivity));
            StartActivityForResult(intent, 100);
        }

        private void SetGuessWordButtons(List<DictonaryWords> RandomWords)
        {
            TrueRandomNumber = random.Next(ListDictonaryWords.Count);
            TrueWord = ListDictonaryWords[TrueRandomNumber];
            DictionaryGameFROMButton.Text = TrueWord.Word;

            Button randomButton = GetRandomButton(ListButton[0]);
            randomButton.Text = TrueWord.TranslatedWord;
            var TrueWordindx = GetTrueWordindxIndx(RandomWords);
            RandomWords.RemoveAt(TrueWordindx);

            randomNumber = random.Next(RandomWords.Count);
            randomButton = GetRandomButton(ListButton[1]);
            randomButton.Text = RandomWords[randomNumber].TranslatedWord;
            RandomWords.RemoveAt(randomNumber);

            randomNumber = random.Next(RandomWords.Count);
            randomButton = GetRandomButton(ListButton[2]);
            randomButton.Text = RandomWords[randomNumber].TranslatedWord;
            RandomWords.RemoveAt(randomNumber);

            randomNumber = random.Next(RandomWords.Count);
            randomButton = GetRandomButton(ListButton[3]);
            randomButton.Text = RandomWords[randomNumber].TranslatedWord;
            RandomWords.RemoveAt(randomNumber);

            if ((GameLevel == GameLevels.Middle) || (GameLevel == GameLevels.Hard))
            {
                randomNumber = random.Next(RandomWords.Count);
                randomButton = GetRandomButton(ListButton[4]);
                randomButton.Text = RandomWords[randomNumber].TranslatedWord;
                RandomWords.RemoveAt(randomNumber);
            }

            if (GameLevel == GameLevels.Hard)
            {
                randomNumber = random.Next(RandomWords.Count);
                randomButton = GetRandomButton(ListButton[5]);
                randomButton.Text = RandomWords[randomNumber].TranslatedWord;
                RandomWords.RemoveAt(randomNumber);
            }
        }

        private void CheckWord(Button button)
        {
            GameResultCalculation.TryCount++;
            DictionaryGameError.Visibility = ViewStates.Invisible;
            DictionaryGameError.Text = "";

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
                    DictionaryGameResult.Text = GameResultCalculation.ResultInGame(ListDictonaryWords.Count);
                }
            }
            else
            {
                AddWrongWord(TrueWord);
                DictionaryGameError.Visibility = ViewStates.Visible;
                DictionaryGameError.Text = "Wrong Word";
                DictionaryGameResult.Text = GameResultCalculation.ResultInGame(ListDictonaryWords.Count);
            }
        }

        
        private List<DictonaryWords> PrepareWordList(List<DictonaryWords> listWords, DictonaryWords words)
        {
            var lst = new List<DictonaryWords>();
            foreach (DictonaryWords item in listWords)
            {
                if (item.Language != words.Language)
                {
                    lst.Add(new DictonaryWords()
                    {
                        ID = item.ID, Word = item.TranslatedWord, TranslatedWord = item.Word, Language = item.Language
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
            GameResultCalculation.SuccessCount++;
            ListDictonaryWords.RemoveAt(TrueRandomNumber);
            InvisibleForm();
        }

        private void GameOverAction()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(DictionaryGameOverActivity));
            intent.PutExtra("GameLevel", GameLevel.ToString());
            intent.PutExtra("Language", Language);
            StartActivityForResult(intent, 100);
            DictionaryGameResult.Text = GameResultCalculation.Result;

        }

        private void AddWrongWord(DictonaryWords translatedWord)
        {
            if (!ListWrongyWords.Contains(translatedWord))
            {
                ListWrongyWords.Add(translatedWord);
            }
        }


        private void InvisibleForm()
        {
            DictionaryGameFROMButton.Visibility = ViewStates.Invisible;
            DictionaryGameToButton1.Visibility = ViewStates.Invisible;
            DictionaryGameToButton2.Visibility = ViewStates.Invisible;
            DictionaryGameToButton3.Visibility = ViewStates.Invisible;
            DictionaryGameToButton4.Visibility = ViewStates.Invisible;
            DictionaryGameToButton5.Visibility = ViewStates.Invisible;
            DictionaryGameToButton6.Visibility = ViewStates.Invisible;
        }

        private void VisibleClearForm(GameLevels gameLevel)
        {
            DictionaryGameFROMButton.Visibility = ViewStates.Visible;
            DictionaryGameToButton1.Visibility = ViewStates.Visible;
            DictionaryGameToButton2.Visibility = ViewStates.Visible;
            DictionaryGameToButton3.Visibility = ViewStates.Visible;
            DictionaryGameToButton4.Visibility = ViewStates.Visible;
            if (gameLevel == GameLevels.Easy)
            {
                DictionaryGameToButton5.Visibility = ViewStates.Invisible;
                DictionaryGameToButton6.Visibility = ViewStates.Invisible;
            }
            if (gameLevel==GameLevels.Middle)
            {
                DictionaryGameToButton5.Visibility = ViewStates.Visible;
                DictionaryGameToButton6.Visibility = ViewStates.Invisible;
            }
            if (gameLevel == GameLevels.Hard)
            {
                DictionaryGameToButton5.Visibility = ViewStates.Visible;
                DictionaryGameToButton6.Visibility = ViewStates.Visible;
            }
           
            DictionaryGameFROMButton.Text = "";
            DictionaryGameToButton1.Text = "";
            DictionaryGameToButton2.Text = "";
            DictionaryGameToButton3.Text = "";
            DictionaryGameToButton4.Text = "";
            DictionaryGameToButton5.Text = "";
            DictionaryGameToButton6.Text = "";
            DictionaryGameResult.Text = "";
        }

        public List<int> GenerateRandom(int count)
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
                case 4:
                    return DictionaryGameToButton5;
                case 5:
                    return DictionaryGameToButton6;
                default:
                    return DictionaryGameToButton1;
            }
        }


        private int GetTrueWordindxIndx(IEnumerable<DictonaryWords> RandomWordses)
        {
            int indx = 0;
            int TrueWordindx = 0;
            foreach (DictonaryWords dictonaryWordse in RandomWordses)
            {
                if ((dictonaryWordse.TranslatedWord == TrueWord.TranslatedWord) && (dictonaryWordse.Word == TrueWord.Word) && (dictonaryWordse.Language == TrueWord.Language))
                {
                    TrueWordindx = indx;
                }
                indx++;
            }
            return TrueWordindx;
        }

        private List<DictonaryWords> CopyList(List<DictonaryWords> list)
        {
            List<DictonaryWords> returnListlis = new List<DictonaryWords>();
            foreach (var VARIABLE in list)
            {
                returnListlis.Add(VARIABLE);
            }
            return returnListlis;
        }
    }

    public class GameResultCalculation
    {
        public static Stopwatch ElapsedStropWatch=new Stopwatch();

        public static int TryCount { get; set; }
        public static int SuccessCount { get; set; }

        public static decimal SuccessPercentage
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

        public static string Result => $"Try : {TryCount} Second:{ElapsedStropWatch.ElapsedMilliseconds/1000}  Sucess: %{SuccessPercentage} ";

        public static string ResultInGame (int ListCount) => String.Format("%{0} Success  {1} Words Remained", SuccessPercentage, ListCount);
    }
}