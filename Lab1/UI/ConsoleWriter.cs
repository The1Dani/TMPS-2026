namespace airport_manager.UI;

public class ConsoleWriter
{

    public ConsoleWriter(IEventSink eventSink)
    {
        eventSink.Sub(WriteHandler);
    }

    private static void WriteHandler(AirportEvent e)
    {
        Console.WriteLine($"Event {e}");
    }
    
}