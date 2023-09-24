using System.Reflection;

namespace SharpConfigProg.Repetition
{
    internal partial class PreparerRegistrationBase
    {
        public Action MethodDelegate { get; private set; }

        public PreparerRegistrationBase()
        {
            AddMethodsToList();
        }

        public void AddMethodsToList()
        {
            Type type = typeof(PreparerRegistration);
            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (MethodInfo methodInfo in methods)
            {
                if (methodInfo.DeclaringType == type &&
                    methodInfo.Name.StartsWith("Register"))
                {
                    Action methodAction = (Action)Delegate.CreateDelegate(typeof(Action), this, methodInfo);
                    MethodDelegate += methodAction;
                }
            }
        }
    }
}
