# JsonPlayerPrefs
Using the style of Unity's PlayerPrefs save data to json files on your hard drive

# Unity c# reference code (Best read in "Code" view)
// Create new JPP variable/instance - readonly is not necessary, but it is recommended
private readonly JPP jpp = new();

### General Setup
// Setup the instance by telling it what name, extension, and filePath you would like it to use for saving data.
// jpp.Setup() must be called before running any other functions. It doesn't hurt to call it more than once, although it won't be helpful.
jpp.Setup("MyFilesName", "json", "C:/MyFolder", true);
// Default values for jpp.Setup() are as follows:
 * FileExtention - "json"
 * FolderPath    - UnityEngine.Application.persistentDataPath
 * Encrypt       - false


// The strings "DEFAULT" and "PERSISTANT_DATA_PATH" will be replaced with the default folder path when used in the folder path
Ex: jpp.Setup("MyFilesName", "json", "DEFAULT/MyFolder");

## Saving / Loading
### Saving a string
public void SaveTheData()
{
  // jpp.SetString("KEY_NAME", "NEW_VALUE");
  jpp.SetString("username", username);

  // Anything you set is only temporarily saved in the jpp instance.
  // To save/update the file to the data in the jpp you need to run this function.
  jpp.SaveAllVars();
}
### Loading a string
public void LoadTheData()
{
  // jpp.GetString("KEY_NAME", "DEFAULT_VALUE");
  username = jpp.GetString("username", username);
}

## Capabilities and Restrictions
### Saveable datatypes
As of version 1.0 the saveable datatypes are:
 * Int
 * Float
 * String
 * Boolean
 * Color
 * (Keycode, Vector2, and Vector3 will likely be added soon)
### File settings
You can choose a name, folder path, and extension for your data, and can even change files mid-game
by running the function ReSetup() and inputting different values.
### Basic Encryption
If you enable encryption before calling Setup() or ReSetup()

## Missing Features
At this time, you cannot unset/delete any data that you save.
