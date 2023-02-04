using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public bool[] inventory; // this might be a string or something later (or a dictionary to better manage data), right now it's a bool assuming 0 = watering can 1 = ... yea
    public int[] cooldowns; // what each time takes, load this by prefabs
    public string currentAction; // what the player is doing, might need to translate to animator bools, or don't use this

}
