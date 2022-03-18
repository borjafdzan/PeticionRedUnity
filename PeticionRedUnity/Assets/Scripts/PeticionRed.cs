using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine;








public class PeticionRed : MonoBehaviour
{
     void Start()
    {
        StartCoroutine(GetRequest("https://servizos.meteogalicia.gal/mgrss/predicion/jsonPredConcellos.action?idConc=15030"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
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
                    Peticion prediccion =  JsonUtility.FromJson<Peticion>(webRequest.downloadHandler.text);
                    Debug.Log("Id concello " + prediccion.predConcello.nome);
                    Debug.Log("Cuenta predicciones " + prediccion.predConcello.listaPredDiaConcello.Length);
                    //Debug.Log("Longitud predicciones " + prediccion.listaPredDiaConcello.Length);
                    foreach(DiaConcello dia in prediccion.predConcello.listaPredDiaConcello){
                        Debug.Log("tmax: " + dia.tMax);
                        Debug.Log("temperatura minima: " + dia.tMin);
                        Debug.Log("Nivel aviso " + dia.nivelAviso);
                    }
                    break;
            }
        }
    }
}
