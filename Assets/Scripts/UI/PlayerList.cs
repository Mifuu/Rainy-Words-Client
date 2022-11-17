using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    // singleton
    static PlayerList instance;

    static List<PlayerListItem> list = new List<PlayerListItem> ();
    
    [Header("PlayerListItem prefab")]
    public GameObject playerListItem;       // player list item prefab
    [Header("Content list")]
    public RectTransform content;               // list content transform
    public Vector2 originOffset = new Vector2();
    public Vector2 eachOffset = new Vector2();
    
    // TEST---------------------------------------------------------------------------------------------
    List<(int id, string name)> testList = new List<(int id, string name)> ();
    void Update() {
        if (ConnectionUIManager.instance == null) return;
        if (ConnectionUIManager.instance.isOn && Input.GetKeyDown(KeyCode.P)) {
            testList.Add((testList.Count, "test" + testList.Count));
            UpdateAllPlayerListItem(testList);
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            if (ConnectionUIManager.instance.isOn && Input.GetKeyDown(KeyCode.O)) {
                if (testList.Count > 0) testList.RemoveAt(testList.Count - 1);
                UpdateAllPlayerListItem(testList);
            }
        }
    }

    // END TEST-----------------------------------------------------------------------------------------

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    /// <summary>Add a PlayerListItem from given parameters</summary>
    public static void AddOnePlayerListItem(int id, string name) {
        UpdateOnePlayerListItem(id, name, PlayerListItem.State.Retain);
    }
    
    /// <summary>Add PlayerListItem or update the matched item state</summary>
    public static void UpdateOnePlayerListItem(int id, string name, PlayerListItem.State state) {
        // check if singleton instance is null
        if (instance == null) return;

        // check if playerListItem with given id and name already exists
        PlayerListItem target = null;
        foreach (PlayerListItem p in list) {
            if (p.id == id && p.name == name) {
                target = p;
                break;
            }
        }

        if (target != null) {
            // exist
            target.SetState(state);
        } else {
            // doesn't exist
            target = instance.AddPlayerListItem(id, name);
            target.SetState(state);
        }
    }

    /// <summary>Update the list. Given item means item in new list. If item already exist in old list, keep. If item doesn't exist add. If no item in new list but exist in old list, remove it</summary>

    public static void UpdateAllPlayerListItem(List<(int id, string name)> idNameList) {
        List<PlayerListItem> buffer = new List<PlayerListItem> (list);

        for (int i = 0; i < idNameList.Count; i++) {
            (int id, string name) idName = idNameList[i];
            PlayerListItem p = GetMatchedInList(idNameList[i]);

            // if match exists
            if (p != null) {
                // do nothing to list and remove match from buffer
                buffer.Remove(p);
            } 
            else {
                // if match doesn't exist, add new
                AddOnePlayerListItem(idName.id, idName.name);
            }
        }

        // destroy all unmatch
        foreach (PlayerListItem p in buffer) {
            list.Remove(p);
            Destroy(p.gameObject);
        }
    }

    /// <summary>Get playerListItem with matching parameter from list</summary>
    static PlayerListItem GetMatchedInList((int id, string name) idName) {
        for (int i = 0; i < list.Count; i++) {
            PlayerListItem p = list[i];
            if (p.id == idName.id && p.name == idName.name) {
                return p;
            }
        }
        return null;
    }

    PlayerListItem AddPlayerListItem(int id, string name) {
        // create new object
        GameObject o = Instantiate(playerListItem, content.transform.position, Quaternion.identity, content);

        // set id and name
        PlayerListItem target = o.GetComponent<PlayerListItem> ();
        target.SetID(id);
        target.SetName(name);

        // add to list
        list.Add(target);

        // return
        return target;
    }
}
