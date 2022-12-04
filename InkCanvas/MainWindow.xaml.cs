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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InkCanvas
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

        private void ColorRBtn_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).Content == null)
            {
                inkCanvas.DefaultDrawingAttributes.Color = Colors.Black;
                return;
            }
            //var x = e.Source as RadioButton;
            switch ((sender as RadioButton).Content.ToString())
            {
                case "Red":
                    inkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
                    inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    defaultMode.IsChecked = true;
                    break;
                case "Green":
                    inkCanvas.DefaultDrawingAttributes.Color = Colors.Green;
                    inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    defaultMode.IsChecked = true;
                    break;
                case "Blue":
                    inkCanvas.DefaultDrawingAttributes.Color = Colors.Blue;
                    inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    defaultMode.IsChecked = true;
                    break;
                case "Black":
                    inkCanvas.DefaultDrawingAttributes.Color = Colors.Black;
                    inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    defaultMode.IsChecked = true;
                    break;
            }
        }

        private void ModesRBtn_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).Content == null)
            {
                inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                return;
            }
            switch ((sender as RadioButton).Content.ToString())
            {
                case "Ink":
                    inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    break;
                case "Select":
                    inkCanvas.EditingMode = InkCanvasEditingMode.Select;
                    break;
                case "Erase":
                    inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    break;
                case "EraseByStroke":
                    inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    break;
            }
        }

        private void DrawingShapeRBtn_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).Content == null)
            {
                inkCanvas.DefaultDrawingAttributes.StylusTip = System.Windows.Ink.StylusTip.Ellipse;
                return;
            }
            switch ((sender as RadioButton).Content.ToString())
            {
                case "Ellipse":
                    inkCanvas.DefaultDrawingAttributes.StylusTip = System.Windows.Ink.StylusTip.Ellipse;
                    break;
                case "Rectangle":
                    inkCanvas.DefaultDrawingAttributes.StylusTip = System.Windows.Ink.StylusTip.Rectangle;
                    break;
            }
        }

        private void BrushSizeRBtn_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).Content == null)
            {
                inkCanvas.DefaultDrawingAttributes.Width = 4;
                inkCanvas.DefaultDrawingAttributes.Height = 4;
                return;
            }
            switch ((sender as RadioButton).Content.ToString())
            {
                case "Small":
                    inkCanvas.DefaultDrawingAttributes.Width = 4;
                    inkCanvas.DefaultDrawingAttributes.Height = 4;
                    break;
                case "Normal":
                    inkCanvas.DefaultDrawingAttributes.Width = 8;
                    inkCanvas.DefaultDrawingAttributes.Height = 8;
                    break;
                case "Large":
                    inkCanvas.DefaultDrawingAttributes.Width = 15;
                    inkCanvas.DefaultDrawingAttributes.Height = 15;
                    break;
            }
        }

        private void ClearAllBtn_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
            //inkCanvas.Strokes.Save();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "jpg files (*.jpg)|*.jpg";

            if (saveFileDialog1.ShowDialog() == true)
            {
                //FileStream fs = new FileStream(saveFileDialog1.FileName,
                //                               FileMode.Create);
                //inkCanvas.Strokes.Save(fs);
                //fs.Close();
                ExportToJpeg(saveFileDialog1.FileName, inkCanvas);
            }
        }

        public void ExportToJpeg(string path, System.Windows.Controls.InkCanvas inkCanvas)
        {
            try
            {
                double
                    x1 = inkCanvas.Margin.Left,
                    x2 = inkCanvas.Margin.Top,
                    x3 = inkCanvas.Margin.Right,
                    x4 = inkCanvas.Margin.Bottom,
                    width = inkCanvas.ActualWidth - (Margin.Left + Margin.Right),
                    height = inkCanvas.ActualHeight - (Margin.Top);
                    //height = inkCanvas.ActualHeight - (Margin.Top + Margin.Bottom);


                if (path == null) return;

                //inkCanvas.Margin = new Thickness(0, 0, 0, 0);

                //Size size1 = new Size(inkCanvas.Width, inkCanvas.Height);
                //Size size2 = new Size(800, 500);

                Size size = new Size(width, height);

                inkCanvas.Measure(size);
                inkCanvas.Arrange(new Rect(size));

                RenderTargetBitmap renderBitmap =
                 new RenderTargetBitmap(
                   (int)width,
                   (int)height,
                   96,
                   96,
                   PixelFormats.Default);

                renderBitmap.Render(inkCanvas);

                using (FileStream fs = File.Open(path, FileMode.Create))
                {
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    encoder.Save(fs);
                }

                //inkCanvas.Margin = new Thickness(x1, x2, x3, x4);
                MessageBox.Show("Saved Successfully...", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //private void ClearAllBtn_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    ClearAllBtn.Background = Brushes.Red;
        //    //ClearAllBtn.Background = new SolidColorBrush(Colors.Red);
        //}

    }
}
