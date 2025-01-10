using UnityEngine;

namespace BodyPartSwap
{
    /// <summary>
    /// A container to send back to the origin of the swap request, so they can process the effects.
    /// </summary>
    public struct SwapCallbackResponse
    {
        public Result result;
        public string name;
        public PartInfo part;

        public enum Result
        {
            Failure,
            Success,
        }
    }
}