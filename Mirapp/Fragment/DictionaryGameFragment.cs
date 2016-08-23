using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Mirapp
{
    public class DictionaryGameFragment : BaseFragment
    {
        private Button DictionaryEasyGameStartButton;
        private Button DictionaryMiddleGameStartButton;
        private Button DictionaryHardGameStartButton;
        private Spinner DictionaryGameSpinner;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();

            LoadViews();

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.DictonaryGameStart, container, false);

            return view;
        }


       

        protected override void FindViews()
        {
            DictionaryGameSpinner = View.FindViewById<Spinner>(Mirapp.Resource.Id.DictionaryGameSpinner);
            DictionaryEasyGameStartButton = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryEasyGameStartButton);
            DictionaryMiddleGameStartButton = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryMiddleGameStartButton);
            DictionaryHardGameStartButton = View.FindViewById<Button>(Mirapp.Resource.Id.DictionaryHardGameStartButton);
        }

        protected override void HandleEvents()
        {
            DictionaryEasyGameStartButton.Click += DictionaryEasyGameStartButtonClick;
            DictionaryMiddleGameStartButton.Click += DictionaryMiddleGameStartButtonClick;
            DictionaryHardGameStartButton.Click += DictionaryHardGameStartButtonClick;

        }
 
        
        private void DictionaryEasyGameStartButtonClick(object sender, EventArgs e)
        {
            StartGame(GameLevels.Easy);
        }
        private void DictionaryMiddleGameStartButtonClick(object sender, EventArgs e)
        {
            StartGame(GameLevels.Middle);
        }
        private void DictionaryHardGameStartButtonClick(object sender, EventArgs e)
        {
            StartGame(GameLevels.Hard);
        }

        private void StartGame(GameLevels gameLevels)
        {
            try
            {
                var intent = new Intent();
                intent.SetClass(this.Activity, typeof(DictonaryGameActivity));
                intent.PutExtra("GameLevel", gameLevels.ToString());
                intent.PutExtra("Language", DictionaryGameSpinner.SelectedItem.ToString());

                StartActivityForResult(intent, 100);

            }
            catch (Exception ex)
            {
                var toast = Toast.MakeText(this.Activity,ex.Message,ToastLength.Short);
                toast.Show();
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