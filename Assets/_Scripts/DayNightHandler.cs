using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
public class DayNightHandler : MonoBehaviour
{
    [Header("Morning To Night ")]
    [SerializeField]
    [Range(0f, 1f)]
    private float dayNightSliderValue;

    [SerializeField] private float transitionLerpSpeed = 10f;
    [Header("Post Proccess Profiles")]
    [SerializeField] private PostProcessVolume ppVolumeDay;
    [SerializeField] private PostProcessVolume ppVolumeEvening;
    [SerializeField] private PostProcessVolume ppVolumeNight;

    private void Update()
    {
        if (dayNightSliderValue <= 0.3f)
        {
            // Day transitions from 0 to 1
            ppVolumeDay.weight = Mathf.Lerp(0f, 1f, dayNightSliderValue / 0.3f);
            ppVolumeEvening.weight = 0f;
            ppVolumeNight.weight = 0f;
        }
        else if (dayNightSliderValue <= 0.6f)
        {
            // Evening transitions from 0 to 1
            ppVolumeDay.weight = 0f;
            ppVolumeEvening.weight = Mathf.Lerp(0f, 1f, (dayNightSliderValue - 0.3f) / 0.3f);
            ppVolumeNight.weight = 0f;
        }
        else if (dayNightSliderValue <= 1f)
        {
            // Night transitions from 0 to 1
            ppVolumeDay.weight = 0f;
            ppVolumeEvening.weight = 0f;
            ppVolumeNight.weight = Mathf.Lerp(0f, 1f, (dayNightSliderValue - 0.6f) / 0.4f);
        }
    }
}
