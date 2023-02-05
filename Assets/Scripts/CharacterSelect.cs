using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelect : MonoBehaviour
{
    bool onDown = false; // just onDown for axis inputs
    bool joined = false; // for checking if a controller player has joined the game
    bool canSelect = true;
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
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    sounds.play(1);
                    canSelect = false;
                }
                break;
            case "Slot 2":
                if (numControllers >= 1)
                {
                    // Display the "Press A to join" message and prepare for displaying the characters in that slot if that player joins
                    if (!joined) message.transform.position = new Vector2(transform.position.x, gameObject.transform.position.y);
                    if (Input.GetButtonDown("GP_Fire1"))
                    {
                        if (!joined)
                        {
                            joined = true;
                            message.transform.position = new Vector2(transform.position.x, transform.position.y + 800);
                            sounds.play(1);
                        }
                        else
                        {
                            sounds.play(1);
                            canSelect = false;
                        }
                    }
                    else
                    {
                        if (joined)
                        {
                            HandleCharacterDisplay();
                        }
                    }
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
        float x = 0;
        if (currentPlayer != null) currentPlayer.transform.position = new Vector2(transform.position.x, transform.position.y - 1000);



        if (gameObject.name == "Slot 1") x = Input.GetAxisRaw("Horizontal");
        else if (gameObject.name == "Slot 2") x = Gamepad.current.leftStick.ReadValue().x;

        if (!onDown && x > 0 && canSelect)
        {
            onDown = true;
            iterator++;
            sounds.play(0);
        }
        else if (!onDown && x < 0 && canSelect)
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
