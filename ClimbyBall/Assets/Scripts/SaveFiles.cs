using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft;
using System.Xml.Serialization;
using System.Xml;

public static class SaveFiles
{
    /// <summary>
    /// Store data as string in PlayerPref
    /// </summary>
    /// <param name="key">You will use this key to store your data with</param>
    /// <param name="data">The data which you want to store it</param>
    public static void SetStringPlayerPref(string key, string data)
    {
        PlayerPrefs.SetString(key, data);
    }
    /// <summary>
    /// Store data as float in PlayerPref
    /// </summary>
    /// <param name="key">You will use this key to store your data with</param>
    /// <param name="data">The data which you want to store it</param>
    public static void SetFloatPlayerPref(string key, float data)
    {
        PlayerPrefs.SetFloat(key, data);
    }
    /// <summary>
    /// Get back the string data which you stored it in PlayerPref
    /// </summary>
    /// <param name="key">The key of this data whihc you saved it with before</param>
    /// <returns></returns>
    public static string GetStringPlayerPref(string key)
    {
        return PlayerPrefs.GetString(key);
    }
    /// <summary>
    /// Get back the float data which you stored it in PlayerPref
    /// </summary>
    /// <param name="key">The key of this data whihc you saved it with before</param>
    /// <returns></returns>
    public static float GetFloatPlayerPref(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        else
        {
            return -1;
        }
    }

    public static void SaveFileAtDataPath(object _object, string file_name, string file_type)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Create(Path.Combine(Application.persistentDataPath, file_name + "." + file_type));
        formatter.Serialize(stream, _object);
        stream.Close();
    }

    public static string ConvertObjectToJson(object _object)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(_object);
    }

    public static T ConvertJsonToObject<T>(string json)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    }

    public static void SaveObjectAsJSONAtPersDataPath(object _object, string file_name)
    {

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_object);
        if (!file_name.EndsWith(".json"))
            file_name += ".json";
        File.WriteAllText(Path.Combine(Application.persistentDataPath, file_name), json);
    }
    public static void SaveObjectAsNewtonsoftJSON_AtPersDataPath(object _object, string file_name)
    {

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_object);

        if (!file_name.EndsWith(".json"))
            file_name += ".json";
        File.WriteAllText(Path.Combine(Application.persistentDataPath, file_name), json);
    }

    /*public static void SaveObjectAsXMLAtPersDataPath(object _object, string file_name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(ToXML(_object));
        string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc.DocumentElement);
        using (FileStream file = File.Create(Path.Combine(Application.persistentDataPath, file_name + ".xml")))
        {
            formatter.Serialize(file, json);

        }

    }*/

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">Path of the file</param>
    /// <param name="type">Saved type of the file (With the dot .)</param>
    /// <param name="delete"></param>
    /// <returns></returns>
    public static bool FileExists(string path, string type, bool delete = false)
    {
        bool exists = File.Exists(path + type);
        if (delete && exists)
        {
            File.Delete(path + type);
        }
        return exists;
    }

    public static bool JsonFileExists(string path, bool delete = false)
    {
        bool exists = File.Exists(path + ".json");
        if (delete && exists)
        {
            File.Delete(path + ".json");
        }
        return exists;
    }
    /// <summary>
    /// Checks if a file with the provided extension (type) exists at the persistant data path for the current application
    /// </summary>
    /// <param name="name">Name of the file</param>
    /// <param name="type">Saved type of the file (With the dot .)</param>
    /// <param name="delete">Delete the file if it exists?s</param>
    /// <returns></returns>
    public static bool FileExistsAtPersPath(string name, string type, bool delete = false)
    {
        name = Path.Combine(Application.persistentDataPath, name + type);
        bool exists = File.Exists(name);
        if (delete && exists)
        {
            File.Delete(name);
        }
        return exists;
    }

    /// <summary>
    /// Checks if a json file exists at the persistant data path for the current application
    /// </summary>
    /// <param name="name">Name of the file</param>
    /// <param name="type">Saved type of the file (With the dot .)</param>
    /// <param name="delete">Delete the file if it exists?s</param>
    /// <returns></returns>
    public static bool JsonFileExistsAtPersPath(string name, bool delete = false)
    {
        name = Path.Combine(Application.persistentDataPath, name + ".json");
        bool exists = File.Exists(name);
        if (delete && exists)
        {
            File.Delete(name);
        }
        return exists;
    }

    public static string ToXML(object _object)
    {
        using (StringWriter sw = new StringWriter())
        {
            //use _object.GetType() instead of typeof(T).
            //At first glance this seems safe to use T as the type, b
            //ut if the "_object" object has been cast to a parent type (ChildClass cast to BaseClass) 
            //it will throw an error
            try
            {
                XmlSerializer xml = new XmlSerializer(_object.GetType());
                xml.Serialize(sw, _object);
                return sw.ToString();
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }
        }
    }

    public static T loadFromXml<T>(string _xml)
    {
        using (StringReader r = new System.IO.StringReader(_xml))
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            return (T)xml.Deserialize(r);
        }


    }

    public static object loadFileFrom(string path)
    {
        if (File.Exists(path))
        {

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Open(path, FileMode.Open))
            {
                return bf.Deserialize(file);
            }
        }
        return null;
    }

    public static string LoadJSONTextFrom(string path)
    {
        if (!path.EndsWith(".json"))
            path = path + ".json";
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }
        return string.Empty;
    }

    public static string LoadJSONTextFromPersPath(string name)
    {

        if (!name.EndsWith(".json"))
            name = Path.Combine(Application.persistentDataPath, name + ".json");
        else
            name = Path.Combine(Application.persistentDataPath, name);

        if (File.Exists(name))
        {
            return File.ReadAllText(name);
        }
        return string.Empty;
    }

    public static bool LoadJSONTextFrom(string path, out string json)
    {
        if (!path.EndsWith(".json"))
            path = path + ".json";
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            return true;
        }
        json = string.Empty;
        return false;
    }

    public static T LoadXMLTextToObjectFrom<T>(string path)
    {
        if (!path.EndsWith(".xml"))
            path = path + ".xml";
        if (File.Exists(path))
        {
            using (StreamReader r = new StreamReader(path))
            {
                return loadFromXml<T>(r.ReadToEnd());
            }

        }
        throw new FileNotFoundException("File at: " + path + " Not Found");
    }

    public static T LoadObjectFromJSONFile<T>(string name)
    {
        string json = LoadJSONTextFrom(Path.Combine(Application.persistentDataPath, name));
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        //return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    }

    /*public static T LoadObjectFromJSONFile<T>(string name)
    {
        string json = LoadJSONTextFrom(Path.Combine(Application.persistentDataPath, name));
        return (T)JsonUtility.FromJson(json, typeof(T));
    }*/
}
