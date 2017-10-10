using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Memory_Training.Activities;

namespace MemoryTraining.Resources
{
    class ResultMenu : DialogFragment
    {
        public TextView _resultForCards;
        private Button _startAgain;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.ResultMenu, container, false);

            _resultForCards = view.FindViewById<TextView>(Resource.Id.resultText);
            _startAgain = view.FindViewById<Button>(Resource.Id.btn_start_again);
            _startAgain.Click += (s, e) =>
            {

            };
            Button goToMenu = view.FindViewById<Button>(Resource.Id.btn_menu_exit);
            goToMenu.Click += (s, e) =>
            {

            };

            _resultForCards = view.FindViewById<TextView>(Resource.Id.resultText);
            ShowResult();
            return view;
        }
 
        string result;

        public string TextResult(int bestResult, int newResult)
        {
            result = "Вы сделали за" + newResult + "секунд." + '\n' + "Ваш лучший результат " + bestResult;
            return result;
        }

        void ShowResult()
        {
            _resultForCards.Text = result;
        }
    }
}