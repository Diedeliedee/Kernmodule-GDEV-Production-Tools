using System.Collections.Generic;

namespace BodyPartSwap
{
	public interface IOptionInterface
	{
        public void Setup(Dictionary<Options, IActiveElement> _compilation);

        public void ApplyCompilation(Dictionary<Options, IActiveElement> _compilation);

        public void SubsribeToBroadcast(SwapRequest _request);

        public void UnsubscribeFromBroadcast(SwapRequest _request);

        public delegate SwapCallbackResponse SwapRequest(Options _type, int _offset);
    }
}