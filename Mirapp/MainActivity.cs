using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;

namespace Mirapp
{
    [Activity(Label = "Mirapp", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light.DialogWhenLarge")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            
            AddTab("Add", 0, new DictonaryFragment());
            AddTab("List", 0, new DictonaryListFragment());
            AddTab("Game", 0, new DictionaryGameFragment());

        }

        void AddTab(string tabText, int iconResourceId, Fragment view)
        {
            var tab = ActionBar.NewTab();
            tab.SetText(tabText);
            //tab.SetIcon(iconResourceId);

            // must set event handler before adding tab
            tab.TabSelected += (sender, e) =>
            {
                var fragment = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += (sender, e) => e.FragmentTransaction.Remove(view);

            ActionBar.AddTab(tab);
        }


    }



}

