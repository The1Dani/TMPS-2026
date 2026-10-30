namespace airport_manager.Domain.Ports;

public interface IFlightFactory
{
    Flight CreateFlight(DateTime now);
}
