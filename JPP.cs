using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JPP
{
    protected SavedData savedData;
    private static readonly string randomNameKey = "1324657"; // Generate a random number to replace the with.
    private string folderPath, fileName, fileExtension;
    private bool hasBeenSetup = false;

    public static string defaultFolderPath = "PERSISTANT_DATA_PATH",  defaultExtension = "json";
    private bool encryptFiles = false;

    public void Setup(string fileName, string fileExtension = "DEFAULT", string folderPath = "DEFAULT", bool encrypt = false)
    {
        if (!hasBeenSetup)
        {
            savedData = new SavedData();

            folderPath = folderPath.Replace("DEFAULT", defaultFolderPath);
            fileExtension = fileExtension.Replace("DEFAULT", defaultExtension);

            folderPath = folderPath.Replace("PERSISTANT_DATA_PATH", $"{Application.persistentDataPath}");

            this.folderPath = folderPath == "DEFAULT" ? defaultFolderPath : folderPath;
            this.fileName = fileName;
            this.fileExtension = fileExtension == "DEFAULT" ? defaultExtension : fileExtension;

            encryptFiles = encrypt;

            LoadAllVars();
            hasBeenSetup = true;
        }
        else
            Debug.LogWarning("Setup() has already been called on this JPP instance.");
    }

    /// <summary>
    /// Be careful with this, it will overrite all temporary data with the saved data from the file.
    /// </summary>
    public void ReSetup(string fileName, string fileExtension = "DEFAULT", string folderPath = "DEFAULT", bool encrypt = false)
    { hasBeenSetup = false; Setup(fileName, fileExtension, folderPath, encrypt); }

    /// <summary>
    /// Update or Save the data from savedData to a json file.
    /// </summary>
    public void SaveAllVars()
    {
        if (!hasBeenSetup)
            DebugError();
        else
        {
            string dataToSave = JsonUtility.ToJson(savedData, !encryptFiles);
            string path = $"{folderPath}/{fileName}.{fileExtension}";

            File.WriteAllText(path, EncryptDecrypt(dataToSave, encryptFiles));
        }
    }

    /// <summary>
    /// Load a json file to savedData.
    /// </summary>
    public void LoadAllVars()
    {
        Debug.Log($"FolderPath = {folderPath} - FileName = {fileName} - FileExtension = {fileExtension}");
        string path = $"{folderPath}/{fileName}.{fileExtension}";

        if (File.Exists(path))
        {
            string saveData = File.ReadAllText(path);
            Debug.Log(saveData);
            Debug.Log(EncryptDecrypt(saveData, DataIsEncrypted(saveData)));
            savedData = JsonUtility.FromJson<SavedData>(EncryptDecrypt(saveData, DataIsEncrypted(saveData)));
        }
        else
            Debug.LogWarning($"File not found at path \"{path}\"");
    }

    #region SetVar
    public void SetInt(string key, int value)
    {
        if (hasBeenSetup)
        {
            if (savedData.intKeys.Contains(key)) savedData.ints[savedData.intKeys.IndexOf(key)] = value;
            else AddKeyValuePair(key, value);
        }
        else
            DebugError();
    }
    public void SetString(string key, string value)
    {
        if (hasBeenSetup)
        { 
            if (savedData.stringKeys.Contains(key)) savedData.strings[savedData.stringKeys.IndexOf(key)] = value;
            else AddKeyValuePair(key, value);
        }
        else
            DebugError();
    }
    public void SetFloat(string key, float value)
    {
        if (hasBeenSetup)
        { 
            if (savedData.floatKeys.Contains(key)) savedData.floats[savedData.floatKeys.IndexOf(key)] = value;
            else AddKeyValuePair(key, value);
        }
        else
            DebugError();
    }
    public void SetBool(string key, bool value)
    {
        if (hasBeenSetup)
        { 
            if (savedData.boolKeys.Contains(key)) savedData.bools[savedData.boolKeys.IndexOf(key)] = value;
            else AddKeyValuePair(key, value);
        }
        else
            DebugError();
    }
    public void SetColor(string key, Color value)
    {
        if (hasBeenSetup)
        { 
            if (savedData.colorKeys.Contains(key)) savedData.colors[savedData.colorKeys.IndexOf(key)] = value;
            else AddKeyValuePair(key, value);
        }
        else
            DebugError();
    }
    #endregion SetVar

    #region GetVar
    public int GetInt(string key, int defaultValue = 0)
    {
        if (hasBeenSetup) return savedData.intKeys.Contains(key) ? savedData.ints[savedData.intKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    public float GetFloat(string key, float defaultValue = 0f)
    {
        if (hasBeenSetup) return savedData.floatKeys.Contains(key) ? savedData.floats[savedData.floatKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    public string GetString(string key, string defaultValue = "")
    {
        if (hasBeenSetup) return savedData.stringKeys.Contains(key) ? savedData.strings[savedData.stringKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    public bool GetBool(string key, bool defaultValue = false)
    {
        if (hasBeenSetup) return savedData.boolKeys.Contains(key) ? savedData.bools[savedData.boolKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    public Color GetColor(string key, Color defaultValue = new Color())
    {
        if (hasBeenSetup) return savedData.colorKeys.Contains(key) ? savedData.colors[savedData.colorKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    #endregion GetVar

    protected void AddKeyValuePair(string key, object value)
    {
        switch (value)
        {
            case int:
                savedData.intKeys.Add(key);
                savedData.ints.Add((int)value);
                break;
            case float:
                savedData.floatKeys.Add(key);
                savedData.floats.Add((float)value);
                break;
            case string:
                savedData.stringKeys.Add(key);
                savedData.strings.Add((string)value);
                break;
            case bool:
                savedData.boolKeys.Add(key);
                savedData.bools.Add((bool)value);
                break;
            case Color:
                savedData.colorKeys.Add(key);
                savedData.colors.Add((Color)value);
                break;
        }

    }

    /// <summary>
    /// Encrypt or Decrypt the data. Ex: Encrypted = !Encrypted;
    /// </summary>
    /// <param name="data"></param> Data to Encrypt/Decrypt.
    private string EncryptDecrypt(string data, bool encrypt)
    {
        // Encrypt the file
        string result = "";

        if (!encrypt)
            return data;

        for (int i = 0; i < data.Length; i++)
            result += (char)(data[i] ^ randomNameKey[i % randomNameKey.Length]);

        return result;
        //return (encryptFiles ? result : data);
    }

    private bool DataIsEncrypted(string data)
    {
        if (data.Length > 0)
            foreach (char c in data)
            {
                Debug.Log(c);
                if (c == ' ' || c == '\n')
                    continue;
                if (c == '{')
                    return false;
                else
                    return true;
            }
    
        return false;
    }

    private void DebugError(string overrideError = "")
    { Debug.LogError("Setup() must be called before running a save/set function"); }

    public bool HasBeenSetup() { return hasBeenSetup; }
    public bool EncryptFiles() { return encryptFiles; }
}

[Serializable]
public class SavedData
{
    // All data that can be saved
    // (You can store any variables, but to save them they must be in this class)\

    // Lists for saved data
    public List<int> ints = new();
    public List<float> floats = new();
    public List<string> strings = new();
    public List<bool> bools = new();
    public List<Color> colors = new();

    // Keys for the lists
    public List<string> intKeys = new();
    public List<string> floatKeys = new();
    public List<string> stringKeys = new();
    public List<string> boolKeys = new();
    public List<string> colorKeys = new();
}