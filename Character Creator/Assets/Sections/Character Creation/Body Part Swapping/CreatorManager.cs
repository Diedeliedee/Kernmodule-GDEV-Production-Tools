using UnityEngine;
using UnityEngine.Events;

namespace BodyPartSwap
{
    public class CreatorManager : MonoBehaviour
    {
        [Header("Scene Events:")]
        [SerializeField] private UnityEvent m_onBackRequested;
        [SerializeField] private UnityEvent m_onPhotoModeRequested;

        private BodyComposition m_composition               = null;
        private OptionFrontman m_options                    = null;
        private RuntimeBodyPartGenerator m_partGenerator    = null;

        /// <summary>
        /// Called when the tool enters the creator scene for the first time.
        /// </summary>
        public void Setup()
        {
            //  Find all components.
            m_composition   = GetComponentInChildren<BodyComposition>(true);
            m_options       = GetComponentInChildren<OptionFrontman>(true);
            m_partGenerator = GetComponentInChildren<RuntimeBodyPartGenerator>(true);

            //  Setup the body composition.
            m_composition.Setup();

            //  Setup the options frontman.
            m_options.Setup(m_composition.compilation);
            m_options.SubsribeToBroadcast(m_composition.ProcessIncomingSwap);
        }

        public void ApplySaveFile(CharacterSetupMemory _save)
        {
            //  Create a blank save if one is not found.
            _save ??= new();

            //  Apply.
            m_composition.ApplyConfiguration(_save.savedIndices);
            m_options.ApplyCompilation(m_composition.compilation);
        }

        public void OnSaveRequestReceived()
        {
            //  Create a new blank save.
            var newSave = new CharacterSetupMemory();

            //  Configure the save with the current part compilation's indices.
            foreach (var pair in m_composition.compilation)
            {
                newSave.savedIndices.Add(pair.Key, pair.Value.index);
            }

            //  Load a path via the file browser.
            var path = ExplorerWrapper.GetSaveLocation("Save Character File", "My Character", ExplorerWrapper.jsonFilter);

            //  Save the file to that path.
            if (FileBridge.SaveTo(newSave, path))
            {
                RecentSaveTracker.RegisterRecentSave(path);
            }
        }

        public void OnExportRequestReceived()
        {
            Blackboard.instance.activeConfiguration = m_composition.ExtractConfiguration();

            m_onPhotoModeRequested.Invoke();
        }

        public void OnBackRequestReceived()
        {
            m_onBackRequested.Invoke();
        }

        public async void OnImportRequestReceived()
        {
            //  Load a path via the file browser.
            var path = ExplorerWrapper.GetLoadLocation("Import Custom Model", ExplorerWrapper.gltfFilter);

            //  Generate the body parts.
            var bodyParts = await m_partGenerator.GenerateBodyParts(path);

            //  Add all the generated bodyparts to the queue.
            foreach (var part in bodyParts)
            {
                m_composition.AddBodypartToQueue(part);
            }
        }

        private void OnDestroy()
        {
            m_options.UnsubscribeFromBroadcast(m_composition.ProcessIncomingSwap);
        }
    }
}