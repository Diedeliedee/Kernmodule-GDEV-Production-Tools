using System;
using UnityEngine;

namespace BodyPartSwap
{
    public interface IActiveElement
    {
        public Options type { get; }
        public int index    { get; }

        public string typeName      { get; }
        public string selectedName  { get; }

        public SwapCallbackResponse ProcessSwap(int _offset);

        public SwapCallbackResponse ApplyIndex(int _index);
    }
}