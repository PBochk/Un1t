using UnityEngine;
using System.IO;

public class GameSaveLoader
{
    //Using ref instead of out, because of possible NullReference
    public bool TryLoadGame(GameSerializer serializer, ref GameState gameState)
    {
        //gameState = null
        //It's better to also be passed as parameter, but nah
        var path = Application.persistentDataPath + "/save.xml";
        if (!File.Exists(path)) return false;
        gameState = serializer.Load();
        return true;
    }

    public void SaveGame(GameSerializer serializer, GameState gameState)
    {
        serializer.Save(gameState);
    }
}