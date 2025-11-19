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
        serializer.Save(gameRunState, fullPath);
    }
    
    public bool TryLoad(ref GameRunState gameRunState)
    {
        if (!serializer.TryLoad(out var newState, fullPath)) return false;
        gameRunState = newState;
        return true;

    }
}