using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    // singleton
    static PlayerList instance;

    static List<(int id, string name)> idNameList = new List<(int id, string name)> ();
    List<PlayerListItem> itemList = new List<PlayerListItem> ();
    
    [Header("PlayerListItem prefab")]
    public GameObject playerListItem;       // player list item prefab
    [Header("Content list")]
    public RectTransform content;               // list content transform
    public Vector2 originOffset = new Vector2();
    public Vector2 eachOffset = new Vector2();
    
    // TEST---------------------------------------------------------------------------------------------
    List<(int id, string name, bool isBusy)> testList = new List<(int id, string name, bool isBusy)> ();
    void Update() {
        if (ConnectionUIManager.instance == null) return;
        if (ConnectionUIManager.instance.isOn && Input.GetKeyDown(KeyCode.P)) {
            testList.Add((testList.Count, "test" + testList.Count, false));
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
        UpdateOnePlayerListItem(id, name, false);
    }
    
    /// <summary>Add PlayerListItem or update the matched item state</summary>
    public static void UpdateOnePlayerListItem(int id, string name, bool isBusy) {
        // check if idNameList with the given id and name already exists
        bool foundPair = false;
        foreach ((int id, string name) i in idNameList) {
            if (i.id == id) {
                foundPair = true;
                break;
            }
        }

        // if not add;
        if (!foundPair) idNameList.Add((id, name));

        // check if singleton instance is null
        if (instance == null) return;

        // check if playerListItem with given id and name already exists
        PlayerListItem target = null;
        foreach (PlayerListItem p in instance.itemList) {
            if (p.id == id) {
                target = p;
                break;
            }
        }

        if (target != null) {
            // exist
            target.SetBusy(isBusy);
        } else {
            // doesn't exist
            target = instance.AddPlayerListItem(id, name);
            target.SetState(PlayerListItem.State.Match);
            target.SetBusy(isBusy);
        }
    }

    /// <summary>Update the list. Given item means item in new list. If item already exist in old list, keep. If item doesn't exist add. If no item in new list but exist in old list, remove it</summary>

    public static void UpdateAllPlayerListItem(List<(int id, string name, bool isBusy)> idNameList) {
        // idNameList
        PlayerList.idNameList.Clear();
        foreach ((int id, string name, bool isBusy) i in idNameList) {
            // PlayerList.idNameList = new List<(int id, string name)> (idNameList);
            PlayerList.idNameList.Add((i.id, i.name));
        }

        // itemList
        if (instance == null) return;
        List<PlayerListItem> buffer = new List<PlayerListItem> (instance.itemList);

        // mod idNameList to remove self from it
        int myID = Client.instance.myId;
        for (int i = 0; i < idNameList.Count; i++) {
            if (idNameList[i].id == myID) {
                // tell MainMenu that is our name
                if (MainMenu.instance != null) MainMenu.instance.SetPLISelf(idNameList[i].name);
                // remove
                idNameList.RemoveAt(i);
                break;
            }
        }

        foreach ((int id, string name, bool isBusy) i in idNameList) {
            PlayerListItem p = GetMatchedInList((i.id, i.name));

            // if match exists
            if (p != null) {
                // do nothing to list and remove match from buffer
                buffer.Remove(p);

                // set busy?
                p.SetBusy(i.isBusy);
            } 
            else {
                // if match doesn't exist, add new
                UpdateOnePlayerListItem(i.id, i.name, i.isBusy);
            }
        }

        // destroy all unmatch
        foreach (PlayerListItem p in buffer) {
            instance.itemList.Remove(p);
            Destroy(p.gameObject);
        }
    }

    /// <summary>Get playerListItem with matching parameter from list</summary>
    static PlayerListItem GetMatchedInList((int id, string name) idName) {
        if (instance == null) return null;

        for (int i = 0; i < instance.itemList.Count; i++) {
            PlayerListItem p = instance.itemList[i];
            if (p.id == idName.id) {
                return p;
            }
        }
        return null;
    }

    public static void GotMatchRequest(int id) {
        if (instance == null) return;
        
        foreach (PlayerListItem p in instance.itemList) {
            if (p.id == id) {
                p.SetState(PlayerListItem.State.Confirm);
                return;
            }
        }
    }

    PlayerListItem AddPlayerListItem(int id, string name) {
        if (instance == null) return null;

        // create new object
        GameObject o = Instantiate(playerListItem, content.transform.position, Quaternion.identity, content);

        // set id and name
        PlayerListItem target = o.GetComponent<PlayerListItem> ();
        target.SetID(id);
        target.SetName(name);

        // add to list
        itemList.Add(target);

        // return
        return target;
    }
    
    public static string GetNameFromIdNameList(int id) {
        foreach ((int id, string name) i in idNameList) {
            if (i.id == id) return i.name;
        }
        Debug.Log("PlayerList: ERROR, can't find matching name for id:" + id);
        return "";
    }
}
