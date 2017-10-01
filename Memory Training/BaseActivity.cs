
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MemoryTraining
{
    public abstract class BaseActivity : Activity
    {
        protected abstract int LayoutId { get; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(LayoutId);
        }

        protected Button InitializeNavigationButton(int btnId, Type gotoActivityType, Bundle args = null)
        {
            var button = FindViewById<Button>(btnId);
            if (button == null)
                return null;

            button.Click += (s, e) =>
            {
                var intent = new Intent(this, gotoActivityType);
                if(args != null)
                    intent.PutExtras(args);
                StartActivity(intent);
            };

            return button;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}