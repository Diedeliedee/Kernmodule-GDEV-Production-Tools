using BodyPartSwap;
using GLTFast;
using GLTFast.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;

public class ExternalBodyPartHandler : MonoBehaviour
{
    public async Task<List<CachedExternalBodyPart>> GenerateBodyParts(string _path)
    {
        var importInstance  = new GltfImport();
        var uri             = new Uri(_path);

        Debug.Log(_path);

        //  Try to load the Gltf file.
        if (!await importInstance.LoadFile(_path, uri))
        {
            Debug.LogError($"Error while trying to import custom model {_path}", this);
            return null;
        }

        //  Instantiate the Gltf file.
        if (!await importInstance.InstantiateMainSceneAsync(transform))
        {
            Debug.LogError($"Somethin went wrong while trying to instantiate custom model {_path}", this);
            return null;
        }

        //  Scan all mesh renderers. Compare them with the specific types.
        var instantiatedRenderers   = GetComponentsInChildren<SkinnedMeshRenderer>();
        var partInstances           = new List<CachedExternalBodyPart>();

        //  Iterate through all the meshes.
        foreach (var mesh in instantiatedRenderers)
        {
            //  Check if the mesh contains the attributes of a bodypart.
            if (!MatchesPartAttributes(mesh, out Options _type))
            {
                Debug.LogWarning($"Did not find suitable bodypart type for model part {mesh.gameObject.name}.", this);
                continue;
            }

            //  Generate the body part, cache it, and add it to the list.
            var generatedBodyPart = CreatePartInfo(_type, mesh);
            var cachedBodyPart = new CachedExternalBodyPart()
            {
                info            = generatedBodyPart,
                registration    = new RegisteredExternalBodyPart
                {
                    path    = _path,
                    uri     = uri,
                    type    = _type,
                }
            };

            partInstances.Add(cachedBodyPart);

            //  C
            Debug.Log($"Succesfully detected model part {mesh.gameObject.name} is a {_type.ToString()}.", this);
        }

        //  If no suitable bodypart were found, log it.
        if (partInstances.Count <= 0)
        {
            Debug.LogError($"Found absolutely NO suitable bodyparts in model of {_path}.. :(", this);

            Cleanup();
            return null;
        }

        //  If there are, pull them back up and stuff.
        Cleanup();
        return partInstances;
    }

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

    private void Cleanup()
    {
        /// Disposing the import instance means destroying the memory that holds the mesh, material, and texture data.
        /// While I did try to preserve that by copying the data, I might as well destroy just the gameobject instead.
        //importInstance.Dispose();

        Destroy(transform.GetChild(0).gameObject);
    }
}
