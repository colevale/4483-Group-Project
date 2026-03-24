using System.Collections.Generic;
using UnityEngine;

public class ColliderList : MonoBehaviour
{
    private List<GameObject> objs;

    public void Start()
    {
        objs = new List<GameObject>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        objs.Add(other.gameObject);
    }

    protected void OnTriggerExit(Collider other)
    {
        objs.Remove(other.gameObject);
    }
    public bool isEmpty()
    {
        return objs.Count == 0;
    }
}
