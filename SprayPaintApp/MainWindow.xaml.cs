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
        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            var radiobutton = sender as RadioButton;
            string radioBpressed = radiobutton.Content.ToString();
            if(radioBpressed == "Load Image")
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
                this.DrawingCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }
           
        }
        private void DrawPanel_KeyUp(object sender, KeyEventArgs e) 
        {
            
        }
        private void SprayPaint_Click(object sender, RoutedEventArgs e)
        {
            strokeAttribute.IsHighlighter = true;
            strokeAttribute.StylusTip = StylusTip.Ellipse;
        }

        private void Pen_Click(object sender, RoutedEventArgs e)
        {
            strokeAttribute = DrawingCanvas.DefaultDrawingAttributes;
            strokeAttribute.IsHighlighter = false;
        }

        private void PaintBrush_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";

            if (saveFileDialog1.ShowDialog() == true)
            {
                FileStream fs = new FileStream(saveFileDialog1.FileName,
                                               FileMode.Create);
                DrawingCanvas.Strokes.Save(fs);
                fs.Close();
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Strokes.RemoveAll();
        }

        //sets selected color to inkcanvas stroke
        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            strokeAttribute.Color = (Color)_colorPicker.SelectedColor;
        }
        private void RightMouseUpHandler(object sender,
                                 System.Windows.Input.MouseButtonEventArgs e)
        {
            Matrix m = new Matrix();
            m.Scale(1.1d, 1.1d);
            ((InkCanvas)sender).Strokes.Transform(m, true);
        }


        private void NumericUpDown_BrushSize(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            strokeAttribute.Width = (double)e.NewValue;
            strokeAttribute.Height = (double)e.NewValue;
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
                viewbox1.Height -= 5;
                viewbox1.Width -= 5;
            }
        }


    }
}
