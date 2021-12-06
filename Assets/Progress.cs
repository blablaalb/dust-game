using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField]
    private int _radius = 100;

    internal void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var screenPos = Input.mousePosition;
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out hit))
            {
                Renderer rend = hit.transform.GetComponent<Renderer>();


                if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null)
                    return;

                Texture2D tex = rend.material.mainTexture as Texture2D;
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= tex.width;
                pixelUV.y *= tex.height;
                SetPixel(tex, pixelUV.x, pixelUV.y);
            }
        }
    }

    private void SetPixel(Texture2D texture, float x, float y)
    {
        for (int i = 0; i < _radius; i++)
        {
            int xPixel = (int)x;
            int yPixel = (int)y + i;
            texture.SetPixel(xPixel, yPixel, Color.green);
            texture.Apply();
        }
    }
}
