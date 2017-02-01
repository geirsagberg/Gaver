using System;
using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Serialization;

namespace Gaver.Web.Utils
{
    public class SignalRContractResolver : IContractResolver
    {

        private readonly Assembly assembly;
        private readonly IContractResolver camelCaseContractResolver;
        private readonly IContractResolver defaultContractSerializer;

        public SignalRContractResolver()
        {
            defaultContractSerializer = new DefaultContractResolver();
            camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
            assembly = typeof(Hub).GetTypeInfo().Assembly;
        }

        public JsonContract ResolveContract(Type type)
        {
            return type.GetTypeInfo().Assembly.Equals(assembly)
                ? defaultContractSerializer.ResolveContract(type)
                : camelCaseContractResolver.ResolveContract(type);
        }
    }
}
