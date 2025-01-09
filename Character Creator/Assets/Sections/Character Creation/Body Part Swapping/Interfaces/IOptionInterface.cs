using System.Collections.Generic;

namespace BodyPartSwap
{
    public interface IOptionInterface
    {
        public void Setup(Dictionary<Options, IChangeablePart> _compilation);
    }
}