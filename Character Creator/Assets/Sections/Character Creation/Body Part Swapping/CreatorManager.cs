using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace BodyPartSwap
{
    public class CreatorManager : MonoBehaviour
    {
        [Header("Scene Events:")]
        [SerializeField] private UnityEvent m_onBackRequested;
        [SerializeField] private UnityEvent m_onPhotoModeRequested;


        private BodyComposition m_composition                   = null;
        private OptionFrontman m_options                        = null;
        private ExternalBodyPartHandler m_externalPartHandler   = null;

        /// <summary>
        /// Called when the tool enters the creator scene for the first time.
        /// </summary>
        public void Setup()
        {
            //  Find all components.
            m_composition           = GetComponentInChildren<BodyComposition>(true);
            m_options               = GetComponentInChildren<OptionFrontman>(true);
            m_externalPartHandler   = GetComponentInChildren<ExternalBodyPartHandler>(true);

            //  Setup the body composition.
            m_composition.Setup();

            //  Setup the options frontman.
            m_options.Setup(m_composition.compilation);
            m_options.Subscribe(m_composition.ProcessIncomingSwap, m_composition.ProcessScale);
        }

        public async void ApplySaveFile(CharacterSetupMemory _save)
        {
            //  Create a blank save if one is not found.
            _save ??= new();

            //  Create a short term memory.
            var shortTermMemory = new OverheadMemory();

            //  If external models are present in the save file.
            if (_save.externalModels != null && _save.externalModels.Count > 0)
            {
                //  Convert registered external models into overhead memory.
                var registeredModels = await m_externalPartHandler.LookupRegisteredModels(_save.externalModels);

                //  If all is well..
                if (registeredModels != null && registeredModels.Count > 0)
                {
                    //  Register them in short-term memory.
                    shortTermMemory.externalModels.AddRange(registeredModels);
                }
            }

            //  Set-up the selection indices.
            shortTermMemory.activeConfiguration = _save.savedConfiguration;

            //  Save short-term memory on the blackboard.
            Blackboard.instance.memory = shortTermMemory;

            //  Apply short-term memory.
            m_composition.ApplyMemory(shortTermMemory);
            m_options.ApplyCompilation(m_composition.compilation);
        }

        public void OnSaveRequestReceived()
        {
            //  Create a new blank save.
            var newSave = new CharacterSetupMemory();

            //  Configure the save with the current part compilation's indices.
            newSave.savedConfiguration = m_composition.ExtractConfiguration();

            //  Save the registered external body parts.
            foreach (var model in Blackboard.instance.memory.externalModels)
            {
                newSave.externalModels.Add(model.ConvertToRegistration());
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
            Blackboard.instance.memory.activeConfiguration = m_composition.ExtractConfiguration();

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
            var model = await m_externalPartHandler.GenerateExternalModel(path);

            //  Obviously if the import has not yielded any results. Don't do anything.
            if (model == null) return;

            //  Add the body parts to the blackboard.
            Blackboard.instance.memory.externalModels.Add(model);

            //  Add all the generated bodyparts to the queue.
            foreach (var part in model.bodyParts)
            {
                m_composition.AddBodypartToQueue(part);
            }
        }

        private void OnDestroy()
        {
            m_options.Unsubscribe(m_composition.ProcessIncomingSwap, m_composition.ProcessScale);
        }
    }
}