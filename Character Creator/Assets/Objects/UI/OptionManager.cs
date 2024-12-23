using UnityEngine;
using BodyPartSwap;

public class OptionManager : MonoBehaviour
{
    [Header("Reference:")]
    [SerializeField] private Transform m_optionContentRoot;
    [SerializeField] private GameObject m_optionPrefab;

    public void ReceiveCatalogue(Catalogue _catalogue)
    {
        //  Create an option handler for each type in the catalogue.
        foreach (var pair in _catalogue.dictionary)
        {
            //  Spawn option handler
            var spawnedOption = Instantiate(m_optionPrefab, m_optionContentRoot, false).GetComponent<OptionHandler>();

            spawnedOption.Setup(pair.Value);
        }
    }
}