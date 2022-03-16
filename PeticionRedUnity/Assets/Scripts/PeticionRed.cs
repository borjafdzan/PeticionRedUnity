using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine;

[System.Serializable]
public class ObjetoRed{
    public PrediccionConcello predConcello;
}
[System.Serializable]
public class PrediccionConcello {
    public int idConcello;
    public DiaConcello[] listaPredDiaConcello;
    public string nome;
}

[System.Serializable]
public class DiaConcello{
    public  Ceo ceo;
    public PcChoiva pcChoiva;
    public string dataPredicion;
    public int nivelAviso;
    public int tMax;
    public int tMin;
    public int tmaxFranxa;
    public int tminFranxa;
    public int uvMax;
}

[System.Serializable]
public class Ceo{
    public int manha;
    public int noite;
    public int tarde;
}

[System.Serializable]
public class PcChoiva{
    public int manha;
    public int noite;
    public int tarde;
}
[System.Serializable]
public class Vento{
    public int manha;
    public int noite;
    public int tarde;
}

public class PeticionRed : MonoBehaviour
{
     void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://servizos.meteogalicia.gal/mgrss/predicion/jsonPredConcellos.action?idConc=15030"));

        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
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
                    ObjetoRed prediccion =  JsonUtility.FromJson<ObjetoRed>(webRequest.downloadHandler.text);
                    Debug.Log("Id concello " + prediccion.predConcello.nome);
                    //Debug.Log("Longitud predicciones " + prediccion.listaPredDiaConcello.Length);
                    break;
            }
        }
    }
}
