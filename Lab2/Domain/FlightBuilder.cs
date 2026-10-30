using airport_manager.Domain.Ports;

namespace airport_manager.Domain;

public class FlightBuilder
{
    private static readonly TimeSpan DefaultBoardingWindow = TimeSpan.FromMinutes(40);

    private string? _number;
    private string? _airline;
    private string? _destination;
    private DateTime? _departureTime;
    private FlightType _type;
    private TimeSpan _boardingWindow;

    public FlightBuilder()
    {
        Reset();
    }

    public FlightBuilder Reset()
    {
        _number = null;
        _airline = null;
        _destination = null;
        _departureTime = null;
        _type = FlightType.Passenger;
        _boardingWindow = DefaultBoardingWindow;
        return this;
    }

    public FlightBuilder WithNumber(string number)
    {
        _number = number;
        return this;
    }

    public FlightBuilder OperatedBy(string airline)
    {
        _airline = airline;
        return this;
    }

    public FlightBuilder DepartingTo(string destination)
    {
        _destination = destination;
        return this;
    }

    public FlightBuilder DepartingAt(DateTime time)
    {
        _departureTime = time;
        return this;
    }

    public FlightBuilder OfType(FlightType type)
    {
        _type = type;
        return this;
    }

    public FlightBuilder WithBoardingWindow(TimeSpan window)
    {
        _boardingWindow = window;
        return this;
    }

    public Flight Build()
    {
        if (string.IsNullOrWhiteSpace(_airline))
        {
            throw new InvalidOperationException("Flight needs an airline");
        }

        if (string.IsNullOrWhiteSpace(_number) || !_number.StartsWith(_airline))
        {
            throw new InvalidOperationException($"Flight number '{_number}' must start with airline code {_airline}");
        }

        if (string.IsNullOrWhiteSpace(_destination))
        {
            throw new InvalidOperationException($"Flight {_number} needs a destination");
        }

        if (_departureTime == null)
        {
            throw new InvalidOperationException($"Flight {_number} needs a departure time");
        }

        if (_boardingWindow <= TimeSpan.Zero)
        {
            throw new InvalidOperationException($"Flight {_number} needs a positive boarding window");
        }

        return new Flight(
            _number,
            _airline,
            _destination,
            _departureTime.Value,
            FlightStatus.Scheduled,
            TimeSpan.Zero,
            _type,
            _boardingWindow);
    }
}
