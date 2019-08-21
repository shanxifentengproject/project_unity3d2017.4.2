﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPools : Singleton<BulletPools>
{

    [SerializeField] private List<GameObject> _bulletList;
    [SerializeField] private int _originCount;


    private Dictionary<string, List<GameObject>> _cache;
    private Dictionary<string, GameObject> _originCache;
    private void Start()
    {
        _cache = new Dictionary<string, List<GameObject>>();
        _originCache = new Dictionary<string, GameObject>();
        for (int i = 0; i < _bulletList.Count; i++)
        {
            var item = _bulletList[i];
            _originCache.Add(item.name, item);
        }
        StartCoroutine(InitiPools());
    }


    private IEnumerator InitiPools()
    {
        int index = 0;
        while (true)
        {
            if (index >= _bulletList.Count)
                yield break;
            for (int i = 0; i < _originCount; i++)
            {
                var obj = Instantiate(_bulletList[index]);
                obj.transform.parent = transform;
                obj.SetActive(false);
                obj.name = _bulletList[index].name;
                if (_cache.ContainsKey(obj.name))
                {
                    _cache[obj.name].Add(obj);
                }
                else
                {
                    List<GameObject> list = new List<GameObject>();
                    list.Add(obj);
                    _cache.Add(obj.name, list);
                }
            }
            index++;
            yield return null;
        }
    }


    public GameObject Spawn(string str)
    {
        if (_cache.ContainsKey(str))
        {
            var list = _cache[str];
            if (list.Count == 0)
            {
                if (_originCache.ContainsKey(str))
                {
                    var obj1 = Instantiate(_originCache[str]);
                    obj1.name = str;
                    obj1.transform.parent = null;
                    return obj1;
                }
                return null;
            }
            var obj = list[0];
            obj.SetActive(true);
            obj.transform.parent = null;
            list.RemoveAt(0);
            return obj;
        }
        Debug.LogError("attempted to spawn a GameObject from recycle bin (" + str +
                              ") but there is no recycle bin setup for it");
        return null;
    }


    public void DeSpawn(GameObject obj)
    {
        string objName = obj.name;
        if (!_cache.ContainsKey(objName))
        {
            Destroy(obj);
        }
        else
        {
            _cache[objName].Add(obj);
            obj.transform.parent = transform;
            obj.SetActive(false);
        }
    }


}