namespace airport_manager.Domain;

public interface IFlightRepository
{
    void Add(Flight flight);
    Flight? Get(string number);
    IReadOnlyList<Flight> GetAll();
    IReadOnlyList<Flight> GetByAirline(string airline);
    void Update(Flight flight);
    IReadOnlyList<Flight> GetActive();
}
