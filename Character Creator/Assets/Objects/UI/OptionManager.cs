using UnityEngine;
using BodyPartSwap;
using UnityEngine.Events;

public class OptionManager : MonoBehaviour
{
    [Header("Reference:")]
    [SerializeField] private Transform m_optionContentRoot;
    [SerializeField] private GameObject m_optionPrefab;

    [Header("Events:")]
    [SerializeField] private UnityEvent<GlobalInfo.Types, BodyPartInfo> m_onSwitchCalled;

    public void ReceiveCatalogue(Catalogue _catalogue)
    {
        //  Create an option handler for each type in the catalogue.
        foreach (var pair in _catalogue.dictionary)
        {
            //  Spawn option handler
            var spawnedOption = Instantiate(m_optionPrefab, m_optionContentRoot, false).GetComponent<OptionHandler>();

            spawnedOption.Setup(pair.Value, OnSwitchCalled);
        }
    }
    
    /// <summary>
    /// This function is called whenever on of the option handlers makes a call to switch a body part.
    /// This command is then broadcasted via the UnityEvent in this class, hopefully reaching the model's swap handler.
    /// </summary>
    private void OnSwitchCalled(GlobalInfo.Types _type, BodyPartInfo _info)
    {
        m_onSwitchCalled.Invoke(_type, _info);
    }
}