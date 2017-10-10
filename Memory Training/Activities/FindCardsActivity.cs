using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Preferences;
using Android.Text.Format;
using Android.Util;
using Android.Views;
using Android.Widget;
using MemoryTraining.Extensions;
using MemoryTraining.Resources;
using MemoryTraining.Views;

namespace MemoryTraining.Activities
{
    [Activity]
    public class FindCardsActivity : BaseActivity
    {
        protected override int LayoutId => Resource.Layout.FindCards;
        
       // LinearLayout _mainPage;
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

        List<ImageButtonEx> allCards;
        ImageButtonEx _firstCard;
        ImageButtonEx _secondCard;
        int step = 10;
        Random _random;
        List<int> _generatedids;
        List<int> _ids4Cards;
        
        TextView _label;
        string text ; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            //List<string> stringForSpinner = new List<string>();
            //stringForSpinner.Add("4x3");
            //stringForSpinner.Add("4x4");
            //stringForSpinner.Add("4x5");
            

            //_spinner = FindViewById<Spinner>(Resource.Id.spinner1);
            //_spinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, stringForSpinner);
            _label = FindViewById<TextView>(Resource.Id.text1);
            player = MediaPlayer.Create(this, Resource.Raw.aplodismenty_vne_pomescheniya);
            _cardsField = FindViewById<GridLayout>(Resource.Id.gridLayout1);
            _cardsField.RowCount = 4;
            _cardsField.ColumnCount = 4;
            allCards = new List<ImageButtonEx>();
            cardstimer = new System.Timers.Timer();
            timeTimer = new System.Timers.Timer();
            timeTimer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            timeTimer.Elapsed += timeTimer_Elapsed;
            cardstimer.Interval = TimeSpan.FromSeconds(0.03).TotalMilliseconds;
            cardstimer.Elapsed += Timer1_Elapsed;
            takeDifficult();
            Button gameMenu = FindViewById<Button>(Resource.Id.gameMenu);
            gameMenu.Click += (s, a) =>
            {
               // FragmentTransaction transcation = FragmentManager.BeginTransaction();

                GameMenu gMenu = new GameMenu();

                //gMenu.Show(FragmentManager,"1");
                //TODO: в качестве тега для фрагмента можно использовать его название типа
                gMenu.Show(FragmentManager, nameof(GameMenu));
                timeTimer.Stop();
            };
        }

       
        /*
        //menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }
         */

        public void takeDifficult()
        {   
          text = Intent.GetStringExtra("difficult");

            if (text == "1")
                  GenerateCards(2, 2);
              if (text  == "2")
                  GenerateCards(4, 4);
             if (text  == "3")
                GenerateCards(4, 5);
        }

        int i;//Time
        private void timeTimer_Elapsed(object sender, ElapsedEventArgs e)
        { 
            RunOnUiThread(() => _label.Text = "Прошло времени: " + i + " секунд");
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
                        card.SetImageResource(card.CurrentResourceId == questId ? (int) card.Tag : questId)));
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
                    //чо за херь  player.Release();  When the MediaRecorder is no longer needed, its resources must be released:
                    //player.Reset();
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
        
        void delCards(ImageButtonEx tmp1, ImageButtonEx tmp2)
        {
            tmp1.Visibility = ViewStates.Gone;
            tmp2.Visibility = ViewStates.Gone;

            allCards.Remove(tmp1);
            allCards.Remove(tmp2);
            saveRecords();
        }

        //TODO: метод по факту не сохраняет результат, а проверяет на готовность к завершению
        //по этому я б назвал как то типа CheckFinish()
        void saveRecords()
        {

            if (allCards.Count == 0)
            {
                timeTimer.Stop();
                ResultMenu resMenu = new ResultMenu(this);
                Context mContext = Android.App.Application.Context;
              
                //TODO: пример использования менеджера
                int lastRes = PreferencesManager.Current.FindCardsBestResultEasy;
                resMenu.Show(FragmentManager, "2");

                resMenu.TextResult(lastRes, i);
                if (i < lastRes)
                    PreferencesManager.Current.SetValue(nameof(PreferencesManager.FindCardsBestResultEasy), i);
            }
        }


        public void GenerateCards(int columnCount, int rowCount)
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
            //allCards = new List<ImageButtonEx>();
            card.SetBackgroundColor(Color.ParseColor("#81C784"));
            card.SetImageResource(Resource.Drawable.animal_card8);
            //  card.SetBackgroundResource(Resource.Drawable.quest);

            //card.SetPadding(d, d, d, d);
            // card.SetBackgroundColor(Color.Black);
            card.Click += card_Click;
            _cardsField.AddView(card);
            card.Tag = GetResourceId4Card();
            var param = (ViewGroup.MarginLayoutParams) card.LayoutParameters;
            var dimenMedium = (int)Resources.GetDimension(Resource.Dimension.small);
            param.SetMargins(dimenMedium, dimenMedium, dimenMedium, dimenMedium);
            
            card.LayoutParameters.Width = 70.ToDp();
            card.LayoutParameters.Height = 100.ToDp();
            allCards.Add(card);
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
 
        
        
        void card_Click(object sender, EventArgs e)
        {
            if (cardstimer.Enabled)
                return;

            if (_firstCard == null)
            {
                _firstCard = (ImageButtonEx)sender;
                timeTimer.Start();
            }
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
 