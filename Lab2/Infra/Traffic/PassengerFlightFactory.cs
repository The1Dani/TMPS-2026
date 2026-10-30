using airport_manager.Domain.Ports;
using airport_manager.Domain;

namespace airport_manager.Infra.Traffic;
public class PassengerFlightFactory(Random random, FlightDirector director) : IFlightFactory
{
    private static readonly string[] Airlines = ["TK", "PC", "LH", "W6"];
    private static readonly string[] Destinations = ["LHR", "CDG", "FRA", "AMS", "FCO", "MAD", "VIE", "ATH"];

    public Flight CreateFlight(DateTime now)
    {
        var airline = random.Pick(Airlines);
        return director.MakePassenger(FlightNumberRegistry.Instance.Next(airline), airline, random.Pick(Destinations),
            now.AddMinutes(random.Next(60, 181)));
    }
}
