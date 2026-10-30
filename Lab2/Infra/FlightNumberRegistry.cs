namespace airport_manager.Infra;

// One registry for the whole app, so different flight factories never hand out the same number.
public sealed class FlightNumberRegistry
{
    private const int FirstNumber = 100;

    public static FlightNumberRegistry Instance { get; } = new();

    private readonly Dictionary<string, int> _next = new();

    private FlightNumberRegistry()
    {
    }

    public string Next(string airline)
    {
        var number = _next.GetValueOrDefault(airline, FirstNumber);
        _next[airline] = number + 1;
        return $"{airline}{number}";
    }
}