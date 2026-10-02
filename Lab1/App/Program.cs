using airport_manager.Infra;
using airport_manager.Services;
using airport_manager.UI;

namespace airport_manager

{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var eventBus = new EventBus();
            _ = new ConsoleWriter(eventBus);

            var flightService = new FlightService(new InMemoryFlightRepository(), eventBus);


            for (;;)
            {
                var delay = Random.Shared.Next(500, 1000);
                var flightNumber = Random.Shared.Next(50, 100);
                Thread.Sleep(delay);
                try
                {
                    flightService.ScheduleFlight(new Flight(
                        $"{flightNumber}",
                        "THY",
                        "IST",
                        "",
                        DateTime.Now,
                        TimeSpan.FromMilliseconds(delay)
                    ));
                }
                catch
                {
                    Console.WriteLine($"Simulation has ended. Winner: {flightNumber}");
                    break;
                }
            }
        }
    }
}