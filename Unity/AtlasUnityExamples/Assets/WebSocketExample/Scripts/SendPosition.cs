using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PositionEvent : UnityEvent<string> { }

[System.Serializable]
public class Position
{
    public float x;
    public float y;
}

public class SendPosition : MonoBehaviour
{
    public PositionEvent OnPositionUpdate;

    IEnumerator Start()
    {
        while (true)
        {
            Position newPosition = new Position
            {
                x = transform.localPosition.x,
                y = transform.localPosition.y
            };

            OnPositionUpdate.Invoke(JsonUtility.ToJson(newPosition));

            yield return new WaitForSeconds(.1f);
        }
    }
}