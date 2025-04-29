using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JPP
{
    protected SavedData savedData;
    private static readonly string randomNameKey = "2849372"; // Generate a random number to replace the with.
    private string folderPath, fileName, fileExtension;
    private bool hasBeenSetup = false;

    public static string defaultFolderPath = "PERSISTANT_DATA_PATH", defaultFileName = "New-JPP-File", defaultExtension = "json";
    public bool encryptFiles = false;

    public void Setup(string fileName = "DEFAULT", string fileExtension = "DEFAULT", string folderPath = "DEFAULT")
    {
        if (!hasBeenSetup)
        {
            savedData = new SavedData();

            defaultFolderPath = defaultFolderPath == "PERSISTANT_DATA_PATH" ? $"{Application.persistentDataPath}" : defaultFolderPath;

            this.folderPath = folderPath == "DEFAULT" ? defaultFolderPath : folderPath;
            this.fileName = fileName == "DEFAULT" ? defaultFileName : fileName;
            this.fileExtension = fileExtension == "DEFAULT" ? defaultExtension : fileExtension;

            LoadAllVars();
            hasBeenSetup = true;
        }
        else
            Debug.LogWarning("Setup() has already been called on this JPP instance.");
    }

    /// <summary>
    /// Be careful with this, it will overrite all temporary data with the saved data from the file.
    /// </summary>
    public void ReSetup(string folderPath = "DEFAULT", string fileName = "DEFAULT", string fileExtension = "DEFAULT")
    { hasBeenSetup = false; Setup(fileName, fileExtension, folderPath); }

    /// <summary>
    /// Update or Save the data from savedData to a json file.
    /// </summary>
    public void SaveAllVars()
    {
        if (!hasBeenSetup)
            DebugError();
        else
        {
            ConvertDictionarys(false);
            string dataToSave = JsonUtility.ToJson(savedData, !encryptFiles);
            string path = $"{folderPath}/{fileName}.{fileExtension}";

            File.WriteAllText(path, EncryptDecrypt(dataToSave));
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
            savedData = JsonUtility.FromJson<SavedData>(EncryptDecrypt(saveData));
            ConvertDictionarys(true);
        }
        else
            Debug.LogWarning($"File not found at path \"{path}\"");
    }

    #region SetVar
    public void SetInt(string key, int value)
    {
        if (hasBeenSetup)
        {
            if (savedData.ints.ContainsKey(key)) savedData.ints[key] = value;
            else savedData.ints.Add(key, value);
        }
        else
            DebugError();
    }
    public void SetString(string key, string value)
    {
        if (hasBeenSetup)
        { 
            if (savedData.strings.ContainsKey(key)) savedData.strings[key] = value;
            else savedData.strings.Add(key, value);
        }
        else
            DebugError();
    }
    public void SetFloat(string key, float value)
    {
        if (hasBeenSetup)
        { 
            if (savedData.floats.ContainsKey(key)) savedData.floats[key] = value;
            else savedData.floats.Add(key, value);
        }
        else
            DebugError();
    }
    public void SetBool(string key, bool value)
    {
        if (hasBeenSetup)
        { 
            if (savedData.bools.ContainsKey(key)) savedData.bools[key] = value;
            else savedData.bools.Add(key, value);
        }
        else
            DebugError();
    }
    public void SetColor(string key, Color value)
    {
        if (hasBeenSetup)
        { 
            if (savedData.colors.ContainsKey(key)) savedData.colors[key] = value;
            else savedData.colors.Add(key, value);
        }
        else
            DebugError();
    }
    #endregion SetVar

    #region GetVar
    public int GetInt(string key, int defaultValue = 0)
    {
        if (hasBeenSetup) return savedData.ints.ContainsKey(key) ? savedData.ints[key] : defaultValue;

        DebugError();
        return defaultValue;
    }
    public float GetFloat(string key, float defaultValue = 0f)
    {
        if (hasBeenSetup) return savedData.floats.ContainsKey(key) ? savedData.floats[key] : defaultValue;

        DebugError();
        return defaultValue;
    }
    public string GetString(string key, string defaultValue = "")
    {
        if (hasBeenSetup) return savedData.strings.ContainsKey(key) ? savedData.strings[key] : defaultValue;

        DebugError();
        return defaultValue;
    }
    public bool GetBool(string key, bool defaultValue = false)
    {
        if (hasBeenSetup) return savedData.bools.ContainsKey(key) ? savedData.bools[key] : defaultValue;

        DebugError();
        return defaultValue;
    }
    public Color GetColor(string key, Color defaultValue = new Color())
    {
        if (hasBeenSetup) return savedData.colors.ContainsKey(key) ? savedData.colors[key] : defaultValue;

        DebugError();
        return defaultValue;
    }
    #endregion GetVar

    #region Dictionary Conversion
    public void ConvertDictionarys(bool toDictionary)
    {
        if (toDictionary) // Convert Arrays to Dictionary(s)
        {
            // Ints
            savedData.ints = new Dictionary<string, int>();
            for (int i = 0; i < savedData.intsA.Length; i++)
                savedData.ints.Add(savedData.intsKeys[i], savedData.intsA[i]);
            // Strings
            savedData.strings = new Dictionary<string, string>();
            for (int i = 0; i < savedData.stringsA.Length; i++)
                savedData.strings.Add(savedData.stringsKeys[i], savedData.stringsA[i]);
            // Floats
            savedData.floats = new Dictionary<string, float>();
            for (int i = 0; i < savedData.floatsA.Length; i++)
                savedData.floats.Add(savedData.floatsKeys[i], savedData.floatsA[i]);
            // Bools
            savedData.bools = new Dictionary<string, bool>();
            for (int i = 0; i < savedData.boolsA.Length; i++)
                savedData.bools.Add(savedData.boolsKeys[i], savedData.boolsA[i]);
            // Colors
            savedData.colors = new Dictionary<string, Color>();
            for (int i = 0; i < savedData.colorsA.Length; i++)
                savedData.colors.Add(savedData.colorsKeys[i], savedData.colorsA[i]);
        }
        else // Convert Dictionary(s) to Arrays
        {
            // Ints
            int i = 0;
            savedData.intsA = new int[savedData.ints.Count];
            savedData.intsKeys = new string[savedData.ints.Count];
            foreach (string key in savedData.ints.Keys)
            {
                savedData.intsA[i] = savedData.ints[key];
                savedData.intsKeys[i] = key;
                i++;
            }
            // Strings
            i = 0;
            savedData.stringsA = new string[savedData.strings.Count];
            savedData.stringsKeys = new string[savedData.strings.Count];
            foreach (string key in savedData.strings.Keys)
            {
                savedData.stringsA[i] = savedData.strings[key];
                savedData.stringsKeys[i] = key;
                i++;
            }
            // Floats
            i = 0;
            savedData.floatsA = new float[savedData.floats.Count];
            savedData.floatsKeys = new string[savedData.floats.Count];
            foreach (string key in savedData.floats.Keys)
            {
                savedData.floatsA[i] = savedData.floats[key];
                savedData.floatsKeys[i] = key;
                i++;
            }
            // Bools
            i = 0;
            savedData.boolsA = new bool[savedData.bools.Count];
            savedData.boolsKeys = new string[savedData.bools.Count];
            foreach (string key in savedData.bools.Keys)
            {
                savedData.boolsA[i] = savedData.bools[key];
                savedData.boolsKeys[i] = key;
                i++;
            }
            // Colors
            i = 0;
            savedData.colorsA = new Color[savedData.colors.Count];
            savedData.colorsKeys = new string[savedData.colors.Count];
            foreach (string key in savedData.colors.Keys)
            {
                savedData.colorsA[i] = savedData.colors[key];
                savedData.colorsKeys[i] = key;
                i++;
            }
        }
    }
    #endregion Dictionary Conversion

    /// <summary>
    /// Encrypt or Decrypt the data. Ex: Encrypted = !Encrypted;
    /// </summary>
    /// <param name="data"></param> Data to Encrypt/Decrypt.
    private string EncryptDecrypt(string data)
    {
        // Encrypt the file
        string result = "";

        for (int i = 0; i < data.Length; i++)
            result += (char)(data[i] ^ randomNameKey[i % randomNameKey.Length]);

        return (encryptFiles ? result : data);
    }

    public void DebugError(string overrideError = "")
    { Debug.LogError("Setup() must be called before running a save/set function"); }
}

[Serializable]
public class SavedData
{
    // All data that can be saved
    // (You can store any variables, but to save them they must be in this class)

    // !!! - Dictionarys can NOT be serialized to json just with Unity's resources - !!!
    public Dictionary<string, int> ints = new();
    public Dictionary<string, float> floats = new();
    public Dictionary<string, string> strings = new();
    public Dictionary<string, bool> bools = new();
    public Dictionary<string, Color> colors = new();
    
    // Arrays for dictionary conversion (Values)
    public int[] intsA = new int[0];
    public float[] floatsA = new float[0];
    public string[] stringsA = new string[0];
    public bool[] boolsA = new bool[0];
    public Color[] colorsA = new Color[0];

    // Arrays for dictionary conversion (Keys)
    public string[] intsKeys = new string[0];
    public string[] floatsKeys= new string[0];
    public string[] stringsKeys = new string[0];
    public string[] boolsKeys = new string[0];
    public string[] colorsKeys = new string[0];
}