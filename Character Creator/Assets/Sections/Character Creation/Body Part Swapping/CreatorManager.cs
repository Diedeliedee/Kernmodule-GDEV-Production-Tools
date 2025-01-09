using System.Collections.Generic;
using SFB;
using UnityEngine;
using UnityEngine.Events;

namespace BodyPartSwap
{
    public class CreatorManager : MonoBehaviour
    {
        [Header("Scene References:")]
        [SerializeField] private ActiveBodyPart m_head;
        [SerializeField] private ActiveBodyPart m_torso;
        [SerializeField] private ActiveBodyPart m_legs;

        [Header("Scene Events:")]
        [SerializeField] private UnityEvent m_onBackRequested;

        private OptionFrontman m_options  = null;
        private PictureTaker m_pictureTaker = null;

        private Dictionary<Options, ActiveBodyPart> m_partCompilation = new();

        /// <summary>
        /// Called when the tool enters the creator scene for the first time.
        /// </summary>
        public void Begin()
        {
            //  Find all components.
            m_options       = GetComponentInChildren<OptionFrontman>();
            m_pictureTaker  = GetComponentInChildren<PictureTaker>();

            //  Compile the loose part references into a dictionary
            m_partCompilation.Add(Options.Head, m_head);
            m_partCompilation.Add(Options.Torso, m_torso);
            m_partCompilation.Add(Options.Legs, m_legs);

            //  Setup the options frontman.
            m_options.Setup(m_partCompilation);

            //  Iterate, and initialize every part in the compilation.
            foreach (var pair in m_partCompilation)
            {
                //  Load the save from the blackboard.
                var saveFile = Blackboard.instance.loadedSave;

                //  Create a blank save if one is not found.
                saveFile ??= new();

                //  If the save file does not have information about a specific part, set it to the default index.
                if (!saveFile.savedIndices.ContainsKey(pair.Key))
                {
                    saveFile.savedIndices.Add(pair.Key, 0);
                }

                //  Update every active part's index with that of the save file's corresponding option.
                pair.Value.ApplyIndex(saveFile.savedIndices[pair.Key]);
            }
        }

        public void OnSaveRequestReceived()
        {
            //  Create a new blank save.
            var newSave = new CharacterSetupMemory();

            //  Configure the save with the current part compilation's indices.
            foreach (var pair in m_partCompilation)
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
            m_pictureTaker.TakePicture();
        }

        public void OnBackRequestReceived()
        {
            m_onBackRequested.Invoke();
        }
    }
}