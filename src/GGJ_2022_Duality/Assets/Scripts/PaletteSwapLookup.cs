using UnityEngine;
using System.Collections;
using System;

[ExecuteInEditMode]
public class PaletteSwapLookup : MonoBehaviour
{
    public GameController gc => GameController.Instance;

    public Texture NightTexture;
    public Texture DayTexture;

    private Texture setTexture;

    Material _mat;

    void OnEnable()
    {
        setTexture = DayTexture;

        if (gc)
            gc.OnDayTransition += OnDayTransition;

        Shader shader = Shader.Find("Custom/2D/PaletteSwapLookup");
        if (_mat == null)
            _mat = new Material(shader);
    }

    void OnDisable()
    {
        if(gc)
            gc.OnDayTransition -= OnDayTransition;

        if (_mat != null)
            DestroyImmediate(_mat);
    }

    private void OnDayTransition(bool isDay)
    {
        if (isDay)
        {
            setTexture = DayTexture;
        }
        else
        {
            setTexture = NightTexture;
        }
    }


    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        _mat.SetTexture("_PaletteTex", setTexture);
        Graphics.Blit(src, dst, _mat);
    }


}