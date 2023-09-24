namespace SharpConfigProg.Preparer
{
    public partial interface IPreparer
    {
        Dictionary<string, object> Prepare();
    }

    public partial interface IPreparer
    {
        public interface ILocalProgramData : IPreparer { }
        public interface IWinderPreparer : IPreparer { }
        public interface IOnlyRootPathsPreparer : IPreparer { }
        public interface INotesSystem : IPreparer { }
        public interface INotesSystem2 : IPreparer { }
    }
}