using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NullOrNotEmptyListAttribute : ValidationAttribute
    {
        private const string defaultError = "'{0}' must have at least one element.";

        public Type Type { get; private set; }

        public NullOrNotEmptyListAttribute(Type type) : base(defaultError)
        {
            Type = type;
        }

        public override bool IsValid(object value)
        {
            object defaultValue = Type.IsValueType ? Activator.CreateInstance(Type) : null;

            if (value == null)
            {
                return true;
            }

            if (value is IEnumerable list)
            {
                foreach (object item in list)
                {
                    if (item != defaultValue)
                    {
                        return true;
                    }
                }
            }
            else if (value is string text)
            {
                if (text != defaultValue as string)
                {
                    return true;
                }
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }
    }
}
