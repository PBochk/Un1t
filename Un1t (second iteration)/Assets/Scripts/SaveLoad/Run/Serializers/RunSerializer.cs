public abstract class RunSerializer
{
    public abstract string FileExtension { get;  }

    public abstract void Save(GameRunState gameRunState, string path);
    public abstract bool TryLoad(out GameRunState gameRunState, string path);
}