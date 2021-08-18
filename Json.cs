using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class Json : MonoBehaviour
{
    [SerializeField] TextAsset asset;
    public static Dictionary<string,string[]> UIText;
    
    public static void saveJson<T>(string filename,T CustomData)
    {
        string tmp = JsonUtility.ToJson(CustomData,true);
        File.WriteAllText($"{Application.dataPath}/Save/{filename}.json", tmp);
    }
    public static T loadSaveFile<T>(string filename)
    {
        string tmp="";
        try
        {
            tmp = File.ReadAllText($"{Application.persistentDataPath}/Save/{filename}.json");
        }
        catch (System.Exception)
        {
            print("error");//TODO:에러메시지 띄워주기
        }
        return JsonUtility.FromJson<T>(tmp);
    }
    public static Dictionary<string,string[]> LoadLocaleJson(string state)
    {
        var data = File.ReadAllText($"{Application.dataPath}/locale/{state}.json");
        Dictionary<string,string[]> localeText = JsonConvert.DeserializeObject<Dictionary<string,string[]>>(data);
        return localeText;
    }
    private void Awake()
    {
        UIText = JsonConvert.DeserializeObject<Dictionary<string,string[]>>(asset.ToString());
    }
}
public enum locale
{
    ko,
    en,
    cn
}