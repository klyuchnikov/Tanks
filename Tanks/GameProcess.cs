using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Timers;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Input;

namespace Tanks
{
    public class GameProcess
    {
        private GameProcess()
        {
            Projectiles = new List<Projectile>();
            process = new Timer(1000);
            processProjectiles = new Timer(300);
            process.Elapsed += new ElapsedEventHandler(process_Elapsed);
            processProjectiles.Elapsed += new ElapsedEventHandler(processProjectiles_Elapsed);
        }

        void processProjectiles_Elapsed(object sender, ElapsedEventArgs e)
        {
            for (var i = 0; i < Projectiles.Count; i++)
            {
                bool res = false;
                var p = Projectiles[i].Position;
                switch (Projectiles[i].Direction)
                {
                    case Direction.Up:
                        res = Map[(int)p.Y - 1, (int)p.X] == CellMap.Ground || Map[(int)p.Y - 1, (int)p.X] == CellMap.Water;
                        break;
                    case Direction.Left:
                        res = Map[(int)p.Y, (int)p.X - 1] == CellMap.Ground || Map[(int)p.Y, (int)p.X - 1] == CellMap.Water;
                        break;
                    case Direction.Right:
                        res = Map[(int)p.Y, (int)p.X + 1] == CellMap.Ground || Map[(int)p.Y, (int)p.X + 1] == CellMap.Water;
                        break;
                    case Direction.Down:
                        res = Map[(int)p.Y + 1, (int)p.X] == CellMap.Ground || Map[(int)p.Y + 1, (int)p.X] == CellMap.Water;
                        break;
                }
                if (res)
                    Projectiles[i].Move();
                else
                {
                    Projectiles[i].Dispose();
                    Projectiles.RemoveAt(i);
                }
            }
        }

        void process_Elapsed(object sender, ElapsedEventArgs e)
        {
            CanvasGame.Dispatcher.Invoke(new Action(() =>
                                                        {
                                                            IsMoveTank = false;

                                                            switch (FirstKeystroke)
                                                            {
                                                                case Key.Right:
                                                                    UserTank.Rotation(true);
                                                                    break;
                                                                case Key.Left:
                                                                    UserTank.Rotation(false);
                                                                    break;
                                                                case Key.Up:
                                                                    IsMoveTank = true;
                                                                    break;
                                                                case Key.Down:
                                                                    break;
                                                                case Key.Space:
                                                                    var pro = new Projectile(this.CanvasGame,
                                                                                             UserTank.Position,
                                                                                             UserTank.Direction);
                                                                    Projectiles.Add(pro);
                                                                    break;
                                                            }
                                                            if (CanMove(UserTank) && IsMoveTank)
                                                                UserTank.MoveForward();
                                                            else
                                                                IsMoveTank = false;

                                                        }));
            FirstKeystroke = null;

        }

        private Timer process;
        private Timer processProjectiles;
        public bool IsMoveTank { get; set; }

        public Canvas CanvasGame { get; set; }

        public Key? FirstKeystroke { get; set; }

        public Tank UserTank { get; private set; }

        public static GameProcess Current = new GameProcess();

        public bool IsLoadMap { get; private set; }

        public CellMap[,] Map;

        public Size SizeMap;

        public List<Projectile> Projectiles { get; private set; }

        private bool CanMove(Tank tank)
        {
            var p = tank.Position;
            switch (tank.Direction)
            {
                case Direction.Up:
                    return Map[(int)p.Y - 1, (int)p.X] == CellMap.Ground;
                case Direction.Left:
                    return Map[(int)p.Y, (int)p.X - 1] == CellMap.Ground;
                case Direction.Right:
                    return Map[(int)p.Y, (int)p.X + 1] == CellMap.Ground;
                case Direction.Down:
                    return Map[(int)p.Y + 1, (int)p.X] == CellMap.Ground;
                default:
                    return false;
            }
        }


        public void LoadMap(string pathFileMap)
        {
            var lines = File.ReadAllLines(pathFileMap);
            try
            {
                var countColums = int.Parse(lines[0].Split(' ')[0]);
                var countRows = int.Parse(lines[0].Split(' ')[1]);
                this.SizeMap = new Size(countRows, countColums);
                this.Map = new CellMap[countRows, countColums];
                for (var i = 0; i < countRows; i++)
                {
                    var arr = lines[i + 1];
                    for (var k = 0; k < countColums; k++)
                    {
                        switch (arr[k])
                        {
                            case 'S':
                                this.Map[i, k] = CellMap.Brick;
                                break;
                            case 'X':
                                this.Map[i, k] = CellMap.Ground;
                                this.UserTank = new Tank(this.CanvasGame, new Point(k, i));
                                break;
                            case 'Y':
                                this.Map[i, k] = CellMap.ComputerTank;
                                break;
                            case ' ':
                                this.Map[i, k] = CellMap.Ground;
                                break;
                            case 'W':
                                this.Map[i, k] = CellMap.Water;
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                this.IsLoadMap = false;
            }
            this.IsLoadMap = true;
        }

        public void PaintMap()
        {
            for (var i = 0; i < SizeMap.Width; i++)
            {
                for (var k = 0; k < SizeMap.Height; k++)
                {
                    var rec = new Rectangle() { Width = 20, Height = 20 };
                    switch (Map[i, k])
                    {
                        case CellMap.Brick:
                            rec.Fill = Brushes.Brown;
                            break;
                        case CellMap.ComputerTank:
                            rec.Fill = Brushes.DarkViolet;
                            break;
                        case CellMap.Ground:
                            rec.Fill = Brushes.DimGray;
                            break;
                        case CellMap.Water:
                            rec.Fill = Brushes.CornflowerBlue;
                            break;
                        case CellMap.UserTank:
                            rec.Fill = Brushes.DimGray;
                            break;
                    }
                    Canvas.SetTop(rec, i * 20);
                    Canvas.SetLeft(rec, k * 20);
                    this.CanvasGame.Children.Add(rec);
                }
            }
        }

        public void StartGame()
        {
            process.Start();
            processProjectiles.Start();
        }

    }
}
