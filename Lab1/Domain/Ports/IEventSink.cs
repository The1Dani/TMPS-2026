namespace airport_manager;

public interface IEventSink
{
    public void Sub(Action<AirportEvent> handler);

    public void Pub(AirportEvent e);
}