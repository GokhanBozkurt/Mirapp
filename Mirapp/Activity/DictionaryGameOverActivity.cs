using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Mirapp 
{
    [Activity(Label = "Game Over", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class DictionaryGameOverActivity : Activity
    {
        private TextView DictionaryGameOverResult;
        private Button DictionaryGameStartAgainButton;
        //private int TryCount;
        //private int SuccessCount;
        //private long ElapsedMilliseconds;
        public string Language { get; set; }

        public GameLevels GameLevel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DictionaryGameOver);

            FindViews();

            HandleEvents();

            ReadIntentExtras();
        }

        private void ReadIntentExtras()
        {
            GameLevel = GameLevelOperation.GetGameLevel(Intent.Extras.GetString("GameLevel"));
            Language = Intent.Extras.GetString("Language");
            //TryCount = Intent.Extras.GetInt("TryCount");
            //SuccessCount = Intent.Extras.GetInt("SuccessCount");
            //ElapsedMilliseconds = Intent.Extras.GetLong("ElapsedMilliseconds");

            DictionaryGameOverResult.Text = GameResultCalculation.Result;
        }


        private void HandleEvents()
        {
            DictionaryGameStartAgainButton.Click += DictionaryGameStartAgainButton_Click;
        }

        private void DictionaryGameStartAgainButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(DictonaryGameActivity));
            intent.PutExtra("GameLevel", GameLevel.ToString());
            intent.PutExtra("Language", Language);

            StartActivityForResult(intent, 100);
        }

        private void FindViews()
        {
            DictionaryGameOverResult = FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameOverResult);
            DictionaryGameStartAgainButton = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameStartAgainButton);
        }
    }
}