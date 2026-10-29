namespace airport_manager.Domain;

public record Flight(
    string Number,
    string Airline,
    string Destination,
    DateTime DepartureTime,
    FlightStatus Status,
    TimeSpan Delay,
    string? Gate = null,
    DateTime? ActualDepartureTime = null
)
{
    public DateTime ExpectedDepartureTime => DepartureTime + Delay;

    public bool IsActive => Status is not (FlightStatus.Departed or FlightStatus.Cancelled);

    private bool _NotInAirportState()
    {
        return Status is not (FlightStatus.Scheduled or FlightStatus.Delayed);
    }

    public Flight StartBoarding(string gate)
    {
        if (_NotInAirportState())
        {
            throw new InvalidOperationException($"{this.ToString()} cannot board while {Status}");
        }

        return this with { Gate = gate, Status = FlightStatus.Boarding };
    }

    public Flight DelayBy(TimeSpan delay)
    {
        if (_NotInAirportState())
        {
            throw new InvalidOperationException($"{this.ToString()} cannot be delayed while {Status}");
        }

        return this with { Delay = Delay + delay, Status = FlightStatus.Delayed};
    }

    public Flight ReleaseGate()
    {
        if (Status is not (FlightStatus.Departed or FlightStatus.Cancelled))
        {
            throw new InvalidOperationException($"{this.ToString()} cannot release gate while {Status}");
        }

        return this with { Gate = null };
    }

    public Flight Depart(DateTime now)
    {
        if (Status != FlightStatus.Boarding)
        {
            throw new InvalidOperationException($"{this.ToString()} cannot depart while {Status}");
        }

        return this with { ActualDepartureTime = now, Status = FlightStatus.Departed };
    }

    public Flight Cancel()
    {
        if (Status is (FlightStatus.Departed or FlightStatus.Cancelled))
        {
            throw new InvalidOperationException($"{this.ToString()} cannot be cancelled while {Status}");
        }

        return this with { Status = FlightStatus.Cancelled };
    }
}