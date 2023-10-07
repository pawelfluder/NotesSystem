namespace SharpConfigProg.ConfigPreparer
{
    public interface IPreparer
    {
        Dictionary<string, object> Prepare();
    }
}