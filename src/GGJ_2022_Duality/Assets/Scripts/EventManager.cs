using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();

    public void StartListening(string eventName, UnityAction listener) {
        UnityEvent ev = null;
        if (this.eventDictionary.TryGetValue(eventName, out ev)) {
            ev.AddListener(listener);
        } else {
            ev = new UnityEvent();
            ev.AddListener(listener);
            this.eventDictionary.Add(eventName, ev);
        }
    }

    public void StopListening(string evName, UnityAction listener = null) {
        UnityEvent ev = null;
        if (this.eventDictionary.TryGetValue(evName, out ev)) {
            if (listener != null) {
                ev.RemoveListener(listener);
            } else {
                ev.RemoveAllListeners();
            }
        }
    }

    public void TriggerEvent (string evName) {
        UnityEvent ev = null;
        if (this.eventDictionary.TryGetValue(evName, out ev)) {
            ev.Invoke();
        }
    }
}
