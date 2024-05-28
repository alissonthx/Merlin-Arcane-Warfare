using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingEffects : MonoBehaviour
{
    public static PostProcessingEffects Instance { get; private set; }
    ColorGrading colorGradingLayer = null;

    private void Awake()
    {
        Instance = this;
    }

    public void DieScreen()
    {
        GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGradingLayer);
        colorGradingLayer.saturation.value = -100f;
    }

    public void ResetScreen()
    {
        GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGradingLayer);
        colorGradingLayer.saturation.value = 0;
    }
}