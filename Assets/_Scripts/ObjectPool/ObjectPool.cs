using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private int poolSize = 5;

    Dictionary<GameObject, Queue<GameObject>> poolDict = new();

    [Header("To Initialize")]
    [SerializeField] GameObject weaponPickup;
    [SerializeField] GameObject ammoPickup;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InitializeNewPool(weaponPickup);
        //InitializeNewPool(ammoPickup);
    }

    private void InitializeNewPool(GameObject prefab)
    {
        poolDict[prefab] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }
    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObj = Instantiate(prefab, transform);
        PooledObject pooled = newObj.GetComponent<PooledObject>();

        if (pooled == null)
            newObj.AddComponent<PooledObject>().originalPrefab = prefab;
        else
            pooled.originalPrefab = prefab;

        newObj.SetActive(false);

        poolDict[prefab].Enqueue(newObj);
    }


    public GameObject GetObject(GameObject prefab)
    {
        if (!poolDict.ContainsKey(prefab))
            InitializeNewPool(prefab);

        if (poolDict[prefab].Count == 0)
            CreateNewObject(prefab);

        GameObject objectToGet = poolDict[prefab].Dequeue();

        objectToGet.SetActive(true);
        objectToGet.transform.parent = null;

        return objectToGet;
    }

    #region Return To Pool

    public void ReturnToPool(GameObject objectToReturn)
    {
        PooledObject pooledObj = objectToReturn.GetComponent<PooledObject>();

        if (pooledObj == null)
        {
            Debug.LogWarning($"{objectToReturn.name} không có PooledObject component!");
            Destroy(objectToReturn); // Hủy object không thuộc pool
            return;
        }

        GameObject originalPrefab = pooledObj.originalPrefab;

        if (originalPrefab == null)
        {
            Debug.LogWarning($"{objectToReturn.name} có PooledObject nhưng originalPrefab = null!");
            Destroy(objectToReturn);
            return;
        }

        // ✅ Tự động tạo pool mới nếu chưa tồn tại
        if (!poolDict.ContainsKey(originalPrefab))
        {
            Debug.Log($"Tạo pool mới cho: {originalPrefab.name}");
            poolDict[originalPrefab] = new Queue<GameObject>();
        }

        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = transform;

        poolDict[originalPrefab].Enqueue(objectToReturn);
    }



    public void DelayReturnToPool(GameObject objectToReturn, float delay = .001f)
    {
        StartCoroutine(DelayReturn(delay, objectToReturn));
    }

    private IEnumerator DelayReturn(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);

        ReturnToPool(objectToReturn);
    }

    #endregion
}
