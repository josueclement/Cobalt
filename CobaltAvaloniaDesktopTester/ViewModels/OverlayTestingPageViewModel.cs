using System.Diagnostics;
using System.Threading.Tasks;
using global::Avalonia.Controls;
using global::Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cobalt.Avalonia.Desktop.Services;
using CobaltAvaloniaDesktopTester.Controls;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public class OverlayTestingPageViewModel : ViewModelBase
{
    public OverlayTestingPageViewModel(IOverlayService overlayService)
    {
        _overlayService = overlayService;
        RunSimpleTaskCommand = new AsyncRelayCommand(RunSimpleTask, CanRunTask);
        RunComplexTaskCommand = new AsyncRelayCommand(RunComplexTask, CanRunTask);
    }

    private readonly IOverlayService _overlayService;

    public string? LastResult
    {
        get;
        set => SetProperty(ref field, value);
    }

    public bool IsBusy
    {
        get;
        private set
        {
            if (!SetProperty(ref field, value)) return;
            RunSimpleTaskCommand.NotifyCanExecuteChanged();
            RunComplexTaskCommand.NotifyCanExecuteChanged();
        }
    }

    public IAsyncRelayCommand RunSimpleTaskCommand { get; }
    public IAsyncRelayCommand RunComplexTaskCommand { get; }

    private bool CanRunTask() => !IsBusy;

    private async Task RunSimpleTask()
    {
        IsBusy = true;
        var sw = Stopwatch.StartNew();

        try
        {
            // Create the progress card
            var card = new ProgressOverlayCard
            {
                Title = "Processing",
                IsIndeterminate = true,
                Message = "Initializing..."
            };

            // Phase 1: Indeterminate - Initializing
            await _overlayService.ShowAsync(card);
            await Task.Delay(2000);

            // Phase 2: Determinate - Steps 1-2
            card.IsIndeterminate = false;
            card.Progress = 0;
            card.Message = "Step 1 of 4...";
            await Task.Delay(1000);

            card.Progress = 25;
            card.Message = "Step 2 of 4...";
            await Task.Delay(1000);

            card.Progress = 50;

            // Phase 3: Indeterminate - Analyzing
            card.IsIndeterminate = true;
            card.Message = "Analyzing results...";
            await Task.Delay(2000);

            // Phase 4: Determinate - Steps 3-4
            card.IsIndeterminate = false;
            card.Progress = 75;
            card.Message = "Step 3 of 4...";
            await Task.Delay(1000);

            card.Progress = 100;
            card.Message = "Step 4 of 4...";
            await Task.Delay(500);

            await _overlayService.HideAsync();
            sw.Stop();
            LastResult = $"Task completed in {sw.Elapsed.TotalSeconds:F1} seconds";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task RunComplexTask()
    {
        IsBusy = true;
        var sw = Stopwatch.StartNew();

        var steps = new[]
        {
            "Validating input data",
            "Connecting to service",
            "Processing records",
            "Generating report",
            "Finalizing"
        };

        try
        {
            // Create the progress card with initial step list
            var card = new ProgressOverlayCard
            {
                Title = "Complex Operation",
                IsIndeterminate = true,
                Message = "Starting...",
                Content = BuildStepList(steps, -1)
            };

            await _overlayService.ShowAsync(card);

            for (int i = 0; i < steps.Length; i++)
            {
                // Switch to indeterminate while "working" on this step
                card.IsIndeterminate = true;
                card.Message = steps[i] + "...";
                card.Content = BuildStepList(steps, i - 1);
                await Task.Delay(1500);

                // Mark step complete with determinate progress
                int completed = i + 1;
                card.IsIndeterminate = false;
                card.Progress = (double)completed / steps.Length * 100;
                card.Content = BuildStepList(steps, i);
                await Task.Delay(500);
            }

            card.Message = "Done!";
            await Task.Delay(500);

            await _overlayService.HideAsync();
            sw.Stop();
            LastResult = $"Task completed in {sw.Elapsed.TotalSeconds:F1} seconds";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static StackPanel BuildStepList(string[] steps, int completedUpTo)
    {
        var panel = new StackPanel { Spacing = 4 };

        for (int i = 0; i < steps.Length; i++)
        {
            var prefix = i <= completedUpTo ? "\u2713 " : "  ";
            var tb = new TextBlock
            {
                Text = prefix + steps[i],
                Foreground = i <= completedUpTo
                    ? new SolidColorBrush(Color.Parse("#4CAF50"))
                    : new SolidColorBrush(Color.Parse("#888888"))
            };
            panel.Children.Add(tb);
        }

        return panel;
    }
}
