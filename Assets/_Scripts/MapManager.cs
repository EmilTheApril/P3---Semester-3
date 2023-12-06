using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject error, ground, spike;
    [SerializeField] Vector2 spawnPoint1, spawnPoint2, spawnPoint3, size1, size2, size3;
    [SerializeField] Vector3 rotation1, rotation2, rotation3;

    void Start()
    {
        //SpawnPlatform("Ground", spawnPoint2, size2, rotation2);
        //SpawnPlatform("Ground", spawnPoint3, size3, rotation3);
        ReadJson();
        //Debug.Log(mapData.name + mapData.pos + mapData.scale + mapData.rotation);
    }

    public void ReadJson()
    {
        string filepath = "C:/Users/Mu898/Documents/GitHub/P3-Semester-3/Assets/data.json";
        try
        {
            string mapdata = System.IO.File.ReadAllText(filepath);
            Debug.Log(mapdata);

            // Parse the JSON data into the mapData object
            mapdata = JsonHelper.fixJson(mapdata);
            Debug.Log(mapdata);
            MapData[] mapData = JsonHelper.FromJson<MapData>(mapdata);
            Debug.Log(mapData.Length);
            Debug.Log(mapData[0].pos[0]);

            // Check if mapData is not null before accessing its properties
            foreach(MapData _mapData in mapData)
            {
                if (_mapData != null)
                {
                    SpawnPlatform(_mapData.name, new Vector2(_mapData.pos[0], _mapData.pos[1]), new Vector2(_mapData.scale[0], _mapData.scale[1]), new Vector3(0, 0, _mapData.rotation[2]));
                }
                else
                {
                    Debug.LogError("mapData is null!");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading JSON file: {e.Message}");
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