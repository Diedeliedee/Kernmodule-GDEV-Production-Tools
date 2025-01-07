using UnityEngine;

namespace BodyPartSwap
{
    public class CreatorManager : MonoBehaviour
    {
        [Header("Scene References:")]
        [SerializeField] private ActiveBodyPart m_head;
        [SerializeField] private ActiveBodyPart m_torso;
        [SerializeField] private ActiveBodyPart m_legs;

        private OptionFrontman m_options = null;

        /// <summary>
        /// Called when the tool enters the creator scene for the first time.
        /// </summary>
        public void Begin()
        {
            //  Find the option front man.
            m_options = GetComponentInChildren<OptionFrontman>();

            //  Setup the options frontman.
            m_options.Setup(OnSwitchCommandReceived, Blackboard.loadedSave);
        }

        /// <summary>
        /// Called whenever the OptionManager broadcasts that one of it's option buttons has requested a body part swap.
        /// </summary>
        public SwapCallbackResponse OnSwitchCommandReceived(Options _type, int _index)
        {
            return GetFromType(_type).ProcessSwap(_index);
        }

        /// <summary>
        /// Converts a body part type enum to an active body part reference.
        /// </summary>
        private ActiveBodyPart GetFromType(Options _type)
        {
            return _type switch
            {
                Options.Head    => m_head,
                Options.Torso   => m_torso,
                Options.Legs    => m_legs,
                _               => null,
            };
        }
    }
}