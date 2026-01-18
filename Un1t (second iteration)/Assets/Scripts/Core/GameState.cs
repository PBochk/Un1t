using System.Xml.Serialization;
using UnityEngine; 

[XmlRoot("GameState")]
public class GameState
{
    [XmlElement("NextSceneIndex")] public int NextSceneIndex = -1;
    [XmlElement("PlayerSaveData")] public PlayerSaveData PlayerSaveData;
    [XmlIgnore] public PlayerModel PlayerModel;
    /// <summary>
    /// A parameterless contructor that exist for the purpose of serialization
    /// </summary>
    public GameState()
    {
        PlayerModel = new PlayerModel();
    }

    public GameState(PlayerModel playerModel)
    {
        PlayerModel = playerModel;
        PlayerModel.PlayerDeath += EntryPoint.Instance.OnPlayerDeath;
    }

    public void OnLoadFromSave()
    {
        PlayerModel.FromSaveData(PlayerSaveData);
    }

    public void OnSave(int nextSceneIndex = -1)
    {
        if(nextSceneIndex != -1)
            NextSceneIndex = nextSceneIndex;
        if(NextSceneIndex != -1)
            PlayerSaveData = PlayerModel.ToSaveData();
    }

    public void OnSaveDelete()
    {
        PlayerSaveData = null;
        NextSceneIndex = -1;
    }
}