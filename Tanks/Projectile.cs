using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Tanks
{
    public class Projectile : IDisposable
    {
        public Direction Direction { get; private set; }
        private Canvas _canvas;
        private Ellipse _share;
        private double DurationAnimation = 300;
        public Point Position
        {
            get
            {
                Point p = new Point(0, 0);
                _canvas.Dispatcher.Invoke(new Action(() =>
                                                         {
                                                             p = new Point(Canvas.GetLeft(_share) / 20,
                                                                              Canvas.GetTop(_share) / 20);
                                                         }));
                return p;
            }
        }
        public Projectile(Canvas canvas, Point startPosition, Direction direction)
        {
            this._canvas = canvas;
            Direction = direction;
            _share = new Ellipse() { Fill = Brushes.Black, RenderTransform = new RotateTransform(0, 10, 10), Height = 4, Width = 4 };
            Canvas.SetZIndex(_share, 2000);
            this._canvas.Children.Add(_share);
            Canvas.SetTop(_share, startPosition.Y * 20 + 8);
            Canvas.SetLeft(_share, startPosition.X * 20 + 8);
        }

        public void Move()
        {
            _canvas.Dispatcher.Invoke(new Action(() =>
                                                     {
                                                         var newvalueX = Canvas.GetLeft(_share);
                                                         var newvalueY = Canvas.GetTop(_share);
                                                         switch (Direction)
                                                         {
                                                             case Direction.Up:
                                                                 newvalueY -= 20;
                                                                 break;
                                                             case Direction.Right:
                                                                 newvalueX += 20;
                                                                 break;
                                                             case Direction.Down:
                                                                 newvalueY += 20;
                                                                 break;
                                                             case Direction.Left:
                                                                 newvalueX -= 20;
                                                                 break;
                                                         }

                                                         DoubleAnimation dbAscendingX =
                                                             new DoubleAnimation(Canvas.GetLeft(_share), newvalueX,
                                                                                 new Duration(
                                                                                     TimeSpan.FromMilliseconds(
                                                                                         DurationAnimation)));
                                                         DoubleAnimation dbAscendingY =
                                                             new DoubleAnimation(Canvas.GetTop(_share), newvalueY,
                                                                                 new Duration(
                                                                                     TimeSpan.FromMilliseconds(
                                                                                         DurationAnimation)));
                                                         Storyboard storyboard = new Storyboard();
                                                         storyboard.Children.Add(dbAscendingX);
                                                         storyboard.Children.Add(dbAscendingY);
                                                         Storyboard.SetTarget(dbAscendingX, _share);
                                                         Storyboard.SetTarget(dbAscendingY, _share);
                                                         Storyboard.SetTargetProperty(dbAscendingX,
                                                                                      new PropertyPath(
                                                                                          Canvas.LeftProperty));
                                                         Storyboard.SetTargetProperty(dbAscendingY,
                                                                                      new PropertyPath(
                                                                                          Canvas.TopProperty));
                                                         storyboard.Begin();
                                                     }));
        }


        public void Dispose()
        {
            _canvas.Dispatcher.Invoke(new Action(() =>
                                                       {
                                                           this._canvas.Children.Remove(this._share);
                                                           this._canvas.UpdateLayout();
                                                           this._share = null;
                                                           this._canvas = null;
                                                       }));
        }
    }
}
