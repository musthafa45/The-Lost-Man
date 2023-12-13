using DG.Tweening;
using System;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public static Fader instance;

    [SerializeField] private CanvasGroup faderCanvasGrp;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        instance = this;
        faderCanvasGrp.alpha = 0f;
    }


    public void DoFadeToBlack(Action OnFadeComplete = null)
    {
        faderCanvasGrp.DOFade(1f, fadeDuration).OnComplete(() => OnFadeComplete?.Invoke());
    }

    public void DoFadeToNormal(Action OnFadeComplete = null)
    {
        faderCanvasGrp.DOFade(0f, fadeDuration).OnComplete(() => OnFadeComplete?.Invoke());
    }
    public void DoFadeToBlack(float duration, Action OnFadeComplete = null)
    {
        faderCanvasGrp.DOFade(1f, duration).OnComplete(() => OnFadeComplete?.Invoke());
    }

    public void DoFadeToNormal(float duration, Action OnFadeComplete = null)
    {
        faderCanvasGrp.DOFade(0f, duration).OnComplete(() => OnFadeComplete?.Invoke());
    }
}