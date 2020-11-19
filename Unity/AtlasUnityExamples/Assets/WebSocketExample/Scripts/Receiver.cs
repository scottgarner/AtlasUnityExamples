using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Receiver : MonoBehaviour
{
    public string objectId;
    private Vector3 newPosition;
    private Vector3 newRotation;

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, Time.deltaTime * 10f);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, newRotation, Time.deltaTime * 10f);

    }

    public void UpdateTransform(string transformPacketString)
    {
        Debug.LogWarning(transformPacketString);


        try
        {
            TransformPacket transformPacket = JsonUtility.FromJson<TransformPacket>(transformPacketString);


            if (transformPacket.objectId == objectId)
            {
                newPosition = new Vector3(
                    transformPacket.positionX + 1,
                    transformPacket.positionY,
                    transformPacket.positionZ
                );

                newRotation = new Vector3(
                    transformPacket.rotationX,
                    transformPacket.rotationY,
                    transformPacket.rotationZ
                );

            }

        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }
    }
}