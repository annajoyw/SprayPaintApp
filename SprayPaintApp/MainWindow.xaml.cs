using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SprayPaintApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }
        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            var radiobutton = sender as RadioButton;
            string radioBpressed = radiobutton.Content.ToString();
            if(radioBpressed == "Select")
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Select a picture";
                op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";
                if (op.ShowDialog() == true)
                {
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = new BitmapImage(new Uri(op.FileName));
                    DrawingCanvas.Background = imageBrush;
                }
            }
            else if(radioBpressed == "Draw")
            {
                this.DrawingCanvas.EditingMode = InkCanvasEditingMode.Ink;
            }
            else if (radioBpressed == "Erase")
            {
                this.DrawingCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            }
           
        }
        private void DrawPanel_KeyUp(object sender, KeyEventArgs e) 
        {
            if((int)e.Key >= 35 && (int)e.Key <= 68)
            {
                switch ((int)e.Key)
                {
                    case 35:
                        strokeAttribute.Width = 2;
                        strokeAttribute.Height = 2;
                        break;

                    case 36:
                        strokeAttribute.Width = 4;
                        strokeAttribute.Height = 4;
                        break;

                    case 37:
                        strokeAttribute.Width = 6;
                        strokeAttribute.Height = 6;
                        break;

                    case 38:
                        strokeAttribute.Width = 8;
                        strokeAttribute.Height = 8;
                        break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using(FileStream fs = new FileStream("MyPicture.bin",
                FileMode.Create))
            {
                this.DrawingCanvas.Strokes.Save(fs);
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream("MyPicture.bin",
                FileMode.Open, FileAccess.Read))
            {
                StrokeCollection sc = new StrokeCollection(fs);
                this.DrawingCanvas.Strokes = sc;
            }
        }
        private void RightMouseUpHandler(object sender,
                                 System.Windows.Input.MouseButtonEventArgs e)
        {
            Matrix m = new Matrix();
            m.Scale(1.1d, 1.1d);
            ((InkCanvas)sender).Strokes.Transform(m, true);
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(op.FileName));
                DrawingCanvas.Background = imageBrush;
            }

        }
    }
}
