using UnityEngine;
using System.Collections;

// Script from https://www.universityofgames.net/articles/capturing-audio-from-a-microphone-in-unity3d/
// Author Leszke W. Krol

[RequireComponent(typeof(AudioSource))]
public class MicrophoneCapture : MonoBehaviour
{
    // Boolean flags shows if the microphone is connected   
    private bool micConnected = false;

    //The maximum and minimum available recording frequencies    
    private int minFreq;
    private int maxFreq;

    //A handle to the attached AudioSource    
    private AudioSource goAudioSource;

    void Start()
    {
        //Check if there is at least one microphone connected    
        if (Microphone.devices.Length <= 0)
        {
            //Throw a warning message at the console if there isn't    
            Debug.LogWarning("Microphone not connected!");
        }
        else //At least one microphone is present    
        {
            //Set our flag 'micConnected' to true    
            micConnected = true;

            //Get the default microphone recording capabilities    
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...    
            if (minFreq == 0 && maxFreq == 0)
            {
                //...meaning 44100 Hz can be used as the recording sampling rate    
                maxFreq = 48000;
            }

            //Get the attached AudioSource component    
            goAudioSource = this.GetComponent<AudioSource>();
        }
    }

    void OnGUI()
    {
        //If there is a microphone    
        if (micConnected)
        {
            //If the audio from any microphone isn't being captured    
            if (!Microphone.IsRecording(null))
            {
                //Case the 'Record' button gets pressed    
                if (GUI.Button(new Rect(25, 25, 20, 20), "R"))
                {
                    //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource    
                    goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);
                }
            }
            else //Recording is in progress    
            {
                //Case the 'Stop and Play' button gets pressed    
                if (GUI.Button(new Rect(25, 25, 20, 20), "S"))
                {
                    Microphone.End(null); //Stop the audio recording    
                    goAudioSource.Play(); //Playback the recorded audio    
                }

                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 25, 200, 50), "Recording in progress...");
            }
        }
        else // No microphone    
        {
            //Print a red "Microphone not connected!" message at the center of the screen    
            GUI.contentColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Microphone not connected!");
        }

    }
}