using System;
using System.Collections.Generic;

namespace BodyPartSwap
{
	public interface IOptionInterface
	{
        public void Setup(Dictionary<Options, IActiveElement> _compilation);

        public void ApplyCompilation(Dictionary<Options, IActiveElement> _compilation);

        public void Subscribe(SwapRequest _swapRequest, Action<Options, int, float> _scaleRequest);

        public void Unsubscribe(SwapRequest _swapRequest, Action<Options, int, float> _scaleRequest);

        public delegate SwapCallback SwapRequest(Options _type, int _offset);
    }
}