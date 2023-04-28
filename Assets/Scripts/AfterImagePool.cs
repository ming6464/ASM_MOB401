using System;
using System.Collections.Generic;
using UnityEngine;

public class AfterImagePool : Singleton<AfterImagePool>
{
    [SerializeField] private GameObject _afterImg;
    
    
    private Queue<GameObject> listImg = new Queue<GameObject>();

    public override void Awake()
    {
        DontLoad(true);
        BuffPool();
    }
    
    private void BuffPool()
    {
        for (int i = 0; i < 7; i++)
        {
            var newAfterImg = Instantiate(_afterImg);
            newAfterImg.transform.SetParent(transform);
            AddToPool(newAfterImg);
        }
    }

    public void AddToPool(GameObject newAfterImg)
    {
        newAfterImg.SetActive(false);
        listImg.Enqueue(newAfterImg);
    }

    public GameObject GetFromPool()
    {
        if(listImg.Count == 0) BuffPool();
        var getAfter = listImg.Dequeue();
        getAfter.SetActive(true);
        return getAfter;
    }
}