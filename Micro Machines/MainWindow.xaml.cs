﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;

namespace Micro_Machines
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageBrush carImage = new ImageBrush();
        ImageBrush carEnemy = new ImageBrush();
        List<Rectangle> whiteStripes = new List<Rectangle>();
        List<Rectangle> itemstodestroy = new List<Rectangle>();
        Random rnd = new Random();
        bool carLeft, carRight;
        int carLimit = 0;
        double timer = 0.0;

        public MainWindow()
        {
            
            InitializeComponent();
            carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/carplayer.png"));
            car.Fill = carImage;
            DispatcherTimer gameTime = new DispatcherTimer();
            gameTime.Interval = TimeSpan.FromMilliseconds(20);
            gameTime.Tick += gameEngine;
            gameTime.Start();
            myCanvas.Focus();
            whiteStripes.Add(whiteFlick0);
            whiteStripes.Add(whiteFlick1);
            whiteStripes.Add(whiteFlick2);
            whiteStripes.Add(whiteFlick3);
            whiteStripes.Add(whiteFlick4);

        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                carLeft = true;
            }
            if(e.Key == Key.Right)
            {
                carRight = true;
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                carLeft = false;
            }
            if (e.Key == Key.Right)
            {
                carRight = false;
            }
        }
        void gameEngine(object sender, EventArgs e)
        {
            timeLabel.Content = "Time: " + String.Format("{0:0.00}", timer);
            timer += 0.025;
            Rect playerHitbox = new Rect(Canvas.GetLeft(car), Canvas.GetTop(car), car.Width, car.Height);
            if(carLimit <=0)
            {
                ImageBrush carEnemyImage = new ImageBrush();
                carEnemyImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car.png"));
                Rectangle carE = new Rectangle
                {
                    Tag = "enemy",
                    Width = 50,
                    Height = 75,
                    Fill = carEnemyImage                    
                };          
                Canvas.SetLeft(carE, rnd.Next(10, 275));
                Canvas.SetTop(carE, -100);
                myCanvas.Children.Add(carE);
                Canvas.SetZIndex(carE, 2);
                carLimit++;
                
            }
            if (carLeft)
            {
                if(Canvas.GetLeft(car) > 10)
                {
                    Canvas.SetLeft(car, Canvas.GetLeft(car) - 5);
                }
            }
            if(carRight)
            {
                if(Canvas.GetLeft(car) < 275)
                {
                    Canvas.SetLeft(car, Canvas.GetLeft(car) + 5);
                }
            }
            foreach(var r in whiteStripes)
            {
                Canvas.SetTop(r, Canvas.GetTop(r) + 5);
                if(Canvas.GetTop(r) > 450)
                {
                    Canvas.SetTop(r, -150);
                }
            }
            foreach(var r in myCanvas.Children.OfType<Rectangle>())
            {
                if(r is Rectangle && (string)r.Tag == "enemy")
                {
                    if (Canvas.GetTop(r) <= 500)
                    {
                        
                        Canvas.SetTop(r, Canvas.GetTop(r) + 10);                        
                    }
                    Rect enemyHitbox = new Rect(Canvas.GetLeft(r), Canvas.GetTop(r), r.Width, r.Height);
                    if(enemyHitbox.IntersectsWith(playerHitbox))
                    {
                        MessageBox.Show("Game Over");
                        itemstodestroy.Add(r);
                        Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();                        
                    }
                    if (Canvas.GetTop(r) >= 500)
                    {
                        itemstodestroy.Add(r);
                        carLimit = 0;
                    }
                }                
            }
            foreach (var x in itemstodestroy)
            {
                myCanvas.Children.Remove(x);
            }
        }
    }
}