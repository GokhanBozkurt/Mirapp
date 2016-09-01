using Android.Content;
using Android.Widget;

namespace Mirapp
{
    public class AdapterBoss
    {
        //private static AdapterBoss instance;

        //public AdapterBoss Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance=new AdapterBoss();
        //        }
        //        return instance;
        //    }
        //}

        //public void Initilaze()
        //{

        //}  

        public static ISpinnerAdapter GetLanguageAdapter(Context context)
        {
            var adapter = ArrayAdapter.CreateFromResource(context, Resource.Array.Languages,Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            return adapter;
        }
    }
}