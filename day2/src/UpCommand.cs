namespace day2;

internal class UpCommand : ICommand
{
    public int CommandUnits { get; init; }

    public Position Apply(Position initialPosition)
    {
        return new Position
        {
            Horizontal = initialPosition.Horizontal,
            Depth = initialPosition.Depth - CommandUnits
        };
    }
}