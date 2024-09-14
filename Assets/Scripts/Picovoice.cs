using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pv;
using UnityEngine.Serialization;
using Cheetah = Pv.Cheetah;

public class Picovoice : MonoBehaviour
{
    private bool journalOpen = false;
    private string accessKey = "";
    Cheetah cheetah = null;
    private PvRecorder recorder = null;
    public string Transcript { private get; set; } = "";

    private string modelPath =
        "./Assets/Packages/Picovoice.Cheetah.2.0.1/build/netstandard2.0/lib/common/cheetah_params.pv";

    public void Start()
    {
        accessKey = Environment.GetEnvironmentVariable("PICOVOICE_CLIENT_ID");
        cheetah = Cheetah.Create(accessKey, modelPath, 1, true);
        recorder = PvRecorder.Create(cheetah.FrameLength, 0);
        print(cheetah.FrameLength);
        print(cheetah.SampleRate);
        ShowAudioDevices();
    }

    public void OpenJournal()
    {
        journalOpen = true;
        Transcript = "";
    }

    public void CloseJournal()
    {
        journalOpen = false;
    }

    private void Update()
    {
        if (journalOpen)
        {
            if (Input.GetKeyDown(KeyCode.V) && !recorder.IsRecording)
                recorder.Start();
            if (Input.GetKeyUp(KeyCode.V) && recorder.IsRecording)
                recorder.Stop();

            if (Input.GetKeyDown(KeyCode.C))
                Transcript = "";
            
            if (Input.GetKey(KeyCode.V))
                PVProcess();
        }
        else if (recorder.IsRecording)
        {
            recorder.Stop();
        }
        
        if (journalOpen && Input.GetKeyDown(KeyCode.V))
            PVProcess();
    }

    private void PVProcess()
    {
        short[] frame = recorder.Read();

        CheetahTranscript result = cheetah.Process(frame);
        if (!string.IsNullOrEmpty(result.Transcript))
        {
            Transcript += result.Transcript;
        }

        if (result.IsEndpoint)
        {
            CheetahTranscript finalTranscriptObj = cheetah.Flush();
        }
    }
    
    public static void ShowAudioDevices()
    {
        string[] devices = PvRecorder.GetAvailableDevices();
        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log($"index: {i}, device name: {devices[i]}");
        }
    }
    
}
