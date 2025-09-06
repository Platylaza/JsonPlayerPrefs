using System;
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
    public bool myBool;
    public Color myColor;
    public KeyCode myKeyCode;
    public Vector2 myVector2;
    public Vector3 myVector3;

    [Header("")]
    public string myString2;
    public SavedData displayableTempData;

    private void Start()
    {
        // Setup the instance by telling it what name, extension, and filePath you would like it to use for saving data.
        // This must be run even before calling any other jpp functions
        jpp.Setup("MyFile-v1.3.0");

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

    private void Update()
    {
        // Not a common function to use, but it can be helpful for debuging
        if (jpp.HasBeenSetup())
            displayableTempData = jpp.GetTempDataAsClass();
    }

    [ContextMenu("SaveTheData()")]
    public void SaveTheData()
    {
        // jpp.Set___("KEY_NAME", "NEW_VALUE"); - Both values are required

        jpp.SetInt("my-Int", myInt);
        jpp.SetFloat("my-Float", myFloat);
        jpp.SetString("my-String", myString);
        jpp.SetBool("my-Bool", myBool);
        jpp.SetColor("my-Color", myColor);
        jpp.SetKeyCode("my-KeyCode", myKeyCode);
        jpp.SetVector2("my-Vector2", myVector2);
        jpp.SetVector3("my-Vector3", myVector3);

        // SetVar automaticly detects the type of NEW_VALUE
        // jpp.SetVar("KEY_NAME", "NEW_VALUE"); - Both values are required
        jpp.SetVar("my-String2", myString2);

        // Anything you set is only temporarily saved in the jpp instance.
        // To save/update the file to be equal to the data in the jpp you need to run this function.
        jpp.SaveAllVars();
    }

    [ContextMenu("LoadTheData()")]
    public void LoadTheData()
    {
        // jpp.Get___("KEY_NAME", "DEFAULT_VALUE"); - Only KEY_NAME is required

        myInt = jpp.GetInt("my-Int", myInt);
        myFloat = jpp.GetFloat("my-Float", myFloat);
        myString = jpp.GetString("my-String", myString);
        myBool = jpp.GetBool("my-Bool", myBool);
        myColor = jpp.GetColor("my-Color", myColor);
        myKeyCode = jpp.GetKeyCode("my-KeyCode", myKeyCode);
        myVector2 = jpp.GetVector2("my-Vector2", myVector2);
        myVector3 = jpp.GetVector3("my-Vector3", myVector3);

        // GetVar automaticly detects the type of DEFAULT_VALUE
        // Unfortuntly, casting is required when using GetVar. Get___() is highly recommended over GetVar()
        // (TYPE)jpp.GetVar("KEY_NAME", "DEFAULT_VALUE"); - Both values are required
        myString2 = (string)jpp.GetVar("my-String2", myString2);
    }

    [ContextMenu("UnsetInt()")]
    public void UnsetInt()
    {
        myInt = 0;
        // jpp.UnsetVar("KEY_NAME", "DATA_TYPE"); - Both values are required
        jpp.UnsetVar("my-Int", "int");

        jpp.SaveAllVars();
    }

    [ContextMenu("UnsetAll()")]
    public void UnsetAll()
    {
        myInt = 0;
        myFloat = 0f;
        myString = "";
        myBool = false;
        myColor = new Color();
        myKeyCode = KeyCode.None;
        myVector2 = new Vector2();
        myVector3 = new Vector3();

        jpp.UnsetAll();
        jpp.SaveAllVars();

        // You can also use this function that will delete all data on the file but keep the temporary data saved in the JPP instance.
        // jpp.ClearFileData();
    }

    [ContextMenu("DeleteFile()")]
    public void DeleteFile()
    {
        // jpp.DeleteFile("TARGET_FILE_PATH"); - No required values
        jpp.DeleteFile("C:\\Users\\smell\\AppData\\LocalLow\\Platylaza\\2D Snake vs Block\\MyFile-v1.3.0MyFile-v1.3.0.");
    }

    [ContextMenu("DuplicateFile()")]
    public void DuplicateFile()
    {
        // jpp.DuplicateFile("PATH_FOR_NEW_FILE", "SOURCE_FILE_PATH"); - Only PATH_FOR_NEW_FILE is required
        jpp.DuplicateFile($"{jpp.FilePathParts()[0]}/{myString}.{jpp.FilePathParts()[2]}");
    }

    // !!! - Fix MoveFile() in JPP.cs - !!!
    [ContextMenu("MoveFile()")]
    public void MoveFile()
    {
        // jpp.MoveFile("NEW_FILE_PATH", "TARGET_FILE_PATH"); - Only NEW_FILE_PATH is required
        Debug.Log(jpp.FilePathParts());
        jpp.MoveFile($"{jpp.FilePathParts()[0]}/{myString2}.{jpp.FilePathParts()[2]}");
    }

    [ContextMenu("RenameFile()")]
    public void RenameFile()
    {
        // jpp.RenameFile("NEW_NAME", "TARGET_FILE_PATH"); - Only NEW_NAME is required
        jpp.RenameFile(myString);
    }

    [ContextMenu("ChangeFileExtension()")]
    public void ChangeFileExtension()
    {
        // jpp.ChangeFileExtension("NEW_EXTENSION", "TARGET_FILE_PATH"); - Only NEW_EXTENSION is required
        jpp.ChangeFileExtension(myString2);
    }
}