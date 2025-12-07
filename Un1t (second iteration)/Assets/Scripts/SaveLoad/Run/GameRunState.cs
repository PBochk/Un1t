

using System.Xml.Serialization;

[XmlRoot("RunState")]
public class GameRunState
{
    [XmlElement("PlayerModel")] public PlayerModel PlayerModel;
    [XmlElement("Stage number")] public int StageNumber;


    /// <summary>
    /// A parameterless contructor that exist for the purpose of serialization
    /// </summary>
    public GameRunState()
    {
    }

    public GameRunState(PlayerModel playerModel)
    {
        PlayerModel = playerModel;
    }
}