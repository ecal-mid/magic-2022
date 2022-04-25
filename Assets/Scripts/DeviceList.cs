using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DeviceList : MonoBehaviour
{
    string url = "https://ecal-mid.ch/magicleap/devices.json";
    public DeviceListData list;
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetJson<DeviceListData>(url, items =>
        {
            list = items;
        }));
    }

    IEnumerator GetJson<T>(string uri, System.Action<T> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + webRequest.downloadHandler.text);

                    var text = webRequest.downloadHandler.text;
                    var data = JsonUtility.FromJson<T>(text);
                    callback(data);
                    break;
            }
        }
    }
}

public struct DeviceListData
{
    List<DeviceListItem> items;
}
[System.Serializable]
public struct DeviceListItem
{
    public string name;
    public string ip;
}
