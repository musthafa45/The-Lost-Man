using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

    [Header("Sky Objects")]
    [SerializeField] private GameObject daySky;
    [SerializeField] private GameObject eveningSky;
    [SerializeField] private GameObject nightSky;

    [Tooltip("Check Inside The Light Setting Ambient Sky Color in HDR")]
    [Header("Render Setting Sky Color")]
    [SerializeField] private Color dayColorSky;
    [SerializeField] private Color nightColorSky;

    [Header("Render Setting Fog Color")]
    [SerializeField] private Color dayColorFog;
    [SerializeField] private Color nightColorFog;

    [Header("Camera Backround Color")]
    [SerializeField] private Color dayColorCameraBg;
    [SerializeField] private Color nightColorCameraBg;

    [Header("Directional Light Color")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Color dayColorDirectionalLight;
    [SerializeField] private Color nightColorDirectionalLight;

    [Header("Water Shader Material Variables")]
    [SerializeField] private Material waterShaderMaterial;
    [SerializeField] private Color dayColorWaterShader;
    [SerializeField] private Color nightColorWaterShader;

    private void Update()
    {
        if (dayNightSliderValue <= 0.3f)
        {
            // Day transitions from 0 to 1
            ppVolumeDay.weight = Mathf.Lerp(0f, 1f, dayNightSliderValue / 0.3f);
            ppVolumeEvening.weight = 0f;
            ppVolumeNight.weight = 0f;

            ScaleSky(daySky, 1f - MapValue(dayNightSliderValue, 0f, 0.3f, 0f, 1f));
            ScaleSky(eveningSky, 0f);
            ScaleSky(nightSky, 0f);
        }
        else if (dayNightSliderValue <= 0.6f)
        {
            // Evening transitions from 0 to 1
            ppVolumeDay.weight = 0f;
            ppVolumeEvening.weight = Mathf.Lerp(0f, 1f, (dayNightSliderValue - 0.3f) / 0.3f);
            ppVolumeNight.weight = 0f;

            ScaleSky(daySky, 0f);
            ScaleSky(eveningSky, MapValue(dayNightSliderValue, 0.3f, 0.6f, 0f, 1f));
            ScaleSky(nightSky, 0f);
        }
        else if (dayNightSliderValue <= 1f)
        {
            // Night transitions from 0 to 1
            ppVolumeDay.weight = 0f;
            ppVolumeEvening.weight = 0f;
            ppVolumeNight.weight = Mathf.Lerp(0f, 1f, (dayNightSliderValue - 0.6f) / 0.4f);

            ScaleSky(daySky, 0f);
            ScaleSky(eveningSky, 0f);
            ScaleSky(nightSky, MapValue(dayNightSliderValue, 0.6f, 1f, 0f, 1f));
        }

        // Lerp colors for different properties
        RenderSettings.ambientSkyColor = Color.Lerp(dayColorSky, nightColorSky, MapValue(dayNightSliderValue, 0.0f, 1.0f, 0.0f, 1.0f));
        RenderSettings.fogColor = Color.Lerp(dayColorFog, nightColorFog, MapValue(dayNightSliderValue, 0.0f, 1.0f, 0.0f, 1.0f));
        Camera.main.backgroundColor = Color.Lerp(dayColorCameraBg, nightColorCameraBg, MapValue(dayNightSliderValue, 0.0f, 1.0f, 0.0f, 1.0f));
        directionalLight.color = Color.Lerp(dayColorDirectionalLight, nightColorDirectionalLight, MapValue(dayNightSliderValue, 0.0f, 1.0f, 0.0f, 1.0f));

        // Lerp material color
        waterShaderMaterial.color = Color.Lerp(dayColorWaterShader, nightColorWaterShader, MapValue(dayNightSliderValue, 0.0f, 1.0f, 0.0f, 1.0f));

    }

    private void ScaleSky(GameObject skyObject, float scale)
    {
        skyObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    private float MapValue(float value, float originalStart, float originalEnd, float newStart, float newEnd)
    {
        return Mathf.Lerp(newStart, newEnd, Mathf.InverseLerp(originalStart, originalEnd, value));
    }
}
