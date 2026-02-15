using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Cobalt.Avalonia.Desktop.Controls.Docking;

namespace CobaltAvaloniaDesktopTester.Views;

public partial class DockingTestingPageView : UserControl
{
    public DockingTestingPageView()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        this.Loaded -= OnLoaded;
        SetupDockingLayout();
    }

    private void SetupDockingLayout()
    {
        var dockingControl = this.FindControl<DockingControl>("MainDockingControl");
        if (dockingControl == null) return;

        // Create panes
        var solutionPane = CreatePane("Solution", "Solution Explorer content here");
        var propertiesPane = CreatePane("Properties", "Properties panel content here", canClose: false);
        var outputPane = CreatePane("Output", "Build output here");
        var debugPane = CreatePane("Debug", "Debug console here");
        var doc1Pane = CreatePane("Doc1", "Document 1 content");
        var doc2Pane = CreatePane("Doc2", "Document 2 content");
        var doc3Pane = CreatePane("Doc3", "Document 3 content");

        // Build binary tree structure
        // Structure:
        // Root (Vertical Split)
        // ├─ Top (Horizontal Split)
        // │  ├─ Left (Horizontal Split)
        // │  │  ├─ Solution Group [Solution]
        // │  │  └─ Center Group [Doc1, Doc2, Doc3]
        // │  └─ Properties Group [Properties]
        // └─ Bottom Group [Output, Debug]

        var centerGroup = new DockTabGroup();
        centerGroup.Panes.Add(doc1Pane);
        centerGroup.Panes.Add(doc2Pane);
        centerGroup.Panes.Add(doc3Pane);
        centerGroup.SelectedPane = doc1Pane;

        var solutionGroup = new DockTabGroup();
        solutionGroup.Panes.Add(solutionPane);
        solutionGroup.SelectedPane = solutionPane;

        var propertiesGroup = new DockTabGroup();
        propertiesGroup.Panes.Add(propertiesPane);
        propertiesGroup.SelectedPane = propertiesPane;

        var bottomGroup = new DockTabGroup();
        bottomGroup.Panes.Add(outputPane);
        bottomGroup.Panes.Add(debugPane);
        bottomGroup.SelectedPane = outputPane;

        var leftCenterSplit = new DockSplitContainer
        {
            Orientation = Orientation.Horizontal,
            First = solutionGroup,
            Second = centerGroup
        };

        var topSplit = new DockSplitContainer
        {
            Orientation = Orientation.Horizontal,
            First = leftCenterSplit,
            Second = propertiesGroup
        };

        var rootSplit = new DockSplitContainer
        {
            Orientation = Orientation.Vertical,
            First = topSplit,
            Second = bottomGroup
        };

        dockingControl.SetRootLayout(rootSplit);
    }

    private DockPane CreatePane(string header, string content, bool canClose = true, bool canMove = true)
    {
        return new DockPane
        {
            Header = header,
            CanClose = canClose,
            CanMove = canMove,
            PaneContent = new Border
            {
                Padding = new Thickness(16),
                Child = new StackPanel
                {
                    Spacing = 8,
                    Children =
                    {
                        new TextBlock
                        {
                            Text = header,
                            FontSize = 18,
                            FontWeight = FontWeight.SemiBold
                        },
                        new TextBlock
                        {
                            Text = content,
                            [!TextBlock.ForegroundProperty] = new DynamicResourceExtension("CobaltForegroundSecondaryBrush")
                        }
                    }
                }
            }
        };
    }
}
