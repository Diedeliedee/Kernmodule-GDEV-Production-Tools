namespace BodyPartSwap
{
    /// <summary>
    /// A container to send back to the origin of the scale request.
    /// </summary>
    public struct ScaleCallback
    {
        public Result result;
        public float newScale;

        public enum Result
        {
            Failure,
            Success,
        }
    }
}