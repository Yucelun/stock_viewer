using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveFile : BaseSingleton
{
    public override void Awake()
    {
    }

    public override void Start()
    {
    }

    public override void OnDestroy()
    {
    }

    public string directoryPath
    {
        get
        {
            return Application.persistentDataPath + "/StreamingAssets" + "/Save";
        }
    }

    public bool exists(string fileName)
    {
        if (!Directory.Exists(this.directoryPath))
        {
            return false;
        }

        var filePath = this.getFilePath(fileName);
        if (!File.Exists(filePath))
        {
            return false;
        }

        return true;
    }

    protected string getFilePath(string fileName)
        {
            return this.directoryPath + "/" + fileName + ".txt";
    }

    public void save(string fileName, object obj)
    {
        var jsonString = JsonConvert.SerializeObject(obj);
        if (!Directory.Exists(this.directoryPath))
        {
            Directory.CreateDirectory(this.directoryPath);
        }

        var filePath = this.getFilePath(fileName);
        StreamWriter streamwriter = File.CreateText(filePath);
        streamwriter.Write(jsonString);
        streamwriter.Close();

    }

    public T load<T>(string fileName)
    {
        var filePath = this.getFilePath(fileName);
        StreamReader streamReader = File.OpenText(filePath);
        var data = streamReader.ReadToEnd();
        streamReader.Close();
        return (T)JsonConvert.DeserializeObject(data, typeof(T));
    }
}