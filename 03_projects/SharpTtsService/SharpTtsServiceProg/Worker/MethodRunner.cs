namespace SharpTtsServiceProg.Worker
{
    public class MethodRunner
    {
        private List<string> methodNames;
        protected object obj;
        protected Type objType;

        public MethodRunner()
        {
            this.obj = this;
            this.objType = obj.GetType();
            methodNames = SetMethodNames();
        }

        public virtual async Task RunMethodAsync(string methodName, params object[] args)
        {
        }

        public List<string> GetMethodNames()
        {
            return methodNames;
        }

        private List<string> SetMethodNames()
        {
            var result = objType.GetMethods()
                .Select(m => m.Name).ToList();

            result.Remove("GetType");
            result.Remove("GetHashCode");
            result.Remove("ToString");
            result.Remove("Equals");
            result.Remove("RunMethodAsync");
            result.Remove("GetMethodNames");

            return result;
        }
    }
}
