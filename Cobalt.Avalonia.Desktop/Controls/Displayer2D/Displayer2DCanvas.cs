using System.Linq;
using global::Avalonia.Controls;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

internal sealed class Displayer2DCanvas : Control
{
    internal Displayer2DControl? Owner { get; set; }

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
}
