using global::Avalonia;
using global::Avalonia.Input;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

public class UserInteraction
{
    public virtual void OnMouseDown(PointerPressedEventArgs e) { }
    public virtual void OnMouseUp(PointerReleasedEventArgs e) { }
    public virtual void OnMouseMove(PointerEventArgs e) { }
    public virtual void OnMouseWheel(PointerWheelEventArgs e) { }
    public virtual void OnMouseDoubleClick(TappedEventArgs e) { }
    public virtual void OnKeyDown(KeyEventArgs e) { }
    public virtual void OnKeyUp(KeyEventArgs e) { }
    public virtual void OnRenderSizeChanged(global::Avalonia.Size newSize) { }
}
