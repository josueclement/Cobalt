using System.Collections.ObjectModel;
using global::Avalonia;
using global::Avalonia.Input;
using global::Avalonia.Media;
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
                ZIndex = 0
            },
            new EllipseShape
            {
                X = 220, Y = 20, Width = 120, Height = 90,
                Fill = new SolidColorBrush(Color.Parse("#59A869")),
                Stroke = new SolidColorBrush(Color.Parse("#3B8C4B")),
                StrokeThickness = 2,
                ZIndex = 1
            },
            new LineShape
            {
                X = 20, Y = 180, X2 = 340, Y2 = 180,
                Stroke = new SolidColorBrush(Color.Parse("#F75464")),
                StrokeThickness = 3,
                ZIndex = 2
            },
            new PointShape
            {
                X = 200, Y = 130,
                Fill = new SolidColorBrush(Color.Parse("#BCBEC4")),
                ZIndex = 3
            },
            new PathShape
            {
                X = 20, Y = 220,
                Geometry = global::Avalonia.Media.Geometry.Parse("M 0,10 L 30,10 L 30,0 L 50,20 L 30,40 L 30,30 L 0,30 Z"),
                Fill = new SolidColorBrush(Color.Parse("#E8A33D")),
                Stroke = new SolidColorBrush(Color.Parse("#C48832")),
                StrokeThickness = 1.5,
                ZIndex = 4
            },
            new TextShape
            {
                X = 20, Y = 310,
                Text = "Hello, Cobalt!",
                FontSize = 18,
                FontFamily = new global::Avalonia.Media.FontFamily("Segoe UI"),
                FontWeight = global::Avalonia.Media.FontWeight.Bold,
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
                X = 8, Y = 8, Width = 240, Height = 28,
                Fill = new SolidColorBrush(Color.FromArgb(200, 30, 30, 50)),
                Stroke = new SolidColorBrush(Color.Parse("#3574F0")),
                StrokeThickness = 1,
                IsFixed = true, ZIndex = 100
            },
            new TextShape
            {
                X = 14, Y = 14,
                Text = "Fixed overlay — pan/zoom freely",
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
    private bool _isPanning;
    private global::Avalonia.Point _lastPoint;

    public override void OnMouseDown(PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
        {
            _isPanning = true;
            _lastPoint = e.GetPosition(Owner);
            e.Pointer.Capture(e.Source as global::Avalonia.Input.IInputElement);
        }
    }

    public override void OnMouseUp(PointerReleasedEventArgs e)
    {
        _isPanning = false;
        e.Pointer.Capture(null);
    }

    public override void OnMouseMove(PointerEventArgs e)
    {
        if (!_isPanning || Owner is null) return;

        var pos = e.GetPosition(Owner);
        Owner.PanX += pos.X - _lastPoint.X;
        Owner.PanY += pos.Y - _lastPoint.Y;
        _lastPoint = pos;
    }

    public override void OnMouseWheel(PointerWheelEventArgs e)
    {
        if (Owner is null) return;

        var zoomDelta = e.Delta.Y > 0 ? 1.1 : 1.0 / 1.1;
        var pivot = e.GetPosition(Owner);
        var worldPivot = Owner.CanvasToWorld(pivot);
        var newZoom = Owner.ZoomFactor * zoomDelta;

        Owner.ZoomFactor = newZoom;
        Owner.PanX = pivot.X - worldPivot.X * newZoom;
        Owner.PanY = pivot.Y - worldPivot.Y * newZoom;
    }

    public override void OnMouseDoubleClick(TappedEventArgs e)
    {
        if (Owner is null) return;
        Owner.ZoomFactor = 1.0;
        Owner.PanX = 0;
        Owner.PanY = 0;
    }
}
