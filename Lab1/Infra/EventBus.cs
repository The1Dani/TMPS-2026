namespace airport_manager.Infra;

public class EventBus : IEventSink
{
    private readonly List<Action<AirportEvent>> _subs = new();

    public void Sub(Action<AirportEvent> handler)
    {
        _subs.Add(handler);
    }

    public void Pub(AirportEvent e)
    {
        foreach (var sub in _subs)
        {
            sub(e);
        }
    }
}