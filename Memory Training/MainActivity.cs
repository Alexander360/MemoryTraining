using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace Memory_Training
{
    [Activity(Label = "Memory_Training", MainLauncher = true)]
    public class MainActivity : Activity
    {
       // LinearLayout _mainPage;
        Button _button1;
        GridLayout _cardsField;
        System.Timers.Timer timer1;
        Spinner _spinner;
        List<int> _numberForCardImage = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };

        int rowCount;
        int columnCount;

        ImageButton Card;
        ImageButton _firstCard;
        ImageButton _secondCard;
        int step = 10;

       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            List<string> stringForSpinner = new List<string>();
            stringForSpinner.Add("4x3");
            stringForSpinner.Add("4x4");
            _spinner = FindViewById<Spinner>(Resource.Id.spinner1);
            _spinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, stringForSpinner);
            
            
            // _mainPage = FindViewById<LinearLayout>(Resource.Id.mainPage1);
            _button1 = FindViewById<Button>(Resource.Id.button1);
            _cardsField = FindViewById<GridLayout>(Resource.Id.gridLayout1);
            //ImageButton imageButton = FindViewById<ImageButton>(Resource.Id.imageButton1);
            _cardsField.RowCount = 4;
            _cardsField.ColumnCount = 4;
            _button1.Click += button1_Click;
            timer1 = new System.Timers.Timer();
            timer1.Interval = TimeSpan.FromSeconds(0.03).TotalMilliseconds;
            timer1.Elapsed += Timer1_Elapsed;
        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            MoveCards(); 
            var needStopTimer = (_secondCard == null && _firstCard.RotationY == 180)
                                || _secondCard.RotationY == 0;

            if (needStopTimer)
                timer1.Stop();

            InvertStep();
            //если открыли вторую карту, даем 2 секунды на посмотреть
            if (_secondCard?.RotationY == 180)
            { 
                timer1.Stop();
                if (_firstCard.Tag.ToString() == _secondCard.Tag.ToString())
                {
                    _secondCard.Dispose();
                    _firstCard.Dispose();
                    //_secondCard.Enabled = false;
                    //_firstCard.Enabled = false;
                    ResetCards();
                    step = 10;
                    return;
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
                timer1.Start();
                
                //TODO:
                //либо закрыть карты если разные
                //TODO:доделать заполнение переменной проверкой одинаковые ли картинки
                //var sameImages = false;
               // if (sameImages)
                //{
                    
                   // _firstCard.Visibility = _secondCard.Visibility = ViewStates.Gone;
                  //  ResetCards();
                  //  return;
               // }
            }

            if (_secondCard != null && _secondCard.RotationY == 0)
            {
                ResetCards();
            }
        }

        private int generateNumForCardTag()
        {
            Random number = new Random();
            int num = number.Next(1, 9);
            if (_numberForCardImage.Contains(num))
            {
                Card.Tag = num;
                _numberForCardImage.Remove(num);
                return num;
            }
            return generateNumForCardTag();
        }


        void generateCards()
        {
            
            for (int row = 0; row < rowCount; row++)
            {
                for (int colum = 0; colum < columnCount; colum++)
                {
                    cardParameters();
                }
            }
        }

        void cardParameters()
        {
            Card = new ImageButton(this);
            Card.SetBackgroundResource(Resource.Drawable.quest);
            Card.Click += card_Click;
            generateNumForCardTag();
            _cardsField.AddView(Card);
            Card.LayoutParameters.Width = 100;
            Card.LayoutParameters.Height = 100;
        }
 
        void button1_Click(object sender, EventArgs e)
        {
            var item = _spinner.SelectedItem.ToString();
            if (item == "4x3")
            {
                columnCount = 3;
                rowCount = 4;
            }
            if (item == "4x4")
            {
                columnCount = 4;
                rowCount = 4;
            }

            generateCards();

        }
        
        
        void card_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                return;

            if (_firstCard == null)
                _firstCard = (ImageButton)sender;
            else if (_secondCard == null && sender != _firstCard)
                _secondCard = (ImageButton)sender;

            else return;

            StartMoveCards();
        }

        private void StartMoveCards()
        {
            timer1.Start();
        }

        private void InvertStep()
        {
            if (_secondCard != null && _secondCard.RotationY == 180 || _secondCard != null && _secondCard.RotationY == 0)
                step *= -1;
        }

        private void MoveCards()
        {
            if (_secondCard != null && step < 0)
            {
                if (_firstCard.RotationY == 90 && _secondCard.RotationY == 90)
                {
                    _firstCard.SetBackgroundResource(Resource.Drawable.quest); 
                    _secondCard.SetBackgroundResource(Resource.Drawable.quest); 
                }
                _secondCard.RotationY += step;
                _firstCard.RotationY += step;
                return;
            }

            if (_secondCard != null)
            {
                _secondCard.RotationY += step;
                if (_secondCard.RotationY == 90)
                    _secondCard.SetBackgroundResource(Resource.Drawable.animal_card1);
            }
            else
            {
                _firstCard.RotationY += step;
                if (_firstCard.RotationY == 90)
                    _firstCard.SetBackgroundResource(Resource.Drawable.animal_card1);
            }
         }

        private void ResetCards()
        {
            _secondCard = null;
            _firstCard = null;
        }
    }
}
 