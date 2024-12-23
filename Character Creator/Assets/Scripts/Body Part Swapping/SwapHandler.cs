using UnityEngine;

namespace BodyPartSwap
{
    public class SwapHandler : MonoBehaviour
    {
        [Header("Scene References:")]
        [SerializeField] private ActiveBodyPart m_head;
        [SerializeField] private ActiveBodyPart m_torso;
        [SerializeField] private ActiveBodyPart m_legs;

        /// <summary>
        /// Called whenever the OptionManager broadcasts that one of it's option buttons has requested a body part swap.
        /// </summary>
        public void OnSwitchCommandReceived(GlobalInfo.Types _type, BodyPartInfo _part)
        {
            GetFromType(_type).SwapWith(_part.prefab);
        }

        /// <summary>
        /// Converts a body part type enum to an active body part reference.
        /// </summary>
        private ActiveBodyPart GetFromType(GlobalInfo.Types _type)
        {
            return _type switch
            {
                GlobalInfo.Types.Head   => m_head,
                GlobalInfo.Types.Torso  => m_torso,
                GlobalInfo.Types.Legs   => m_legs,
                _                       => null,
            };
        }
    }
}