using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace Tanks
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GameProcess.Current.CanvasGame = this.canvas1;
            canvas1.KeyDown += Grid_KeyDown;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //   var openFileDialog = new OpenFileDialog();
            //    if (openFileDialog.ShowDialog() == true)
            //    {
            //      GameProcess.Current.LoadMap(openFileDialog.FileName);
            //   }
            GameProcess.Current.LoadMap("map.txt");
            if (GameProcess.Current.IsLoadMap)
                GameProcess.Current.PaintMap();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Focusable = false;
            canvas1.Focus();
            GameProcess.Current.StartGame();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            GameProcess.Current.UserTank.Rotation(false);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            GameProcess.Current.UserTank.MoveForward();

        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (GameProcess.Current.FirstKeystroke == null)
                GameProcess.Current.FirstKeystroke = e.Key;
        }
    }
}
