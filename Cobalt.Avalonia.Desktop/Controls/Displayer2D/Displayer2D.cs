using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

public class Displayer2D : TemplatedControl
{
    public static readonly StyledProperty<ObservableCollection<DrawingObject>?> DrawingObjectsProperty =
        AvaloniaProperty.Register<Displayer2D, ObservableCollection<DrawingObject>?>(nameof(DrawingObjects));

    public static readonly StyledProperty<ObservableCollection<DrawingObjectGroup>?> DrawingObjectGroupsProperty =
        AvaloniaProperty.Register<Displayer2D, ObservableCollection<DrawingObjectGroup>?>(nameof(DrawingObjectGroups));

    public static readonly StyledProperty<UserInteraction?> UserInteractionProperty =
        AvaloniaProperty.Register<Displayer2D, UserInteraction?>(nameof(UserInteraction));

    public static readonly StyledProperty<double> ZoomFactorProperty =
        AvaloniaProperty.Register<Displayer2D, double>(nameof(ZoomFactor), defaultValue: 1.0);

    public static readonly StyledProperty<double> PanXProperty =
        AvaloniaProperty.Register<Displayer2D, double>(nameof(PanX), defaultValue: 0.0);

    public static readonly StyledProperty<double> PanYProperty =
        AvaloniaProperty.Register<Displayer2D, double>(nameof(PanY), defaultValue: 0.0);

    public static readonly StyledProperty<IImage?> BackgroundImageProperty =
        AvaloniaProperty.Register<Displayer2D, IImage?>(nameof(BackgroundImage));

    public static readonly DirectProperty<Displayer2D, Point?> WorldMousePositionProperty =
        AvaloniaProperty.RegisterDirect<Displayer2D, Point?>(
            nameof(WorldMousePosition),
            o => o.WorldMousePosition);

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
    public double PanX { get => GetValue(PanXProperty); set => SetValue(PanXProperty, value); }
    public double PanY { get => GetValue(PanYProperty); set => SetValue(PanYProperty, value); }

    public IImage? BackgroundImage
    {
        get => GetValue(BackgroundImageProperty);
        set => SetValue(BackgroundImageProperty, value);
    }

    private Point? _worldMousePosition;
    public Point? WorldMousePosition
    {
        get => _worldMousePosition;
        set => SetAndRaise(WorldMousePositionProperty, ref _worldMousePosition, value);
    }

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

    public void ZoomToFit(Rect worldBounds, double padding = 20)
    {
        var viewWidth = Bounds.Width - padding * 2;
        var viewHeight = Bounds.Height - padding * 2;
        if (viewWidth <= 0 || viewHeight <= 0 || worldBounds.Width <= 0 || worldBounds.Height <= 0)
            return;

        var zoom = Math.Min(viewWidth / worldBounds.Width, viewHeight / worldBounds.Height);
        var worldCenterX = worldBounds.X + worldBounds.Width / 2;
        var worldCenterY = worldBounds.Y + worldBounds.Height / 2;

        ZoomFactor = zoom;
        PanX = Bounds.Width / 2 - worldCenterX * zoom;
        PanY = Bounds.Height / 2 - worldCenterY * zoom;
    }

    public void ZoomToFit(double padding = 20)
    {
        var bgImage = BackgroundImage;
        if (bgImage is null) return;
        ZoomToFit(new Rect(0, 0, bgImage.Size.Width, bgImage.Size.Height), padding);
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
        else if (change.Property == BackgroundImageProperty
              || change.Property == ZoomFactorProperty
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

    protected override Size ArrangeOverride(Size finalSize)
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

    public void Refresh() => _canvas?.InvalidateVisual();

    private void InvalidateCanvas() => _canvas?.InvalidateVisual();
}
