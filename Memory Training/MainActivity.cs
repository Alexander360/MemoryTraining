using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Timers;
using Android.Views;
using System.Threading;

namespace Memory_Training
{
    [Activity(Label = "Memory_Training", MainLauncher = true)]
    public class MainActivity : Activity
    {
       // LinearLayout _mainPage;
        Button _button1;//, button2;
        GridLayout _cardsField;
        System.Timers.Timer timer1;
       // List<Point> _cards_Point = new List<Point>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
           // _mainPage = FindViewById<LinearLayout>(Resource.Id.mainPage1);
            _button1 = FindViewById<Button>(Resource.Id.button1);
            _cardsField = FindViewById<GridLayout>(Resource.Id.gridLayout1);
             
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
                Thread.Sleep(TimeSpan.FromSeconds(1));
                timer1.Start();
                //TODO:
                //либо закрыть карты если разные
                //TODO:доделать заполнение переменной проверкой одинаковые ли картинки
                var sameImages = false;
                if (sameImages)
                {
                    _firstCard.Visibility = _secondCard.Visibility = ViewStates.Gone;
                    ResetCards();
                    return;
                }
            }

            if (_secondCard != null && _secondCard.RotationY == 0)
            {
                ResetCards();
            }
        }

        Button Card;
        
        int i = 0;
        void generateCards()
        {
            Card = new Button(this);
            Card.Click += card_Click;
            Card.Text = 5.ToString();
            _cardsField.AddView(Card);
            Card.LayoutParameters.Width = 300;
            Card.LayoutParameters.Height = 300;
        }
        void button1_Click(object Cards, EventArgs e)
        {
            i++;
            for (int j=0; j<4; j++)
            {
                for(int k=0;k<4;k++)
                {
                    generateCards();
                }
            }
        }
        Button _firstCard;
        Button _secondCard;
        bool _isFirstCardOpen = false;
        bool _isSecondCardOpen = false;
        int step = 10;
        void card_Click(object sender, EventArgs e)
        {
            if(timer1.Enabled)
                return;

            if (_firstCard == null)
                _firstCard = (Button)sender;
            else if (_secondCard == null)
                _secondCard = (Button)sender;
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
                _secondCard.RotationY += step;
                _firstCard.RotationY += step;
                return;
            }
            if (_secondCard != null)
                _secondCard.RotationY += step;
            else
                _firstCard.RotationY += step;
        }
        
        private void ResetCards()
        {
            _secondCard = null;
            _firstCard = null;
        }
 
    }
}
 