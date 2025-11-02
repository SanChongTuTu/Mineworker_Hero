using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileHandler
{
    public static void SaveToJSON<T>(List<T> dataToSave, string fileName)
    {
        string content = JsonHelper.ToJson<T>(dataToSave);
        
        Debug.Log(GetPath(fileName));
        
        WriteFile(GetPath(fileName), content);
    }

    public static List<T> LoadFromJSON<T>(string fileName)
    {
        string content = ReadFile(GetPath(fileName));

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<T>();
        }

        List<T> result = JsonHelper.FromJson<T>(content).ToList();
        return result;
    }

    private static string GetPath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    private static void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.Write(content);
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                string content = streamReader.ReadToEnd();
                return content;
            }
        }
        
        return "";
    }
}

public static class JsonHelper
{
    public static List<T> FromJson<T>(string json)
    {
        if (string.IsNullOrEmpty(json) || json == "{}")
            return new List<T>();
        
        if (json.StartsWith("{\"Items\":"))
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items != null ? wrapper.Items.ToList() : new List<T>();
        }
        
        try
        {
            T item = JsonUtility.FromJson<T>(json);
            return new List<T> { item };
        }
        catch
        {
            return new List<T>();
        }
    }

    public static string ToJson<T>(List<T> list, bool prettyPrint = false)
    {
        if (list == null || list.Count == 0)
            return "{}";
        
        if (list.Count == 1)
        {
            return JsonUtility.ToJson(list[0], prettyPrint);
        }
        
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = list.ToArray();
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
