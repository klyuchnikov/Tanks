using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tanks
{
    public class Tank
    {
       
        public Point Position
        {
            get
            {
                return new Point(Canvas.GetLeft(_share) / 20, Canvas.GetTop(_share) / 20);
            }
        }
        public Direction Direction { get; private set; }
        private Canvas _canvas;
        private readonly Polygon _share;
        private double DurationAnimation = 300;

        public Tank(Canvas canvas, Point startPosition)
        {
            this._canvas = canvas;
            _share = new Polygon() { Fill = Brushes.GreenYellow, RenderTransform = new RotateTransform(0, 10, 10) };
            _share.Points.Add(new Point(10, 0));
            _share.Points.Add(new Point(20, 20));
            _share.Points.Add(new Point(0, 20));
            Canvas.SetZIndex(_share, 1000);
            this._canvas.Children.Add(_share);
            Canvas.SetTop(_share, startPosition.Y * 20);
            Canvas.SetLeft(_share, startPosition.X * 20);
        }

        public void Rotation(bool isRight)
        {
            _canvas.Dispatcher.Invoke(new Action(() =>
            {
                var oldvalueAngle =
                    (_share.RenderTransform as RotateTransform).Angle;
                var newvalueAngle = isRight
                                        ? oldvalueAngle + 90
                                        : oldvalueAngle - 90;
                DoubleAnimation dbAscending =
                    new DoubleAnimation(oldvalueAngle,
                                        newvalueAngle,
                                        new Duration(
                                            TimeSpan.
                                                FromMilliseconds
                                                (DurationAnimation)));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(dbAscending);
                Storyboard.SetTarget(dbAscending, _share);
                Storyboard.SetTargetProperty(dbAscending,
                                            new PropertyPath(
                                                "RenderTransform.Angle"));
                storyboard.Begin();
                switch (Direction)
                {
                    case Direction.Up:
                        Direction = isRight
                                        ? Direction.Right
                                        : Direction.Left;
                        break;
                    case Direction.Right:
                        Direction = isRight
                                        ? Direction.Down
                                        : Direction.Up;
                        break;
                    case Direction.Down:
                        Direction = isRight
                                        ? Direction.Left
                                        : Direction.Right;
                        break;
                    case Direction.Left:
                        Direction = isRight
                                        ? Direction.Up
                                        : Direction.Down;
                        break;
                }
            }), null);
        }

        public void MoveForward()
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
    }
}
