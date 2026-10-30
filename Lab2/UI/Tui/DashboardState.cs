using airport_manager.Domain.Ports;
using airport_manager.Domain;

namespace airport_manager.UI.Tui;

public class DashboardState : IEventSub
{
    private const int MaxLogLines = 15;
    private static readonly TimeSpan KeepAfterDeparture = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan KeepAfterCancel = TimeSpan.FromMinutes(15);

    private readonly Dictionary<string, Flight> _flights = new();
    private readonly Dictionary<string, DateTime> _removeAt = new();
    private readonly Queue<AirportEvent> _log = new();

    public IReadOnlyCollection<Flight> Flights => _flights.Values;
    public IReadOnlyCollection<AirportEvent> Log => _log;

    public void Update(AirportEvent e)
    {
        if (e is FlightEvent flightEvent)
        {
            var flight = flightEvent.Flight;
            _flights[flight.Number] = flight;

            switch (e)
            {
                case FlightDeparted:
                    _removeAt[flight.Number] = e.OccurredAt + KeepAfterDeparture;
                    break;
                case FlightCancelled:
                    // Shown until its scheduled time + 15 min, or cancel time + 15 min if cancelled later.
                    var shownUntil = flight.DepartureTime > e.OccurredAt ? flight.DepartureTime : e.OccurredAt;
                    _removeAt[flight.Number] = shownUntil + KeepAfterCancel;
                    break;
            }

            RemoveOldFlights(e.OccurredAt);
        }

        _log.Enqueue(e);
        if (_log.Count > MaxLogLines)
        {
            _log.Dequeue();
        }
    }

    // Departed/cancelled flights stay on the board for a while, measured in simulated time.
    private void RemoveOldFlights(DateTime now)
    {
        var expired = _removeAt
            .Where(entry => entry.Value <= now)
            .Select(entry => entry.Key)
            .ToList();

        foreach (var number in expired)
        {
            _flights.Remove(number);
            _removeAt.Remove(number);
        }
    }
}
