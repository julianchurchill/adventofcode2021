namespace day2;

internal class NullCommand : ICommand
{
    public Position Apply(Position initialPosition)
    {
        return initialPosition;
    }
}