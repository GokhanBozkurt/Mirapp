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
    }
}