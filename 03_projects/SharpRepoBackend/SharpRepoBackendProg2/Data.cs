namespace SharpRepoBackendProg2
{
    public class Data
    {
        public Data(string name, List<string> content)
        {
            Name = name;
            Content = content;
        }

        public string Name { get; set; }
        public List<string> Content { get; set; }
    }
}
