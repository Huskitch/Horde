#pragma warning disable SA1633 // File must have header
#pragma warning disable SA1615 // Xml docs
#pragma warning disable SA1611 // More xml docs
#pragma warning disable SA1600 // More xml docs

using System;
using System.Reflection;
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1633:FileMustHaveHeader", Justification = "Reviewed.")]
namespace HoardeGame.Tweening
{
    internal class GlideInfo
    {
        private static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public string PropertyName { get; private set; }

        public Type PropertyType { get; private set; }

        private FieldInfo field;
        private PropertyInfo prop;
        private object target;

        public object Value
        {
            get
            {
                return field != null ?
                    field.GetValue(target) :
                    prop.GetValue(target, null);
            }

            set
            {
                if (field != null)
                {
                    field.SetValue(target, value);
                }
                else
                {
                    prop.SetValue(target, value, null);
                }
            }
        }

        public GlideInfo(object target, PropertyInfo info)
        {
            this.target = target;
            prop = info;
            PropertyName = info.Name;
            PropertyType = prop.PropertyType;
        }

        public GlideInfo(object target, FieldInfo info)
        {
            this.target = target;
            field = info;
            PropertyName = info.Name;
            PropertyType = info.FieldType;
        }

        public GlideInfo(object target, string property, bool writeRequired = true)
        {
            this.target = target;
            PropertyName = property;

            var targetType = target as Type ?? target.GetType();

            if ((field = targetType.GetField(property, flags)) != null)
            {
                PropertyType = field.FieldType;
            }
            else if ((prop = targetType.GetProperty(property, flags)) != null)
            {
                PropertyType = prop.PropertyType;
            }
            else
            {
                // Couldn't find either
                throw new Exception($"Field or {(writeRequired ? "read/write" : "readable")} property '{property}' not found on object of type {targetType.FullName}.");
            }
        }
    }
}