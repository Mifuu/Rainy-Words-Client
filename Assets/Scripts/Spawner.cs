using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool isOn = true;
    public Vector2 spawnSize;
    public GameObject spawnObject;
    public float baseInterval = 1;
    public AnimationCurve intervalFactorCurve;

    //----------------------Functions-----------------------
    void Start() {
        StartCoroutine(SpawnCR());
    }

    void SpawnObject() {
        Vector2 pos = new Vector2();
        pos.x = transform.position.x - spawnSize.x / 2;
        pos.y = transform.position.y - spawnSize.y / 2;
        pos.x += Random.value * spawnSize.x;
        pos.y += Random.value * spawnSize.y;

        Instantiate(spawnObject, pos, Quaternion.identity);
    }

    IEnumerator SpawnCR() {
        while (isOn && spawnObject != null) {
            yield return new WaitForSeconds(baseInterval);
            SpawnObject();
        }
        
    }

    // Draw spawning area so it is easy to debug
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnSize);
    }
}
