using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int[] cooldowns; // what each time takes, load this by prefabs
    // [water, feeding, pesticide, selling, planting, harvesting]
    public int[] rates; // how effective each action is
    //public string currentAction; // what the player is doing, might need to translate to animator bools, or don't use this

}
