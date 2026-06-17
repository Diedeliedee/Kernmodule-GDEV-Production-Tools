using System.Collections.Generic;
using BodyPartSwap;

public class OverheadMemory
{
    public Dictionary<Options, int> activeConfiguration     = null;
    public List<CachedExternalBodyPart> externalBodyParts   = new();
}
