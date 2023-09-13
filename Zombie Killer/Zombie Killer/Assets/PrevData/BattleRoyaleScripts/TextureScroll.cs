using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    public Vector2 uvAnimationRate = new Vector2( 1.0f, 0.0f );
    public string textureName = "_MainTex";
    public Material scrollerMat;
    
    Vector2 uvOffset = Vector2.zero;

    private void LateUpdate() 
    {
        uvOffset += ( uvAnimationRate * Time.deltaTime );
        scrollerMat.SetTextureOffset( textureName, uvOffset );
    }
}
