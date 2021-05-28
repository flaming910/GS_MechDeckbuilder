using System;
using MDB.Cards;
using MDB.Characters;

namespace MDB.Utils
{
    public static class ConversionUtils
    {
        public static BodyPartType ConvertTargetToBodyPart(TargetType type)
        {
            if (type.ToString().Contains("Enemy"))
            {
                return (BodyPartType)Enum.Parse(typeof(BodyPartType), type.ToString().Substring(5));
            }
            else
            {
                return (BodyPartType)Enum.Parse(typeof(BodyPartType), type.ToString().Substring(6));
            }

        }
    }


}


