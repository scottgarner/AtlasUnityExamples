using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class RequestAudio : MonoBehaviour
{
    public string url;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(RequestAudioRoutine());
    }

    IEnumerator RequestAudioRoutine()
    {
        if (String.IsNullOrEmpty(url))
        {
            Debug.LogWarning("RequestAudio: No url specified.");
            yield break;
        }

        using (UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            Debug.Log("RequestAudio: Sending request.");
            yield return unityWebRequest.SendWebRequest();

            Debug.Log("RequestAudio: Processing request.");
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.LogWarning("RequestAudio: Error: " + unityWebRequest.error);
            }
            else
            {
                Debug.Log("RequestAudio: Setting clip.");
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
                if (audioSource)
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }
            }
        }
    }
}