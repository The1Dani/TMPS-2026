using airport_manager.Domain.Ports;
using airport_manager.Domain;

namespace airport_manager.Infra;
public class InMemoryFlightRepository : IFlightRepository
{
    private readonly Dictionary<string, Flight> _store = new();

    public void Add(Flight flight) => _store.Add(flight.Number, flight);

    public Flight? Get(string number)
    {
        return _store.GetValueOrDefault(number);
    }

    public IReadOnlyList<Flight> GetAll()
    {
        return _store.Values.ToList().AsReadOnly();
    }

    public IReadOnlyList<Flight> GetByAirline(string airline)
    {
        return _store.Values.Where(x => x.Airline == airline).ToList().AsReadOnly();
    }

    public void Update(Flight flight)
    {
        _store[flight.Number] = flight;
    }

    public IReadOnlyList<Flight> GetActive()
    {
        var all = GetAll();
        return all.Where(x => x.IsActive).ToList();
    }
}