namespace SharpConfigProg.Preparer
{
    public partial interface IPreparer
    {
        Dictionary<string, object> Prepare();
    }

    public partial interface IPreparer
    {
        public interface ILocalProgramData { }
        public interface IWinder { }
        public interface IOnlyRootPaths { }
        public interface INotesSystem { }
    }
}