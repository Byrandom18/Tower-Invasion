using System.Collections.Generic;
using UnityEngine;

public class DropSystem : MonoBehaviour
{
    // Список префабов, которые можно спавнить
    public List<GameObject> prefabs = new List<GameObject>();
    public List<GameObject> collectiblesPrefabs = new List<GameObject>();

    // Метод для спавна случайного префаба
    //public void SpawnRandomPrefab()
    //{
    //    if (prefabs.Count == 0)
    //    {
    //        Debug.LogWarning("Список префабов пуст!");
    //        return;
    //    }

    //    int randomIndex = Random.Range(0, prefabs.Count);
    //    Instantiate(prefabs[randomIndex], transform.position, Quaternion.identity);
    //}

    public void SpawnRandomCollectiblePrefab()
    {
        if (collectiblesPrefabs.Count == 0)
        {
            Debug.LogWarning("Список префабов пуст!");
            return;
        }
        
        int randomIndex = Random.Range(0, collectiblesPrefabs.Count);
        Instantiate(collectiblesPrefabs[randomIndex], transform.position, Quaternion.identity);
    }

    // Метод для спавна префаба по индексу
    public void SpawnPrefab()
    {
        if (prefabs.Count == 0)
        {
            Debug.LogError("Список префабов пуст!");
            return;
        }
        
        int randomIndex = Random.Range(0, collectiblesPrefabs.Count);
        Instantiate(prefabs[randomIndex], transform.position, Quaternion.identity);
    }


}
