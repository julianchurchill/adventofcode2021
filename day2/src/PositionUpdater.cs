namespace day2;

using day2.PositionCommands;

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
            updatablePosition = new PositionCommandFactory().GetCommand(command).Apply(updatablePosition);
        }
        return updatablePosition;
    }
}