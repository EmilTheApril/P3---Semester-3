using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject error, ground, spike;
    [SerializeField] Vector2 spawnPoint1, spawnPoint2, spawnPoint3, size1, size2, size3;
    [SerializeField] Vector3 rotation1, rotation2, rotation3;
    [SerializeField] MapData mapData;

    void Start()
    {
        SpawnPlatform("Ground", spawnPoint2, size2, rotation2);
        SpawnPlatform("Ground", spawnPoint3, size3, rotation3);
        ReadJson();
        Debug.Log(mapData.name + mapData.pos + mapData.scale + mapData.rotation);
    }

    public void ReadJson()
    {
        string filepath = "C:/Users/Mu898/Documents/GitHub/P3-Semester-3/Assets/data.json";
        string mapdata = System.IO.File.ReadAllText(filepath);
        Debug.Log(mapdata);

        // Parse the JSON data into the mapData object
        mapData = JsonUtility.FromJson<MapData>(mapdata);

        // Check if mapData is not null before accessing its properties
        if (mapData != null)
        {
            Debug.Log(mapData.name + new Vector2(mapData.pos[0], mapData.pos[1]) + new Vector2(mapData.scale[0], mapData.scale[1]) + new Vector3(0, 0, mapData.rotation[2]));
            SpawnPlatform(mapData.name, new Vector2(mapData.pos[0], mapData.pos[1]), new Vector2(mapData.scale[0], mapData.scale[1]), new Vector3(0, 0, mapData.rotation[2]));
        }
        else
        {
            Debug.LogError("mapData is null!");
        }
    }

    public void SpawnPlatform(string _object, Vector2 _spawnPoint, Vector2 _size, Vector3 _rotation)
    {
        GameObject _prefab = FindPrefab(_object);
        GameObject _platform = Instantiate(_prefab, _spawnPoint, Quaternion.identity, this.gameObject.transform);
        _platform.transform.localScale += new Vector3(_size.x, _size.y, 0);
        _platform.transform.rotation = Quaternion.Euler(0, 0, _rotation.z);
    }

    public GameObject FindPrefab(string _name)
    {
        switch(_name) 
        {
            case "Ground":
                return ground;
            case "Spike":
                return spike;
            default:
                Debug.Log("Object not found");
                return error;
        }
    }

    public void ClearMap()
    {
        foreach (Transform child in this.transform)
        {
	        GameObject.Destroy(child.gameObject);
        }
    }
}