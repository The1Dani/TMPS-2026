using airport_manager.App;
using Spectre.Console;

namespace airport_manager.UI.Tui;

public class TuiApp(ISimulation simulation, DashboardState state)
{
    // Speed = simulated minutes per real second. Each +/- press changes it by SpeedStep.
    private const int MinSpeed = 1;
    private const int MaxSpeed = 20;
    private const int SpeedStep = 1;

    private bool _paused;
    private volatile bool _quit;
    private int _speed = 3;

    public void Run()
    {
        // Ctrl+C would otherwise kill the process before the terminal is restored.
        Console.CancelKeyPress += OnCancelKeyPress;
        try
        {
            AnsiConsole.AlternateScreen(RunLive);
        }
        finally
        {
            Console.CancelKeyPress -= OnCancelKeyPress;
        }
    }

    private void RunLive()
    {
        AnsiConsole.Live(Render())
            .Start(ctx =>
            {
                while (!_quit)
                {
                    HandleInput();
                    if (!_paused)
                    {
                        simulation.Tick();
                    }

                    ctx.UpdateTarget(Render());
                    Thread.Sleep(1000 / _speed);
                }
            });
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;
        _quit = true;
    }

    private Layout Render() => DashboardRenderer.Render(state, simulation.Now, _paused, _speed);

    private void HandleInput()
    {
        while (!Console.IsInputRedirected && Console.KeyAvailable)
        {
            switch (Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.Spacebar:
                    _paused = !_paused;
                    break;
                case ConsoleKey.OemPlus or ConsoleKey.Add:
                    _speed = Math.Min(MaxSpeed, _speed + SpeedStep);
                    break;
                case ConsoleKey.OemMinus or ConsoleKey.Subtract:
                    _speed = Math.Max(MinSpeed, _speed - SpeedStep);
                    break;
                case ConsoleKey.Q or ConsoleKey.Escape:
                    _quit = true;
                    break;
            }
        }
    }
}
