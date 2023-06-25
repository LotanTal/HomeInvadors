using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // List of events to trigger
    public List<GameObject> events;
    private int currentEvent = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there are any events left to trigger
        if (currentEvent < events.Count)
        {
            // Get the Event component of the current event game object
            Event currentEventComponent = events[currentEvent].GetComponent<Event>();

            // Trigger the next event
            events[currentEvent].SetActive(true);
            currentEvent++;
        }
    }
}
