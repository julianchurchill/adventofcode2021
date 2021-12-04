namespace day2.PositionCommands;

internal interface ICommand
{
    Position Apply(Position initialPosition);
}