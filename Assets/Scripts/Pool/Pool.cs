using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private ObjectPool _prefab;
    
    [Space (10)]
    [SerializeField, Min(0)] private int _minCapacity;
    [SerializeField, Min(0)] private int _maxCapacity;

    [Space(10)]
    [SerializeField] private bool _autoExpand;

    private List<ObjectPool> _pool;

    private void Start()
    {
        CreatePool();
    }
    private void OnValidate()
    {
        if (_autoExpand)
        {
            _maxCapacity = int.MaxValue;
        }
    }
    private void CreatePool()
    {
        _pool = new List<ObjectPool>(_minCapacity);

        for (var i = 0; i < _minCapacity; i++)
        {
            CreateElement();
        }
    }
    private ObjectPool CreateElement(bool isActiveByDefault = false)
    {
        var createObject = Instantiate (_prefab);
        createObject.gameObject.SetActive(isActiveByDefault);

        _pool.Add(createObject);

        return createObject;
    }

    private bool TryGetElement(out ObjectPool element)
    {
        foreach (var item in _pool)
        {
            if(!item.gameObject.activeInHierarchy)
            {
                element = item;
                item.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }
    public ObjectPool GetFreeElement(Vector3 position, Quaternion rotation)
    {
        var element = GetFreeElement(position);
        element.transform.rotation = rotation;
        return element;
    }

    private ObjectPool GetFreeElement(Vector3 position)
    {
        var element = GetFreeElement();
        element.transform.position = position;
        return element;
    }

    private ObjectPool GetFreeElement()
    {
        if(TryGetElement(out var element))
        {
            return element;
        }

        if(_autoExpand)
        {
            return CreateElement(true);
        }

        if(_pool.Count < _maxCapacity)
        {
            return CreateElement(true);
        }

        throw new Exception("Pool is over!");
    }
}
