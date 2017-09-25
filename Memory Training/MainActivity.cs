using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
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
        List<int> _numberForCardImage = new List<int>()
        {
            Resource.Drawable.animal_card1,
            Resource.Drawable.animal_card2,
            Resource.Drawable.animal_card3,
            Resource.Drawable.animal_card4,
            Resource.Drawable.animal_card5,
            Resource.Drawable.animal_card6,
            Resource.Drawable.animal_card7,
            Resource.Drawable.animal_card8,
            Resource.Drawable.animal_card9,
        };

        int rowCount;
        int columnCount;

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
            var movedCards = MoveCards();

            if (movedCards.First().RotationY == 90)
            {
                movedCards.ForEach(card => card.SetBackgroundResource((int)card.Tag));
            }

            var needStopTimer = (_secondCard == null && _firstCard.RotationY == 180)
                                || _secondCard.RotationY == 0;

            if (needStopTimer)
                timer1.Stop();

            InvertStep();
            //если открыли вторую карту, даем 2 секунды на посмотреть
            if (_secondCard?.RotationY == 180)
            { 
                timer1.Stop();
                if (_firstCard.Tag == _secondCard.Tag)
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

        void generateCards()
        {
            _random = new Random();
            var maxCount = rowCount * columnCount / 2;
            _ids4Cards = _numberForCardImage.GetRange(0, maxCount);

            _generatedids = new List<int>();

            for (int row = 0; row < rowCount; row++)
            {
                for (int colum = 0; colum < columnCount; colum++)
                {
                    GenerateCard();
                }
            }
        }

        void GenerateCard()
        {
            var сard = new ImageButton(this);
            сard.SetBackgroundResource(Resource.Drawable.quest);
            сard.Click += card_Click;
            _cardsField.AddView(сard);
            сard.Tag = GetResourceId4Card();
            сard.LayoutParameters.Width = 150;
            сard.LayoutParameters.Height = 150;
        }

        private Random _random;
        private List<int> _generatedids;
        private List<int> _ids4Cards;

        private int GetResourceId4Card()
        {
            var randomIndex = _random.Next(0, _ids4Cards.Count-1);
            var randomId = _ids4Cards[randomIndex];

            if (_generatedids.Count(id => id == randomId) == 2)
            {
                _ids4Cards.Remove(randomId);

                return GetResourceId4Card();
            }

            _generatedids.Add(randomId);

            return randomId;
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

        /// <summary>
        /// Move cards
        /// </summary>
        /// <returns>list of moved cards </returns>
        private List<ImageButton> MoveCards()
        {
            var movedCards = new List<ImageButton>();
            if (_secondCard != null && step < 0)
            {
                _secondCard.RotationY += step;
                _firstCard.RotationY += step;

                movedCards.Add(_firstCard);
                movedCards.Add(_secondCard);
                return movedCards;
            }

            var card4Move = _secondCard ?? _firstCard;

            card4Move.RotationY += step;

            movedCards.Add(card4Move);
            return movedCards;
         }

        private void ResetCards()
        {
            _secondCard = null;
            _firstCard = null;
        }
    }
}
 