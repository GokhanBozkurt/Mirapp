using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Mirapp
{
    [Activity(Label = "Mirapp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            AddTab("Tab 1", Resource.Drawable.minus_32, new SampleTabFragment());
            AddTab("Tab 2", Resource.Drawable.plus_32, new SampleTabFragment2());

        }

        void AddTab(string tabText, int iconResourceId, Fragment view)
        {
            var tab = ActionBar.NewTab();
            tab.SetText(tabText);
            tab.SetIcon(iconResourceId);

            // must set event handler before adding tab
            tab.TabSelected += (sender, e) =>
            {
                var fragment = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += (sender, e) => e.FragmentTransaction.Remove(view);

            this.ActionBar.AddTab(tab);
        }


        class SampleTabFragment : BaseFragment
        {
            public override void OnActivityCreated(Bundle savedInstanceState)
            {
                base.OnActivityCreated(savedInstanceState);

                FindViews();

                HandleEvents();

                IList<string> phoneNumbers = new List<string>();
                for (int i = 0; i < 10; i++)
                {
                    phoneNumbers.Add(String.Format("{0} number", i));
                }

                listView.Adapter= new HomeScreenAdapter(this.Activity, phoneNumbers);
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);
                var view = inflater.Inflate(Resource.Layout.List, container, false);

                return view;
            }
        }

        class SampleTabFragment2 : BaseFragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.Tab, container, false);
                var sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
                sampleTextView.Text = "sample fragment text 2";

                return view;
            }
        }




    }



}

