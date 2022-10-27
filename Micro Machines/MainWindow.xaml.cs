using System;
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
        ImageBrush explosion = new ImageBrush();
        ImageBrush carEnemyImage = new ImageBrush();
        List<Rectangle> whiteStripes = new List<Rectangle>();
        List<Rectangle> itemstodestroy = new List<Rectangle>();
        Random rnd = new Random();
        bool carLeft, carRight, carShoot, carShootTicker = false, stripesMoving = true, timeMoving = true;
        int carLimit = 0, enemySpeed = 10, score = 0;
        double timer = 0.0, newShoot = 0.0;

        public MainWindow()
        {
            
            InitializeComponent();
            carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/carplayer.png"));
            car.Fill = carImage;
            explosion.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/ex.png"));
            carEnemyImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car.png"));
            carE.Fill = carEnemyImage;
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
            if(e.Key == Key.Space)
            {
                if (!carShootTicker)
                {
                    carShoot = true;
                    Rectangle enemyShoot = new Rectangle
                    {
                        Tag = "Shot",
                        Width = 5,
                        Height = 15,
                        Fill = Brushes.Black,
                        Stroke = Brushes.Black
                    };
                    Canvas.SetLeft(enemyShoot, Canvas.GetLeft(car) + car.Width / 2);
                    Canvas.SetTop(enemyShoot, Canvas.GetTop(car) - 10);
                    Canvas.SetZIndex(enemyShoot, 2);
                    myCanvas.Children.Add(enemyShoot);
                    carShootTicker = true;
                }
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
            if (e.Key == Key.Space)
            {
                carShoot = false;
            }
        }
        void gameEngine(object sender, EventArgs e)
        {
            newShoot += 0.025;
            if (newShoot >= 2.0)
            {
                carShootTicker = false;
                newShoot = 0;
            }
            if (timer >= 10) enemySpeed = 15;
            if (timer >= 20) enemySpeed = 20;
            if(timeMoving) timeLabel.Content = "Time: " + String.Format("{0:0.00}", timer);
            scoreLabel.Content = "Score: " + score;
            timer += 0.025;
            Rect playerHitbox = new Rect(Canvas.GetLeft(car) + 10, Canvas.GetTop(car) + 10, car.Width - 20, car.Height - 20);
            Rect enemyHitbox = new Rect(Canvas.GetLeft(carE), Canvas.GetTop(carE), carE.Width, carE.Height);
            if (carLimit == 0)
            {
                Canvas.SetLeft(carE, rnd.Next(10, 275));
                Canvas.SetTop(carE, -100);
                carLimit=1;
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
            if (stripesMoving)
            {
                foreach (var r in whiteStripes)
                {
                    Canvas.SetTop(r, Canvas.GetTop(r) + 5);
                    if (Canvas.GetTop(r) > 450)
                    {
                        Canvas.SetTop(r, -150);
                    }
                }
            }

            if (Canvas.GetTop(carE) <= 500)
            {                        
                Canvas.SetTop(carE, Canvas.GetTop(carE) + enemySpeed);                        
            }            
            if (enemyHitbox.IntersectsWith(playerHitbox))
            {
                car.Width = 150;
                car.Height = 150;
                Canvas.SetTop(car, Canvas.GetTop(car) - 50);
                Canvas.SetLeft(car, Canvas.GetLeft(car) - 50);
                Canvas.SetZIndex(car, 3);
                car.Fill = explosion;
                itemstodestroy.Add(carE);
                GameOver go = new GameOver();
                stripesMoving = false;
                timeMoving = false;
                go.Show();
            }
            if (Canvas.GetTop(carE) >= 500)
            {
                Canvas.SetLeft(carE, rnd.Next(10, 275));
                Canvas.SetTop(carE, -100);
                carLimit = 0;
            }
            foreach(var r in myCanvas.Children.OfType<Rectangle>())
            {
                if (r is Rectangle && (string)r.Tag == "Shot")
                {
                    if (Canvas.GetTop(r) > 5)
                    {
                        Canvas.SetTop(r, Canvas.GetTop(r) - 10);
                    }
                    if (Canvas.GetTop(r) < 5)
                    {
                        itemstodestroy.Add(r);
                    }
                    Rect shootHitbox = new Rect(Canvas.GetLeft(r), Canvas.GetTop(r), r.Width, r.Height);
                    if(shootHitbox.IntersectsWith(enemyHitbox))
                    {
                        score++;
                        Canvas.SetLeft(carE, rnd.Next(10, 275));
                        Canvas.SetTop(carE, -100);
                        itemstodestroy.Add(r);
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