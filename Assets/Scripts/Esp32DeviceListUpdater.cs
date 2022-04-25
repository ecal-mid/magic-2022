using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[DefaultExecutionOrder(-20)] 
public class Esp32DeviceListUpdater : MonoBehaviour
{
    [Serializable]
    public struct DeviceList
    {
        public List<DeviceListItem> data;
    
        [Serializable]
        public struct DeviceListItem
        {
            public string name;
            public string ip;
        }

    }
    
    public string url = "https://ecal-mid.ch/magicleap/devices.json";
    
    void Awake()
    {
        StartCoroutine(UpdateList());
    }

    IEnumerator UpdateList()
    {
        GetComponent<Esp32DeviceManager>().enabled = false;
        yield return GetJson<DeviceList>(url, list =>
        {
            list.data.Sort((a,b)=>a.name.CompareTo(b.name));
            var deviceManager = GetComponent<Esp32DeviceManager>();
            deviceManager.settings.clients.Clear();
            for (int i = 0; i < list.data.Count; i++)
            {
                deviceManager.settings.clients.Add(new Esp32ClientConnectionSettings
                {
                    name = list.data[i].name,
                    address = list.data[i].ip,
                    port = 9999
                });
            }
            GetComponent<Esp32DeviceManager>().enabled = true;
        });
        GetComponent<Esp32DeviceManager>().enabled = true;
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
                   // Debug.Log("Received: " + webRequest.downloadHandler.text);

                    var text = webRequest.downloadHandler.text;
                    var data = JsonUtility.FromJson<T>(text);
                    callback(data);
                    break;
            }
        }
    }
}
