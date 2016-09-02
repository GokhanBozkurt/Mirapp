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
        private TextView DictionaryGameOverResultTryCount;
        private TextView DictionaryGameOverResultSuccessCount;
        private TextView DictionaryGameOverResultSuccessPercentage;
        private TextView DictionaryGameOverResultSuccessTime;
        private TextView DictionaryGameOverResultLanguage;
        private TextView DictionaryGameOverResultGameLevel;
        private Button DictionaryGameStartAgainButton;
        private Button DictionaryMainButton;
        
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
            DictionaryGameOverResultGameLevel.Text = String.Format("Level    : {0}", GameLevel);
            DictionaryGameOverResultLanguage.Text = String.Format("Language    : {0}", Language);
            DictionaryGameOverResultTryCount.Text = String.Format("Try Count   : {0}", GameResultCalculation.TryCount);
            DictionaryGameOverResultSuccessCount.Text = String.Format("Sucess Count: {0}", GameResultCalculation.SuccessCount);
            DictionaryGameOverResultSuccessPercentage.Text = String.Format("Sucess      : % {0}", GameResultCalculation.SuccessPercentage);
            DictionaryGameOverResultSuccessTime.Text = String.Format("Elapsed     : {0} Second", GameResultCalculation.ElapsedStropWatch.ElapsedMilliseconds/1000);
        }


        private void HandleEvents()
        {
            DictionaryGameStartAgainButton.Click += DictionaryGameStartAgainButton_Click;
            DictionaryMainButton.Click += DictionaryMainButton_Click;
        }

        private void DictionaryMainButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(MainActivity));
            StartActivityForResult(intent, 100);
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
            DictionaryGameOverResultTryCount = FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameOverResultTryCount);
            DictionaryGameOverResultSuccessCount = FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameOverResultSuccessCount);
            DictionaryGameOverResultSuccessPercentage = FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameOverResultSuccessPercentage);
            DictionaryGameOverResultSuccessTime = FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameOverResultSuccessTime);
            DictionaryGameOverResultLanguage = FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameOverResultLanguage);
            DictionaryGameOverResultGameLevel = FindViewById<TextView>(Mirapp.Resource.Id.DictionaryGameOverResultGameLevel);
            DictionaryGameStartAgainButton = FindViewById<Button>(Mirapp.Resource.Id.DictionaryGameStartAgainButton);
            DictionaryMainButton = FindViewById<Button>(Mirapp.Resource.Id.DictionaryMainButton);
            
        }
    }
}