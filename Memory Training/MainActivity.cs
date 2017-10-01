using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Timers;
using Android.Graphics;
using Android.Nfc.CardEmulators;
using Android.Media;
using Android.Text.Format;

namespace Memory_Training
{
    [Activity(Label = "Memory_Training", MainLauncher = true)]
    public class MainActivity : Activity
    {
       // LinearLayout _mainPage;
        Button _button1;
        GridLayout _cardsField;
        System.Timers.Timer cardstimer;
        System.Timers.Timer timeTimer;
        Spinner _spinner;
        private MediaPlayer player;
        private List<int> _numberForCardImage = new List<int>()
        {
            Resource.Drawable.cards1,
            Resource.Drawable.cards2,
            Resource.Drawable.cards3,
            Resource.Drawable.cards4,
            Resource.Drawable.cards5,
            Resource.Drawable.cards6,
            Resource.Drawable.cards7,
            Resource.Drawable.cards8, 
            Resource.Drawable.card7,
            Resource.Drawable.card8
        };

        int rowCount;
        int columnCount;

        ImageButtonEx _firstCard;
        ImageButtonEx _secondCard;
        int step = 10;
        Random _random;
        List<int> _generatedids;
        List<int> _ids4Cards;
        
        TextView _label;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            List<string> stringForSpinner = new List<string>();
            stringForSpinner.Add("4x3");
            stringForSpinner.Add("4x4");
            stringForSpinner.Add("4x5"); 

            _spinner = FindViewById<Spinner>(Resource.Id.spinner1);
            _spinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, stringForSpinner);
            _label = FindViewById<TextView>(Resource.Id.text1);
            player = MediaPlayer.Create(this, Resource.Raw.aplodismenty_vne_pomescheniya);
            _button1 = FindViewById<Button>(Resource.Id.button1);
            _cardsField = FindViewById<GridLayout>(Resource.Id.gridLayout1);
            _cardsField.RowCount = 4;
            _cardsField.ColumnCount = 4;
            _button1.Click += button1_Click;
            
            cardstimer = new System.Timers.Timer();
            timeTimer = new System.Timers.Timer();
            timeTimer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            timeTimer.Elapsed += timeTimer_Elapsed;
            cardstimer.Interval = TimeSpan.FromSeconds(0.03).TotalMilliseconds;
            cardstimer.Elapsed += Timer1_Elapsed;
        }

        int i;//Time i
        private void timeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() => _label.Text = "Прошло времени " + i + " секунд");
            i++;
        }
        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            var movedCards = MoveCards(); 

            if (movedCards.First().RotationY == 90)
            { 
                var questId = Resource.Drawable.Logopit1;
                RunOnUiThread(() =>
                    movedCards.ForEach(card =>
                        card.SetImageResource(card.CurrentResourceId == questId ? (int)card.Tag : questId)));
            }
           

            var needStopTimer = (_secondCard == null && _firstCard.RotationY == 180)
                                || _secondCard.RotationY == 0;

            if (needStopTimer)
                cardstimer.Stop();

            InvertStep();
            //если открыли вторую карту, даем 2 секунды на посмотреть
            if (_secondCard?.RotationY == 180)
            {
                cardstimer.Stop();
                if (_firstCard.Tag.ToString() == _secondCard.Tag.ToString())
                {
                    player.Start();
                    var tmp1 = _firstCard;
                    var tmp2 = _secondCard;
                    ResetCards();
                    step = 10;
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    player.Pause();
                    //player.Stop();
                    RunOnUiThread(() => delCards(tmp1, tmp2));
               //чо за херь  player.Release();    
                    return;
                }
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                cardstimer.Start();
            }

            if (_secondCard != null && _secondCard.RotationY == 0)
            {
                ResetCards();
            }
        }

        void delCards(ImageButton tmp1, ImageButton tmp2)
        {
            tmp1.Visibility = ViewStates.Gone;
            tmp2.Visibility = ViewStates.Gone;
        }
        void GenerateCards(int columnCount, int rowCount)
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
            var card = new ImageButtonEx(this);
            card.SetImageResource(Resource.Drawable.Logopit1);
          //  card.SetBackgroundResource(Resource.Drawable.quest);
            card.SetPadding(20,20,20,20);
           // card.SetBackgroundColor(Color.Black);
            card.Click += card_Click;
            _cardsField.AddView(card);
            card.Tag = GetResourceId4Card();
            card.LayoutParameters.Width = 150;
            card.LayoutParameters.Height = 200;
        }

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
            timeTimer.Start();
             
            var item = _spinner.SelectedItem.ToString();
            if (item == "4x3")
                GenerateCards(4, 3);
            if (item == "4x4")
                GenerateCards(4, 4);
            if (item == "4x5")
                GenerateCards(4, 5);
        }
        
        void card_Click(object sender, EventArgs e)
        {
            if (cardstimer.Enabled)
                return;

            if (_firstCard == null)
                _firstCard = (ImageButtonEx)sender;
            else if (_secondCard == null && sender != _firstCard)
                _secondCard = (ImageButtonEx)sender;

            else return;

            StartMoveCards();
        }

        private void StartMoveCards()
        {
            cardstimer.Start();
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
        private List<ImageButtonEx> MoveCards()
        {
            var movedCards = new List<ImageButtonEx>();
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
 