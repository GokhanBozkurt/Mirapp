using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Mirapp
{
    public class BaseFragment : Fragment
    {
        protected ListView listView;

        protected void HandleEvents()
        {
            listView.ItemClick += listView_ItemClick;
        }

        private void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
        }

        protected void FindViews()
        {
            listView = this.View.FindViewById<ListView>(Mirapp.Resource.Id.Listview1);

        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}