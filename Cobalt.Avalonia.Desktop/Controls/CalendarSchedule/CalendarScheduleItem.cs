using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.CalendarSchedule;

public class CalendarScheduleItem
{
    public string? Title { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public IBrush? Color { get; set; }
    public string? Description { get; set; }
}

public enum CalendarViewMode
{
    Week,
    Month
}

public enum ScheduleInteractionMode
{
    None,
    Move,
    ResizeTop,
    ResizeBottom
}

public class CalendarScheduleItemChangedEventArgs : EventArgs
{
    public required CalendarScheduleItem Item { get; init; }
    public DateTimeOffset OriginalStart { get; init; }
    public DateTimeOffset OriginalEnd { get; init; }
    public DateTimeOffset NewStart { get; init; }
    public DateTimeOffset NewEnd { get; init; }
}
