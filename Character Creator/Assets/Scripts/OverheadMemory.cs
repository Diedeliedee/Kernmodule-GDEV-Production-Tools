using System.Collections.Generic;
using BodyPartSwap;

public class OverheadMemory
{
    public Dictionary<Options, BodyPartConfiguration> activeConfiguration   = null;
    public List<CachedExternalModel> externalModels                         = new();
}
