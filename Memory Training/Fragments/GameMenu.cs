using Android.App;
using Android.OS;
using Android.Views;

namespace MemoryTraining.Resources
{
    class GameMenu : DialogFragment 
    { 
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Menu, container, false);
            return view;
        }

    }
}