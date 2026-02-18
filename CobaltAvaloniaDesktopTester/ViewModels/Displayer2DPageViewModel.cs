using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Cobalt.Avalonia.Desktop.Controls.Displayer2D;
using Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public class Displayer2DPageViewModel : ViewModelBase
{
    public ObservableCollection<DrawingObject> Objects { get; }
    public ObservableCollection<DrawingObjectGroup> Groups { get; }
    public UserInteraction Interaction { get; }

    public Displayer2DPageViewModel()
    {
        Objects =
        [
            new RectangleShape
            {
                X = 20, Y = 20, Width = 150, Height = 80,
                Fill = new SolidColorBrush(Color.Parse("#3574F0")),
                FillHover = new SolidColorBrush(Color.Parse("#7AB0FF")),
                StrokeHover = new SolidColorBrush(Color.Parse("#FFFFFF")),
                Rotation = 45,
                ZIndex = 0
            },
            new EllipseShape
            {
                X = 220, Y = 20, Width = 120, Height = 90,
                Fill = new SolidColorBrush(Color.Parse("#59A869")),
                Stroke = new SolidColorBrush(Color.Parse("#3B8C4B")),
                StrokeThickness = 2,
                FillHover = new SolidColorBrush(Color.Parse("#90E09F")),
                StrokeHover = new SolidColorBrush(Color.Parse("#FFFFFF")),
                ZIndex = 1
            },
            new LineShape
            {
                X = 20, Y = 180, X2 = 340, Y2 = 180,
                Stroke = new SolidColorBrush(Color.Parse("#F75464")),
                StrokeThickness = 3,
                StrokeHover = new SolidColorBrush(Color.Parse("#FFD700")),
                ZIndex = 2
            },
            new PointShape
            {
                X = 200, Y = 130,
                Fill = new SolidColorBrush(Color.Parse("#BCBEC4")),
                FillHover = new SolidColorBrush(Color.Parse("#FFFFFF")),
                StrokeHover = new SolidColorBrush(Color.Parse("#3574F0")),
                ZIndex = 3,
                Width = 15, Height = 15
            },
            new PathShape
            {
                X = 20, Y = 220,
                Geometry = Geometry.Parse("M 0,10 L 30,10 L 30,0 L 50,20 L 30,40 L 30,30 L 0,30 Z"),
                Fill = new SolidColorBrush(Color.Parse("#E8A33D")),
                Stroke = new SolidColorBrush(Color.Parse("#C48832")),
                StrokeThickness = 8.5,
                FillHover = new SolidColorBrush(Color.Parse("#FFD580")),
                StrokeHover = new SolidColorBrush(Color.Parse("#FFFFFF")),
                ZIndex = 4
            },
            new TextShape
            {
                X = 20, Y = 310,
                Text = "Hello, Cobalt!",
                FontSize = 18,
                FontFamily = new FontFamily("Segoe UI"),
                FontWeight = FontWeight.Bold,
                Foreground = new SolidColorBrush(Color.Parse("#BCBEC4")),
                ZIndex = 5
            },
            new ImageShape
            {
                X = 200, Y = 220, Width = 120, Height = 80,
                Source = CreateTestImage(),
                ZIndex = 6
            },
            new RectangleShape
            {
                X = 8, Y = 8, Width = 490, Height = 28,
                Fill = new SolidColorBrush(Color.FromArgb(200, 30, 30, 50)),
                Stroke = new SolidColorBrush(Color.Parse("#3574F0")),
                StrokeThickness = 1,
                IsFixed = true, ZIndex = 100
            },
            new TextShape
            {
                X = 14, Y = 14,
                Text = "Fixed overlay — pan/zoom freely — hover shapes to see FillHover/StrokeHover",
                FontSize = 13,
                Foreground = new SolidColorBrush(Color.Parse("#BCBEC4")),
                IsFixed = true, ZIndex = 101
            }
        ];

        Groups = [new SampleDrawingObjectGroup()];

        Interaction = new PanZoomInteraction();
    }

    private static IImage CreateTestImage()
    {
        var group = new DrawingGroup();
        group.Children.Add(new GeometryDrawing
        {
            Brush = new SolidColorBrush(Color.Parse("#1E1E2E")),
            Geometry = new RectangleGeometry(new Rect(0, 0, 120, 80))
        });
        group.Children.Add(new GeometryDrawing
        {
            Pen = new Pen(new SolidColorBrush(Color.Parse("#3574F0")), 2),
            Geometry = Geometry.Parse("M 0,0 L 120,80 M 120,0 L 0,80")
        });
        group.Children.Add(new GeometryDrawing
        {
            Brush = new SolidColorBrush(Color.Parse("#E8A33D")),
            Pen = new Pen(new SolidColorBrush(Color.Parse("#C48832")), 1.5),
            Geometry = new EllipseGeometry(new Rect(40, 25, 40, 30))
        });
        return new DrawingImage(group);
    }
}

internal sealed class SampleDrawingObjectGroup : DrawingObjectGroup
{
    public SampleDrawingObjectGroup()
    {
        Items.Add(new RectangleShape
        {
            X = 380, Y = 20, Width = 60, Height = 40,
            Fill = new SolidColorBrush(Color.Parse("#3574F0")),
            ZIndex = 0
        });
        Items.Add(new RectangleShape
        {
            X = 460, Y = 20, Width = 60, Height = 40,
            Fill = new SolidColorBrush(Color.Parse("#59A869")),
            ZIndex = 0
        });
        Items.Add(new RectangleShape
        {
            X = 380, Y = 80, Width = 60, Height = 40,
            Fill = new SolidColorBrush(Color.Parse("#E8A33D")),
            ZIndex = 0
        });
    }

    public override void RecalculateCoordinates() { }

    public override void UnregisterEvents()
    {
        UnregisterAllItemEvents();
    }
}

internal sealed class PanZoomInteraction : UserInteraction
{
    public override void OnMouseDown(PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(null).Properties.IsMiddleButtonPressed)
            StartPan_OnMouseDown(e);
    }

    public override void OnMouseUp(PointerReleasedEventArgs e)
    {
        StopPan_OnMouseUp(e);
    }

    public override void OnMouseMove(PointerEventArgs e)
    {
        Pan_OnMouseMove(e);
    }

    public override void OnMouseWheel(PointerWheelEventArgs e)
    {
        Zoom_OnMouseWheel(e);
    }

    public override void OnMouseDoubleClick(TappedEventArgs e)
    {
        ResetZoom_OnMouseDoubleClick(e);
    }
}
