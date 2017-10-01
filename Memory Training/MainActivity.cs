using Android.App;
using Android.OS;
using Android.Widget;
using Memory_Training.Activities;

namespace MemoryTraining
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : BaseActivity
    {
        protected override int LayoutId => Resource.Layout.Main;

        private Button _btnSettings;
        private Button _btnExit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitializeNavigationButton(Resource.Id.btn_new, typeof(ChooseTrainingActivity));
                
            _btnSettings = FindViewById<Button>(Resource.Id.btn_settings);
            _btnExit = FindViewById<Button>(Resource.Id.btn_exit);

        }

        
    }
}
 