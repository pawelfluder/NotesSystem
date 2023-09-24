using System.Reflection;

namespace SharpRepoBackendProg.Repetition
{
    internal partial class AdditionalRegistrationBase
    {
        protected List<Action> actionList { get; private set; }

        public AdditionalRegistrationBase()
        {
            actionList = new List<Action>();
            AddMethodsToList();
        }

        public void Invoke()
        {
            foreach (var action in actionList)
            {
                action.Invoke();
            }
        }

        public void AddMethodsToList()
        {
            Type type = this.GetType();
            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (MethodInfo methodInfo in methods)
            {
                if (methodInfo.DeclaringType == type &&
                    methodInfo.Name.StartsWith("Register"))
                {
                    Action methodAction = (Action)Delegate.CreateDelegate(typeof(Action), this, methodInfo);
                    actionList.Add(methodAction);
                }
            }
        }
    }
}
