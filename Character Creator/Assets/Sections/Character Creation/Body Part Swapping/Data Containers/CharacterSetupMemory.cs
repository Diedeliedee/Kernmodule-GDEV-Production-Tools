using System.Collections.Generic;
using BodyPartSwap;

public class CharacterSetupMemory
{
    /// <summary>
    /// A dictionary of queue indices, attached to their corresponding part enum value.
    /// </summary>
    public Dictionary<Options, int> savedIndices = new();

    /// <summary>
    /// A collection of externally registered body parts, that have been imported into the previous save.
    /// </summary>
    public List<ExternalModelRegistration> externalModels = new();

    //  Example
    /*
    public Dictionary<int, int> savedIndices = new()
    {
        { Options.Head, 0 },
        { Options.Torso, 0 },
        { Options.Legs, 0 },
    };
    */
}