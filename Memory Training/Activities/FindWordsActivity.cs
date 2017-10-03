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
using MemoryTraining.Views;

namespace MemoryTraining.Activities
{
    [Activity(Label = "FindWordsActivity")]
    public class FindWordsActivity : BaseActivity
    {
        protected override int LayoutId => Resource.Layout.FindWords;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}