using System.Collections.Generic;

public class CharacterSetupMemory
{
    /// <summary>
    /// A dictionary of queue indices, attached to their corresponding part indices. The indices are ordered the same way the 'Options' enum is ordered.
    /// </summary>
    public Dictionary<int, int> savedIndices = new();

    //  Example
    /*
    public Dictionary<int, int> savedIndices = new()
    {
        { (int)Options.Head, 0 },
        { (int)Options.Torso, 0 },
        { (int)Options.Legs, 0 },
    };
    */
}