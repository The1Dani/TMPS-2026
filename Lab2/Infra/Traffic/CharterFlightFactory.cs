using airport_manager.Domain.Ports;
using airport_manager.Domain;

namespace airport_manager.Infra.Traffic;

public class CharterFlightFactory(Random random, FlightDirector director) : IFlightFactory
{
    private static readonly string[] Airlines = ["XQ", "BJ"];
    private static readonly string[] Destinations = ["AYT", "HRG", "PMI"];

    public Flight CreateFlight(DateTime now)
    {
        var airline = random.Pick(Airlines);
        return director.MakeCharter(FlightNumberRegistry.Instance.Next(airline), airline, random.Pick(Destinations),
            now.AddMinutes(random.Next(90, 241)));
    }
}