namespace day2;

using day2.PositionAndAimCommands;

internal class PositionAndAimUpdater
{
    public PositionAndAim GetPosition(string[] commands)
    {
        PositionAndAim initialPosition = new PositionAndAim
        {
            Horizontal = 0,
            Depth = 0,
            Aim = 0
        };
        return GetPosition(commands, initialPosition);
    }

    public PositionAndAim GetPosition(string[] commands, PositionAndAim initialPosition)
    {
        PositionAndAim updatablePosition = initialPosition;
        foreach(var command in commands)
        {
            updatablePosition = new PositionAndAimCommandFactory().GetCommand(command)
                .Apply(updatablePosition);
        }
        return updatablePosition;
    }
}