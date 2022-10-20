using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner1 : MonoBehaviour
{
    public bool isOn = true;
    public Vector2 spawnSize;
    public GameObject spawnObject;
    public float baseInterval = 2;
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
    void SpawnObject1() {
        Instantiate(spawnObject, transform.position, Quaternion.identity);
    }

    IEnumerator SpawnCR() {
        if (!isOn || spawnObject == null) yield break;

        while (Time.time < 5) {
            yield return new WaitForSeconds(baseInterval);
            SpawnObject();
        }

        yield return new WaitForSeconds(0.2f);
        Word.type = Word.Type.Sequence;
        Word.SetDict("wordlistrick");

        while (true) {
            yield return new WaitForSeconds(baseInterval * 0.75f);
            SpawnObject();
        }
        
    }

    // Draw spawning area so it is easy to debug
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnSize);
    }
}
