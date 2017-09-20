using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Timers;
using Android.Views;
using System.Collections.Generic;
using System.Drawing;

namespace Memory_Training
{
    [Activity(Label = "Memory_Training", MainLauncher = true)]
    public class MainActivity : Activity
    {
       // LinearLayout _mainPage;
        Button _button1;//, button2;
        GridLayout _cardsField;
        Timer timer1;
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
        }
        Button Card;
        
        int i = 0;
        void generateCards()
        {
            Card = new Button(this);
            Card.Click += card_Click;

            _cardsField.AddView(Card);
            Card.LayoutParameters.Width = 100;
            Card.LayoutParameters.Height = 100;
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
            /* if (_firstCardOpen == null)
             {
                 _firstCardOpen = (Button)sender;
                 _isFirstCardOpen = true;
             }
             else if (_secondCardOpen == null)
             {
                 _secondCardOpen = (Button)sender;
                 _isSecondCardOpen = true;
             }
            timer1 = new Timer();
            timer1.Interval = TimeSpan.FromSeconds(0.03).TotalMilliseconds;
            timer1.Start();
            int step = 10;
            //TimeSpan.FromSeconds(0.03).TotalMilliseconds;//FromMilliseconds(3)
            timer1.Elapsed += (s, args) =>
            {
             if (_isFirstCardOpen == true && _isSecondCardOpen == false)
                  {
                      if (_firstCardOpen.RotationY < 180)
                          _firstCardOpen.RotationY += 10;
                      else
                          timer1.Stop();
                  }
                  else if (_isFirstCardOpen == true && _isSecondCardOpen == true)
                  {
                      if (_secondCardOpen.RotationY < 180)
                          _secondCardOpen.RotationY += 10;
                      else
                      {
                          timer1.Stop();
                          _isFirstCardOpen = false;
                          _isSecondCardOpen = false;
                      }
                  }
                  if (_isFirstCardOpen == false && _isSecondCardOpen == false)
                  {
                      if (_firstCardOpen.RotationY > 0 && _secondCardOpen.RotationY > 0)
                      {
                          _firstCardOpen.RotationY -= 10;
                          _secondCardOpen.RotationY -= 10;
                      }
                      else if (_firstCardOpen.RotationY == 0 && _secondCardOpen.RotationY == 0)
                         timer1.Stop();
                  }
             };
             */
            if (_firstCard == null)
                _firstCard = (Button)sender;
            else if (_secondCard == null)
                _secondCard = (Button)sender;
            else return;

            timer1 = new Timer();
            timer1.Interval =  TimeSpan.FromSeconds(0.03).TotalMilliseconds;
            timer1.Start(); 
            //TimeSpan.FromSeconds(0.03).TotalMilliseconds;//FromMilliseconds(3)
            timer1.Elapsed += (s, args) =>
            {/*
                if (_secondCard != null && step < 0 && _secondCard.RotationY > 0)
                {

                    _secondCard.RotationY += step;
                    _firstCard.RotationY += step;
                     
                        return;
                }
                if (_secondCard != null && step > 0 && _secondCard.RotationY < 180)
                    _secondCard.RotationY += step;
                else if (step > 0 && _firstCard.RotationY < 180)
                    _firstCard.RotationY += step;
                if (_secondCard.RotationY == 180 || _secondCard.RotationY == 0)
                {
                    if (_secondCard.RotationY == 0)
                    {
                        _secondCard = null;
                        _firstCard = null;
                        timer1.Stop();
                        step *= -1;
                    }
                    else
                    {
                        step *= -1;
                        timer1.Stop();
                    }
                }
                */
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
                if (_firstCard.RotationY == 180 || _secondCard.RotationY == 180 || _secondCard.RotationY == 0)
                    timer1.Stop();
                if (_secondCard!=null && _secondCard.RotationY == 180 || _secondCard != null && _secondCard.RotationY == 0)
                              step *= -1;
                 
                if (_secondCard.RotationY == 0)
                {
                    _secondCard = null;
                    _firstCard = null;

                }
            };
           
        }
        
 
    }
}
 