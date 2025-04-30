using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPP_Test : MonoBehaviour
{
    // Create new JPP variable/instance - readonly is not necessary, but it is recommended
    private readonly JPP jpp = new();

    public int myInt;
    public float myFloat;
    public string myString;
    public Color myColor;
    public bool myBool;

    private void Start()
    {
        // Setup the instance by telling it what name, extension, and filePath you would like it to use for saving data.
        // This must be run even before calling any other jpp functions
        jpp.Setup("MyFilesName");

        // Default values for jpp.Setup() are as follows:
        /*
         * FileExtention - "json"
         * FolderPath - UnityEngine.Application.persistentDataPath
         * Encrypt - false
         * 
         * The strings "DEFAULT" and "PERSISTANT_DATA_PATH" will be replaced with the default folder path when used in the folder path
         * Ex: jpp.Setup("MyFilesName", "json", "DEFAULT/MyFolder");
         */
    }

    [ContextMenu("SaveTheData()")]
    public void SaveTheData()
    {
        // jpp.Set___("KEY_NAME", "NEW_VALUE");

        jpp.SetInt("my-Int", myInt);
        jpp.SetFloat("my-Float", myFloat);
        jpp.SetString("my-String", myString);
        jpp.SetColor("my-Color", myColor);
        jpp.SetBool("my-Bool", myBool);

        // Anything you set is only temporarily saved in the jpp instance.
        // To save/update the file to be equal to the data in the jpp you need to run this function.
        jpp.SaveAllVars();
    }

    [ContextMenu("LoadTheData()")]
    public void LoadTheData()
    {
        // jpp.Get___("KEY_NAME", "DEFAULT_VALUE");

        myInt = jpp.GetInt("my-Int", myInt);
        myFloat = jpp.GetFloat("my-Float", myFloat);
        myString = jpp.GetString("my-String", myString);
        myColor = jpp.GetColor("my-Color", myColor);
        myBool = jpp.GetBool("my-Bool", myBool);
    }
}