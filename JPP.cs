using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JPP
{
    protected SavedData savedData;
    private static readonly string randomNameKey = "1324657"; // Generate a random number to replace the with.
    private string folderPath, fileName, fileExtension;
    private bool hasBeenSetup = false;
    private bool encryptFiles = false;

    /// <summary> 
    /// This is a string that will be used as a default value for all JPP instances when calling Setup() or ReSetup(). <br/>
    /// If you change this value for 1 JPP instance it will change it for all current and new instances. (This does not save after the game is closed.)
    /// </summary>
    public static string defaultFolderPath = "PERSISTANT_DATA_PATH", defaultExtension = "json";

    #region Main Functions
    /// <summary>
    /// Essential for the proper use of JPP and required before saving or loading any data. <br/>
    /// This function sets-up the JPP instance so that it can be used properly and safely.
    /// </summary>
    public void Setup(string fileName, string fileExtension = "DEFAULT", string folderPath = "DEFAULT", bool encrypt = false)
    {
        if (!hasBeenSetup)
        {
            savedData ??= new SavedData();

            SetFilePath(fileName, fileExtension, folderPath);

            encryptFiles = encrypt;

            LoadAllVars();
            hasBeenSetup = true;
        }
        else
            Debug.LogWarning("JPP: Setup() has already been called on this JPP instance.");
    }

    /// <summary>
    /// Essentially resets the JPP instance, this is dangerous to do, so you must be careful using it. <br/>
    /// This function will overrite all temporary data with the data that is saved on the file.
    /// </summary>
    public void ReSetup(string fileName, string fileExtension = "DEFAULT", string folderPath = "DEFAULT", bool encrypt = false)
    { hasBeenSetup = false; Setup(fileName, fileExtension, folderPath, encrypt); }

    /// <summary> Update or Save the data from this JPP's temporary data to a json file. </summary>
    public void SaveAllVars()
    {
        if (!hasBeenSetup)
            DebugError();
        else
        {
            string dataToSave = JsonUtility.ToJson(savedData, !encryptFiles);
            string path = FilePath();

            File.WriteAllText(path, EncryptDecrypt(dataToSave, encryptFiles));
        }
    }

    /// <summary> Load a json file to this JPP's temporary data storage. </summary>
    public void LoadAllVars()
    {
        string path = FilePath();

        if (File.Exists(path))
        {
            string saveData = File.ReadAllText(path);
            Debug.Log(saveData);
            Debug.Log(EncryptDecrypt(saveData, DataIsEncrypted(saveData)));
            savedData = JsonUtility.FromJson<SavedData>(EncryptDecrypt(saveData, DataIsEncrypted(saveData)));
        }
        else
            Debug.LogWarning($"JPP: File not found at path \"{path}\"");
    }
    #endregion Main Functions

    #region SetVar
    /// <summary> Saves an int to this JPP's temporary data storage. </summary>
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
    /// <summary> Saves a string to this JPP's temporary data storage. </summary>
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
    /// <summary> Saves a float to this JPP's temporary data storage. </summary>
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
    /// <summary> Saves a bool to this JPP's temporary data storage. </summary>
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
    /// <summary> Saves a Color to this JPP's temporary data storage. </summary>
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
    /// <summary> Saves a KeyCode to this JPP's temporary data storage. </summary>
    public void SetKeyCode(string key, KeyCode value)
    {
        if (hasBeenSetup)
        {
            if (savedData.keycodeKeys.Contains(key)) savedData.keycodes[savedData.keycodeKeys.IndexOf(key)] = value;
            else AddKeyValuePair(key, value);
        }
        else
            DebugError();
    }
    /// <summary> Saves a Vector2 to this JPP's temporary data storage. </summary>
    public void SetVector2(string key, Vector2 value)
    {
        if (hasBeenSetup)
        {
            if (savedData.vector2Keys.Contains(key)) savedData.vector2s[savedData.vector2Keys.IndexOf(key)] = value;
            else AddKeyValuePair(key, value);
        }
        else
            DebugError();
    }
    /// <summary> Saves a Vector3 to this JPP's temporary data storage. </summary>
    public void SetVector3(string key, Vector3 value)
    {
        if (hasBeenSetup)
        {
            if (savedData.vector3Keys.Contains(key)) savedData.vector3s[savedData.vector3Keys.IndexOf(key)] = value;
            else AddKeyValuePair(key, value);
        }
        else
            DebugError();
    }
    /// <summary> 
    /// Saves a variable to this JPP's temporary data storage. <br/>
    /// This function automatically detects which type you are saving.
    /// </summary>
    public void SetVar(string key, object value)
    {
        switch (value)
        {
            case int:
                SetInt(key, (int)value);
                break;
            case float:
                SetFloat(key, (float)value);
                break;
            case string:
                SetString(key, (string)value);
                break;
            case bool:
                SetBool(key, (bool)value);
                break;
            case Color:
                SetColor(key, (Color)value);
                break;
            case KeyCode:
                SetKeyCode(key, (KeyCode)value);
                break;
            case Vector2:
                SetVector2(key, (Vector2)value);
                break;
            case Vector3:
                SetVector3(key, (Vector3)value);
                break;
        }
    }
    #endregion SetVar

    #region GetVar
    /// <summary> Returns an int from this JPP's temporary data storage. </summary>
    public int GetInt(string key, int defaultValue = 0)
    {
        if (hasBeenSetup) return savedData.intKeys.Contains(key) ? savedData.ints[savedData.intKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    /// <summary> Returns a float from this JPP's temporary data storage. </summary>
    public float GetFloat(string key, float defaultValue = 0f)
    {
        if (hasBeenSetup) return savedData.floatKeys.Contains(key) ? savedData.floats[savedData.floatKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    /// <summary> Returns a string from this JPP's temporary data storage. </summary>
    public string GetString(string key, string defaultValue = "")
    {
        if (hasBeenSetup) return savedData.stringKeys.Contains(key) ? savedData.strings[savedData.stringKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    /// <summary> Returns a bool from this JPP's temporary data storage. </summary>
    public bool GetBool(string key, bool defaultValue = false)
    {
        if (hasBeenSetup) return savedData.boolKeys.Contains(key) ? savedData.bools[savedData.boolKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    /// <summary> Returns a Color from this JPP's temporary data storage. </summary>
    public Color GetColor(string key, Color defaultValue = new Color())
    {
        if (hasBeenSetup) return savedData.colorKeys.Contains(key) ? savedData.colors[savedData.colorKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    /// <summary> Returns a KeyCode from this JPP's temporary data storage. </summary>
    public KeyCode GetKeyCode(string key, KeyCode defaultValue = KeyCode.None)
    {
        if (hasBeenSetup) return savedData.keycodeKeys.Contains(key) ? savedData.keycodes[savedData.keycodeKeys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    /// <summary> Returns a Vector2 from this JPP's temporary data storage. </summary>
    public Vector2 GetVector2(string key, Vector2 defaultValue = new Vector2())
    {
        if (hasBeenSetup) return savedData.vector2Keys.Contains(key) ? savedData.vector2s[savedData.vector2Keys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    /// <summary> Returns a Vector3 from this JPP's temporary data storage. </summary>
    public Vector3 GetVector3(string key, Vector3 defaultValue = new Vector3())
    {
        if (hasBeenSetup) return savedData.vector3Keys.Contains(key) ? savedData.vector3s[savedData.vector3Keys.IndexOf(key)] : defaultValue;

        DebugError();
        return defaultValue;
    }
    /// <summary> 
    /// Returns a varialbe from this JPP's temporary data storage. <br/>
    /// This function automatically detects which type you are looking for based on the defaultValue parameter.
    /// </summary>
    public object GetVar(string key, object defaultValue)
    {
        switch (defaultValue)
        {
            case int:
                return GetInt(key, (int)defaultValue);
            case float:
                return GetFloat(key, (float)defaultValue);
            case string:
                return GetString(key, (string)defaultValue);
            case bool:
                return GetBool(key, (bool)defaultValue);
            case Color:
                return GetColor(key, (Color)defaultValue);
            case KeyCode:
                return GetKeyCode(key, (KeyCode)defaultValue);
            case Vector2:
                return GetVector2(key, (Vector2)defaultValue);
            case Vector3:
                return GetVector3(key, (Vector3)defaultValue);
        }
        return defaultValue;
    }
    #endregion GetVar

    #region UnsetVar
    /// <summary> Unsets/Deletes all data from this JPP's temporary data storage. </summary>
    public void UnsetAll(string fileName = "DEFAULT", string fileExtension = "DEFAULT", string folderPath = "DEFAULT")
    {
        Dictionary<string, string> formatedPath = FormatedFilePath(fileName, fileExtension, folderPath);
        string path = $"{formatedPath["folder"]}/{formatedPath["name"]}.{formatedPath["name"]}";

        if (path == FilePath())
            savedData = new SavedData();

        // This would also delete all data from the file itself.
        /* string dataToSave = JsonUtility.ToJson(new SavedData(), !encryptFiles);
        File.WriteAllText(path, EncryptDecrypt(dataToSave, encryptFiles));*/
    }
    /// <summary> Unsets/Deletes a variable from this JPP's temporary data storage. Types: [all, int, float, string, bool, color, keycode, vector2, vector3] </summary>
    public void UnsetVar(string key, string type) 
    {
        type = type.ToLower().Replace(" ", "");

        switch (type)
        {
            case "all":
                UnsetVar(key, "int");
                UnsetVar(key, "float");
                UnsetVar(key, "string");
                UnsetVar(key, "bool");
                UnsetVar(key, "color");
                UnsetVar(key, "keycode");
                UnsetVar(key, "vector2");
                UnsetVar(key, "vector3");
                break;
            case "int":
                if (savedData.intKeys.Contains(key))
                {
                    int index = savedData.intKeys.IndexOf(key);
                    savedData.ints.RemoveAt(index);
                    savedData.intKeys.RemoveAt(index);
                }
                break;
            case "float":
                if (savedData.floatKeys.Contains(key))
                {
                    int index = savedData.floatKeys.IndexOf(key);
                    savedData.floats.RemoveAt(index);
                    savedData.floatKeys.RemoveAt(index);
                }
                break;
            case "string":
                if (savedData.stringKeys.Contains(key))
                {
                    int index = savedData.stringKeys.IndexOf(key);
                    savedData.strings.RemoveAt(index);
                    savedData.stringKeys.RemoveAt(index);
                }
                break;
            case "bool":
                if (savedData.boolKeys.Contains(key))
                {
                    int index = savedData.boolKeys.IndexOf(key);
                    savedData.bools.RemoveAt(index);
                    savedData.boolKeys.RemoveAt(index);
                }
                break;
            case "color":
                if (savedData.colorKeys.Contains(key))
                {
                    int index = savedData.colorKeys.IndexOf(key);
                    savedData.colors.RemoveAt(index);
                    savedData.colorKeys.RemoveAt(index);
                }
                break;
            case "keycode":
                if (savedData.keycodeKeys.Contains(key))
                {
                    int index = savedData.keycodeKeys.IndexOf(key);
                    savedData.keycodes.RemoveAt(index);
                    savedData.keycodeKeys.RemoveAt(index);
                }
                break;
            case "vector2":
                if (savedData.vector2Keys.Contains(key))
                {
                    int index = savedData.vector2Keys.IndexOf(key);
                    savedData.vector2s.RemoveAt(index);
                    savedData.vector2Keys.RemoveAt(index);
                }
                break;
            case "vector3":
                if (savedData.vector3Keys.Contains(key))
                {
                    int index = savedData.vector3Keys.IndexOf(key);
                    savedData.vector3s.RemoveAt(index);
                    savedData.vector3Keys.RemoveAt(index);
                }
                break;
        }
    }
    #endregion UnsetVar

    #region File Edit Functions
    /// <summary> Deletes ALL DATA from from the FILE, but keeps the temporary data. </summary>
    public void ClearFileData(string fileName = "DEFAULT", string fileExtension = "DEFAULT", string folderPath = "DEFAULT")
    {
        SavedData tempData = savedData;

        UnsetAll(fileName, fileExtension, folderPath);
        SaveAllVars();

        savedData = tempData;
    }
    /// <summary> DELETES THE FILE this is NOT REVERSABLE. </summary>
    public void DeleteFile(string path = "DEFAULT") 
    {
        path = path == "DEFAULT" ? FilePath() : path;
        File.Delete(path);

        if (File.Exists(path))
            DebugError($"Failed to delete file at path \"{path}\" .");
        else
            Debug.Log($"JPP: Successfully deleted file at path \"{path}\".");
    }
    /// <summary> Creates a copy of the file at a target path. </summary>
    public void DuplicateFile(string newFilePath, string sourceFilePath = "DEFAULT", bool overriteFile = false) 
    {
        sourceFilePath = sourceFilePath == "DEFAULT" ? FilePath() : sourceFilePath;
        if (!overriteFile && File.Exists(newFilePath))
        {
            DebugError($"Could not duplicate file, there is already a file at path \n{newFilePath}\n, and overriteFile is set to false");
        }
        else
        {
            File.Copy(sourceFilePath, newFilePath, overriteFile);
            Debug.Log($"JPP: Successfully duplicated file from \n{sourceFilePath}\n to \n{newFilePath}\n.");
        }
    }
    /// <summary> Moves the file to a target path. </summary>
    public void MoveFile(string newPath, string currentPath = "DEFAULT")
    {
        currentPath = currentPath == "DEFAULT" ? FilePath() : currentPath;
        File.Move(currentPath, newPath);
        Debug.Log($"JPP: Successfully moved \"{currentPath}\" to \n{newPath}\n.");
    }
    /// <summary> Renames the file. </summary>
    public void RenameFile(string newName, string path = "DEFAULT")
    {
        path = path == "DEFAULT" ? FilePath() : path;
        string folderName = Path.GetDirectoryName(path);
        string fileName = Path.GetFileName(path);
        string fileExtension = Path.GetExtension(path);

        File.Move(path, $"{path.Replace(fileName, "")}{newName}{fileExtension}");
        Debug.Log($"JPP: Successfully renamed \"{fileName}\" to \n{newName}\n.");
    }
    /// <summary> Changes the extension of the file. </summary>
    public void ChangeFileExtension(string newExtension, string path = "DEFAULT")
    {
        path = path == "DEFAULT" ? FilePath() : path;
        string folderName = Path.GetDirectoryName(path);
        string fileExtension = Path.GetExtension(path);
        string fileName = Path.GetFileName(path).Replace(fileExtension, "");

        File.Move(path, $"{path.Replace(fileExtension, "")}.{newExtension}");
        Debug.Log($"JPP: Successfully changed \"{fileName}{fileExtension}\" to \n{fileName}.{newExtension}\n.");
    }
    #endregion File Edit Functions

    #region Private Util Functions
    /// <summary> Adds a new variable to this JPP's temporary data storage. (Auto-detects variable type) </summary>
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
            case KeyCode:
                savedData.keycodeKeys.Add(key);
                savedData.keycodes.Add((KeyCode)value);
                break;
            case Vector2:
                savedData.vector2Keys.Add(key);
                savedData.vector2s.Add((Vector2)value);
                break;
            case Vector3:
                savedData.vector3Keys.Add(key);
                savedData.vector3s.Add((Vector3)value);
                break;
        }

    }
    /// <summary> Encrypts/Decrypts data if encrypt is true, else it returns the input data. </summary>
    protected string EncryptDecrypt(string data, bool encrypt)
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
    /// <summary> Checks if the file is in json format. If so, returns false; else, returns true. </summary>
    protected bool DataIsEncrypted(string data)
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

    /// <summary> Returns the file path as a dictionary which is practically an associative string array with key value pairs. </summary>
    protected Dictionary<string, string> FormatedFilePath(string fileName, string fileExtension = "DEFAULT", string folderPath = "DEFAULT")
    {
        Dictionary<string, string> pathParts = new();

        folderPath = folderPath.Replace("DEFAULT", defaultFolderPath);
        fileExtension = fileExtension.Replace("DEFAULT", defaultExtension);

        folderPath = folderPath.Replace("PERSISTANT_DATA_PATH", $"{Application.persistentDataPath}");
        folderPath = folderPath.Replace("CURRENT_PATH", this.folderPath);

        pathParts["folder"] = folderPath;
        pathParts["name"] = fileName;
        pathParts["extension"] = fileExtension;

        return pathParts;
    }

    /// <summary> Formats a message to be sent to the Unity console as an error. </summary>
    private void DebugError(string overrideError = "")
    { overrideError = overrideError == "" ? "Setup() must be called before running a save/set or get function" : overrideError; Debug.LogError($"JPP: {overrideError}."); }
    #endregion Private Util Functions

    #region Public Util Functions
    /// <returns> True if the Setup() function has been called, False if the Setup() function has not been called. </returns>
    public bool HasBeenSetup() { return hasBeenSetup; }
    /// <returns> True if encryption is enabled, False if encryption is disabled. </returns>
    public bool EncryptFilesEnabled() { return encryptFiles; }
    /// <returns>string "{folderPath}/{fileName}.{fileExtension}"</returns>
    public string FilePath() { return $"{folderPath}/{fileName}.{fileExtension}"; }
    /// <summary> Returns a string array of the file path segmented into 3 essential parts. </summary>
    /// <returns>string [folderPath, fileName, fileExtension]</returns>
    public string[] FilePathParts()
    { return new string[] {folderPath, fileName, fileExtension}; }
    /// <summary> Sets the JPP's path to look for the json file or to save the json to. </summary>
    public void SetFilePath(string fileName, string fileExtension = "DEFAULT", string folderPath = "DEFAULT")
    {
        Dictionary<string, string> strings = FormatedFilePath(fileName, fileExtension, folderPath);
        fileName = strings["name"];
        fileExtension = strings["extension"];
        folderPath = strings["folder"];

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        this.folderPath = folderPath == "DEFAULT" ? defaultFolderPath : folderPath;
        this.fileName = fileName;
        this.fileExtension = fileExtension == "DEFAULT" ? defaultExtension : fileExtension;
    }
    /// <summary> Returns the JPP's savedData variable. </summary>
    public SavedData GetTempDataAsClass() { return savedData; }
    #endregion Public Util Functions
}

/// <summary> A class that is setup for being saved to JSON, and contains all data that is saved/loaded while in play-mode. </summary>
[System.Serializable]
public class SavedData
{
    // All data that can be saved

    // Lists for saved data
    [Header(" - Lists of data:")]
    public List<int> ints = new();
    public List<float> floats = new();
    public List<string> strings = new();
    public List<bool> bools = new();
    public List<Color> colors = new();
    public List<KeyCode> keycodes = new();
    public List<Vector2> vector2s = new();
    public List<Vector3> vector3s = new();

    // Keys for the lists
    [Header(" - Lists of keys:")]
    public List<string> intKeys = new();
    public List<string> floatKeys = new();
    public List<string> stringKeys = new();
    public List<string> boolKeys = new();
    public List<string> colorKeys = new();
    public List<string> keycodeKeys = new();
    public List<string> vector2Keys = new();
    public List<string> vector3Keys = new();
}