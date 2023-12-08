using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonConverter : MonoBehaviour
{
    List<JSONObject> jsonlist = new List<JSONObject>();

    public void JsonReader()
    {
        JSONObject json = new JSONObject();
        jsonlist.Add(json);
    }
    
}
[System.Serializable]
public class JSONObject
{
    public string name;
    public Vector2 pos;
    public Vector2 scale;
    public Vector3 rotation;

    public static JSONObject CreateFromJSON(string jsonInput)
    {
        return JsonUtility.FromJson<JSONObject>(jsonInput);
    }
}