using System.Diagnostics.CodeAnalysis;

namespace airport_manager.Services;

public class GateAllocator(List<string> startingGates)
{
    private readonly SortedSet<string> _availableGates = new(startingGates);
    public int GateCount { get; } = startingGates.Count;

    public bool TryAssign([NotNullWhen(true)] out string? gate)
    {
        
        gate = _availableGates.Min;
        if (gate == null)
        {
            return false;
        }

        _availableGates.Remove(gate);
        return true;
    }

    public void Release(string gate)
    {
        _availableGates.Add(gate);
    }
}