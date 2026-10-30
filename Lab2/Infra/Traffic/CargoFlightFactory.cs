using airport_manager.Domain;
using airport_manager.Domain.Ports;

namespace airport_manager.Infra.Traffic;

public class CargoFlightFactory(Random random, FlightDirector director) : IFlightFactory
{
    private static readonly string[] Airlines = ["CV", "5X"];
    private static readonly string[] Destinations = ["LEJ", "CGN", "LGG"];

    public Flight CreateFlight(DateTime now)
    {
        var airline = random.Pick(Airlines);
        return director.MakeCargo(FlightNumberRegistry.Instance.Next(airline), airline, random.Pick(Destinations),
            now.AddMinutes(random.Next(120, 301)));
    }
}
