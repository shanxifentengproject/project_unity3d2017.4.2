
using System.Collections.Generic;
using UnityEngine;

public class ScenePropMrg : Singleton<ScenePropMrg>
{

    private List<GameObject> _propObjects;

    public void Add(GameObject prop)
    {
        if (_propObjects == null)
            _propObjects = new List<GameObject>();

        _propObjects.Add(prop);
    }

    public void Remove(GameObject prop)
    {
        if (_propObjects == null)
        {
            return;
        }

        if (_propObjects.Contains(prop))
        {
            _propObjects.Remove(prop);
        }
    }

    public void RemoveAll()
    {
        if (_propObjects == null)
        {
            return;
        }

        for (int i = 0; i < _propObjects.Count; i++)
        {
            var prop = _propObjects[i];
            PropPools.Instance.DeSpawn(prop);
        }
        _propObjects.Clear();
    }



}
