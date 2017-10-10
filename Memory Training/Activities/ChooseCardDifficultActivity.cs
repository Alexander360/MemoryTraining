
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace MemoryTraining.Activities
{
    [Activity(Label = "ChooseCardDifficultActivity")]
    public class ChooseCardDifficultActivity : BaseActivity
    {
        Button _easyDifficult;
        Button _normalDifficult;

        Button _hardDifficult;
         

        protected override int LayoutId => Resource.Layout.ChooseCardDifficult;
 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            _easyDifficult = FindViewById<Button>(Resource.Id.btn_easy_difficult);
         
            _easyDifficult.Click += (s, e) =>
            {
                var findCardsAct = new Intent(this, typeof(FindCardsActivity));
                findCardsAct.PutExtra("difficult", "1");
                StartActivity(findCardsAct);
            };

            _normalDifficult = FindViewById<Button>(Resource.Id.btn_normal_difficult);
            _normalDifficult.Click += (s, e) =>
            {
                var findCardsAct = new Intent(this, typeof(FindCardsActivity));
                findCardsAct.PutExtra("difficult", "2");
                StartActivity(findCardsAct);
            };

            _hardDifficult = FindViewById<Button>(Resource.Id.btn_hard_difficult);
            _hardDifficult.Click += (s, e) =>
            {
                var findCardsAct = new Intent(this, typeof(FindCardsActivity));
                findCardsAct.PutExtra("difficult", "3");
                StartActivity(findCardsAct);
            };

        }
    }
}