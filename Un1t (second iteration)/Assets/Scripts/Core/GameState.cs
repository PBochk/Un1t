using System.Xml.Serialization;

[XmlRoot("GameState")]
public class GameState
{
    //[XmlElement("PlayerModel")] public PlayerModel PlayerModel;
    [XmlElement("NextSceneIndex")] public int NextSceneIndex;


    /// <summary>
    /// A parameterless contructor that exist for the purpose of serialization
    /// </summary>
    public GameState()
    {
    }

    //public GameState(PlayerModel playerModel)
    //{
    //    PlayerModel = playerModel;
    //}

    public void OnLoadFromSave()
    {
    }

    public void OnSave(int nextSceneIndex)
    {
    }

    public void OnSaveDelete()
    {
    }
}