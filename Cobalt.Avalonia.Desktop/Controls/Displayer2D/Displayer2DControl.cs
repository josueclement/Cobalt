using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using global::Avalonia;
using global::Avalonia.Controls;
using global::Avalonia.Controls.Primitives;
using global::Avalonia.Input;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

public class Displayer2DControl : TemplatedControl
{
    public static readonly StyledProperty<ObservableCollection<DrawingObject>?> DrawingObjectsProperty =
        AvaloniaProperty.Register<Displayer2DControl, ObservableCollection<DrawingObject>?>(nameof(DrawingObjects));

    public static readonly StyledProperty<ObservableCollection<DrawingObjectGroup>?> DrawingObjectGroupsProperty =
        AvaloniaProperty.Register<Displayer2DControl, ObservableCollection<DrawingObjectGroup>?>(nameof(DrawingObjectGroups));

    public static readonly StyledProperty<UserInteraction?> UserInteractionProperty =
        AvaloniaProperty.Register<Displayer2DControl, UserInteraction?>(nameof(UserInteraction));

    public static readonly StyledProperty<double> ZoomFactorProperty =
        AvaloniaProperty.Register<Displayer2DControl, double>(nameof(ZoomFactor), defaultValue: 1.0);

    public static readonly StyledProperty<double> PanXProperty =
        AvaloniaProperty.Register<Displayer2DControl, double>(nameof(PanX), defaultValue: 0.0);

    public static readonly StyledProperty<double> PanYProperty =
        AvaloniaProperty.Register<Displayer2DControl, double>(nameof(PanY), defaultValue: 0.0);

    public ObservableCollection<DrawingObject>? DrawingObjects
    {
        get => GetValue(DrawingObjectsProperty);
        set => SetValue(DrawingObjectsProperty, value);
    }

    public ObservableCollection<DrawingObjectGroup>? DrawingObjectGroups
    {
        get => GetValue(DrawingObjectGroupsProperty);
        set => SetValue(DrawingObjectGroupsProperty, value);
    }

    public UserInteraction? UserInteraction
    {
        get => GetValue(UserInteractionProperty);
        set => SetValue(UserInteractionProperty, value);
    }

    public double ZoomFactor { get => GetValue(ZoomFactorProperty); set => SetValue(ZoomFactorProperty, value); }
    public double PanX       { get => GetValue(PanXProperty);       set => SetValue(PanXProperty,       value); }
    public double PanY       { get => GetValue(PanYProperty);       set => SetValue(PanYProperty,       value); }

    public Point WorldToCanvas(Point worldPoint)
    {
        var zoom = ZoomFactor;
        return new Point(worldPoint.X * zoom + PanX, worldPoint.Y * zoom + PanY);
    }

    public Point CanvasToWorld(Point canvasPoint)
    {
        var zoom = ZoomFactor;
        if (zoom == 0.0) return canvasPoint;
        return new Point((canvasPoint.X - PanX) / zoom, (canvasPoint.Y - PanY) / zoom);
    }

    private Displayer2DCanvas? _canvas;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // Detach from old canvas
        if (_canvas != null)
            _canvas.Owner = null;

        _canvas = e.NameScope.Find<Displayer2DCanvas>("PART_Canvas");

        if (_canvas != null)
            _canvas.Owner = this;

        if (UserInteraction != null)
            UserInteraction.Owner = this;

        // Detach old pointer/key events then reattach
        PointerPressed -= OnPointerPressed;
        PointerReleased -= OnPointerReleased;
        PointerMoved -= OnPointerMoved;
        PointerWheelChanged -= OnPointerWheelChanged;
        DoubleTapped -= OnDoubleTapped;
        KeyDown -= OnKeyDown;
        KeyUp -= OnKeyUp;

        PointerPressed += OnPointerPressed;
        PointerReleased += OnPointerReleased;
        PointerMoved += OnPointerMoved;
        PointerWheelChanged += OnPointerWheelChanged;
        DoubleTapped += OnDoubleTapped;
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;

        InvalidateCanvas();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DrawingObjectsProperty)
        {
            if (change.OldValue is ObservableCollection<DrawingObject> oldObjects)
            {
                oldObjects.CollectionChanged -= OnDrawingObjectsCollectionChanged;
                foreach (var obj in oldObjects)
                    obj.PropertyChanged -= OnDrawingObjectPropertyChanged;
            }

            if (change.NewValue is ObservableCollection<DrawingObject> newObjects)
            {
                newObjects.CollectionChanged += OnDrawingObjectsCollectionChanged;
                foreach (var obj in newObjects)
                    obj.PropertyChanged += OnDrawingObjectPropertyChanged;
            }

            InvalidateCanvas();
        }
        else if (change.Property == UserInteractionProperty)
        {
            if (change.OldValue is UserInteraction oldInteraction)
                oldInteraction.Owner = null;
            if (change.NewValue is UserInteraction newInteraction)
                newInteraction.Owner = this;
        }
        else if (change.Property == ZoomFactorProperty
              || change.Property == PanXProperty
              || change.Property == PanYProperty)
        {
            InvalidateCanvas();
        }
        else if (change.Property == DrawingObjectGroupsProperty)
        {
            if (change.OldValue is ObservableCollection<DrawingObjectGroup> oldGroups)
            {
                oldGroups.CollectionChanged -= OnGroupsCollectionChanged;
                foreach (var group in oldGroups)
                {
                    group.PropertyChanged -= OnDrawingObjectPropertyChanged;
                    foreach (var item in group.Items)
                        item.PropertyChanged -= OnDrawingObjectPropertyChanged;
                }
            }

            if (change.NewValue is ObservableCollection<DrawingObjectGroup> newGroups)
            {
                newGroups.CollectionChanged += OnGroupsCollectionChanged;
                foreach (var group in newGroups)
                {
                    group.PropertyChanged += OnDrawingObjectPropertyChanged;
                    foreach (var item in group.Items)
                        item.PropertyChanged += OnDrawingObjectPropertyChanged;
                }
            }

            InvalidateCanvas();
        }
    }

    private void OnDrawingObjectsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (DrawingObject obj in e.OldItems)
                obj.PropertyChanged -= OnDrawingObjectPropertyChanged;
        }

        if (e.NewItems != null)
        {
            foreach (DrawingObject obj in e.NewItems)
                obj.PropertyChanged += OnDrawingObjectPropertyChanged;
        }

        InvalidateCanvas();
    }

    private void OnGroupsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (DrawingObjectGroup group in e.OldItems)
            {
                group.PropertyChanged -= OnDrawingObjectPropertyChanged;
                foreach (var item in group.Items)
                    item.PropertyChanged -= OnDrawingObjectPropertyChanged;
            }
        }

        if (e.NewItems != null)
        {
            foreach (DrawingObjectGroup group in e.NewItems)
            {
                group.PropertyChanged += OnDrawingObjectPropertyChanged;
                foreach (var item in group.Items)
                    item.PropertyChanged += OnDrawingObjectPropertyChanged;
            }
        }

        InvalidateCanvas();
    }

    private void OnDrawingObjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        InvalidateCanvas();
    }

    protected override global::Avalonia.Size ArrangeOverride(global::Avalonia.Size finalSize)
    {
        var result = base.ArrangeOverride(finalSize);
        UserInteraction?.OnRenderSizeChanged(result);
        return result;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e) =>
        UserInteraction?.OnMouseDown(e);

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e) =>
        UserInteraction?.OnMouseUp(e);

    private void OnPointerMoved(object? sender, PointerEventArgs e) =>
        UserInteraction?.OnMouseMove(e);

    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e) =>
        UserInteraction?.OnMouseWheel(e);

    private void OnDoubleTapped(object? sender, TappedEventArgs e) =>
        UserInteraction?.OnMouseDoubleClick(e);

    private void OnKeyDown(object? sender, KeyEventArgs e) =>
        UserInteraction?.OnKeyDown(e);

    private void OnKeyUp(object? sender, KeyEventArgs e) =>
        UserInteraction?.OnKeyUp(e);

    private void InvalidateCanvas() => _canvas?.InvalidateVisual();
}
