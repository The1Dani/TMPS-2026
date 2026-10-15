namespace airport_manager.Domain;

public interface IEventSink
{
    public void Sub(IEventSub handler);

    public void Pub(AirportEvent e);
}