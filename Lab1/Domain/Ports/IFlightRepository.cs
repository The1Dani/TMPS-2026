namespace airport_manager;

public interface IFlightRepository
{
    void Add(Flight flight);
    Flight? Get(string number);
    List<Flight> GetAll();
    List<Flight> GetByAirline(string airline);
}
