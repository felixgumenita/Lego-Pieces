using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class SaveLoad
{
    public static string PlayerDataAdditional = "_player";
    public static void SaveCellData<T>(List<T> save, string fileName)
    {
        Debug.Log(GetPath(fileName));
        string content = JsonHelper.ToJson<T>(save.ToArray());
        WriteFile(GetPath(fileName), content);
    }

    public static void SavePlayerData<T>(T save, string fileName)
    {
        Debug.Log(GetPath(fileName));
        string content = JsonUtility.ToJson(save);
        WriteFile(GetPath(fileName), content);
    }

    public static List<T> LoadLevelData<T>(string fileName)
    {
        string content = ReadFile(GetPath(fileName));

        if(string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<T>();
        }

        List<T> res = JsonHelper.FromJson<T>(content).ToList();

        return res;
    }

    public static T LoadPlayerData<T>(string fileName)
    {
        string content = ReadFile(GetPath(fileName));

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return default(T);
        }

        T res = JsonUtility.FromJson<T>(content);

        return res;
    }

    private static string GetPath(string fileName)
    {
        return $"{Application.persistentDataPath}/{fileName}";
    }

    private static void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using(StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using(StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }
}
