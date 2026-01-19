/// <summary>
/// Reads files and writes to it
/// </summary>
public abstract class GameSerializer
{
    public abstract void Save(GameState gameState);
    public abstract GameState Load();
}