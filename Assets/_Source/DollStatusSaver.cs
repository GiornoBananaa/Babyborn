using System;
using System.IO;
using R3;
using UnityEngine;

public class DollStatusSaver : IDisposable
{
    [Serializable]
    private class DollStatusData
    {
        public float Satiety;
        public float Energy;
    }
    
    private const string _dollStatusProperty = "DollStatus";
    private readonly string _globalPath = Application.persistentDataPath + "/" + "{0}.json";
    
    private readonly CompositeDisposable _disposable = new();
    private readonly DollStatus _dollStatus;

    private DollStatusData _saveData;
    
    public DollStatusSaver(DollStatus dollStatus)
    {
        _dollStatus = dollStatus;
        LoadStatus();
        _disposable.Add(dollStatus.Satiety.Subscribe((_) => SaveStatus()));
        _disposable.Add(dollStatus.Energy.Subscribe((_) => SaveStatus()));
    }
    
    public void Dispose()
    {
        _disposable?.Dispose();
    }

    public void LoadStatus()
    {
        _saveData = LoadProperty<DollStatusData>(_dollStatusProperty);
        if (_saveData == null)
        {
            _saveData = new DollStatusData()
            {
                Satiety = 1f,
                Energy = 1f,
            };
        }
        _dollStatus.Satiety.Value = _saveData.Satiety;
        _dollStatus.Energy.Value = _saveData.Energy;
    }
    
    public void SaveStatus()
    {
        if(_saveData == null) return;
        _saveData.Satiety = _dollStatus.Satiety.Value;
        _saveData.Energy = _dollStatus.Energy.Value;
        SaveProperty(_dollStatusProperty, _saveData);
    }

    private T LoadProperty<T>(string fileName, T defaultValue = default)
    {
        string path = string.Format(_globalPath, fileName);
        if(File.Exists(path))
        {
            Debug.Log("Saving property to " + path);
            string json = File.ReadAllText(path);
            T data = JsonUtility.FromJson<T>(json);
            return data;
        }
        return defaultValue;
    }
    
    private void SaveProperty<T>(string fileName, T value)
    {
        string path = string.Format(_globalPath, fileName);
        Debug.Log("Saving property to " + path);
        string json = JsonUtility.ToJson(value);
        File.WriteAllText(path, json);
    }
}