using System;
using System.Collections;
using UnityEngine;

public class CleaningPlane : MonoBehaviour
{
    [SerializeField] 
    private RenderTexture _texture;
    [SerializeField] 
    private int _pixelSkipAmount = 16;


    internal void Start()
    {
        CalculateTransparentRatio(OnCalculatedRatio);
    }

    public void Reset()
    {
        _texture.Release();
    }

    public void CalculateTransparentRatio(System.Action<float> onFinish)
    {
        StartCoroutine(CalculateTransparentRatioCoroutine(onFinish));
    }
    
    private IEnumerator CalculateTransparentRatioCoroutine(System.Action<float> onFinish)
    {
        int pixels = 0;
        var tex = new Texture2D(_texture.width, _texture.height, TextureFormat.ARGB32, mipChain: false, linear: false);

        RenderTexture.active = _texture;
        tex.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
        tex.Apply();

        var colors = tex.GetPixels32();

        for(int y = 0; y < _texture.height; y += _pixelSkipAmount)
        {
            for(int x = 0; x < _texture.width; x += _pixelSkipAmount)
            {
                if (colors[y * _texture.width + x].a > 127)
                    pixels++;
            }
            yield return null;
        }

        float ratio = pixels / (float)(_texture.width / _pixelSkipAmount * _texture.height / _pixelSkipAmount);
        Destroy(tex);
        yield return null;
        onFinish?.Invoke(ratio);
    }

    private void OnCalculatedRatio(float ratio)
    {
        Debug.Log("Completed: " + (Mathf.RoundToInt(ratio * 100)).ToString() + "%");
        CalculateTransparentRatio(OnCalculatedRatio);
    }
}
