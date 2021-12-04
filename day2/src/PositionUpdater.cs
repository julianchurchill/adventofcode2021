namespace day2;

internal class PositionUpdater
{
    public Position GetPosition(string[] commands)
    {
        Position initialPosition = new Position
        {
            Horizontal = 0,
            Depth = 0
        };
        Position updatablePosition = initialPosition;
        foreach(var command in commands)
        {
            updatablePosition = new CommandFactory().GetCommand(command).Apply(updatablePosition);
        }
        return updatablePosition;
    }
}