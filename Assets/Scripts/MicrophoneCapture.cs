using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneCapture : MonoBehaviour {

    // Use this for initialization

    public string deviceName;
    public float time;
    public AudioSource aud;

    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    public float clipLoudness;
    private float[] clipSampleData;

    void Start ()
    {
        time = 0;

        deviceName = Microphone.devices[0];
        aud = GetComponent<AudioSource>();
        aud.clip = Microphone.Start(deviceName, true, 1, 44100);
        //aud.loop = true;

        while (!(Microphone.GetPosition(null) > 0)) { }
        Debug.Log("Im playing audio");
        aud.Play();

        clipSampleData = new float[sampleDataLength];

    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update ()
    {
        //aud.clip.GetData()
        //if (time > 3)
        //{
        //    Debug.Log("Restarted");
        //    time = 0;
        //    aud.Play();
        //}
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            aud.clip.GetData(clipSampleData, aud.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
        }
    }
}
