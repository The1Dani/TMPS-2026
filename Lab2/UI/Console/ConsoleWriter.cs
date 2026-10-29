namespace airport_manager.Domain.UI;

public class ConsoleWriter() : IEventSub
{

    public void Update(AirportEvent e)
    {
        Console.WriteLine($"Event {e}");
    }
    
}