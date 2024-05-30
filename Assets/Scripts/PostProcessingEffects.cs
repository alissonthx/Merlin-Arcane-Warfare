using System;
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

    private void OnEnable()
    {
        GameManager.OnGameWaiting += PostProcessingEffects_OnGameWaiting;
    }

    private void OnDisable()
    {

        GameManager.OnGameWaiting -= PostProcessingEffects_OnGameWaiting;
    }

    private void PostProcessingEffects_OnGameWaiting(object sender, EventArgs e)
    {
        BlackWhiteScreen();
    }

    public void BlackWhiteScreen()
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