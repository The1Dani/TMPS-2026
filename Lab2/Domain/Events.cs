namespace airport_manager.Domain;

public abstract record AirportEvent(DateTime OccurredAt);

public abstract record FlightEvent(Flight Flight, DateTime OccurredAt) : AirportEvent(OccurredAt);

public record FlightScheduled(Flight Flight, DateTime OccurredAt) : FlightEvent(Flight, OccurredAt);

public record FlightDelayed(Flight Flight, DateTime OccurredAt) : FlightEvent(Flight, OccurredAt);

public record FlightBoarding(Flight Flight, DateTime OccurredAt) : FlightEvent(Flight, OccurredAt);

public record FlightDeparted(Flight Flight, DateTime OccurredAt) : FlightEvent(Flight, OccurredAt);

public record FlightCancelled(Flight Flight, DateTime OccurredAt) : FlightEvent(Flight, OccurredAt);