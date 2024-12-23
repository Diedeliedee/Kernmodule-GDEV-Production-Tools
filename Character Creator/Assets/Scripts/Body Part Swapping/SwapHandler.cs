using UnityEngine;
using UnityEngine.Events;

namespace BodyPartSwap
{
    public class SwapHandler : MonoBehaviour
    {
        [Header("Scene References:")]
        [SerializeField] private ActiveBodyPart m_head;
        [SerializeField] private ActiveBodyPart m_torso;
        [SerializeField] private ActiveBodyPart m_legs;
    }
}