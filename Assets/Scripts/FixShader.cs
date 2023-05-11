using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixShader : MonoBehaviour
{

    private void Start()
    {
        var targetRenderer = gameObject.GetComponent<Renderer>();
        var targetMaterial = targetRenderer.material;
        // Change the shader of the target material to Standard
        //if (targetMaterial.shader == Shader.Find("hidden/internalerrorshader"))
        //{
            targetMaterial.shader = Shader.Find("Standard");
        //}
    }
}
