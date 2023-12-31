using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicSoundDetector : MonoBehaviour
{
    public static MicSoundDetector Instance { get; private set; }

    [SerializeField] private float frequencySensitivity = 10.0f;
    private AudioSource audioSource;
    // Default Mic
    private string selectedDevice;
    // Block for audioSource.GetOutputData()
    private static float[] samples = new float[128];

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        /**
         * if there are microphones,
         * select the default mic,
         * set the audioSource.clip to the default mic, looped, for 1 second, at the sampleRate
         * set loop to true
         */
        InitializeMic();
    }

    private void InitializeMic()
    {
        if (Microphone.devices.Length > 0)
        {
            selectedDevice = Microphone.devices[0].ToString();
            audioSource.clip = Microphone.Start(selectedDevice, true, 1, AudioSettings.outputSampleRate);
            audioSource.loop = true;

            /**
             * While the position of the mic in the recording is greater than 0,
             * play the clip (that should be the mic)
             */
            while (!(Microphone.GetPosition(selectedDevice) > 0))
            {
                audioSource.Play();
            }
        }
    }

    void Update()
    {
        GetOutputData();

        Debug.Log("Your Mic Sound " + Mathf.Abs(GetOutputData()));
        //Debug.Log("Your Mic Sound " + GetOutputData().ToString("F1"));

    }

    /**
     * Load the block samples with data from the audioSource output
     * Average the values across the size of the block.
     * vals is the volume of the mic, used to control block height
     * Block height represents candle flame getting larger
     */
    public float GetOutputData()
    {
        audioSource.GetOutputData(samples, 0);

        float vals = 0.0f;

        for (int i = 0; i < 128; i++)
        {
            vals += Mathf.Abs(samples[i]);
        }
        vals /= 128.0f;

        gameObject.transform.localScale = new Vector3(1.0f, 1.0f + (vals * frequencySensitivity), 1.0f);
        EventManager.Instance.InvokeGetAnyMicOutPutData(vals * frequencySensitivity);

        return vals * frequencySensitivity;
    }

}
