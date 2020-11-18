using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

[RequireComponent(typeof(MeshRenderer))]
public class RequestTexture : MonoBehaviour
{
    public string url;

    private MeshRenderer meshRenderer;
    private Material material;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer) material = meshRenderer.material;

        StartCoroutine(RequestTextureRoutine());
    }

    IEnumerator RequestTextureRoutine()
    {
        if (String.IsNullOrEmpty(url))
        {
            Debug.LogWarning("RequestTexture: No url specified.");
            yield break;
        }

        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
        {
            Debug.Log("RequestTexture: Sending request.");
            yield return unityWebRequest.SendWebRequest();

            Debug.Log("RequestTexture: Processing request.");
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.LogWarning("RequestTexture: Error: " + unityWebRequest.error);
            }
            else
            {
                Debug.Log("RequestTexture: Setting texture.");
                Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest);
                if (material && material.HasProperty("_MainTex"))
                {
                    material.SetTexture("_MainTex", texture);
                }
            }
        }
    }
}