using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventData
{
    public string id;
    public string color;
    public float x;
    public float y;
}

public class ClientData
{
    public GameObject gameObject;
    public EventData eventData;
}

public class Spawner : MonoBehaviour
{

    public GameObject clientPrefab;
    Dictionary<string, ClientData> clients = new Dictionary<string, ClientData>();

    void Update()
    {
        foreach (KeyValuePair<string, ClientData> keyValuePair in clients)
        {
            ClientData client = keyValuePair.Value;

            Vector3 newPosition = new Vector3(
                (client.eventData.x - .5f) * 4,
                ((1 - client.eventData.y) - .5f) * 4,
                0
            );

            client.gameObject.transform.localPosition = newPosition;
        }
    }

    public void UpdateData(string data)
    {
        EventData eventData = JsonUtility.FromJson<EventData>(data);

        if (clients.ContainsKey(eventData.id))
        {
            clients[eventData.id].eventData = eventData;
        }
        else
        {
            ClientData clientData = new ClientData();
            clientData.gameObject = Instantiate(clientPrefab, transform);

            Color color = Color.white;
            ColorUtility.TryParseHtmlString(eventData.color, out color);
            clientData.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", color);

            clientData.eventData = eventData;
            clients.Add(eventData.id, clientData);
        }

    }
}
