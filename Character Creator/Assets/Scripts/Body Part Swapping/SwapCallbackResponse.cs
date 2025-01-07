namespace BodyPartSwap
{
    /// <summary>
    /// A container to send back to the origin of the swap request, so they can process the effects.
    /// </summary>
    public struct SwapCallbackResponse
    {
        public int responseIndex;
        public PartInfo part;
    }
}