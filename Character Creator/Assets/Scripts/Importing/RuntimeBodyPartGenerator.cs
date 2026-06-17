using BodyPartSwap;
using GLTFast;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class RuntimeBodyPartGenerator : MonoBehaviour
{
    public async Task<List<PartInfo>> GenerateBodyParts(string _path)
    {
        var importInstance  = new GltfImport();
        var uri             = new System.Uri(_path);

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
        var partInstances           = new List<PartInfo>();

        //  Iterate through all the meshes.
        foreach (var mesh in instantiatedRenderers)
        {
            //  Scan if the mesh name corresponds to any of the established part types.
            foreach (Options partType in Enum.GetValues(typeof(Options)))
            {
                //  If the name doesn't correspond. Continue.
                if (!mesh.sharedMesh.name.Contains(partType.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log($"Imported model part {mesh.sharedMesh.name} is not a bodypart type of: {partType.ToString()}. Continuing probe.", this);
                    continue;
                }

                //  If the name does correspond. Create a PartInfo instance.
                var partInstance = ScriptableObject.CreateInstance<PartInfo>();

                //  Set it up, and add it to the list.
                partInstance.name = mesh.gameObject.name;
                partInstance.Setup(partType, Instantiate(mesh.sharedMesh), Instantiate(mesh.sharedMaterial));
                partInstances.Add(partInstance);

                //  Debug!!
                Debug.Log($"Succesfully detected model part {mesh.gameObject.name} is a {partType.ToString()}.", this);
                continue;
            }

            //  If the loop is complete, the model part did not match any of the part names.
            Debug.LogWarning($"Did not find suitable bodypart type for model part {mesh.gameObject.name}.", this);
        }

        //  If no suitable bodypart were found, log it.
        if (partInstances.Count <= 0)
        {
            Debug.LogError($"Found absolutely NO suitable bodyparts in model of {_path}.. :(", this);

            //importInstance.Dispose();
            return null;
        }

        //  If there are, pull them back up and stuff.
        //importInstance.Dispose();
        return partInstances;
    }
}
