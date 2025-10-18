using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;

namespace UI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class QAttribute : Attribute
    {
        public string Name { get; set; }
        public string Classes { get; set; }
        public bool Existing { get; set; }
    }

    public static class QueryAttributeProcessor
    {
        public static void AssignElementResults(this VisualElement root, object target)
        {
            foreach (var (member, att) in target.GetType().GetQAttributeMembers())
            {
                Action<object, object> setMemberValue;
                Type type;
                var fieldName = member.Name;
                if (member is FieldInfo field)
                    (type, setMemberValue) = (field.FieldType, field.SetValue);
                else if (member is PropertyInfo property)
                    (type, setMemberValue) = (property.PropertyType, property.SetValue);
                else continue;

                if (fieldName.StartsWith("_"))
                {
                    fieldName = char.ToUpperInvariant(fieldName[1]) + fieldName[2..];
                }

                var queryResult = string.IsNullOrEmpty(att.Name) && att.Classes == null
                    ? root.Q(fieldName)
                    : root.Q(att.Name, att.Classes);
                if (queryResult == null)
                {
                    throw new Exception("Couldn't find element with name " + fieldName + " for type " +
                                        target.GetType().Name);
                }
#if UNITY_EDITOR
                if (!type.IsAssignableFrom(queryResult.GetType()))
                {
                    throw new Exception(
                        $"Found element with wrong type {queryResult.GetType()} for field {fieldName} in {target.GetType().Name}");
                }
#endif

                setMemberValue(target, queryResult);
            }
        }

        private const BindingFlags QAttributeTargetFlags = BindingFlags.FlattenHierarchy
                                                           | BindingFlags.Instance
                                                           | BindingFlags.Static
                                                           | BindingFlags.Public
                                                           | BindingFlags.NonPublic;

        public static (MemberInfo, QAttribute)[] GetQAttributeMembers(this Type type)
        {
            return (from member in GetFieldsAndProperties()
                let att = member.GetCustomAttribute<QAttribute>()
                where att != null
                select (member, att)).ToArray();

            IEnumerable<MemberInfo> GetFieldsAndProperties()
            {
                foreach (var field in type.GetFields(QAttributeTargetFlags))
                    yield return field;
                foreach (var prop in type.GetProperties(QAttributeTargetFlags))
                    yield return prop;
            }
        }
    }
}