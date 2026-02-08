using System.Diagnostics;
using System.Threading.Tasks;
using global::Avalonia.Controls;
using global::Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cobalt.Avalonia.Desktop.Services;

namespace Enigma.Avalonia.ViewModels;

public partial class OverlayTestingPageViewModel : ViewModelBase
{
    private readonly IOverlayService _overlayService;

    [ObservableProperty]
    private string? _lastResult;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RunSimpleTaskCommand))]
    [NotifyCanExecuteChangedFor(nameof(RunComplexTaskCommand))]
    private bool _isBusy;

    public OverlayTestingPageViewModel(IOverlayService overlayService)
    {
        _overlayService = overlayService;
    }

    private bool CanRunTask() => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanRunTask))]
    private async Task RunSimpleTask()
    {
        IsBusy = true;
        var sw = Stopwatch.StartNew();

        try
        {
            // Phase 1: Indeterminate - Initializing
            _overlayService.Show(o =>
            {
                o.Title = "Processing";
                o.IsIndeterminate = true;
                o.Message = "Initializing...";
            });
            await Task.Delay(2000);

            // Phase 2: Determinate - Steps 1-2
            _overlayService.Update(o =>
            {
                o.IsIndeterminate = false;
                o.Progress = 0;
                o.Message = "Step 1 of 4...";
            });
            await Task.Delay(1000);

            _overlayService.Update(o =>
            {
                o.Progress = 25;
                o.Message = "Step 2 of 4...";
            });
            await Task.Delay(1000);

            _overlayService.Update(o => o.Progress = 50);

            // Phase 3: Indeterminate - Analyzing
            _overlayService.Update(o =>
            {
                o.IsIndeterminate = true;
                o.Message = "Analyzing results...";
            });
            await Task.Delay(2000);

            // Phase 4: Determinate - Steps 3-4
            _overlayService.Update(o =>
            {
                o.IsIndeterminate = false;
                o.Progress = 75;
                o.Message = "Step 3 of 4...";
            });
            await Task.Delay(1000);

            _overlayService.Update(o =>
            {
                o.Progress = 100;
                o.Message = "Step 4 of 4...";
            });
            await Task.Delay(500);

            _overlayService.Hide();
            sw.Stop();
            LastResult = $"Task completed in {sw.Elapsed.TotalSeconds:F1} seconds";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanRunTask))]
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
            _overlayService.Show(o =>
            {
                o.Title = "Complex Operation";
                o.IsIndeterminate = true;
                o.Message = "Starting...";
                o.Content = BuildStepList(steps, -1);
            });

            for (int i = 0; i < steps.Length; i++)
            {
                // Switch to indeterminate while "working" on this step
                _overlayService.Update(o =>
                {
                    o.IsIndeterminate = true;
                    o.Message = steps[i] + "...";
                    o.Content = BuildStepList(steps, i - 1);
                });
                await Task.Delay(1500);

                // Mark step complete with determinate progress
                int completed = i + 1;
                _overlayService.Update(o =>
                {
                    o.IsIndeterminate = false;
                    o.Progress = (double)completed / steps.Length * 100;
                    o.Content = BuildStepList(steps, i);
                });
                await Task.Delay(500);
            }

            _overlayService.Update(o => o.Message = "Done!");
            await Task.Delay(500);

            _overlayService.Hide();
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
