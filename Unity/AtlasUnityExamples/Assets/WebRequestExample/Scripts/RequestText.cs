using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RequestText : MonoBehaviour
{
    public string url;

    private Text textField;

    void Start()
    {
        textField = GetComponent<Text>();
        StartCoroutine(RequestTextRoutine());
    }

    IEnumerator RequestTextRoutine()
    {
        if (String.IsNullOrEmpty(url))
        {
            Debug.LogWarning("RequestText: No url specified.");
            yield break;
        }

        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
        {
            Debug.Log("RequestText: Sending request.");
            yield return unityWebRequest.SendWebRequest();

            Debug.Log("RequestText: Processing request.");
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.LogWarning("RequestText: Error: " + unityWebRequest.error);
            }
            else
            {
                Debug.Log("RequestText: Setting text.");
                string contents = unityWebRequest.downloadHandler.text;
                textField.text = contents;
            }
        }
    }
}