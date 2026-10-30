using airport_manager.Domain;
using airport_manager.Infra.Traffic;
using airport_manager.UI.Tui;
using airport_manager.Services;
using airport_manager.Infra;

namespace airport_manager.App

{
    internal static class Program
    {
        private static void Main()
        {
            var eventBus = new EventBus();
            var flightService =
                new FlightService(new InMemoryFlightRepository(), new GateAllocator(["A1", "A2", "A3", "A4"]), eventBus);

            var random = new Random((int)DateTime.Now.ToBinary());
            var director = new FlightDirector(new FlightBuilder());
            var traffic = new TrafficMix(random, [
                (new PassengerFlightFactory(random, director), 0.7),
                (new CargoFlightFactory(random, director), 0.15),
                (new CharterFlightFactory(random, director), 0.15),
            ]);
            var flightSim = new FlightSimulator(flightService, random, DateTime.Today.AddHours(6), traffic);

            var dashboard = new DashboardState();
            eventBus.Sub(dashboard);
            new TuiApp(flightSim, dashboard).Run();
        }
    }
}
