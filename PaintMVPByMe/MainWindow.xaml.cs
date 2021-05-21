using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintMVPByMe
{
    public partial class MainWindow : Window
    {
        public bool isEllipse = false;
        public bool isRectangle = false;
        public bool isLine = false;
        double bias = 117.96;
        Brush colorValue;

        LineGeometry line1;
        RectangleGeometry rectangle;
        Path path;
        int thickness = 1;
        Point leftTopPoint, startEllipsePoint;
        Rect rect;
        EllipseGeometry ellipse;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void main_draw_canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (isLine)
            {
               draw_line(e, 1, colorValue);
            }
            else if (isRectangle)
            {
                DrawRectangle(e, 1, colorValue);
            }
            else if (isEllipse)
            {
                drawElipse(e, 1, colorValue);
            }
        }

        private void main_draw_canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isLine | isRectangle | isEllipse)
            {
                main_draw_canvas.Children.Remove(path);
                main_draw_canvas.Children.Add(path);
            }


        }

        private void main_draw_canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLine)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    draw_line(e, 2, colorValue);
                }
            }
            else if (isRectangle)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DrawRectangle(e, 2, colorValue);
                }
            }
            else if (isEllipse)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
               {
                    drawElipse(e, 2, colorValue);
                }
            }
        }

        private void draw_line(object e, int v, Brush colorValue)
        {
            switch (v)
            {
                case 1:
                    MouseButtonEventArgs args = (MouseButtonEventArgs)e;
                    Point point = new Point(args.GetPosition(this).X, args.GetPosition(this).Y - bias);
                    line1 = new LineGeometry();
                    line1.StartPoint = point;
                    line1.EndPoint = point;
                    break;
                case 2:
                    MouseEventArgs args1 = (MouseEventArgs)e;
                    Point endPoint = new Point(args1.GetPosition(this).X, args1.GetPosition(this).Y - bias);
                    main_draw_canvas.Children.Remove(path);
                    line1.EndPoint = endPoint;
                    break;
            }
            path = new Path();
            path.Stroke = colorValue;
            path.StrokeThickness = thickness;
            path.Data = line1;
            main_draw_canvas.Children.Add(path);
        }
        
        private void DrawRectangle(object e, int v, Brush colorValue)
        {

            switch (v)
            {
                case 1:
                    MouseButtonEventArgs args = (MouseButtonEventArgs)e;
                    leftTopPoint = new Point(args.GetPosition(this).X, args.GetPosition(this).Y - 100);
                    rect = new Rect();
                    rect.Location = leftTopPoint;
                    rect.Size = new Size(0, 0);
                    rectangle = new RectangleGeometry();
                    rectangle.Rect = rect;
                    break;
                case 2:
                    MouseEventArgs args1 = (MouseEventArgs)e;
                    double width = args1.GetPosition(this).X - leftTopPoint.X;
                    double height = args1.GetPosition(this).Y - leftTopPoint.Y - 100;
                    if (width < 0 & height < 0)
                    {
                        rect.Width = Math.Abs(width);
                        rect.Height = Math.Abs(height);
                        rectangle.Rect = rect;
                        rectangle.Transform = new RotateTransform(180, leftTopPoint.X, leftTopPoint.Y);

                    }
                    else if (width < 0 & height > 0)
                    {
                        rect.Width = Math.Abs(width);
                        rect.Height = Math.Abs(height);
                        rectangle.Rect = rect;
                        rectangle.Transform = new RotateTransform(180, leftTopPoint.X, leftTopPoint.Y + height / 2);
                    }
                    else if (width > 0 & height < 0)
                    {
                        rect.Width = Math.Abs(width);
                        rect.Height = Math.Abs(height);
                        rectangle.Rect = rect;
                        rectangle.Transform = new RotateTransform(180, leftTopPoint.X + width / 2, leftTopPoint.Y);
                    }
                    else if(width > 0 & height > 0)
                    {
                        rect = new Rect();
                        rect.Location = leftTopPoint;
                        rect.Width = width;
                        rect.Height = height;
                        rectangle = new RectangleGeometry();
                        rectangle.Rect = rect;
                    }

                    main_draw_canvas.Children.Remove(path);
                    break;
            }



            path = new Path();
            path.Stroke = colorValue;
            path.StrokeThickness = thickness;

            GeometryGroup myGeometryGroup2 = new GeometryGroup();
            myGeometryGroup2.Children.Add(rectangle);

            path.Data = myGeometryGroup2;
            main_draw_canvas.Children.Add(path);
        }

        private void drawElipse(object e, int v, Brush colorValue)
        {

            switch (v)
            {
                case 1:
                    MouseButtonEventArgs args = (MouseButtonEventArgs)e;
                    startEllipsePoint = new Point(args.GetPosition(this).X, args.GetPosition(this).Y - bias);
                    ellipse = new EllipseGeometry();
                    break;
                case 2:
                    main_draw_canvas.Children.Remove(path);
                    MouseEventArgs args1 = (MouseEventArgs)e;
                    Point currentPoint = new Point(args1.GetPosition(this).X, args1.GetPosition(this).Y - bias);
                    Point radius = new Point((currentPoint.X - startEllipsePoint.X) / 2, (currentPoint.Y - startEllipsePoint.Y) / 2);
                    ellipse.Center = new Point(currentPoint.X - radius.X, currentPoint.Y - radius.Y);
                    ellipse.RadiusX = radius.X;
                    ellipse.RadiusY = radius.Y;
                    break;
                default:
                    break;
            }
            path = new Path();
            path.Stroke = colorValue;
            path.StrokeThickness = thickness;
            path.Data = ellipse;
            main_draw_canvas.Children.Add(path);
        }

        private void select_figure_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
        switch (buttonName)
            {
                case "select_line":
                    isLine = true;
                    isEllipse = false;
                    isRectangle = false;
                    main_draw_canvas.EditingMode = InkCanvasEditingMode.None;
                    break;
                case "select_circle":
                    isEllipse = true;
                    isLine = false;
                    isRectangle = false;
                    main_draw_canvas.EditingMode = InkCanvasEditingMode.None;
                    break;
                case "select_square":
                    main_draw_canvas.EditingMode = InkCanvasEditingMode.None;
                    isRectangle = true;
                    isLine = false;
                    isEllipse = false;
                    break;
                case "clear_button":
                    main_draw_canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    isRectangle = false;
                    isLine = false;
                    isEllipse = false;
                    break;
                case "pen_button":
                    main_draw_canvas.EditingMode = InkCanvasEditingMode.Ink;
                    isRectangle = false;
                    isLine = false;
                    isEllipse = false;
                    break;
            }

        }

        private void Choose_Color_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Brush backgroundColor = button.Background;

            if (backgroundColor is SolidColorBrush)
            {
                colorValue = (SolidColorBrush)backgroundColor;
                main_draw_canvas.DefaultDrawingAttributes.Color = ((SolidColorBrush)colorValue).Color;
            }
        }

        private void StrokeThicknessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            thickness = comboBox.SelectedIndex + 1;
            main_draw_canvas.DefaultDrawingAttributes.Width = thickness;
        }

        private void fill_figure_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void main_draw_canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            main_draw_canvas.Children.RemoveAt(main_draw_canvas.Children.Count - 1);
        }
    }
}
