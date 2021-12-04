namespace day2.PositionCommands;

internal class PositionCommandFactory
{
    public ICommand GetCommand(string command)
    {
        var commandSeparator = ' ';
        var commandSplit = command.Split(commandSeparator);
        var commandAction = commandSplit[0];
        var commandUnits = int.Parse(commandSplit[1]);
        if(commandAction == "forward")
        {
            return new ForwardCommand { CommandUnits = commandUnits };
        }
        if(commandAction == "down")
        {
            return new DownCommand { CommandUnits = commandUnits };
        }
        if(commandAction == "up")
        {
            return new UpCommand { CommandUnits = commandUnits };
        }
        return new NullCommand();
    }
}