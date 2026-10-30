using airport_manager.Infra.Traffic;
using airport_manager.Domain;
using airport_manager.Services;

namespace airport_manager.App;

public class FlightSimulator(FlightService fs, Random random, DateTime start, TrafficMix traffic) : ISimulation
{
    public DateTime Now { get; private set; } = start;

    private readonly double _spawnChance = (fs.GetGateCount() + 1) / 60.0;

    private const double TroubleChance = 0.02 / 60;
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
        var flight = traffic
            .Pick()
            .CreateFlight(Now);
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

        if (Now >= flight.ExpectedDepartureTime - flight.BoardingWindow)
        {
            fs.StartBoarding(flight.Number, Now);
            return;
        }
        
        if (random.NextDouble() < TroubleChance)
        {
            fs.Delay(flight.Number, TimeSpan.FromMinutes(random.Next(10, 46)), Now);
        }
    }
}
