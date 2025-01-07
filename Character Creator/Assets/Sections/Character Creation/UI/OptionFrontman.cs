using UnityEngine;
using BodyPartSwap;
using System.Collections.Generic;

public class OptionFrontman : MonoBehaviour
{
    [Header("Reference:")]
    [SerializeField] private Transform m_optionContentRoot;
    [SerializeField] private GameObject m_optionPrefab;
    
    //  Cache:
    private List<OptionHandler> m_options = new();

    public void Setup(Dictionary<Options, ActiveBodyPart> _compilation)
    {
        //  Attach all the parts to the corresponding option handlers.
        foreach (var pair in _compilation)
        {
            //  Spawn option handler
            var spawnedOption = Instantiate(m_optionPrefab, m_optionContentRoot, false).GetComponent<OptionHandler>();

            //  Set-up, and add to the list.
            spawnedOption.Setup(pair.Value);
            m_options.Add(spawnedOption);
        }
    }
}