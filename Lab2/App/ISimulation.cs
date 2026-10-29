namespace airport_manager.App;

public interface ISimulation
{
    DateTime Now { get; }

    void Tick();
}
