using BodyPartSwap;
using System.Collections.Generic;

public struct ExternalModelRegistration
{
    public string path;
    public List<Mesh> meshes;

    public struct Mesh
    {
        public string name;
        public Options type;
    }
}
