using airport_manager.Domain.Ports;
using airport_manager.Domain;

namespace airport_manager.Services;

public class FlightService(IFlightRepository repo, GateAllocator gates, IEventSink eventSink)
{
    private Flight GetExisting(string number)
    {
        return repo.Get(number) ?? throw new Exception($"Flight {number} does not exist");
    }

    public void Schedule(Flight flight, DateTime now)
    {
        var f = repo.Get(flight.Number);
        if (f != null)
        {
            throw new Exception($"Flight {flight.Number} already exists");
        }

        repo.Add(flight);
        eventSink.Pub(new FlightScheduled(flight, now));
    }

    public void Depart(string number, DateTime now)
    {
        var departed = GetExisting(number).Depart(now);
        gates.Release(departed.Gate!);
        departed = departed.ReleaseGate();
        repo.Update(departed);
        eventSink.Pub(new FlightDeparted(departed, now));
    }

    public void Delay(string number, TimeSpan by, DateTime now)
    {
        var flight = GetExisting(number);
        flight = flight.DelayBy(by);
        repo.Update(flight);
        eventSink.Pub(new FlightDelayed(flight, now));
    }

    public void StartBoarding(string number, DateTime now)
    {
        var flight = GetExisting(number);
        if (gates.TryAssign(out var gate))
        {
            flight = flight.StartBoarding(gate);
            repo.Update(flight);
            eventSink.Pub(new FlightBoarding(flight, now));
        }
        else
        {
            Delay(number, TimeSpan.FromMinutes(15), now);
        }
    }

    public void Cancel(string number, DateTime now)
    {
        var flight = GetExisting(number).Cancel();
        if (flight.Gate != null) gates.Release(flight.Gate);
        repo.Update(flight);
        eventSink.Pub(new FlightCancelled(flight, now));
    }

    public IReadOnlyCollection<Flight> GetActiveFlights()
    {
        return repo.GetActive();
    }

    public int GetGateCount()
    {
        return gates.GateCount;
    }
}