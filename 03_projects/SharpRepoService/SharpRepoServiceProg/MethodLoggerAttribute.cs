using MethodDecorator.Fody.Interfaces;
using System;
using System.Reflection;

namespace SharpRepoServiceCoreProj
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Assembly | AttributeTargets.Module)]
    public class MethodLoggerAttribute : Attribute, IMethodDecorator
    {
        private bool enable = false;
        string separatorStart = "<--------------------";
        string separatorEnd = "-------------------->";
        MethodBase methodBase;
        // instance, method and args can be captured here and stored in attribute instance fields
        // for future usage in OnEntry/OnExit/OnException
        public void Init(object instance, MethodBase methodBase, object[] args)
        {
            if (enable)
            {
                Console.WriteLine(separatorStart);
                Console.WriteLine($"methodName: {methodBase.Name}");
                foreach (var arg in args)
                {
                    Console.WriteLine($"arg: {arg}");
                }
                Console.WriteLine(separatorEnd);
                Console.WriteLine(string.Empty);
                //Console.WriteLine(string.Format("Init: {0} [{1}]", method.DeclaringType.FullName + "." + method.Name, args.Length));
            }
        }

        public void OnEntry()
        {
                
        }

        public void OnExit()
        {
            //var normalMethod = methodBase as MethodInfo;
            //if (normalMethod != null)
            //{
            //    var gg = normalMethod.ReturnType;
            //    ParameterInfo gg2 = normalMethod.ReturnParameter;
                
            //}
        }

        public void OnException(Exception exception)
        {
            if (enable)
            {
                Console.WriteLine(string.Format("OnException: {0}: {1}", exception.GetType(), exception.Message));
            }
        }
    }
}
