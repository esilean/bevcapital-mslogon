using BevCapital.Logon.Domain.Core.Events;
using System;

namespace BevCapital.Logon
{
    public static class EventTypeHelper
    {
        public static string GetTypeName(Type type)
        {
            var name = type.FullName.ToLower().Replace("+", ".");

            if (type is IEvent)
            {
                name += "_event";
            }

            return name;
        }

        public static string GetTypeName<T>()
        {
            return GetTypeName(typeof(T));
        }
    }
}
