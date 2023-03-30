using System.Collections.Generic;
using CaptainCoder.Core.Dice;

public class Dummy
{
    public void DoStuff()
    {
        DiceNotation notation = DiceNotation.Parse("1d6 + Strength");

        Player p = new Player();

        RollResult result = notation.Roll(p);
        //result.Message; "3d6 (2 + 3 + 1 = 6) + Str (5) = 11"
        //result.Notation; "3d6 + Str"
        //result.Value; 11

        int x = 5 + (int)result;
        
    }
}

public class Player : IRollContext
{
    int Strength = 5;
    public int Lookup(string Id)
    {
        return Id switch
        {
            nameof(Strength) => Strength
        };
    }
}