using BevCapital.Logon.Domain.Core.Events;
using System;
using System.Linq;

namespace BevCapital.Logon
{
    public static class EventTypeHelper
    {
        public static string GetTypeName(Type type)
        {
            var name = type.FullName.ToLower().Replace("+", ".").Split('.').Last();

            return name;
        }

        public static string GetTypeName<T>()
        {
            return GetTypeName(typeof(T));
        }
    }
}
