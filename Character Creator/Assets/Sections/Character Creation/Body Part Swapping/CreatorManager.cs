using SFB;
using UnityEngine;
using UnityEngine.Events;

namespace BodyPartSwap
{
    public class CreatorManager : MonoBehaviour
    {
        [Header("Scene Events:")]
        [SerializeField] private UnityEvent m_onBackRequested;
        [SerializeField] private UnityEvent m_onPhotoModeRequested;

        private BodyComposition m_composition   = null;
        private OptionFrontman m_options        = null;
        private PictureTaker m_pictureTaker     = null;

        /// <summary>
        /// Called when the tool enters the creator scene for the first time.
        /// </summary>
        public void Begin()
        {
            //  Find all components.
            m_composition   = GetComponentInChildren<BodyComposition>();
            m_options       = GetComponentInChildren<OptionFrontman>();
            m_pictureTaker  = GetComponentInChildren<PictureTaker>();

            //  Load the save from the blackboard.
            var saveFile = Blackboard.instance.loadedSave;

            //  Create a blank save if one is not found.
            saveFile ??= new();

            //  Setup the body composition.
            m_composition.Setup();
            m_composition.ApplyConfiguration(saveFile);

            //  Setup the options frontman.
            m_options.Setup(m_composition.compilation);
            m_options.SubsribeToBroadcast(m_composition.ProcessIncomingSwap);
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

            //  Create extension filter for saving..
            var extenstions = new ExtensionFilter[]
            {
                new("Json Files", "json"),
                new("Text Files", "txt"),
            };

            //  Load a path via the file browser.
            var path = StandaloneFileBrowser.SaveFilePanel("Save Character File", Application.persistentDataPath, "My Character", extenstions);

            //  Error handling.
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Something went wrong during file selection! Beware..", this);
                return;
            }

            //  Save the file to that path.
            if (FileBridge.SaveTo(newSave, path))
            {
                RecentSaveTracker.RegisterRecentSave(path);
            }
        }

        public void OnExportRequestReceived()
        {
            m_onPhotoModeRequested.Invoke();
        }

        public void OnBackRequestReceived()
        {
            m_onBackRequested.Invoke();
        }

        private void OnDestroy()
        {
            m_options.UnsubscribeFromBroadcast(m_composition.ProcessIncomingSwap);
        }
    }
}