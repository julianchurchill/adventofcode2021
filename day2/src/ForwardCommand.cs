namespace day2;

internal class ForwardCommand : ICommand
{
    public int CommandUnits { get; init; }

    public Position Apply(Position initialPosition)
    {
        return new Position
        {
            Horizontal = initialPosition.Horizontal + CommandUnits,
            Depth = initialPosition.Depth
        };
    }
}