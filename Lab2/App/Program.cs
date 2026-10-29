using airport_manager.Domain.Infra;
using airport_manager.Domain.UI;
using airport_manager.Services;
using airport_manager.UI.Tui;

namespace airport_manager.App

{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var eventBus = new EventBus();
            var flightService =
                new FlightService(new InMemoryFlightRepository(), new GateAllocator(["A1", "A2", "A3", "A4"]), eventBus);
            var flightSim = new FlightSimulator(flightService, new Random((int)DateTime.Now.ToBinary()), DateTime.Today.AddHours(6));
            
            if (args.Contains("--tui"))
            {
                var dashboard = new DashboardState();
                eventBus.Sub(dashboard);
                new TuiApp(flightSim, dashboard).Run();
                return;
            }
            else
            {
                var consoleWriter = new ConsoleWriter();
                eventBus.Sub(consoleWriter);
            }

            
            flightSim.Run();
        }
    }
}