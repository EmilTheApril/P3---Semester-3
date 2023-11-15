using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Vector2 spawnPoint, size;
    [SerializeField] Vector3 rotation;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlatform(prefab, spawnPoint, size, rotation);
        SpawnPlatform(prefab, -spawnPoint, size, -rotation);
    }

    public void SpawnPlatform(GameObject _prefab, Vector2 _spawnPoint, Vector2 _size, Vector3 _rotation)
    {
        GameObject _platform = Instantiate(_prefab, _spawnPoint, Quaternion.identity, this.gameObject.transform);
        _platform.transform.localScale += new Vector3(_size.x, _size.y, 0);
        _platform.transform.rotation = Quaternion.Euler(0, 0, _rotation.z);
    }

    public void ClearMap()
    {
        foreach (Transform child in this.transform)
        {
	        GameObject.Destroy(child.gameObject);
        }
    }
}