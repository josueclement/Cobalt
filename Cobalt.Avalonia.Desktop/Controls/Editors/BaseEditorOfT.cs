using Avalonia;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace Cobalt.Avalonia.Desktop.Controls.Editors;

#pragma warning disable AVP1002 // Properties are accessed via non-generic concrete types in XAML
public abstract class BaseEditor<T> : BaseEditor where T : struct
{
    public static readonly StyledProperty<T?> ValueProperty =
        AvaloniaProperty.Register<BaseEditor<T>, T?>(nameof(Value), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public static readonly StyledProperty<string?> FormatStringProperty =
        AvaloniaProperty.Register<BaseEditor<T>, string?>(nameof(FormatString));

    public static readonly StyledProperty<bool> NullWhenEmptyProperty =
        AvaloniaProperty.Register<BaseEditor<T>, bool>(nameof(NullWhenEmpty));
#pragma warning restore AVP1002

    private bool _isSyncing;
    private bool _textModifiedByUser;

    public T? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string? FormatString
    {
        get => GetValue(FormatStringProperty);
        set => SetValue(FormatStringProperty, value);
    }

    /// <summary>
    /// When true, clearing the text sets Value to null.
    /// When false (default), clearing the text sets Value to default(T).
    /// </summary>
    public bool NullWhenEmpty
    {
        get => GetValue(NullWhenEmptyProperty);
        set => SetValue(NullWhenEmptyProperty, value);
    }

    protected abstract bool TryParse(string? text, out T result);
    protected abstract string FormatValue(T value);

    static BaseEditor()
    {
        ValueProperty.Changed.AddClassHandler<BaseEditor<T>>((editor, _) =>
        {
            editor.SyncTextFromValue();
        });

        TextProperty.Changed.AddClassHandler<BaseEditor<T>>((editor, _) =>
        {
            if (!editor._isSyncing)
                editor._textModifiedByUser = true;
            editor.SyncValueFromText();
        });
    }

    private void SyncTextFromValue()
    {
        if (_isSyncing) return;
        _isSyncing = true;
        try
        {
            Text = Value.HasValue ? FormatValue(Value.Value) : null;
            ClearParseError();
        }
        finally
        {
            _isSyncing = false;
        }
    }

    private void SyncValueFromText()
    {
        if (_isSyncing) return;
        _isSyncing = true;
        try
        {
            if (string.IsNullOrEmpty(Text))
            {
                Value = NullWhenEmpty ? null : default(T);
                ClearParseError();
            }
            else if (TryParse(Text, out var parsed))
            {
                Value = parsed;
                ClearParseError();
            }
            else
            {
                SetParseError($"Invalid value '{Text}'");
            }
        }
        finally
        {
            _isSyncing = false;
        }
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        CommitValue();
    }

    private void CommitValue()
    {
        if (!_textModifiedByUser)
        {
            // User didn't edit — just reformat from current Value
            _isSyncing = true;
            try
            {
                Text = Value.HasValue ? FormatValue(Value.Value) : null;
            }
            finally
            {
                _isSyncing = false;
            }
            return;
        }

        _textModifiedByUser = false;

        if (string.IsNullOrEmpty(Text))
        {
            Value = NullWhenEmpty ? null : default(T);
            ClearParseError();
            return;
        }

        if (TryParse(Text, out var parsed))
        {
            _isSyncing = true;
            try
            {
                Value = parsed;
                Text = FormatValue(parsed);
                ClearParseError();
            }
            finally
            {
                _isSyncing = false;
            }
        }
        else
        {
            SetParseError($"Invalid value '{Text}'");
        }
    }
}
