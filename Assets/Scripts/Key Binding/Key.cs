using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour {
    private Event keyEvent;
    private bool waiting;
    private KeyCode newKey;
    public static Dictionary<String, KeyCode> keys;

    // Use this for initialization
    void Start () {
        waiting = false;
	}
    /*
    private void Set()
    {
        Debug.Log("clicked");
        //grabs the text that is in the text of button
        string key = text.text;
        //convert text to tmp keycode
        KeyCode tmp = (KeyCode)System.Enum.Parse(typeof(KeyCode), key);

        //loop till cancel
        while (!Input.GetKey(KeyCode.Escape))
        {
            //check if event is key event
            if (keyEvent.isKey)
            {
                Debug.Log("setting key");
                //loop keycode listing
                for (int i = 0; i < keys.Length; i++)
                {
                    //loop for keycode that matches the textbased keycode
                    if (keys[i] == tmp)
                    {
                        //set new keycode
                        keys[i] = keyEvent.keyCode;

                        //set new button text
                        text.text = keyEvent.keyCode.ToString();

                        //break for loop early
                        return;
                    }
                    
                }
                //cancel loop on finished key assignment
                return;
            }
            
        }
    }
    */
    
    void OnGUI()
    {
        //Debug.Log("onGui called");
        keyEvent = Event.current;
        if ((keyEvent.type == EventType.KeyDown || keyEvent.type == EventType.MouseDown) && waiting)
        {
            newKey = keyEvent.keyCode;
            waiting = false;
        }   
    }

    public void Set(Text text)
    {
        if (!waiting)
            StartCoroutine(AssignKey(text));
    }

    IEnumerator WaitForKey()
    {
        while ((keyEvent.type != EventType.KeyDown && keyEvent.type != EventType.MouseDown))
            yield return null;
    }

    public IEnumerator AssignKey(Text text)
    {
        waiting = true;
        text.text = "Waiting";
        yield return WaitForKey();

        keys[text.GetComponentInChildren<Text>().text] = newKey;
        text.text = newKey.ToString();
    }
    public static void Populate()
    {
        keys = new Dictionary<String, KeyCode>()
        {
            { "Forward", KeyCode.W },
            { "Left", KeyCode.A },
            { "Backwards", KeyCode.S },
            { "Right", KeyCode.D },
            { "Jump", KeyCode.Space },

            { "Sprint", KeyCode.LeftShift },
            { "Crouch", KeyCode.LeftControl },

            { "Reload", KeyCode.R },
            { "Pick Up", KeyCode.F },

            { "Fire", KeyCode.Mouse0 },
            { "Scope", KeyCode.Mouse1 },
            { "Quick Melee", KeyCode.Mouse2 }

            //{ "Forward", KeyCode.W },

        };
    }

    public static void Load()
    {
        keys = Data.keys;
    }

    public static Vector3 GetAxis()
    {
        float x, y, z;
        if (Input.GetKeyDown(keys["Forward"])) { x = 1; } else if (Input.GetKeyDown(keys["Backwards"])) { x = -1; } else { x = 0; }
        if (Input.GetKeyDown(keys["Jump"])) { y = 1; } else { y = 0; }
        if (Input.GetKeyDown(keys["Right"])) { z = 1; } else if (Input.GetKeyDown(keys["Left"])) { z = -1; } else { z = 0; }

        return new Vector3(x, y, z);
    }

    public static float GetAxis(char dir)
    {
        if (dir == 'x' || dir == 'X') { if (Input.GetKeyDown(keys["Forward"])) { return 1; } else if (Input.GetKeyDown(keys["Backwards"])) { return -1; } }
        if (dir == 'y' || dir == 'Y') { if (Input.GetKeyDown(keys["Jump"])) { return 1; } else { return 0; } }
        if (dir == 'z' || dir == 'Z') { if (Input.GetKeyDown(keys["Right"])) { return 1; } else if (Input.GetKeyDown(keys["Left"])) { return -1; } }
        return new float();
    }
}
