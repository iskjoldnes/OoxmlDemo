using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OoxmlDemo.ExternalServices.Tools
{
    public static class PropertyNavigator
    {
        public static Dictionary<Type, Func<string, Func<object, object>>> _typeCache = new Dictionary<Type, Func<string, Func<object, object>>>();
        public static Func<string, Func<object, object>> GetGetters( Type type)
        {
            Func<string, Func<object, object>> getters;
            if ( ! _typeCache.TryGetValue( type, out getters))
            {
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    getters = i =>
                    {
                        return o =>
                        {
                            if (o is IEnumerable)
                            {
                                int n;
                                int.TryParse(i, out n);
                                foreach (var oo in o as IEnumerable)
                                {
                                    if (n-- == 0)
                                    {
                                        return oo;
                                    }
                                }
                            }
                            return null;
                        };
                    };
                }
                else
                {
                    var allProperties = type.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public)
                        .Select(p => new KeyValuePair<string, Func<object, object>>(p.Name, o => p.GetValue(o)))
                        .Concat(type.GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public)
                        .Select(p => new KeyValuePair<string, Func<object, object>>(p.Name, o => p.GetValue(o))))
                        .ToDictionary(kvp => kvp.Key.ToLower(), kvp => kvp.Value);
                    getters = pn =>
                    {
                        Func<object, object> getter;
                        return allProperties.TryGetValue(pn.ToLower(), out getter) ? getter : null;
                    };
                }
                _typeCache[type] = getters;
            }
            return getters;
        }

        public static Func<object, object> GetGetter( Type type, string propertyName)
        {
            return (GetGetters(type)?? (o => null))(propertyName);
        }

        public static object GetPropertyValue(object entity, string propertyName)
        {
            return entity != null 
                ? (GetGetter(entity.GetType(), propertyName) ?? (o => null))(entity)
                : null;
        }

        public static object GetPropertyValue(object entity, IEnumerable<string> propertyNames)
        {
            foreach( var propertyName in propertyNames)
            {
                if( (entity = (GetGetter(entity.GetType(), propertyName) ?? (o => null))(entity)) == null)
                {
                    break;
                }
            }
            return entity;
        }

    }
}