using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONSaving : MonoBehaviour
{
    private SaveData playerData;

    private string path = "";
    private string persistentPath = "";

    private void Awake() => SetPaths();

    private void SetPaths()
    {
        //This one isn't even in use
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";

        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    //Checks the Persistent Data path for all files within the location
    public bool CheckForExistingSave()
    {
        string[] saves = Directory.GetFiles(Application.persistentDataPath);
        if (saves.Length == 0)
        {
            Debug.Log("No Existing Save Files");
            return false;
        }

        for (int i = 0; i < saves.Length; i++)
        {
            //This works so long as the file name contains SaveData
            if (!saves[i].Contains("SaveData")) continue;
            StreamReader reader = new StreamReader(saves[i]);
            string json = reader.ReadToEnd();

            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log(data.saveFileName);
        }
        return true;
    }

    public void SaveData()
    {
        string savePath = persistentPath;
        //Debug.Log("Saving Data at " + savePath);

        CreatePlayerData();
        string json = JsonUtility.ToJson(playerData, true);
        Debug.Log(json);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    private void CreatePlayerData()
    {
        playerData = new SaveData();
    }

    public void LoadData()
    {
        StreamReader reader = new StreamReader(persistentPath);
        string json = reader.ReadToEnd();

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        //Debug.Log("this isn't doing anything yet");
        SetPlayerValues(data);
    }

    private void SetPlayerValues(SaveData data)
    {
        //Load back in all saved data to appropriate places
    }
}