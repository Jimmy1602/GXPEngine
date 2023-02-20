using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace XmlReader
{
    public class xmlReader
    {
        static XmlSerializer serial = new XmlSerializer(typeof(CharacterSheet));

        public static CharacterSheet ReadCharacterMap(string filename)
        {
            TextReader reader = new StreamReader(filename);
            CharacterSheet myMap = serial.Deserialize(reader) as CharacterSheet;
            reader.Close();

            return myMap;
        }

        //for boomerang
        //speed="20"
		//backSpeed="40">

        public static AttackSheet ReadAttackMap(string filename)
        {
            TextReader reader = new StreamReader(filename);
            AttackSheet myMap = serial.Deserialize(reader) as AttackSheet;
            reader.Close();

            return myMap;
        }

        public static void WriteMap(string filename, CharacterSheet map)
        {
            TextWriter writer = new StreamWriter(filename);
            serial.Serialize(writer, map);
            writer.Close();
        }
    }

    [XmlRootAttribute("characterList")]
    public class CharacterSheet : PropertyContainer
    {
        [XmlElement("character")]
        public CharacterProperties[] characters;

        
    }

    [XmlRootAttribute("character")]
    public class CharacterProperties : PropertyContainer
    {
        [XmlAttribute("name")]
        public string name;
        
        [XmlAttribute("maxMoveSpeed")]
        public int maxMoveSpeed;
        [XmlAttribute("moveSpeedUp")]
        public float moveSpeedUp;
        [XmlAttribute("moveSlowDown")]
        public float moveSlowDown;
        [XmlAttribute("groundSlowDown")]
        public float groundSlowDown;

        [XmlAttribute("dashCheckTime")]
        public int dashCheckTime;
        [XmlAttribute("dashTime")]
        public int dashTime;
        [XmlAttribute("dashSpeed")]
        public int dashSpeed;

        [XmlAttribute("jumpHeight")]
        public float jumpHeight;
        [XmlAttribute("jumpHoldTime")]
        public int jumpHoldTime;
        [XmlAttribute("jumpHoldHeight")]
        public float jumpHoldHeight;
        [XmlAttribute("jumpBuffer")]
        public int jumpBuffer;

        [XmlAttribute("maxGravity")]
        public int maxGravity;
        [XmlAttribute("gravity")]
        public float gravity;

        [XmlAttribute("idleStartFrame")]
        public int idleStartFrame;
        [XmlAttribute("idleFrames")]
        public int idleFrames;
        [XmlAttribute("idleFrameDelay")]
        public int idleFrameDelay;

        [XmlAttribute("jumpFrame")]
        public int jumpFrame;

        [XmlAttribute("runStartFrame")]
        public int runStartFrame;
        [XmlAttribute("runFrames")]
        public int runFrames;
        [XmlAttribute("runFramesDelay")]
        public int runFramesDelay;

        [XmlAttribute("attackStartFrame")]
        public int attackStartFrame;
        [XmlAttribute("attackFrames")]
        public int attackFrames;
        [XmlAttribute("attackFramesDelay")]
        public int attackFramesDelay;

        [XmlAttribute("specialStartFrame")]
        public int specialStartFrame;
        [XmlAttribute("specialFrames")]
        public int specialFrames;
        [XmlAttribute("specialFramesDelay")]
        public int specialFramesDelay;

        [XmlAttribute("deadFrame")]
        public int deadFrame;

    }

    [XmlRootAttribute("attackList")]
    public class AttackSheet : PropertyContainer
    {
        [XmlElement("attack")]
        public AttackProperties[] attacks;
    }

    [XmlRootAttribute("attack")]
    public class AttackProperties : PropertyContainer
    {
        [XmlAttribute("name")]
        public string name;

        [XmlAttribute("windupTime")]
        public int windupTime;

        [XmlAttribute("damage")]
        public float damage;

        [XmlAttribute("time")]
        public int time;

        [XmlAttribute("xKnockback")]
        public float xKnockback;
        [XmlAttribute("yKnockback")]
        public float yKnockback;

        [XmlAttribute("iMillis")]
        public int iMillis;
    }

    public class PropertyContainer
    {
        [XmlElement("properties")]
        public PropertyList propertyList;

        /// <summary>
        /// Returns true if this object has a property with name [key] of type [type].
        /// As [type], you can pass in "int", "float", "bool", "string" and "color".
        /// </summary>
        public bool HasProperty(string key, string type)
        {
            if (propertyList == null)
                return false;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key && p.Type == type)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the value of this object's string property with name [key], if it has such a property.
        /// Otherwise, it returns the default value that you can pass as second parameter.
        /// </summary>
        public string GetStringProperty(string key, string defaultValue = "")
        {
            if (propertyList == null)
                return defaultValue;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key)
                    return p.Value;
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the value of this object's float property with name [key], if it has such a property.
        /// Otherwise, it returns the default value that you can pass as second parameter.
        /// </summary>
        public float GetFloatProperty(string key, float defaultValue = 1)
        {
            if (propertyList == null)
                return defaultValue;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key && p.Type == "float")
                    return float.Parse(p.Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the value of this object's int property with name [key], if it has such a property.
        /// Otherwise, it returns the default value that you can pass as second parameter.
        /// </summary>
        public int GetIntProperty(string key, int defaultValue = 1)
        {
            if (propertyList == null)
                return defaultValue;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key && p.Type == "int")
                    return int.Parse(p.Value);
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the value of this object's bool property with name [key], if it has such a property.
        /// Otherwise, it returns the default value that you can pass as second parameter.
        /// </summary>
        public bool GetBoolProperty(string key, bool defaultValue = false)
        {
            if (propertyList == null)
                return defaultValue;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key && p.Type == "bool")
                    return bool.Parse(p.Value);
            }
            return defaultValue;
        }

        /*
        /// <summary>
        /// Returns the value of this object's color property with name [key], if it has such a property.
        /// Otherwise, it returns the default value that you can pass as second parameter.
        /// The returned color can be set directly as color value of a GXPEngine Sprite.
        /// </summary>
        public uint GetColorProperty(string key, uint defaultvalue = 0xffffffff)
        {
            if (propertyList == null)
                return defaultvalue;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key && p.Type == "color")
                {
                    return TiledUtils.GetColor(p.Value);
                }
            }
            return defaultvalue;
        }
        */
    }

    [XmlRootAttribute("properties")]
    public class PropertyList
    {
        [XmlElement("property")]
        public Property[] properties;

        override public string ToString()
        {
            string output = "";
            foreach (Property p in properties)
                output += p.ToString();
            return output;
        }
    }

    [XmlRootAttribute("property")]
    public class Property
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("type")]
        public string Type = "string";
        [XmlAttribute("value")]
        public string Value;

        override public string ToString()
        {
            return "Property: Name: " + Name + " Type: " + Type + " Value: " + Value + "\n";
        }
    }
}
