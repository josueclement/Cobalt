using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

internal sealed class Displayer2DCanvas : Control
{
    internal Displayer2DControl? Owner { get; set; }

    public Displayer2DCanvas()
    {
        PointerMoved += OnPointerMoved;
        PointerExited += OnPointerExited;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (Owner is null) return;

        // Clip to canvas bounds to prevent overflow artifacts during rapid zoom
        using var clip = context.PushClip(new global::Avalonia.Rect(Bounds.Size));

        var zoom = Owner.ZoomFactor;
        var panX = Owner.PanX;
        var panY = Owner.PanY;

        var objects = Enumerable.Empty<DrawingObject>();

        if (Owner.DrawingObjects != null)
            objects = objects.Concat(Owner.DrawingObjects);

        if (Owner.DrawingObjectGroups != null)
        {
            foreach (var group in Owner.DrawingObjectGroups)
                objects = objects.Concat(group.Items);
        }

        foreach (var obj in objects.OrderBy(o => o.ZIndex))
        {
            if (!obj.IsVisible) continue;
            obj.RecalculateCoordinates(zoom, panX, panY);
            obj.Render(context);
        }
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(this);
        UpdateHoverState(pos);
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        UpdateHoverState(null);
    }

    private void UpdateHoverState(global::Avalonia.Point? pos)
    {
        if (Owner is null) return;

        bool changed = false;

        var objects = Enumerable.Empty<DrawingObject>();
        if (Owner.DrawingObjects != null)
            objects = objects.Concat(Owner.DrawingObjects);
        if (Owner.DrawingObjectGroups != null)
            foreach (var group in Owner.DrawingObjectGroups)
                objects = objects.Concat(group.Items);

        foreach (var obj in objects)
        {
            if (obj is not Shape shape) continue;
            bool shouldHover = pos.HasValue && shape.IsVisible && shape.HitTest(pos.Value);
            if (shape.IsHovered != shouldHover)
            {
                shape.IsHovered = shouldHover;
                changed = true;
            }
        }

        if (changed) InvalidateVisual();
    }
}
