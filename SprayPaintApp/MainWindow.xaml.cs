using Microsoft.Win32;
using SprayPaintApp.Data;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

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


        //returns byte array of the inkcanvas to store in a DB
        private byte[] getByteArray()
        {
            MemoryStream ms = new MemoryStream();
            DrawingCanvas.Strokes.Save(ms);
            byte[] bytes = ms.ToArray();
            return bytes;
        }
     
        //update
        //delete
        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            var radiobutton = sender as RadioButton;
            string radioBpressed = radiobutton.Content.ToString();
            if (radioBpressed == "Load Image")
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
            else if (radioBpressed == "Draw")
            {
                this.DrawingCanvas.EditingMode = InkCanvasEditingMode.Ink;
            }
            else if (radioBpressed == "Erase")
            {
                this.DrawingCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }

        }

        private void ToolButton_Click(object sender, RoutedEventArgs e)
        {
            var radiobutton = sender as RadioButton;
            string radioBpressed = radiobutton.Content.ToString();

            if(radioBpressed == "AirBrush")
            {
                strokeAttribute.StylusTipTransform = new Matrix(1, 0, 0.5, 1, 1, 4);
            }
            else if(radioBpressed == "Pen")
            {
                strokeAttribute.IsHighlighter = false;
                strokeAttribute.StylusTipTransform = new Matrix(1, 0, 0, 1, 0, 0);
            }
            else if (radioBpressed == "Marker")
            {
                strokeAttribute.StylusTipTransform = new Matrix(1, 0, 0, 5, 0, 0);
            }
            else if (radioBpressed == "Highlighter")
            {
                strokeAttribute.IsHighlighter = true;
                strokeAttribute.StylusTip = StylusTip.Ellipse;
            }
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            strokeAttribute.Color = (Color)_colorPicker.SelectedColor;
        }

        private void NumericUpDown_BrushSize(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            strokeAttribute.Width = (double)e.NewValue;
            strokeAttribute.Height = (double)e.NewValue;
        }

        /*the idea here was to create a "spray paint" effect by coloring random dots within a radius, I created the method that calculates
        where to put the random dots however I'm struggling to figure out how to edit the stroke attribute to represent the effect*/
        private void SprayPaintButton_Click(object sender, RoutedEventArgs e)
        {
            //double radius = strokeAttribute.Width/2;
            //double area = radius * radius * Math.PI;
            //double dotsPerTick = Math.Ceiling(area / 30);
            //for (int i = 0; i < dotsPerTick; i++)
            //{
            //    double[] offset = randomPointInRadius(radius);
            //}

           // DrawingAttributes spray = new DrawingAttributes();
        }
    
        private double[] randomPointInRadius(double radius)
        {
            Random rnd = new Random();
            double x =  rnd.NextDouble() * radius * 2;
            double y = rnd.NextDouble() * radius * 2;
            if (x * x + y * y <= 1)
            {
                x = x * radius;
                y = y * radius;
            }
            double[] myArray = new double[] { x, y };
            return myArray;
        }

        //saves only the ink
        private void SaveInkButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "isf files (*.isf)|*.isf";

            if (saveFileDialog1.ShowDialog() == true)
            {
                FileStream fs = new FileStream(saveFileDialog1.FileName,
                                               FileMode.Create);
                DrawingCanvas.Strokes.Save(fs);
                fs.Close();
            }
        }

        //saves new image with ink
        private void SaveBitMapButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";

            if (saveFileDialog1.ShowDialog() == true)
            {
                FileStream fs = new FileStream(saveFileDialog1.FileName,
                                               FileMode.Create);
                int marg = int.Parse(DrawingCanvas.Margin.Left.ToString());
                RenderTargetBitmap rtb =
                        new RenderTargetBitmap((int)DrawingCanvas.ActualWidth - marg,
                                (int)DrawingCanvas.ActualHeight - marg, 0, 0,
                            PixelFormats.Default);
                rtb.Render(DrawingCanvas);
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));

                encoder.Save(fs);
                fs.Close();
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Strokes.RemoveAll();
        }

        //the following two methods are still a WIP
        private void RightMouseUpHandler(object sender,
                                 System.Windows.Input.MouseButtonEventArgs e)
        {
            Matrix m = new Matrix();
            m.Scale(1.1d, 1.1d);
            ((InkCanvas)sender).Strokes.Transform(m, true);
        }

        private void ZoomButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string buttonPressed = button.Content.ToString();
            if (buttonPressed == "+")
            {
                DrawingCanvas.Height += 5;
                DrawingCanvas.Width += 5;
               
            }
            if (buttonPressed == "-")
            {
                DrawingCanvas.Height -= 5;
                DrawingCanvas.Width -= 5;
            }
        }
    }
}
