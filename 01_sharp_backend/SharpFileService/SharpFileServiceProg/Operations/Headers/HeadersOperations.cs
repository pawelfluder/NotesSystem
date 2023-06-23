namespace SharpFileServiceProg.Operations.Headers
{
    public class HeadersOperations
    {
        public HeadersOperationsConversion Convert { get; }
        public HeadersOperationsSelectNeeded Select { get; }

        public HeadersOperations()
        {
            Convert = new HeadersOperationsConversion();
            Select = new HeadersOperationsSelectNeeded();
        }
    }
}
