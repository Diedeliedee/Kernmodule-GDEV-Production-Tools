namespace BodyPartSwap
{
    public static class GlobalInfo
    {
        //  Make sure these enum types are above 0 and counting up, so that they can be used in an array!
        public enum Types
        {
            Head    = 0,
            Torso   = 1,
            Legs    = 2
        }

        //  Also make sure to update this every time you add a new entry into the enum.
        public const int typeAmount = 3;
    }
}
