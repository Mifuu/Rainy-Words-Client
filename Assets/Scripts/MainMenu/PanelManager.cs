using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    // singleton
    public static PanelManager instance;

    public Panel[] panels;
    public Stack<Panel> panelStack = new Stack<Panel> ();

    [Header("Transition")]
    public bool doTransition = true;
    public GameObject transitionPanel;
    public Animator transitionAnimator;

    // for FancyButton
    [Header("For FancyButton")]
    public int frameSincePanelUpdated = 0;

    // --------------Starts----------------

    void Awake() {
        // singleton
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start() {
        // set initial panel
        if (panelStack.Count == 0) panelStack.Push(panels[0]);
    }

    void Update() {
        frameSincePanelUpdated++;
    }

    // --------------Publics----------------

    ///<summary>Transition to next panel</summary>
    public void Next(string panelName) {
        // check if string empty
        if (panelName.Equals("")) return;

        Panel next = GetPanel(panelName);

        // check if null
        if (next == null) {
            Debug.Log("PanelManager: Error, panelName \"" + panelName + "\" is not found");
            return;
        }

        // set panel and push to stack
        SetPanel(panelStack.Peek(), next);
        panelStack.Push(next);
    }

    public void Next(string panelName, float delay) {
        StartCoroutine(DelayNextCR(panelName, delay));
    }

    IEnumerator DelayNextCR(string panelName, float delay) {
        yield return new WaitForSecondsRealtime(delay);
        Next(panelName);
    }

    public void Previous() {
        // check stack
        if (panelStack.Count <= 1) {
            Debug.Log("PanelManager: Error, no previous panel in the stack");
            return;
        }

        // pop current
        Panel current = panelStack.Pop();

        // while top of panelStack cannot be previous (e.g.loading panel)
        while (!panelStack.Peek().canBePrevious) {
            // check stack
            if (panelStack.Count <= 1) {
                Debug.Log("PanelManager: Error, no suitable previous panel in the stack");
                return;
            }
            panelStack.Pop();
        }
        Panel prev = panelStack.Peek();
        
        // set panel
        SetPanel(current, prev);
    }

    public void ClearStack() {
        Panel current = panelStack.Peek();

        panelStack.Clear();
        
        panelStack.Push(current);
    }

    public void DisableAllNotCurrent() {
        // check if stack is empty
        if (panelStack.Count == 0) {
            panelStack.Push(panels[0]);
        }

        foreach (Panel p in panels) {
            p.obj.SetActive(false);
        }

        panelStack.Peek().obj.SetActive(true);
    }

    // --------------Statics----------------

    public static void StaticNext(string panelName) {
        if (instance != null) {
            instance.Next(panelName);
        } else {
            Debug.Log("PanelManager: Error, instance is null");
        }
    }

    public static void StaticNext(string panelName, float delay) {
        if (instance != null) {
            instance.Next(panelName, delay);
        } else {
            Debug.Log("PanelManager: Error, instance is null");
        }
    }

    public static void StaticPrevious() {
        if (instance != null) {
            instance.Previous();
        } else {
            Debug.Log("PanelManager: Error, instance is null");
        }
    }

    public static void StaticClearStack() {
        if (instance != null) {
            instance.ClearStack();
        } else {
            Debug.Log("PanelManager: Error, instance is null");
        }
    }

    public static void StaticDisableAllNotCurrent() {
        if (instance != null) {
            instance.DisableAllNotCurrent();
        } else {
            Debug.Log("PanelManager: Error, instance is null");
        }
    }

    // --------------Utilities----------------

    ///<summary>Get panel from panels by name</summary>
    Panel GetPanel(string panelName) {
        Panel p = null;
        foreach (Panel _p in panels) {
            if (_p.name.Equals(panelName)) {
                p = _p;
                break;
            }
        }
        
        return p;
    }

    ///<summary>Change panel</summary>
    void SetPanel(Panel prev, Panel next) {
        // forward to coroutine
        StartCoroutine(SetPanelCR(prev, next));
    }

    IEnumerator SetPanelCR(Panel prev, Panel next) {
        // transition IN
        Panel.Transition transition = GetTransition(prev, next);
        if (transitionPanel != null && doTransition) {
            switch (transition) {
                case Panel.Transition.FadeDrop:
                    // put transition panel in front and activate
                    transitionPanel.transform.SetAsLastSibling();
                    transitionAnimator.SetTrigger("DropFadeIn");
                    yield return new WaitForSecondsRealtime(0.5f);
                    break;
                case Panel.Transition.None:
                    break;
            }
        }
        
        // activate next and disable? prev panel
        if (next.disablePrevPanel) prev.obj.SetActive(false);
        next.obj.SetActive(true);
        // bring obj to the front
        next.obj.transform.SetAsLastSibling();
        // put transition panel in front
        transitionPanel.transform.SetAsLastSibling();
        // reset frame
        frameSincePanelUpdated = 0;
        yield return new WaitForSecondsRealtime(0.1f);

        // transition OUT
        if (transitionPanel != null && doTransition) {
            switch (transition) {
                case Panel.Transition.FadeDrop:
                    transitionAnimator.SetTrigger("DropFadeOut");
                    yield return new WaitForSecondsRealtime(0.35f);
                    break;
                case Panel.Transition.None:
                    break;
            }
        }

        // bring obj to the front
        next.obj.transform.SetAsLastSibling();
    }

    /// <summary>get proper transition from prev to next panel</summary>
    Panel.Transition GetTransition(Panel prev, Panel next) {
        // set default
        Panel.Transition t = prev.transitionToOther;

        // check override
        foreach (Panel.TTOOverride o in prev.ttoOverrides) {
            // get next panel in TTOOverride
            Panel targetPanel = GetPanel(o.nextPanelName);
            if (targetPanel == null) continue;
            if (targetPanel == next) {
                // if target panel == next, set new t and break to finish up
                t = o.transition;
                break;
            }
        }

        // return t
        return t;
    }

    [System.Serializable]
    public class Panel {
        public string name;
        public GameObject obj;
        [Tooltip("If true, when changing to this panel, disable old panel")]
        public bool disablePrevPanel = true;
        [Tooltip("If false, will not appear in Previous(...). Useful for loading panel")]
        public bool canBePrevious = true;

        public enum Transition{None, FadeDrop}
        [Header("Transition")]
        public Transition transitionToOther = Transition.None;
        public TTOOverride[] ttoOverrides = {};
        

        [System.Serializable]
        public struct TTOOverride {
            // add this to override default transitionToOther in specific scene
            public string nextPanelName;
            public Transition transition;
        }
    }
}