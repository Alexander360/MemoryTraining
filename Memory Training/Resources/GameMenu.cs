using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MemoryTraining.Activities;

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