using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TransformEvent : UnityEvent<string> { }

[System.Serializable]
public struct TransformPacket
{
    public string objectId;

    public float positionX;
    public float positionY;
    public float positionZ;

    public float rotationX;
    public float rotationY;
    public float rotationZ;
}

public class Sender : MonoBehaviour
{
    public string objectId;
    public TransformEvent OnTransformUpdate;

    IEnumerator Start()
    {
        while (true)
        {
            TransformPacket transformPacket = new TransformPacket
            {
                objectId = this.objectId,
                positionX = transform.localPosition.x,
                positionY = transform.localPosition.y,
                positionZ = transform.localPosition.z,

                rotationX = transform.rotation.eulerAngles.x,
                rotationY = transform.rotation.eulerAngles.y,
                rotationZ = transform.rotation.eulerAngles.z
            };

            string transformPacketString = JsonUtility.ToJson(transformPacket);
            OnTransformUpdate.Invoke(transformPacketString);

            Debug.Log(transformPacketString);

            yield return new WaitForSeconds(.1f);
        }
    }
}