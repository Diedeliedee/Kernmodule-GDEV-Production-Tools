using BodyPartSwap;
using GLTFast;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExternalBodyPartHandler : MonoBehaviour
{
    public async Task<List<CachedExternalModel>> LookupRegisteredModels(List<ExternalModelRegistration> _registrations)
    {
        var externalModels = new List<CachedExternalModel>();

        foreach (var registration in _registrations)
        {
            externalModels.Add(await LookupRegisteredModel(registration));
        }
        return externalModels;
    }

    public async Task<CachedExternalModel> LookupRegisteredModel(ExternalModelRegistration _registration)
    {
        //  Attempt to insantiate a file from the path.
        if (!await InstantiateFromGltf(_registration.path))
        {
            Cleanup();
            return null;
        }

        //  Scan all mesh renderers and see which corresponds with which mesh in the registry.
        var partInstances = CreatePartInfosFromRegistration(GetComponentsInChildren<SkinnedMeshRenderer>(), _registration);
        if (partInstances == null || partInstances.Count <= 0)
        {
            Cleanup();
            return null;
        }

        //  If all is well, pull the back up!
        Cleanup();
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
        var uri             = new Uri(_path);

        Debug.Log($"Attempting to import Gltf file at: {_path}", this);

        //  Try to load the Gltf file.
        if (!await importInstance.LoadFile(_path, uri))
        {
            Debug.LogError($"Error while trying to import custom model {_path}", this);
            return false;
        }

        //  Instantiate the Gltf file.
        if (!await importInstance.InstantiateMainSceneAsync(transform))
        {
            Debug.LogError($"Somethin went wrong while trying to instantiate custom model {_path}", this);
            return false;
        }

        Debug.Log($"Instantiation of file at: {_path} supposedly succeeded!", this);
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
            Debug.LogError($"Found absolutely NO suitable bodyparts in {_renderers}.. :(", this);
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

        Destroy(transform.GetChild(0).gameObject);
    }
}
