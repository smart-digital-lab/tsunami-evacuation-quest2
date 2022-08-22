using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Public Variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public float timeBeforeEarthquake = 10.0f;
    public float timeForEarthquake = 5.0f;
    public float timeToReachSafety = 120.0f;
    public float timeToTsunami = 120.0f;
    public float timeForTsunami = 30.0f;
    public float safeHeight = 44.0f;
    public GameObject theXRRig;
    public CameraShake cameraShaker;
    public string timerText = "";
    public TextMeshPro[] messages;
    public TextMeshPro[] timers;

    public GameObject[] preQuakeSounds;
    public GameObject[] preQuakeObjects;
    public GameObject[] earthQuakeSounds;
    public GameObject[] postQuakeSounds;
    public GameObject[] postQuakeObjects;
    public GameObject[] tsunamiObjects;
    public GameObject[] underWaterObjects;
    public GameObject[] tsunamiSounds;
    public GameObject[] resetButton;

    // ------------------------------------------------------------------------------------------------------------------------------------------------------


    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Private Variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private bool timing = false;
    public float timer = float.NegativeInfinity;
    private Vector3 startPosition;
    private Quaternion startOrientation;
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Delegate function for the actions
    public delegate void doAction();
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Hashtable to hold the transitions where possible state changes can be quickly found and accessed
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public Hashtable theGame = new Hashtable();
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Possible game states
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public enum game_states {_, start, before_eq, during_eq, after_eq, at_vehicle, at_safepoint, wave_coming_alive, wave_coming_dead, game_over_alive, game_over_dead, resetting, show_reset_button}
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Possible events
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public enum game_events {go, timed_out, touched_vehicle, touched_safepoint, touched_dangerpoint, touched_sea, touched_bondary, start_eq, end_eq, start_wave, end_wave, drown, reset}
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Current game state
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public game_states current_state;
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Object to hold the details of transitions
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public class transition {
        public game_states new_state;
        public doAction the_Action;

        public transition (game_states stateout, doAction theAction)
        {
            new_state = stateout; the_Action = theAction;
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Create the unique hash for the hashtable based on the states and event
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public string hash (game_states statein, game_events eventin)
    {
        return (statein.ToString() + eventin.ToString());
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Add a transition to the game Hashtable
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public void AddTransition(ref Hashtable theGame, game_states statein, game_events eventin, game_states stateout, doAction theAction)
    {
        transition new_transition = new transition (stateout, theAction);
        string key = hash (statein, eventin);
        if (theGame.ContainsKey(key))
        {
            theGame[key] = new_transition;
        }
        else
        {
            theGame.Add(hash(statein, eventin), new_transition);
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Check the Automation for a possible event change given the current state
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public void SendEvent(ref Hashtable theGame, game_events eventin)
    {
        string key = hash (current_state, eventin);
        string anystate_key = hash (game_states._, eventin);
        if (theGame.ContainsKey(key))
        {
            transition theTransition = (transition) theGame[key];
            if (theTransition.new_state != game_states._)
            {
                current_state = theTransition.new_state;
            }
            theTransition.the_Action();
        }
        else if (theGame.ContainsKey(anystate_key))
        {
            transition theTransition = (transition) theGame[anystate_key];
            if (theTransition.new_state != game_states._)
            {
                current_state = theTransition.new_state;
            }
            theTransition.the_Action();
        }
        Debug.Log ("Current State " + current_state.ToString());
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Set up the transitions and ge the ball rolling
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        // Store the starting position and orientation for later
        startPosition = theXRRig.transform.position;
        startOrientation = theXRRig.transform.rotation;

        // Listen for events coming from the XR Controllers and other devices
        if (XRRig.EventQueue != null) XRRig.EventQueue.AddListener(onDeviceEvent);

        // Set up the transitions
        //                         Current State                    Event                               New State                       Action
        AddTransition(ref theGame, game_states.start,               game_events.go,                     game_states.before_eq,          SetUp);

        AddTransition(ref theGame, game_states.before_eq,           game_events.timed_out,              game_states.during_eq,          Earthquake);
        AddTransition(ref theGame, game_states.during_eq,           game_events.timed_out,              game_states.after_eq,           Post_Earthquake);

        AddTransition(ref theGame, game_states._,                   game_events.touched_vehicle,        game_states._,                  Touch_Vehicle);
        AddTransition(ref theGame, game_states._,                   game_events.touched_safepoint,      game_states.at_safepoint,       Touch_SafePoint);
        AddTransition(ref theGame, game_states._,                   game_events.touched_dangerpoint,    game_states.after_eq,           Touch_DangerPoint);
        AddTransition(ref theGame, game_states._,                   game_events.touched_sea,            game_states.after_eq,           Touch_Sea);
        AddTransition(ref theGame, game_states.after_eq,            game_events.timed_out,              game_states.wave_coming_dead,   Tsunami_Dead);
        AddTransition(ref theGame, game_states.wave_coming_dead,    game_events.timed_out,              game_states.game_over_dead,     Game_Over_Dead);
        
        AddTransition(ref theGame, game_states.at_safepoint,        game_events.timed_out,              game_states.wave_coming_alive,  Tsunami_Alive);
        AddTransition(ref theGame, game_states.wave_coming_alive,   game_events.timed_out,              game_states.game_over_alive,    Game_Over_Alive);

        AddTransition(ref theGame, game_states.after_eq,            game_events.drown,                  game_states._,                  Bring_Up_Water);
        AddTransition(ref theGame, game_states.wave_coming_dead,    game_events.drown,                  game_states._,                  Bring_Up_Water);
        AddTransition(ref theGame, game_states.game_over_dead,      game_events.drown,                  game_states._,                  Bring_Up_Water);
        
        AddTransition(ref theGame, game_states._,                   game_events.timed_out,              game_states.resetting,          Show_Button);
        AddTransition(ref theGame, game_states._,                   game_events.reset,                  game_states.start,              Reset_Game);

        // Set the initial state
        current_state = game_states.start;

        // Send the first event
        SendEvent(ref theGame, game_events.go);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Various action functions
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void SetUp()
    {
        // Turn off all the post earthquake sounds and objects
        // Reset the user to the start position
        // Set the countdown timer to time between start and earthquake
        Debug.Log ("Setup");
        theXRRig.transform.position = startPosition;
        theXRRig.transform.rotation = startOrientation;

        PlaySounds(preQuakeSounds, true, true);
        Activate(preQuakeObjects, true);
        Activate(postQuakeObjects, false);
        PlaySounds(postQuakeSounds, false, true);
        PlaySounds(earthQuakeSounds, false, false);
        Activate(tsunamiObjects, false);
        PlaySounds(tsunamiSounds, false, false);
        Activate(underWaterObjects, false);
        Activate(resetButton, false);
        Clear_Messages();

        timerText = "Time until earthquake";

        SetTimer(timeBeforeEarthquake);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Earthquake()
    {
        // Turn on and play the Earthquake sounds
        // Do the rumbling of the viewport
        // Set the countdown timer for the earthquake
        Debug.Log ("Earthquake");
        PlaySounds(preQuakeSounds, false, true);
        Activate(preQuakeObjects, false);
        Activate(postQuakeObjects, false);
        PlaySounds(postQuakeSounds, false, true);
        PlaySounds(earthQuakeSounds, true, false);
        Activate(tsunamiObjects, false);
        PlaySounds(tsunamiSounds, false, false);
        Activate(underWaterObjects, false);
        Activate(resetButton, false);
        Clear_Messages();

        if (cameraShaker != null) cameraShaker.DoShake();

        timerText = "WARNING EARTHQUAKE";

        SetTimer(timeForEarthquake);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Post_Earthquake()
    {
        Debug.Log ("Post Earthquake");
        PlaySounds(preQuakeSounds, false, true);
        Activate(preQuakeObjects, false);
        Activate(postQuakeObjects, true);
        PlaySounds(postQuakeSounds, true, true);
        PlaySounds(earthQuakeSounds, false, false);
        Activate(tsunamiObjects, false);
        PlaySounds(tsunamiSounds, false, false);
        Activate(underWaterObjects, false);
        Activate(resetButton, false);
        Clear_Messages();

        timerText = "Time to reach safety";

        SetTimer(timeToReachSafety);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Touch_Vehicle()
    {
        Debug.Log ("Touch Vehicle");
        if (current_state == game_states.after_eq)
        {
            timer -= 12.0f;
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Touch_SafePoint()
    {
        Debug.Log ("Touch Safepoint");

        // if too low, move viewpoint up
        if (theXRRig.transform.position.y < 20)
        {
            theXRRig.transform.position = new Vector3(theXRRig.transform.position.x, safeHeight, theXRRig.transform.position.z);
        }
        timer = 1.0f;
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Touch_DangerPoint()
    {
        Debug.Log ("Touch Dangerpoint");
        timer -= 6.0f;
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Touch_Sea()
    {
        Debug.Log ("Touch Sea");
        if (current_state == game_states.after_eq)
        {
            timer -= 6.0f;
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Tsunami_Alive()
    {
        Debug.Log ("Tsunami alive");
        PlaySounds(preQuakeSounds, false, true);
        Activate(preQuakeObjects, false);
        Activate(postQuakeObjects, false);
        PlaySounds(postQuakeSounds, true, true);
        PlaySounds(earthQuakeSounds, false, false);
        Activate(tsunamiObjects, false);
        PlaySounds(tsunamiSounds, true, false);
        Activate(underWaterObjects, false);
        Activate(resetButton, false);
        Clear_Messages();

        SetTimer(timeToTsunami);        
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Tsunami_Dead()
    {
        Debug.Log ("Tsunami dead");
        PlaySounds(preQuakeSounds, false, true);
        Activate(preQuakeObjects, false);
        Activate(postQuakeObjects, false);
        PlaySounds(postQuakeSounds, true, true);
        PlaySounds(earthQuakeSounds, false, false);
        Activate(tsunamiObjects, false);
        PlaySounds(tsunamiSounds, true, false);
        Activate(underWaterObjects, false);
        Activate(resetButton, false);
        Clear_Messages();

        SetTimer(timeToTsunami);        
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Game_Over_Dead()
    {
        Debug.Log ("Game over dead");
        PlaySounds(preQuakeSounds, false, true);
        Activate(preQuakeObjects, false);
        Activate(postQuakeObjects, false);
        PlaySounds(postQuakeSounds, false, true);
        PlaySounds(earthQuakeSounds, false, false);
        Activate(tsunamiObjects, true);
        PlaySounds(tsunamiSounds, true, false);
        Activate(underWaterObjects, false);
        Activate(resetButton, false);
        Clear_Messages();

        SetTimer(timeForTsunami);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Game_Over_Alive()
    {
        Debug.Log ("Game over alive");
        PlaySounds(preQuakeSounds, false, true);
        Activate(preQuakeObjects, false);
        Activate(postQuakeObjects, false);
        PlaySounds(postQuakeSounds, true, true);
        PlaySounds(earthQuakeSounds, false, false);
        Activate(tsunamiObjects, true);
        PlaySounds(tsunamiSounds, true, false);
        Activate(underWaterObjects, false);        
        Activate(resetButton, false);
        Clear_Messages();

        SetTimer(timeForTsunami);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Bring_Up_Water()
    {
        Activate(underWaterObjects, true);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Show_Button()
    {
        Debug.Log ("Showing Button");

        PlaySounds(preQuakeSounds, false, true);
        Activate(preQuakeObjects, false);
        Activate(postQuakeObjects, false);
        PlaySounds(postQuakeSounds, false, true);
        PlaySounds(earthQuakeSounds, false, false);
        // Activate(tsunamiObjects, true);
        PlaySounds(tsunamiSounds, false, false);
        // Activate(underWaterObjects, false);
        Activate(resetButton, true);
        Clear_Messages();
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Reset_Game()
    {
        Debug.Log ("Resetting Game");

        timing = false;
        timer = float.NegativeInfinity;
        current_state = game_states.start;
        Clear_Messages();
        SendEvent(ref theGame, game_events.go);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Clear_Messages()
    {
        if (messages != null) 
        {
            foreach (TextMeshPro messageText in messages)
            {
                messageText.text = "";
            }  
        }
        if (timers != null) 
        {
            foreach (TextMeshPro timerText in timers)
            {
                timerText.text = "";
            }  
        }        
    }



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Check the timer and set off events as required
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        if (timing)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if ((timers != null) && (current_state == game_states.after_eq))
                {
                    foreach (TextMeshPro theTimerText in timers)
                    {
                        theTimerText.text = timerText + " : " + ConvertSecondsToMinutes(timer * 10.0f);
                    }
                }
                else
                {
                    foreach (TextMeshPro theTimerText in timers)
                    {
                        theTimerText.text = "";
                    }                    
                }
            }
            else
            {
                Debug.Log ("Sending Timeout");
                timing = false;
                SendEvent(ref theGame, game_events.timed_out);
            }
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private void SetTimer(float newTime)
    {
        Debug.Log ("Setting Timer to " + newTime);
        timing = true;
        timer = newTime;
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Activate(GameObject[] gameObjects, bool activation)
    {
        foreach (GameObject theObject in gameObjects)
        {
            theObject.SetActive(activation);
            if(activation)
            {
                MoveWaveInAndUp moveScript = theObject.GetComponent<MoveWaveInAndUp>();
                if (moveScript != null) moveScript.Go();
            }

            Transform[] moreGameObjects = theObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform anotherGameObject in moreGameObjects)
            {
                anotherGameObject.gameObject.SetActive(activation);
                if(activation)
                {
                    MoveWaveInAndUp moveScript2 = anotherGameObject.GetComponent<MoveWaveInAndUp>();
                    if (moveScript2 != null) moveScript2.Go();
                }
            }
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private void PlaySounds(GameObject[] gameObjects, bool play, bool loop)
    {
        foreach (GameObject theObject in gameObjects)
        {
            theObject.SetActive(play);

            if (play)
            {
                AudioSource[] audioSources = theObject.GetComponentsInChildren<AudioSource>(true);
                foreach (AudioSource audioSource in audioSources)
                {
                    if (play) 
                    {
                        audioSource.gameObject.SetActive(true);
                        audioSource.loop = loop;
                        audioSource.Play(); 
                    }
                    else 
                    {
                        audioSource.Stop();
                        audioSource.gameObject.SetActive(false);
                    }
                }

                AudioSource mainAudioSource = theObject.GetComponent<AudioSource>();
                if (mainAudioSource != null)
                {
                    mainAudioSource.gameObject.SetActive(play);
                    mainAudioSource.loop = loop;
                    if (play) 
                    {
                        mainAudioSource.gameObject.SetActive(true);
                        mainAudioSource.loop = loop;
                        mainAudioSource.Play();
                    }
                    else
                    {
                        mainAudioSource.Stop(); 
                        mainAudioSource.gameObject.SetActive(false);
                    }
                }
            }
        }        
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Global Console Events can be sent from anywhere
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private void onDeviceEvent(XREvent theEvent)
    {
        if (((theEvent.eventType == XRDeviceEventTypes.message) || (theEvent.eventType == XRDeviceEventTypes.console)) && (theEvent.eventAction == XRDeviceActions.CHANGE))
        {
            Debug.Log(theEvent.data.ToString());
            switch (theEvent.data.ToString())
            {
                case "SAFE":
                    SendEvent(ref theGame, game_events.touched_safepoint);
                    break;
                case "UNSAFE":
                    SendEvent(ref theGame, game_events.touched_dangerpoint);
                    break;
                case "SEA":
                    SendEvent(ref theGame, game_events.touched_sea);
                    break;
                case "CAR":
                    SendEvent(ref theGame, game_events.touched_vehicle);
                    break;
                case "RESET":
                    SendEvent(ref theGame, game_events.reset);
                    break;
                case ".":
                    SendEvent(ref theGame, game_events.reset);
                    break;
                case "DROWN":
                    SendEvent(ref theGame, game_events.drown);
                    break;
                default:
                    break;
            }
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private string ConvertSecondsToMinutes(float theTime)
    {
        string output = "";
        int minutes = Mathf.FloorToInt(theTime / 60.0f);
        if (minutes > 0)
        {
            output += minutes.ToString() + " minutes ";
        }
        return (output + Mathf.RoundToInt(theTime % 60.0f).ToString() + " seconds");
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------

}
