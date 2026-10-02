namespace airport_manager;

public record Flight(
    string Number,
    string Airline,
    string Destination,
    string Gate,
    DateTime DepartureTime,
    TimeSpan? Delay
);