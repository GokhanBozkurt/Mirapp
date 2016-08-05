using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;

namespace Mirapp
{
    public class ListTabFragment : BaseFragment
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

            listView.Adapter = new ListAdapter(this.Activity, phoneNumbers);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.List, container, false);
            return view;
        }
    }
}