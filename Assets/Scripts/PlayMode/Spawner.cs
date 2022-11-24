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
    public float baseIntervalMultiplayer = 1f;
    public float speedFactorMultiplayer = 1.6f;

    //----------------------Functions-----------------------
    void Awake() {
        // singleton
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    void Start() {
        // StartCoroutine(SinglePlayerSpawnerCR());
        wordQueue.Clear();
    }

    public void StartSinglePlayerSpawner(int modeID) {
        switch (modeID) {
            case 0:
                StartCoroutine(SinglePlayerSpawnerCR(baseInterval * 2.3f, 1.6f));
                break;
            case 1:
                StartCoroutine(SinglePlayerSpawnerCR(baseInterval * 1.4f, 1.25f));
                break;
            case 2:
                StartCoroutine(SinglePlayerSpawnerCR(baseInterval * 0.9f, 1));
                break;
            case 3:
                StartCoroutine(NetcentricSpawnerCR(baseInterval * 1.5f, 0.5f));
                break;
        }
    }

    // 
    public void StartMultiPlayerSpawner() {
        StartCoroutine(MultiPlayerSpawnerCR());
    }

    IEnumerator SinglePlayerSpawnerCR(float initTimer, float speedFactor) {
        float timer = initTimer;
        while (isOn && spawnObject != null) {
            if (!GameManager.isPaused) {
                yield return 0;
                timer -= Time.deltaTime;
                if (timer < 0) {
                    SpawnRandomObject(speedFactor);
                    timer = initTimer;
                }
            } else {
                yield return 0;
            }
        }
        
    }

    IEnumerator MultiPlayerSpawnerCR() {
        float timer = baseIntervalMultiplayer;
        while (isOn && spawnObject != null) {
            if (!GameManager.isPaused) {
                yield return 0;
                timer -= Time.deltaTime;
                if (timer < 0) {
                    SpawnObjectMultiplayer();
                    timer = baseIntervalMultiplayer;
                }
            } else {
                yield return 0;
            }
        }
    }

    IEnumerator NetcentricSpawnerCR(float initTimer, float speedFactor) {
        float timer = initTimer;
        while (isOn && spawnObject != null) {
            if (!GameManager.isPaused) {
                yield return 0;
                timer -= Time.deltaTime;
                if (timer < 0) {
                    SpawnObjectNetcentric(speedFactor);
                    timer = initTimer;
                }
            } else {
                yield return 0;
            }
        }
    }

    // spawn word with random text
    void SpawnRandomObject(float speedFactor) {
        // calculate spawn size
        float spawnSizeX = spawnStart.position.x - spawnEnd.position.x;

        // randomize spawn position
        Vector2 pos = new Vector2();
        pos.x = transform.position.x - spawnSizeX / 2;
        pos.y = spawnStart.position.y;
        pos.x += Random.value * spawnSizeX;

        // instantiate new obj
        GameObject i = Instantiate(spawnObject, new Vector3 (pos.x, pos.y, transform.position.z), Quaternion.identity);

        // set text to random text in wordlist
        Word w = i.GetComponent<Word>();
        w.SetText(WordList.GetRandomWord("wordlist10000"));
        w.velocityY *= speedFactor;
    }

    // spawn multiplayer
    void SpawnObjectMultiplayer() {
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
            GameObject i = Instantiate(spawnObject, new Vector3 (pos.x, pos.y, transform.position.z), Quaternion.identity);         

            // set object to have the top value from the queue
            Word w = i.GetComponent<Word>();
            w.SetText(wordQueue.Dequeue());
            w.velocityY *= speedFactorMultiplayer;
        }
    }

    // spawn net centric
    void SpawnObjectNetcentric(float speedFactor) {
        // calculate spawn size
        float spawnSizeX = spawnStart.position.x - spawnEnd.position.x;

        // randomize spawn position
        Vector2 pos = new Vector2();
        pos.x = transform.position.x - spawnSizeX / 2;
        pos.y = spawnStart.position.y;
        pos.x += Random.value * spawnSizeX;

        // instantiate new obj
        GameObject i = Instantiate(spawnObject, new Vector3 (pos.x, pos.y, transform.position.z), Quaternion.identity);

        // set text to random text in wordlist
        Word w = i.GetComponent<Word>();
        w.SetText(GetRandomNetcentric());
        w.velocityY *= speedFactor;
    }

    (string shown, string real) GetRandomNetcentric() {
        int index = Random.Range(0, netcentricList.Length);
        return netcentricList[index];
    }

    (string shown, string real)[] netcentricList = {
        ("ISP","internet service provider"),
        ("IXP","internet exchange point"),
        ("RFC","request for comments"),
        ("HTTP","hypertext transfer protocol"),
        ("HTTPS","hypertext transfer protocol secure"),
        ("SSH","secure shell"),
        ("TCP","transmission control protocol"),
        ("UDP","user datagram protocol"),
        ("IP","internet protocol"),
        ("PPP","peer to peer protocol"),
        ("SMTP","single mail transfer protocol"),
        ("POP","post office protocol"),
        ("DNS","domain name system"),
        ("DB","database"),
        ("TLD","top level domain"),
        ("CDN","content delivery network"),
        ("CBR","constant bit rate"),
        ("VBR"," variable bit rate"),
        ("DASH","dynamic adaptive streaming over http"),
        ("OTT","over the top"),
        ("RTT","round trip time"),
        ("URL","uniform resource locator"),
        ("LAN","local area network"),
        ("VLAN","virtual local area network"),
        ("WAN","wide area network"),
        ("MUX","multiplexing"),
        ("DEMUX","demultiplexing"),
        ("RDT","reliable data transfer"),
        ("FSM","finite state machine"),
        ("ACK","acknowledgement"),
        ("NAK","negative acknowledgement"),
        ("ARQ","automatic repeat query"),
        ("GBN","go back n"),
        ("AIMD","additive increase multiplicative decrease"),
        ("CWND","congestion window"),
        ("HOL","head of line"),
        ("TTL","time to live"),
        ("QUIC","quick udp internet connections"),
        ("TLS","transport layer security"),
        ("ATM","asynchronous transfer mode"),
        ("FCFS","first come first served"),
        ("FIFO","first in first out"),
        ("CIDR","classless interdomain routing"),
        ("DHCP","dynamic host configuration protocol"),
        ("NAT","network address translation"),
        ("SDN","software defined network"),
        ("ODL","open daylight"),
        ("AS","autonomous system"),
        ("API","application programming interface"),
        ("OSPF","open shortest path first"),
        ("BGP","border gateway protocol"),
        ("eBGP","external border gateway protocol"),
        ("iBGP","internal border gateway protocol"),
        ("BF","bellman-ford"),
        ("ICMP","internet control message protocol"),
        ("CLI","command line interface"),
        ("SNMP","simple network management protocol"),
        ("ARP","address resolution protocol"),
        ("NIC","network interface card"),
        ("MAC","media access control")
    };
}
