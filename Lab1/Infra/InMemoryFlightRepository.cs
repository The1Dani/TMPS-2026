namespace airport_manager.Infra;

public class InMemoryFlightRepository : IFlightRepository
{
    private readonly Dictionary<string, Flight> _store = new();

    public void Add(Flight flight) => _store.Add(flight.Number, flight);

    public Flight? Get(string number)
    {
        return _store.GetValueOrDefault(number);
    }

    public List<Flight> GetAll()
    {
        return _store.Values.ToList();
    }

    public List<Flight> GetByAirline(string airline)
    {
        return _store.Values.Where(x => x.Airline == airline).ToList();
    }
}