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
    public RectTransform spawnStart;
    public RectTransform spawnEnd;
    public GameObject spawnObject;
    public float baseInterval = 1;

    //----------------------Functions-----------------------
    void Awake() {
        // singleton
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    void Start() {
        StartCoroutine(SinglePlayerSpawnerCR());
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
        // calculate spawn size
        float spawnSizeX = spawnStart.position.x - spawnEnd.position.x;

        // randomize spawn position
        Vector2 pos = new Vector2();
        pos.x = transform.position.x - spawnSizeX / 2;
        pos.y = spawnStart.position.y;
        pos.x += Random.value * spawnSizeX;

        // instantiate new obj
        GameObject i = Instantiate(spawnObject, pos, Quaternion.identity);

        // TODO: check if offline or online
        // set text to random text in wordlist
        i.GetComponent<Word>().SetText(WordList.GetRandomWord("wordlist10000"));
    }

    // spawn word from the list
    void SpawnRandomObject(string e) {
        // calculate spawn size
        float spawnSizeX = spawnStart.position.x - spawnEnd.position.x;

        // randomize spawn position
        Vector2 pos = new Vector2();
        pos.x = transform.position.x - spawnSizeX / 2;
        pos.y = spawnStart.position.y;
        pos.x += Random.value * spawnSizeX;

        // check if the queue is not empty
        if(wordQueue.Count != 0) {
            // instantiate new obj
            GameObject i = Instantiate(spawnObject, pos, Quaternion.identity);         

            // set object to have the top value from the queue
            i.GetComponent<Word>().SetText(wordQueue.Dequeue());
        }
 
    }
}
