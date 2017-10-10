using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MemoryTraining.Resources
{
    class ResultMenu : DialogFragment
    {
        public TextView _resultForCards;
        private Button _startAgain;

        private readonly Activity _contextActivity;

        public ResultMenu(Activity contextActivity)
        {
            _contextActivity = contextActivity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.ResultMenu, container, false);

            _resultForCards = view.FindViewById<TextView>(Resource.Id.resultText);
            _startAgain = view.FindViewById<Button>(Resource.Id.btn_start_again);
            _startAgain.Click += (s, e) =>
            {
                var intent = _contextActivity.Intent;
                _contextActivity.Finish();
                StartActivity(intent);
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
        //TODO:тут публичный метод не нужно значение нужно брать с PreferencesManager.Current
        public string TextResult(int bestResult, int newResult)
        {
            //пример как заполнять строку с ресурсов значениями
            var template = Context.GetString(Resource.String.result_template);
            var stringWithParams = string.Format(template, newResult, bestResult);
            return result;
        }

        void ShowResult()
        {
            _resultForCards.Text = result;
        }
    }
}