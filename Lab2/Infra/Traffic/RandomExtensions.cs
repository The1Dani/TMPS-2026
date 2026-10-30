namespace airport_manager.Infra.Traffic;

public static class RandomExtensions
{
    public static T Pick<T>(this Random random, T[] items) => items[random.Next(items.Length)];
}
