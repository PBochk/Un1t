

using System.Xml.Serialization;

public class GameRunState
{
    [XmlElement("Player")]
    private PlayerModel playerModel;
    private int stageNumber;
        
    public PlayerModel PlayerModel => playerModel;
    public int StageNumber => stageNumber;


    /// <summary>
    /// A parameterless contructor that exist for the purpose of serialization
    /// </summary>
    public GameRunState()
    {
    }
}