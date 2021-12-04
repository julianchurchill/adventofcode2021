namespace day2.PositionAndAimCommands;

internal class ForwardCommand : ICommand
{
    public int CommandUnits { get; init; }

    public PositionAndAim Apply(PositionAndAim initialPosition)
    {
        return new PositionAndAim
        {
            Horizontal = initialPosition.Horizontal + CommandUnits,
            Depth = initialPosition.Depth + initialPosition.Aim * CommandUnits,
            Aim = initialPosition.Aim
        };
    }
}