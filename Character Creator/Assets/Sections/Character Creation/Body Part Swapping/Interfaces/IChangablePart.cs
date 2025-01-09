using System;

namespace BodyPartSwap
{
    public interface IChangeablePart
    {
        public Options type { get; }
        public int index { get; }

        public void SubscribeToCallback(Action<SwapCallbackResponse> _callback);

        public void UnsubrscibeFromCallback(Action<SwapCallbackResponse> _callback);

        public void ProcessSwap(int _offset);

        public void ApplyIndex(int _index);
    }
}