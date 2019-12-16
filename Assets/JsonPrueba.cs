using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class JsonPrueba : MonoBehaviour
{

    public string[] sesiones;
    public string palabras;

    void Save() {
        JSONObject jsontest = new JSONObject();
        jsontest.AddField("palabras", palabras);


        JSONArray sesiones = new JSONArray();
        sesiones.Add(sesiones);

        //jsontest.AddField("sesiones", sesiones);
        Debug.Log(jsontest.ToString());
    }

    void Load() {

    }
    void Start()
    {


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();

    }
}
