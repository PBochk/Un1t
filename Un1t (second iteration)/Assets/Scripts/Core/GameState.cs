using System.Xml.Serialization;

[XmlRoot("GameState")]
public class GameState
{
    //[XmlElement("PlayerModel")] public PlayerModel PlayerModel;
    [XmlElement("NextSceneIndex")] public int NextSceneIndex;
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
    }

    public void OnLoadFromSave()
    {
        PlayerModel.FromSaveData(PlayerSaveData);
    }

    public void OnSave(int nextSceneIndex)
    {
        PlayerSaveData = PlayerModel.ToSaveData();
        NextSceneIndex = nextSceneIndex;
    }

    public void OnSaveDelete()
    {
    }
}