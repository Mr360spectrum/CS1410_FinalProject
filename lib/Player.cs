namespace lib;

public class Player
{
    private string name;
    public string Name
    {
        get
        {
            if (this.name == "" || this.name is null)
            {
                throw new EmptyNameException("'name' returned null or an empty string.");
            }
            return this.name;
        }
        set 
        {
            if (value == "" || value is null)
            {
                throw new EmptyNameException("Player name cannot be set as null or an empty string.");
            }
        }
    }

    public Player(string inName)
    {
        this.name = inName;
    }
}