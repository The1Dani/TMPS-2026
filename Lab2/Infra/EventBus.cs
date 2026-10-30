using airport_manager.Domain.Ports;
using airport_manager.Domain;

namespace airport_manager.Infra;

public class EventBus : IEventSink
{
    private readonly List<IEventSub> _subs = new();

    public void Sub(IEventSub handler)
    {
        _subs.Add(handler);
    }

    public void Pub(AirportEvent e)
    {
        foreach (var sub in _subs)
        {
            sub.Update(e);
        }
    }
}