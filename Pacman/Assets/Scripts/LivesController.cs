using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesController : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;

    public void SetLivesAmount(int amount)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < amount; i++)
        {
            var el = Instantiate(heartPrefab);
            el.transform.parent = gameObject.transform;
        }
    }
}
