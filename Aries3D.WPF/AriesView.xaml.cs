using System.Windows.Controls;
using Aries3D.Core;
using Stride.CommunityToolkit.Engine;
using Stride.Core.Diagnostics;
using Stride.Core.Presentation.Controls;
using Stride.Engine;
using Stride.Games;

namespace Aries3D.WPF;

/// <summary>
/// Interaction logic for AriesView.xaml
/// </summary>
public partial class AriesView : UserControl
{
    private Thread gameThread;

    private readonly TaskCompletionSource<bool> gameStartedTaskSource = new TaskCompletionSource<bool>();

    private IntPtr windowHandle;

    public AriesView()
    {
        InitializeComponent();

        gameThread = new Thread(SafeAction.Wrap(GameRunThread))
        {
            IsBackground = true,
            Name = "Game Thread"
        };
        gameThread.SetApartmentState(ApartmentState.STA);

        Loaded += (sender, args) =>
        {
            StartGame();
        };
    }

    private async Task StartGame()
    {
        gameThread.Start();

        await gameStartedTaskSource.Task;

        SceneView.Content = new GameEngineHost(windowHandle);
    }

    private void GameRunThread()
    {
        // Create the form from this thread
        // EmbeddedGameForm is in Stride.Editor. You may need to copy this class to your own project.
        var form = new EmbeddedGameForm()
        {
            TopLevel = false,
            Visible = false
        };
        windowHandle = form.Handle;

        var context = new GameContextWinforms(form);

        gameStartedTaskSource.SetResult(true);
        var game = new TeapotDemo();
        game.Run(context, (Scene scene) =>
        {
            game.Window.IsBorderLess = true;
            game.SetupBase3DScene();
        });
    }
}