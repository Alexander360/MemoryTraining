using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using MemoryTraining;
using MemoryTraining.Activities;

namespace Memory_Training.Activities
{
    [Activity(Label = "@string/choose_training")]
    public class ChooseTrainingActivity : BaseActivity
    {
        protected override int LayoutId => Resource.Layout.ChooseTraining;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            InitializeNavigationButton(Resource.Id.btn_find_cards, typeof(ChooseCardDifficultActivity));
            InitializeNavigationButton(Resource.Id.btn_find_words, typeof(FindWordsActivity));

            //xepь
            //InitializeNavigationButton(Resource.Id.btn_find_cards, typeof(FindCardsActivity));
        }
    }
}