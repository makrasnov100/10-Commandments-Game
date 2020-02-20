using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public int numTimes;
    public float delayBetweenTimes;
    public float randomDelayTillStart;

    public GameObject EffectPrefab;

    public void SpawnEffect()
    {
        StartCoroutine(SpawnEfects());
    }

    IEnumerator SpawnEfects()
    {
        yield return new WaitForSeconds(Random.Range(0, randomDelayTillStart));

        for (int i = 0; i < numTimes; i++)
        {
            GameObject cur = Instantiate(EffectPrefab, transform.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(delayBetweenTimes);
        }
    }

}
