using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ReadItLater.Data
{
    public static class PropertyInfoExtensions
    {
        public static DataColumn GetProperty(this PropertyInfo[] properties, string name)
        {
            var type = properties.Single(p => p.Name.Equals(name)).PropertyType;

            if (type.IsEnum)
                type = typeof(int);

            return new DataColumn(name, Nullable.GetUnderlyingType(type) ?? type);
        }
    }
}
