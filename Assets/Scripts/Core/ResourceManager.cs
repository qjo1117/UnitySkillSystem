using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            // ObjectPoolManager�� �ִ� �༮���� Ȯ���Ѵ�.
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
            {
                name = name.Substring(index + 1);
            }
            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
            {
                return go as T;
            }
        }

        return Resources.Load<T>(path);
    }

    public GameObject NewPrefab(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null) {
            Debug.Log($"Failed to Load prefab : {path}");
            return null;
        }

        // Poolable�� ������ ������ PoolManager�� �ִ� �༮�̴�.
        if (original.GetComponent<Poolable>() != null) {
            return Managers.Pool.Pop(original, parent).gameObject;
        }

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void DelPrefab(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        // ���࿡ Ǯ���� �ʿ��� ���̶�� -> Ǯ�� �Ŵ������� �ð�����.
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null) {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

    public void Destroy(GameObject go, float time)
    {
        if (go == null)
        {
            return;
        }

        Object.Destroy(go);
    }
}
