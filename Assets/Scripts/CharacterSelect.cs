using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    bool onDown = false; // just onDown for axis inputs
    bool joined; // for checking if a controller player has joined the game
    int iterator; // current character for this slice
    public GameObject[] prefabs; // stores the UI players
    public GameObject currentPlayer; // to store for next scene and manage where the prefabs go
    public GameObject message;

    public SoundEffectPlayer sounds;
    void Update()
    {
        List<string> controllerList = new List<string>();
        foreach (string str in Input.GetJoystickNames())
        {
            if (str != "")
            {
                controllerList.Add(str);
            }
        }
        int numControllers = controllerList.Count;

        switch (gameObject.name)
        {
            case "Slot 1":
                HandleCharacterDisplay();
                break;
            case "Slot 2":
                if (numControllers >= 1)
                {
                    // Display the "Press A to join" message and prepare for displaying the characters in that slot if that player joins
                    message.transform.position = new Vector2(transform.position.x, gameObject.transform.position.y);
                    if (Input.GetButtonDown("Fire1") && !joined)
                    {
                        joined = true;
                        message.transform.position = new Vector2(transform.position.x, gameObject.transform.position.y - 2000);
                        HandleCharacterDisplay();
                        sounds.play(1);
                    }
                }
                else
                {
                    // Do not display anything in that slot
                }
                break;
            case "Slot 3":
                if (numControllers >= 2)
                {
                    // Display the "Press A to join" message and prepare for displaying the characters in that slot if that player joins
                    message.transform.position = new Vector2(transform.position.x, gameObject.transform.position.y);
                }
                else
                {
                    // Do not display anything in that slot
                    
                }
                break;
            case "Slot 4":
                if (numControllers >= 3)
                {
                    // Display the "Press A to join" message and prepare for displaying the characters in that slot if that player joins
                    message.transform.position = new Vector2(transform.position.x, gameObject.transform.position.y);
                }
                else
                {
                    // Do not display anything in that slot
                    
                }
                break;
        }
    }

    void HandleCharacterDisplay()
    {
        if (currentPlayer != null) currentPlayer.transform.position = new Vector2(transform.position.x, transform.position.y - 1000);
        float x = Input.GetAxisRaw("Horizontal");
        if (!onDown && x > 0)
        {
            onDown = true;
            iterator++;
            sounds.play(0);
        }
        else if (!onDown && x < 0)
        {
            onDown = true;
            iterator--;
            sounds.play(0);
        }
        else if (x == 0) onDown = false;
        if (iterator > 3) iterator = 0;
        else if (iterator < 0) iterator = 3;
        prefabs[iterator].transform.position = transform.position;
        currentPlayer = prefabs[iterator];
    }
}
