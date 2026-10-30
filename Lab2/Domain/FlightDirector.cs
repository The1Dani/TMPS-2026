namespace airport_manager.Domain;

// Knows the recipe for each kind of flight; the caller only supplies what varies per flight.
public class FlightDirector(FlightBuilder builder)
{
    public Flight MakePassenger(string number, string airline, string destination, DateTime departure)
    {
        return Route(number, airline, destination, departure)
            .OfType(FlightType.Passenger)
            .WithBoardingWindow(TimeSpan.FromMinutes(40))
            .Build();
    }

    public Flight MakeCargo(string number, string airline, string destination, DateTime departure)
    {
        return Route(number, airline, destination, departure)
            .OfType(FlightType.Cargo)
            .WithBoardingWindow(TimeSpan.FromMinutes(20))
            .Build();
    }

    public Flight MakeCharter(string number, string airline, string destination, DateTime departure)
    {
        return Route(number, airline, destination, departure)
            .OfType(FlightType.Charter)
            .WithBoardingWindow(TimeSpan.FromMinutes(60))
            .Build();
    }

    private FlightBuilder Route(string number, string airline, string destination, DateTime departure)
    {
        return builder.Reset()
            .WithNumber(number)
            .OperatedBy(airline)
            .DepartingTo(destination)
            .DepartingAt(departure);
    }
}