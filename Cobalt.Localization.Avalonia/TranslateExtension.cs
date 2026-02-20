using global::Avalonia.Markup.Xaml;

namespace Cobalt.Localization.Avalonia;

public class TranslateExtension : MarkupExtension
{
    public string Key { get; set; }

    public string? DefaultText { get; set; }

    public TranslateExtension(string key)
    {
        Key = key;
    }

    public TranslateExtension()
    {
        Key = string.Empty;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new TranslationObservable(Key, DefaultText);
    }
}
