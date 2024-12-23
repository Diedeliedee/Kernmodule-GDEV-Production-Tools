using UnityEngine;
using UnityEngine.Events;

namespace BodyPartSwap
{
    public class SwapManager : MonoBehaviour
    {
        [Header("Events:")]
        [SerializeField] private UnityEvent<Catalogue> m_onCatalogueCreated;

        private readonly Catalogue m_catalogue = new();

        private void Start()
        {
            //  Assemble all body part scriptable objects, and create a catalogue with them.
            m_catalogue.InsertUnsorted(Resources.LoadAll<BodyPartInfo>("Body Parts"));

            //  Emit the catalogue via an event.
            m_onCatalogueCreated.Invoke(m_catalogue);
        }
    }
}