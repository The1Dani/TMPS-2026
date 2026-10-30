namespace airport_manager.Domain.Ports;

public interface IEventSink
{
    public void Sub(IEventSub handler);

    public void Pub(AirportEvent e);
}