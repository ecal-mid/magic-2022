using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialInBuild : MonoBehaviour
{
    public Material material;
    void Start()
    {
        if(!Application.isEditor)
            GetComponent<MeshRenderer>().sharedMaterial = material;
    }
}
