using UnityEngine;
using System.Collections.Generic;

// 오브젝트 풀: 최대 풀 크기를 넘어서면 새로 생성하지 않음
public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private readonly Queue<T> pool = new Queue<T>(); // 비활성 오브젝트 큐
    private readonly T prefab;                        // 풀링할 프리팹
    private readonly Transform parent;                // 부모 오브젝트
    private readonly int maxSize;                     // 최대 풀 크기
    private int currentCount;                         // 현재 생성된 오브젝트 수

    public ObjectPool(T prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.maxSize = initialSize;
        this.currentCount = 0;
        for (int i = 0; i < initialSize; i++)
        {
            pool.Enqueue(CreateNew());
        }
    }

    // 새 오브젝트 생성
    private T CreateNew()
    {
        var obj = GameObject.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        currentCount++;
        return obj;
    }

    // 풀에서 오브젝트 꺼내기
    public T Get()
    {
        if (pool.Count > 0)
        {
            var obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            obj.OnGetFromPool();
            return obj;
        }
        else if (currentCount < maxSize)
        {
            var obj = CreateNew();
            obj.gameObject.SetActive(true);
            obj.OnGetFromPool();
            return obj;
        }
        else
        {
            // 최대 개수 도달 시 null 반환
            Debug.LogWarning("풀 최대 개수에 도달했습니다. 추가 생성하지 않습니다.");
            return null;
        }
    }

    // 오브젝트 반환
    public void Return(T obj)
    {
        obj.OnReturnToPool();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        pool.Enqueue(obj);
    }
}
