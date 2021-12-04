namespace day2.PositionCommands;

internal class NullCommand : ICommand
{
    public Position Apply(Position initialPosition)
    {
        return initialPosition;
    }
}