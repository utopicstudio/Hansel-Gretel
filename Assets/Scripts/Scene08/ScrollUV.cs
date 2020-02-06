using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class ScrollUV : MonoBehaviour
{
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";
    new Renderer renderer;
    public bool Scroll;

    Vector2 uvOffset = Vector2.zero;
    private void Start()
    {
        Scroll = true;
        renderer = GetComponent<Renderer>();
    }
    
    void LateUpdate()
    {
        if (Scroll)
        {
            uvOffset += (uvAnimationRate * Time.deltaTime);
        }
        

        if (renderer.enabled)
        {
            //renderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
            renderer.sharedMaterials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
    }
}