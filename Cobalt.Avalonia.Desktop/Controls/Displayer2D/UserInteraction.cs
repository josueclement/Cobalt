using Avalonia;
using Avalonia.Input;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

public class UserInteraction
{
    // Set by Displayer2D when this interaction is assigned.
    public Displayer2D? Owner { get; internal set; }

    public virtual void OnMouseDown(PointerPressedEventArgs e) { }
    public virtual void OnMouseUp(PointerReleasedEventArgs e) { }
    public virtual void OnMouseMove(PointerEventArgs e) { }
    public virtual void OnMouseWheel(PointerWheelEventArgs e) { }
    public virtual void OnMouseDoubleClick(TappedEventArgs e) { }
    public virtual void OnKeyDown(KeyEventArgs e) { }
    public virtual void OnKeyUp(KeyEventArgs e) { }
    public virtual void OnRenderSizeChanged(global::Avalonia.Size newSize) { }

    private bool _isPanning;
    private Point _lastPoint;
    
    protected void StartPan_OnMouseDown(PointerPressedEventArgs e)
    {
        if (_isPanning)
            return;
        
        _isPanning = true;
        _lastPoint = e.GetPosition(Owner);
        e.Pointer.Capture(e.Source as IInputElement);
    }
    
    protected void StopPan_OnMouseUp(PointerReleasedEventArgs e)
    {
        _isPanning = false;
        e.Pointer.Capture(null);
    }
    
    protected void Pan_OnMouseMove(PointerEventArgs e)
    {
        if (!_isPanning || Owner is null) return;
        
        var pos = e.GetPosition(Owner);
        Owner.PanX += pos.X - _lastPoint.X;
        Owner.PanY += pos.Y - _lastPoint.Y;
        _lastPoint = pos;
    }
    
    protected void Zoom_OnMouseWheel(PointerWheelEventArgs e)
    {
        if (Owner is null) return;

        var zoomDelta = e.Delta.Y > 0 ? 1.4 : 1.0 / 1.4;
        var pivot = e.GetPosition(Owner);
        var worldPivot = Owner.CanvasToWorld(pivot);
        var newZoom = Owner.ZoomFactor * zoomDelta;

        Owner.ZoomFactor = newZoom;
        Owner.PanX = pivot.X - worldPivot.X * newZoom;
        Owner.PanY = pivot.Y - worldPivot.Y * newZoom;
    }
    
    protected void ResetZoom_OnMouseDoubleClick(TappedEventArgs e)
    {
        if (Owner is null) return;
        Owner.ZoomFactor = 1.0;
        Owner.PanX = 0;
        Owner.PanY = 0;
    }

    protected void ZoomToFit_OnMouseDoubleClick(TappedEventArgs e)
    {
        if (Owner is null) return;

        if (Owner.BackgroundImage is not null)
            Owner.ZoomToFit();
        else
            ResetZoom_OnMouseDoubleClick(e);
    }
}
