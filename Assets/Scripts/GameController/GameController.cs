using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController SharedInstance;

    [SerializeField]
    private AState[] states;
    private List<AState> m_StateStack = new List<AState>();
    private Dictionary<string, AState> m_StateDict = new Dictionary<string, AState>();

    public LevelProgress levelProgress;

    private void OnEnable()
    {
        SharedInstance = this;
        levelProgress = new LevelProgress();

        /**
        * Init States
        */
        m_StateDict.Clear();

        if (states.Length == 0)
            return;

        for (int i = 0; i < states.Length; ++i)
        {
            states[i].controller = this;
            m_StateDict.Add(states[i].GetName(), states[i]);
        }
        m_StateStack.Clear();

        // Init first state (Loadout)
        PushState(states[0].GetName());
    }

    public AState FindState(string stateName)
    {
        AState state;
        if (!m_StateDict.TryGetValue(stateName, out state))
        {
            return null;
        }

        return state;
    }

    // State management
    public void SwitchState(string newState)
    {
        AState state = FindState(newState);
        if (state == null)
        {
            Debug.LogError("Can't find the state named " + newState);
            return;
        }

        m_StateStack[m_StateStack.Count - 1].Exit(state);
        state.Enter(m_StateStack[m_StateStack.Count - 1]);
        m_StateStack.RemoveAt(m_StateStack.Count - 1);
        m_StateStack.Add(state);
    }

    public void PopState()
    {
        if (m_StateStack.Count < 2)
        {
            Debug.LogError("Can't pop states, only one in stack.");
            return;
        }

        m_StateStack[m_StateStack.Count - 1].Exit(m_StateStack[m_StateStack.Count - 2]);
        m_StateStack[m_StateStack.Count - 2].Enter(m_StateStack[m_StateStack.Count - 2]);
        m_StateStack.RemoveAt(m_StateStack.Count - 1);
    }

    public void PushState(string name)
    {
        AState state;
        if (!m_StateDict.TryGetValue(name, out state))
        {
            Debug.LogError("Can't find the state named " + name);
            return;
        }

        if (m_StateStack.Count > 0)
        {
            m_StateStack[m_StateStack.Count - 1].Exit(state);
            state.Enter(m_StateStack[m_StateStack.Count - 1]);
        }
        else
        {
            state.Enter(null);
        }
        m_StateStack.Add(state);
    }
}

public abstract class AState : MonoBehaviour
{
    [HideInInspector]
    public GameController controller;

    public abstract void Enter(AState from);
    public abstract void Exit(AState to);

    public abstract string GetName();
}

