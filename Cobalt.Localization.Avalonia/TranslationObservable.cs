namespace Cobalt.Localization.Avalonia;

public class TranslationObservable : IObservable<object?>
{
    private readonly string _key;
    private readonly string? _defaultText;

    public TranslationObservable(string key, string? defaultText)
    {
        _key = key;
        _defaultText = defaultText;
    }

    public IDisposable Subscribe(IObserver<object?> observer)
    {
        return new TranslationSubscription(_key, _defaultText, observer);
    }

    private sealed class TranslationSubscription : IDisposable
    {
        private readonly string _key;
        private readonly string? _defaultText;
        private readonly IObserver<object?> _observer;
        private bool _disposed;

        public TranslationSubscription(string key, string? defaultText, IObserver<object?> observer)
        {
            _key = key;
            _defaultText = defaultText;
            _observer = observer;

            LocalizationManager.Service.CurrentLanguageChanged += OnLanguageChanged;
            Emit();
        }

        private void OnLanguageChanged(object? sender, string newLanguage)
        {
            Emit();
        }

        private void Emit()
        {
            var text = _defaultText is not null
                ? LocalizationManager.Service.GetTranslation(_key, _defaultText)
                : LocalizationManager.Service.GetTranslation(_key);

            _observer.OnNext(text ?? _key);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            LocalizationManager.Service.CurrentLanguageChanged -= OnLanguageChanged;
        }
    }
}
