using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // singleton
    public static GameManager instance;

    // var
    bool isConnected;   // use for checking if the game is connected to the server
    public enum NextSceneMode {Menu, Single, Multi}
    public static NextSceneMode nextSceneMode = NextSceneMode.Single;
    public static bool isPaused = false;
    int singleModeID = 0;
    string multiModeOtherName = "";
    string multiModeName = "";
    int multiModeMyId = 0;
    int multiModeOtherId = 0;
    bool receivedAssignID = false;
    bool receivedPlayerList = false;

    // play var
    PlayManager playManager;

    // settings
    public string menuSceneName = "MainMenu1";
    public string playSceneName = "GameTest";

    [Header("Object References")]
    public UnityEngine.Rendering.VolumeProfile volumeProfile;

    void Awake() {
        // singleton
        if (instance == null) instance = this;
        else Destroy(this);

        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("GameManager.OnSceneLoaded(), nextSceneMode: " + nextSceneMode.ToString());
        string sceneName = scene.name;
        if (sceneName.Equals(menuSceneName)) {
            PanelManager.PlayTransition(false, PanelManager.Panel.Transition.FadeDrop);
        } else if (sceneName.Equals(playSceneName)) {
            // find PlayManager
            playManager = FindObjectOfType<PlayManager> ();
            if (playManager == null) {
                Debug.Log("GameManager: ERROR Can't find PlayManager!");
                return;
            }

            // start the game accordingly
            if (nextSceneMode == NextSceneMode.Multi) {
                // start multi
                playManager.SetupMultiplayer(multiModeName, multiModeOtherName, 300);
                PanelManager.PlayTransition(false, PanelManager.Panel.Transition.FadeDrop);

                // tell server player ready to play
                ConnectionManager.DeliverMsg("{\"readyToPlay\":[" + $"{Client.instance.myId},{multiModeOtherId}" + "]}");
            } else {
                // start single
                playManager.SetupSingleplayer(3);
                PanelManager.PlayTransition(false, PanelManager.Panel.Transition.FadeDrop);
            }
        }
    }
    //-----------------Coroutine/Action Sequence---------------------
    IEnumerator MenuWaitForConnectionCR() {
        // wait until received {assignID} and {playerList}
        while (true) {
            if (receivedAssignID && receivedPlayerList) {
                receivedAssignID = false;
                receivedPlayerList = false;
                break;
            }
            yield return null;
        }

        PanelManager.StaticNext("Online Players Panel");
    }

    public void WaitForConnectionToPlayerList() {
        StartCoroutine(MenuWaitForConnectionCR());
    }

    //-----------------From Out Function---------------------
    public void SetIsConnected(bool isConnected) {
        this.isConnected = isConnected;
    }

    public void ReceivedAssignID() {
        receivedAssignID = true;
    }

    public void ReceivedPlayerList() {
        receivedPlayerList = true;
    }

    public void ReceivedMatchStart(int[] matchStart) {
        int myId = Client.instance.myId;
        int otherId = 0;
        if (matchStart[0] == myId) {
            otherId = matchStart[1];
        } else if (matchStart[1] == myId) {
            otherId = matchStart[0];
        } else {
            Debug.Log("GameManager: ERROR no user id in matchStart!");
            return;
        }

        multiModeName = PlayerList.GetNameFromIdNameList(myId);
        multiModeOtherName = PlayerList.GetNameFromIdNameList(otherId);
        multiModeMyId = myId;
        multiModeOtherId = otherId;

        ChangeSceneMultiPlayer();
    }

    public void ChangeSceneMenu() {
        nextSceneMode = NextSceneMode.Menu;
        StartCoroutine(ChangeSceneMenuCR());
    }

    public void ChangeSceneMenu(string panelName) {
        nextSceneMode = NextSceneMode.Menu;
        StartCoroutine(ChangeSceneMenuCR(panelName));
    }

    public void ChangeSceneSinglePlayer() {
        ChangeSceneSinglePlayer(singleModeID);
    }

    public void ChangeSceneSinglePlayer(int id) {
        singleModeID = id;
        nextSceneMode = NextSceneMode.Single;
        StartCoroutine(ChangeScenePlayCR());
    }

    public void ChangeSceneMultiPlayer() {
        nextSceneMode = NextSceneMode.Multi;
        StartCoroutine(ChangeScenePlayCR());
    }

    IEnumerator ChangeSceneMenuCR() {
        float time = PanelManager.PlayTransition(true, PanelManager.Panel.Transition.FadeDrop);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(menuSceneName);
    }
    IEnumerator ChangeSceneMenuCR(string panelName) {
        float time = PanelManager.PlayTransition(true, PanelManager.Panel.Transition.FadeDrop);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(menuSceneName);
        PanelManager.StaticNext("Single P Conclusion Panel");
    }

    IEnumerator ChangeScenePlayCR() {
        float time = PanelManager.PlayTransition(true, PanelManager.Panel.Transition.FadeDrop);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(playSceneName);
    }

    public void PlayTransition() {
        PanelManager.PlayTransition(true, PanelManager.Panel.Transition.FadeDrop);
    }

    public bool InMenuScene() {
        return SceneManager.GetActiveScene().name.Equals(menuSceneName);
    }

    public void Pause(bool i) {
        if (nextSceneMode != NextSceneMode.Single) isPaused = false;
        isPaused = i;
    }

    //-----------------Event---------------------
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
