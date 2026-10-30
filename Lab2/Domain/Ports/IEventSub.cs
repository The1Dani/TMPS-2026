namespace airport_manager.Domain.Ports;

public interface IEventSub
{
    public void Update(AirportEvent e);
}