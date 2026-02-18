using System;
using Avalonia.Media;
using Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Groups;

public sealed class LineMovingObjectGroup : DrawingObjectGroup
{
    private const double HitboxThickness = 20.0;

    private readonly LineShape _line;
    private readonly RectangleShape _hitbox;
    private readonly CircleShape _point1;
    private readonly CircleShape _point2;

    public IBrush? LineStroke          { get => _line.Stroke;          set => _line.Stroke          = value; }
    public double  LineStrokeThickness { get => _line.StrokeThickness; set => _line.StrokeThickness = value; }

    public LineMovingObjectGroup(double x1, double y1, double x2, double y2)
    {
        _line = new LineShape
        {
            Stroke = new SolidColorBrush(Colors.White),
            StrokeThickness = 2.0,
            ZIndex = 0
        };

        _hitbox = new RectangleShape
        {
            IsFixedHeight = true,
            Fill = new SolidColorBrush(Color.Parse("#33ffffff")),
            Stroke = null,
            ZIndex = 1
        };

        _point1 = new CircleShape
        {
            IsMovable = true,
            IsFixedWidth = true,
            IsFixedHeight = true,
            Width = 12,
            Height = 12,
            Fill = new SolidColorBrush(Color.Parse("#3574F0")),
            FillHover = new SolidColorBrush(Colors.White),
            Stroke = new SolidColorBrush(Colors.White),
            StrokeThickness = 1.5,
            ZIndex = 2
        };

        _point2 = new CircleShape
        {
            IsMovable = true,
            IsFixedWidth = true,
            IsFixedHeight = true,
            Width = 12,
            Height = 12,
            Fill = new SolidColorBrush(Color.Parse("#3574F0")),
            FillHover = new SolidColorBrush(Colors.White),
            Stroke = new SolidColorBrush(Colors.White),
            StrokeThickness = 1.5,
            ZIndex = 2
        };

        _point1.CenterX = x1;
        _point1.CenterY = y1;
        _point2.CenterX = x2;
        _point2.CenterY = y2;

        Items.Add(_line);
        Items.Add(_hitbox);
        Items.Add(_point1);
        Items.Add(_point2);
    }

    public override void RecalculateCoordinates()
    {
        var x1 = _point1.CenterX;
        var y1 = _point1.CenterY;
        var x2 = _point2.CenterX;
        var y2 = _point2.CenterY;

        _line.X  = x1; _line.Y  = y1;
        _line.X2 = x2; _line.Y2 = y2;

        var dx     = x2 - x1;
        var dy     = y2 - y1;
        var length = Math.Sqrt(dx * dx + dy * dy);
        var angle  = Math.Atan2(dy, dx) * 180.0 / Math.PI;

        _hitbox.Width   = length;
        _hitbox.Height  = HitboxThickness;
        _hitbox.CenterX = (x1 + x2) / 2.0;
        _hitbox.CenterY = (y1 + y2) / 2.0;
        _hitbox.Rotation = angle;
    }

    public override void UnregisterEvents()
    {
        UnregisterAllItemEvents();
    }
}
