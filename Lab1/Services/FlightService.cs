namespace airport_manager.Services;

public class FlightService(IFlightRepository repo, IEventSink eventSink)
{
    public void ScheduleFlight(Flight flight)
    {
        var f = repo.Get(flight.Number);
        if (f != null)
        {
            throw new Exception($"Flight {flight.Number} already exists");
        }
        
        repo.Add(flight);
        eventSink.Pub(new AirportEvent($"flight, {flight}"));
        
    }
    
}