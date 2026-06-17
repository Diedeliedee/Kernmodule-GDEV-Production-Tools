using System.Collections.Generic;
using BodyPartSwap;

public class CachedExternalModel
{
    public string path              = string.Empty;
    public List<PartInfo> bodyParts = new();

    /// <summary>
    /// Turns this run-time cache of an external model into a portable and save-able registration of the path and remembered type of each mesh in this model.
    /// </summary>
    public ExternalModelRegistration ConvertToRegistration()
    {
        var meshRegistrations = new List<ExternalModelRegistration.Mesh>();

        foreach (var part in bodyParts)
        {
            var mesh = new ExternalModelRegistration.Mesh()
            {
                name = part.name,
                type = part.type,
            };
            meshRegistrations.Add(mesh);
        }

        return new ExternalModelRegistration()
        {
            path    = path,
            meshes  = meshRegistrations
        };
    }
}
