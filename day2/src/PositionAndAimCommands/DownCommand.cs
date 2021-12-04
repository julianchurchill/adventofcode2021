namespace day2.PositionAndAimCommands;

internal class DownCommand : ICommand
{
    public int CommandUnits { get; init; }

    public PositionAndAim Apply(PositionAndAim initialPosition)
    {
        return new PositionAndAim
        {
            Horizontal = initialPosition.Horizontal,
            Depth = initialPosition.Depth,
            Aim = initialPosition.Aim + CommandUnits
        };
    }
}