namespace airport_manager.Domain.Ports.UI;

public class ConsoleWriter() : IEventSub
{

    public void Update(AirportEvent e)
    {
        Console.WriteLine($"Event {e}");
    }
    
}