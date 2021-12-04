namespace day2.PositionAndAimCommands;

internal class NullCommand : ICommand
{
    public PositionAndAim Apply(PositionAndAim initialPosition)
    {
        return initialPosition;
    }
}