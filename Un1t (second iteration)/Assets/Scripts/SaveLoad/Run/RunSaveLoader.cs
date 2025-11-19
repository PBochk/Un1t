using UnityEngine;

public class RunSaveLoader
{
    private const string FILE_NAME = "run";
    private string fullPath;
    
    private RunSerializer serializer;

    public RunSaveLoader(RunSerializer serializer)
    {
        this.serializer = serializer;
        fullPath = $"{Application.persistentDataPath}/{FILE_NAME}.{serializer.FileExtension}";
    }

    public void Save(GameRunState gameRunState)
    {
        throw new System.NotImplementedException();
    }
    
    public bool TryLoad(ref GameRunState gameRunState)
    {
        throw new System.NotImplementedException();
    }
}