using airport_manager.Domain.Ports;

namespace airport_manager.Infra.Traffic;

// Decides which factory makes the next flight, weighted by how common each kind should be.
public class TrafficMix
{
    private readonly Random _random;
    private readonly IReadOnlyList<(IFlightFactory Factory, double Weight)> _entries;
    private readonly double _totalWeight;

    public TrafficMix(Random random, IReadOnlyList<(IFlightFactory Factory, double Weight)> entries)
    {
        if (entries.Any(e => e.Weight < 0))
        {
            throw new ArgumentException("Traffic weights cannot be negative", nameof(entries));
        }

        _totalWeight = entries.Sum(e => e.Weight);
        if (_totalWeight <= 0)
        {
            throw new ArgumentException("Traffic mix needs at least one factory with a positive weight", nameof(entries));
        }

        _random = random;
        _entries = entries;
    }

    public IFlightFactory Pick()
    {
        var roll = _random.NextDouble() * _totalWeight;
        foreach (var (factory, weight) in _entries)
        {
            roll -= weight;
            if (roll < 0)
            {
                return factory;
            }
        }

        // Only reachable through floating point rounding; fall back to the last weighted entry.
        return _entries.Last(e => e.Weight > 0).Factory;
    }
}
