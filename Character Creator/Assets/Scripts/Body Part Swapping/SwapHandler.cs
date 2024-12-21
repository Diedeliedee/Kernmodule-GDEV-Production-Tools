using System.Collections.Generic;
using UnityEngine;

namespace BodyPartSwap
{
    public class SwapHandler : MonoBehaviour
    {
        [Header("Scene References:")]
        [SerializeField] private ActiveBodyPart m_head;
        [SerializeField] private ActiveBodyPart m_torso;
        [SerializeField] private ActiveBodyPart m_legs;

        private Dictionary<GlobalInfo.Types, Queue<GameObject>> m_bodyPartCatalogue;

        private void Awake()
        {
            //  Assemble all body part scriptable objects, and create a catalogue with them.
            CreateCatalogue(Resources.LoadAll<BodyPart>("Body Parts"));
        }

        public void CreateCatalogue(BodyPart[] _assembledBodyParts)
        {
            m_bodyPartCatalogue = new();

            //  Create a dictionary entry for each body part type.
            for (int i = 0; i < GlobalInfo.typeAmount; i++)
            {
                m_bodyPartCatalogue.Add((GlobalInfo.Types)i, new Queue<GameObject>());
            }

            //  Sort them into the catalogue.
            for (int i = 0; i < _assembledBodyParts.Length; i++)
            {
                m_bodyPartCatalogue[_assembledBodyParts[i].type].Enqueue(_assembledBodyParts[i].prefab);
            }
        }
    }
}