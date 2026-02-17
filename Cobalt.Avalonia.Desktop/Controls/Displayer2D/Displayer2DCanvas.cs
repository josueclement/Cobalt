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
            if (obj.IsVisible)
                obj.Render(context);
        }
    }
}
