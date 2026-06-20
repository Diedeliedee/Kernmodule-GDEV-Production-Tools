using BodyPartSwap;
using GLTFast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class ExternalBodyPartHandler : MonoBehaviour
{
    [Header("Events:")]
    [SerializeField] private UnityEvent<string, Color> m_onMessageLogRequired;

    [Header("Properties:")]
    [SerializeField] private Color m_logColor   = Color.white;
    [SerializeField] private Color m_errorColor = Color.red;

    [Header("Reference:")]
    [SerializeField] private PartInfo m_backupHead;
    [SerializeField] private PartInfo m_backupTorso;
    [SerializeField] private PartInfo m_backupLegs;

    //  Cache:
    private List<GltfImport> m_instances = new();

    public async Task<List<CachedExternalModel>> LookupRegisteredModels(List<ExternalModelRegistration> _registrations)
    {
        var externalModels = new List<CachedExternalModel>();

        foreach (var registration in _registrations)
        {
            var model = await LookupRegisteredModel(registration);

            if (model == null) continue;
            externalModels.Add(model);
        }
        return externalModels;
    }

    public async Task<CachedExternalModel> LookupRegisteredModel(ExternalModelRegistration _registration)
    {
        //  Attempt to insantiate a file from the path.
        if (!await InstantiateFromGltf(_registration.path))
        {
            Cleanup();
            return GetBackupCachedModel(_registration.path);
        }

        //  Scan all mesh renderers and see which corresponds with which mesh in the registry.
        var partInstances = CreatePartInfosFromRegistration(GetComponentsInChildren<SkinnedMeshRenderer>(), _registration);
        if (partInstances == null || partInstances.Count <= 0)
        {
            Cleanup();
            return GetBackupCachedModel(_registration.path);
        }

        //  If all is well, pull the back up!
        Cleanup();

        //  Log that all went well!
        m_onMessageLogRequired.Invoke($"Succesfully re-imported your model at: {_registration.path}!", m_logColor);

        //  Return that.
        return new CachedExternalModel
        {
            path        = _registration.path,
            bodyParts   = partInstances,
        };
    }

    public async Task<CachedExternalModel> GenerateExternalModel(string _path)
    {
        //  Attempt to insantiate a file from the path.
        if (!await InstantiateFromGltf(_path))
        {
            Cleanup();
            return null;
        }

        //  Scan all mesh renderers. Compare them with the specific types.
        var partInstances = CreatePartInfosFromObject(GetComponentsInChildren<SkinnedMeshRenderer>());
        if (partInstances == null || partInstances.Count <= 0)
        {
            Cleanup();
            return null;
        }

        //  If there are, pull them back up and stuff.
        Cleanup();

        //  Log that stuff y'know.
        m_onMessageLogRequired.Invoke($"Succesfully imported a model at: {_path}!", m_logColor);

        //  Send it up!!
        return new CachedExternalModel
        {
            path        = _path,
            bodyParts   = partInstances,
        };
    }

    /// <summary>
    /// Instatiates a game object from the imported Gltf file.
    /// </summary>
    private async Task<bool> InstantiateFromGltf(string _path)
    {
        var importInstance  = new GltfImport();
        Uri uri             = null;

        //  Log. :)
        Debug.Log($"Attempting to import Gltf file at: {_path}", this);

        //  Try to make a Uri. (Yes that can break the program too.
        try
        {
            uri = new Uri(_path);
        }
        catch
        {
            if (string.IsNullOrEmpty(_path))
            {
                m_onMessageLogRequired.Invoke($"File path seems to be empty! Could be because you closed your file explorer.", m_logColor);
                importInstance.Dispose();
                return false;
            }

            m_onMessageLogRequired.Invoke($"The file path:\n{_path} does not seem to be valid somehow. I don't know how you managed to cause this.", m_errorColor);
            importInstance.Dispose();
            return false;
        }

        try
        {
            //  Try to load the Gltf file.
            if (!await importInstance.LoadFile(_path, uri))
            {
                m_onMessageLogRequired.Invoke($"Something went wrong while trying to import your custom model at:\n{_path}", m_errorColor);
            importInstance.Dispose();
                return false;
            }
        }
        catch (FileNotFoundException e)
        {
            m_onMessageLogRequired.Invoke($"We couldn't find your model at:\n{_path}.\nIt probably got moved somewhere else!", m_errorColor);
            importInstance.Dispose();
            return false;
        }

        //  Instantiate the Gltf file.
        if (!await importInstance.InstantiateMainSceneAsync(transform))
        {
            m_onMessageLogRequired.Invoke($"Something went wrong while trying to import your custom model at:\n{_path}", m_errorColor);
            importInstance.Dispose();
            return false;
        }

        //  If everything went well, yippeee!
        m_instances.Add(importInstance);
        return true;
    }

    /// <summary>
    /// Loops through every mesh in the renderers, and checks whether they are present in the given registration. So that the right settings can be applied.
    /// </summary>
    /// <returns>A list of all correctly configured part info classes.</returns>
    private List<PartInfo> CreatePartInfosFromRegistration(SkinnedMeshRenderer[] _renderers, ExternalModelRegistration _registration)
    {
        var instances = new List<PartInfo>();

        foreach (var mesh in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            //  If the name of this mesh is the same as the registered mesh. Treat it the SAME!@@!2
            foreach (var registeredMesh in _registration.meshes)
            {
                if (!string.Equals(mesh.name, registeredMesh.name, StringComparison.Ordinal)) continue;

                instances.Add(CreatePartInfo(registeredMesh.type, mesh));
                break;
            }
        }

        //  If no suitable bodypart were found, log it.
        if (instances.Count <= 0)
        {
            Debug.LogError($"Found absolutely NO suitable bodyparts in {_renderers}.. :(", this);
            return null;
        }

        //  Otherwise, return epic style.
        return instances;
    }

    /// <summary>
    /// Loops through every mesh in the renderers, and checks whether they cointain a certain name that corresponds to one of the available body part types.
    /// </summary>
    /// <returns>A list of all correctly configured part info classes.</returns>
    private List<PartInfo> CreatePartInfosFromObject(SkinnedMeshRenderer[] _renderers)
    {
        var instances = new List<PartInfo>();

        //  Iterate through all the meshes.
        foreach (var mesh in _renderers)
        {
            //  Check if the mesh contains the attributes of a bodypart.
            if (!MatchesPartAttributes(mesh, out Options _type))
            {
                Debug.LogWarning($"Did not find suitable bodypart type for model part {mesh.gameObject.name}.", this);
                continue;
            }

            //  Generate the body part, cache it, and add it to the list.
            instances.Add(CreatePartInfo(_type, mesh));

            //  Log result.
            Debug.Log($"Succesfully detected model part {mesh.gameObject.name} is a {_type.ToString()}.", this);
        }

        //  If no suitable bodypart were found, log it.
        if (instances.Count <= 0)
        {
            m_onMessageLogRequired.Invoke($"The imported file does not seems to be a compatible body part..", m_errorColor);
            return null;
        }

        //  Otherwise, return epic style.
        return instances;
    }

    /// <summary>
    /// Creates a PartInfo ScriptableObject instances from a mesh renderer, and a body part type.
    /// </summary>
    private PartInfo CreatePartInfo(Options _type, SkinnedMeshRenderer _mesh)
    {
        var partInstance = ScriptableObject.CreateInstance<PartInfo>();

        //  Set it up.
        partInstance.name = _mesh.gameObject.name;
        partInstance.Setup(_type, _mesh.sharedMesh, _mesh.sharedMaterial);

        //  Correctly duplicating materials
        //partInstance.material.CopyPropertiesFromMaterial(_mesh.sharedMaterial);

        //  Return!
        return partInstance;
    }

    /// <returns>Yes if the name of a mesh matches the name of one of the available option types.</returns>
    private bool MatchesPartAttributes(SkinnedMeshRenderer _mesh, out Options _type)
    {
        _type = Options.Head;

        //  Scan if the mesh name corresponds to any of the established part types.
        foreach (Options partType in Enum.GetValues(typeof(Options)))
        {
            //  If the name doesn't correspond. Continue.
            if (!_mesh.sharedMesh.name.Contains(partType.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"Imported model part {_mesh.sharedMesh.name} is not a bodypart type of: {partType.ToString()}. Continuing probe.", this);
                continue;
            }

            //  If it does, epic!
            _type = partType;
            return true;
        }

        //  If the loop is complete, the model part did not match any of the part names.
        return false;
    }

    /// <summary>
    /// Cleans up any game objects created from the procedures in this class.
    /// </summary>
    private void Cleanup()
    {
        /// Disposing the import instance means destroying the memory that holds the mesh, material, and texture data.
        /// While I did try to preserve that by copying the data, I might as well destroy just the gameobject instead.
        //importInstance.Dispose();

        if (transform.childCount <= 0) return;
        Destroy(transform.GetChild(0).gameObject);
    }

    private void OnDestroy()
    {
        foreach (var instance in m_instances) instance.Dispose();
        m_instances.Clear();
    }

    /// <returns>A dummy model in case something goes wrong. :)</returns>
    private CachedExternalModel GetBackupCachedModel(string _path)
    {
        return new()
        {
            path        = _path,
            bodyParts   = new()
                {
                    m_backupHead,
                    m_backupTorso,
                    m_backupLegs,
                }
        };
    }
}
