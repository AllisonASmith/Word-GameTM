using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    bool onDown; // just onDown for axis inputs
    int iterator; // current character for this slice
    public GameObject[] prefabs; // stores the UI players
    public GameObject currentPlayer; // to store for next scene and manage where the prefabs go 

    public SoundEffectPlayer sounds;
    void Update()
    {
        if (currentPlayer != null) currentPlayer.transform.position = new Vector2(transform.position.x, transform.position.y - 1000);
        float x = Input.GetAxisRaw("Horizontal");
        if(!onDown && x > 0)
        {
            onDown = true;
            iterator--;
            sounds.play(0);
        }
        else if (!onDown && x < 0)
        {
            onDown = true;
            iterator++;
            sounds.play(0);
        }
        else if (x == 0) onDown = false;
        if (iterator > 3) iterator = 0;
        else if (iterator < 0) iterator = 3;
        prefabs[iterator].transform.position = transform.position;
        currentPlayer = prefabs[iterator];
    }
}
