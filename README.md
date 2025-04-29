# JsonPlayerPrefs
Using the style of Unity's PlayerPrefs save data to json files on your hard drive

# Unity c# reference code
// Create new JPP variable/instance - readonly is not necessary, but it is recommended
private readonly JPP jpp = new();

// Setup the instance by telling it what name, extension, and filePath you would like it to use for saving data.
jpp.Setup("MyFilesName", "json", "C:/MyFolder");
// Default values for jpp.Setup() are as follows:
/*
 * Filename      - "New-JPP-File"
 * FileExtention - "json"
 * FolderPath    - UnityEngine.Application.persistentDataPath
 */

// Example for saving a string:
public void SaveTheData()
{
  // jpp.GetString("KEY_NAME", "DEFAULT_VALUE");
  jpp.SetString("username", username);

  // Anything you set is only temporarily saved in the jpp instance.
  // To save/update the file to the data in the jpp you need to run this function.
  jpp.SaveAllVars();
}
public void LoadTheData()
{
  // jpp.GetString("KEY_NAME", "DEFAULT_VALUE");
  username = jpp.GetString("username", username);
}
