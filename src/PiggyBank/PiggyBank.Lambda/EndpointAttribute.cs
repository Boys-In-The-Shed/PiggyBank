using System;

namespace PiggyBank.Lambda 
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class EndpointAttribute : Attribute 
    {
        public EndpointAttribute(string method, string path)
        {
            Method = method;
            Path = path;
        }

        public string Method { get; }
        public string Path { get; }
    }
}