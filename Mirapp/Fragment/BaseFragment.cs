using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
namespace Mirapp
{
    public class BaseFragment : Fragment
    {
        protected ListView listView;

        protected virtual void HandleEvents()
        {
            if (listView!=null)
            {
                //listView.ItemClick += listView_ItemClick;
                //listView.ItemLongClick += listView_ItemLongClick;
            }
            
        }

        protected virtual  void FindViews()
        {
            listView = this.View.FindViewById<ListView>(Mirapp.Resource.Id.Listview1);
            listView.ChoiceMode = ChoiceMode.Single;

        }
    }
}