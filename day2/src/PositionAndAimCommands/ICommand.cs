namespace day2.PositionAndAimCommands;

internal interface ICommand
{
    PositionAndAim Apply(PositionAndAim initialPosition);
}