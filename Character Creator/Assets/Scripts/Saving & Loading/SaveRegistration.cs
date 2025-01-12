using System.IO;

public class SaveRegistration
{
    public string location  = "Nowhere";
    public string date      = "00:00 00-00-0000";
    public string version   = "0.0";

    public string name => Path.GetFileName(location);
}
