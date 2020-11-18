using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    public void SetColorFromBytes(byte[] bytes)
    {
        Color color = new Color(bytes[0] / 255f, bytes[1] / 255f, bytes[2] / 255f, bytes[2] / 255f);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material.SetColor("_Color", color);
    }
}
