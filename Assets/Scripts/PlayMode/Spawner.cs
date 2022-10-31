using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // singleton
    public static Spawner instance;

    // wordlist
    public static Queue<string> wordQueue = new Queue<string>();

    // variables
    public bool isOn = true;
    public Vector2 spawnSize;
    public GameObject spawnObject;
    public float baseInterval = 1;

    //----------------------Functions-----------------------
    void Awake() {
        // singleton
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    public void StartSinglePlayerSpawnerCR() {
        StartCoroutine(SinglePlayerSpawnerCR());
    }

    // 
    public void StartMultiPlayerSpawnerCR() {
        StartCoroutine(MultiPlayerSpawnerCR());
    }

    IEnumerator SinglePlayerSpawnerCR() {
        while (isOn && spawnObject != null) {
            yield return new WaitForSeconds(baseInterval);
            SpawnRandomObject();
        }
        
    }

    IEnumerator MultiPlayerSpawnerCR() {
        while (isOn && spawnObject != null) {
            yield return new WaitForSeconds(baseInterval);
            SpawnRandomObject("e");
        }
        
    }

    // spawn word with random text
    void SpawnRandomObject() {
        // randomize spawn position
        Vector2 pos = new Vector2();
        pos.x = transform.position.x - spawnSize.x / 2;
        pos.y = transform.position.y - spawnSize.y / 2;
        pos.x += Random.value * spawnSize.x;
        pos.y += Random.value * spawnSize.y;

        // instantiate new obj
        GameObject i = Instantiate(spawnObject, pos, Quaternion.identity);

        // TODO: check if offline or online
        // set text to random text in wordlist
        i.GetComponent<Word>().SetText(WordList.GetRandomWord("wordlist10000"));
    }

    // spawn word from the list
    void SpawnRandomObject(string e) {
        // randomize spawn position
        Vector2 pos = new Vector2();
        pos.x = transform.position.x - spawnSize.x / 2;
        pos.y = transform.position.y - spawnSize.y / 2;
        pos.x += Random.value * spawnSize.x;
        pos.y += Random.value * spawnSize.y;

        // check if the queue is not empty
        if(wordQueue.Count != 0) {
            // instantiate new obj
            GameObject i = Instantiate(spawnObject, pos, Quaternion.identity);         

            // set object to have the top value from the queue
            i.GetComponent<Word>().SetText(wordQueue.Dequeue());
        }
 
    }

    // Draw spawning area so it is easy to debug
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnSize);
    }
}
