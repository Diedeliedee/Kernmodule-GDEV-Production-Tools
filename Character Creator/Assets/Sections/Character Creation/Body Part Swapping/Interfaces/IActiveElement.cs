using System;
using UnityEngine;

namespace BodyPartSwap
{
    public interface IActiveElement
    {
        public Options type     { get; }
        public int index        { get; }
        public Vector3 scale    { get; }

        public string typeName      { get; }
        public string selectedName  { get; }

        public void Setup();

        public SwapCallback ProcessSwap(int _offset);

        public SwapCallback ApplyIndex(int _index);

        public void ProcessScale(int _axis, float _scale);

        public void ProcessScale(Vector3 _scale);

        public bool ContainsBodyPart(PartInfo _bodypart);

        public void AddToQueue(PartInfo _bodyPart);
    }
}