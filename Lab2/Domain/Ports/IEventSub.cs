namespace airport_manager.Domain;

public interface IEventSub
{
    public void Update(AirportEvent e);
}