using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPFunctions : MonoBehaviour
{
   public string url = "";

    

   void UploadFile(){
    //WebRequest.Put(string url, string data);
   }

   IEnumerator DownloadFile(){
    UnityWebRequest Req = UnityWebRequest.Get(url);
        yield return Req.SendWebRequest();
 
        if (Req.result != UnityWebRequest.Result.Success) {
            Debug.Log(Req.error);
        }
        else {
           
            Debug.Log(Req.downloadHandler.text);
 
            //Retrieve result as binary data
           // byte[] results = www.downloadHandler.data;
        }
   }

   IEnumerator POST(){
   /* var request = new UnityWebRequest(url, "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonFileToUpload);
    request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();

    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();
    Debug.Log("Status Code: " + request.responseCode);*/
   
     yield return new WaitForSeconds(5);
   }

   IEnumerator GET()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }



}
