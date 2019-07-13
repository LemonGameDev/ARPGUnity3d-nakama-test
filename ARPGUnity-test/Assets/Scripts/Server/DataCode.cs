using System;

namespace Server
{
    public enum DataCode
    {
        POSITION = 1
    }

    public static class EnumExtensions
    {

        public static int ToInt(this DataCode code)
        {
            return (int) code;
        }

        public static DataCode ToDataCode(this long codeInt)
        {
            return (DataCode)Enum.ToObject(typeof(DataCode) , codeInt);
        }
    }
}