using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject ground;
    [SerializeField] Vector2 spawnPoint1, spawnPoint2, spawnPoint3, size1, size2, size3;
    [SerializeField] Vector3 rotation1, rotation2, rotation3;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlatform(ground, spawnPoint1, size1, rotation1);
        SpawnPlatform(ground, spawnPoint2, size2, rotation2);
        SpawnPlatform(ground, spawnPoint3, size3, rotation3);
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