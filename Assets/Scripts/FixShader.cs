using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixShader : MonoBehaviour
{
    /*
    Shader defaultShader;

    private void Awake()
    {
        var renderer = gameObject.GetComponentsInChildren<Renderer>();
        foreach (var rendererAll in renderer)
        {
            Material material = rendererAll.material;
            Debug.Log("Awake" + material.shader.ToString());
            defaultShader = material.shader;
            //material.shader = Shader.Find("Standard");
        }

        //var targetRenderer = gameObject.GetComponent<Renderer>();
        //var targetMaterial = targetRenderer.material;
        //Debug.Log(targetMaterial.shader);
    }

    private void Start()
    {
        var renderer = gameObject.GetComponentsInChildren<Renderer>();
        foreach (var rendererAll in renderer)
        {
            Material material = rendererAll.material;
            Debug.Log("Start" + material.shader.ToString());
            material.shader = defaultShader;
            Debug.Log("After" + material.shader.ToString());
            //material.shader = Shader.Find("Standard");
        }

        // Change the shader of the target material to Standard
        //if (targetMaterial.shader == Shader.Find("hidden/internalerrorshader"))
        //{
        //  targetMaterial.shader = Shader.Find("Standard");
        //}
    }
    */
}
