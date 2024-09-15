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
    public string Transcript { get; private set; } = "";

    private bool keyboardMode = false;

    public PlayerMovement playerMovement;

    public SceneBuilder sb;

    private string modelPath =
        "./Assets/Packages/Picovoice.Cheetah.2.0.1/build/netstandard2.0/lib/common/cheetah_params.pv";
    private string typedText = "";

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
            if (keyboardMode)
            {



                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (typedText.Length > 0)
                    {
                        typedText = typedText.Substring(0, typedText.Length - 1);
                    }
                }
                else
                {
                    typedText += Input.inputString;
                    Transcript = typedText;
                }

                if (Input.GetKeyDown(KeyCode.Return))  
                {
                    playerMovement.speed = 7;
                    sb.StartJournal(Transcript);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    keyboardMode = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.V) && !recorder.IsRecording)
                    recorder.Start();
                if (Input.GetKeyUp(KeyCode.V) && recorder.IsRecording)
                {
                    CheetahTranscript finalTranscriptObj = cheetah.Flush();
                    Transcript += finalTranscriptObj.Transcript;
                    //recorder.Stop();
                }

                if (Input.GetKeyDown(KeyCode.C))
                    Transcript = "";

                if (Input.GetKey(KeyCode.V))
                    PVProcess();
                if (Input.GetKeyDown(KeyCode.G))
                    sb.StartJournal(Transcript);
                if (Input.GetKeyDown(KeyCode.K))
                {
                    keyboardMode = true;
                    playerMovement.speed = 0;
                }
            }
            
        }
        else if (recorder.IsRecording && !keyboardMode)
        {
            recorder.Stop();
        }
        
        if (journalOpen && Input.GetKeyDown(KeyCode.V) && !keyboardMode)
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
