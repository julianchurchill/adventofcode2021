namespace day2;

internal interface ICommand
{
    Position Apply(Position initialPosition);
}