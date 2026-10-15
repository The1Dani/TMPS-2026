using airport_manager.Domain;
using airport_manager.Services;

namespace airport_manager.App;

public class FlightSimulator(FlightService fs, Random random, DateTime start) : ISimulation
{
    public DateTime Now { get; private set; } = start;

    private static readonly string[] Airlines = ["TK", "PC", "LH", "W6"];
    private static readonly string[] Destinations = ["LHR", "CDG", "FRA", "AMS", "FCO", "MAD", "VIE", "ATH"];

    private readonly double _spawnChance = (fs.GetGateCount() + 1) / 60.0;

    private readonly Dictionary<string, int> _nextNumber = new();
    
    private static readonly TimeSpan BoardingWindow = TimeSpan.FromMinutes(40);
    
    private const double TroubleChance = 0.02;
    private static readonly TimeSpan MaxDelay = TimeSpan.FromHours(3);
    public void Tick()
    {
        Now = Now.AddMinutes(1);

        if (random.NextDouble() < _spawnChance)
        {
            ScheduleNewFlight();
        }

        foreach (var flight in fs.GetActiveFlights())
        {
            Advance(flight);
        }
    }
    private void ScheduleNewFlight()
    {
        var airline = Airlines[random.Next(Airlines.Length)];
        var number = _nextNumber.GetValueOrDefault(airline, 100);
        _nextNumber[airline] = number + 1;

        var flight = new Flight(
            $"{airline}{number}",
            airline,
            Destinations[random.Next(Destinations.Length)],
            Now.AddMinutes(random.Next(60, 181)),
            FlightStatus.Scheduled,
            TimeSpan.Zero);

        fs.Schedule(flight, Now);
    }
    
    private void Advance(Flight flight)
    {
        if (flight.Status == FlightStatus.Boarding)
        {
            if (Now >= flight.ExpectedDepartureTime)
            {
                fs.Depart(flight.Number, Now);
            }
            return;
        }
        
        if (flight.Delay > MaxDelay)
        {
            fs.Cancel(flight.Number, Now);
            return;
        }

        if (Now >= flight.ExpectedDepartureTime - BoardingWindow)
        {
            fs.StartBoarding(flight.Number, Now);
        }
        
        if (random.NextDouble() < TroubleChance)
        {
            fs.Delay(flight.Number, TimeSpan.FromMinutes(random.Next(10, 46)), Now);
        }
    }
    
    
    public void Run()
    {
        for (;;)
        {
            var delay = Random.Shared.Next(500, 1000);
            var flightNumber = Random.Shared.Next(50, 100);
            Thread.Sleep(delay);
            try
            {
                
            }
            catch
            {
                Console.WriteLine($"Simulation has ended. Winner: {flightNumber}");
                break;
            }
        }
    }

    
}