using System;
using System.Collections.Generic;
using System.Text;

namespace TrainTime.Models
{
    public class StringValueAttribute:Attribute
    {
        public string Svalue { get; protected set; }

        public StringValueAttribute(string value)
        {
            this.Svalue = value;
        }
    }
    public static class CommonAttribute
    {

        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            System.Reflection.FieldInfo fieldInfo = type.GetField(value.ToString());

            //範囲外の値チェック
            if (fieldInfo == null) return null;

            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].Svalue : null;

        }
    }
}
