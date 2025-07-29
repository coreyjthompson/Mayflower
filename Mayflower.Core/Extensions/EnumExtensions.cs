using System.ComponentModel;

namespace Mayflower.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
            {
                return string.Empty;
            }

            var field = type.GetField(name);
            if (field == null)
            {
                return string.Empty;
            }

            var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attr == null)
            {
                return string.Empty;
            }

            return attr.Description;
        }

        public static string ToName(this Enum value)
        {
            var type = value.GetType();
            if(type == null)
            {
                return string.Empty;
            }

            var name = Enum.GetName(type, value);
            if (name == null)
            {
                return string.Empty;
            }

            return name;
        }
    }
}