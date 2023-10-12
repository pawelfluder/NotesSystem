namespace SharpHttpRequesterTests.JsonObjects
{
    public class Updates
    {
        public List<object> matches { get; set; }
        public List<object> blocks { get; set; }
        public List<object> inbox { get; set; }
        public List<object> liked_messages { get; set; }
        public List<object> harassing_messages { get; set; }
        public List<object> lists { get; set; }
        public List<object> goingout { get; set; }
        public List<object> deleted_lists { get; set; }
        public List<object> squads { get; set; }
        public DateTime last_activity_date { get; set; }
        public PollInterval poll_interval { get; set; }
    }

    public class PollInterval
    {
        public int standard { get; set; }
        public int persistent { get; set; }
    }
}
