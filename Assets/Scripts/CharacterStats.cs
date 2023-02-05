using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    public int[] cooldowns; // what each time takes, load this by prefabs
    // [water, feeding, pesticide, selling, planting, harvesting]
    public int[] rates; // how effective each action is
    public CharacterSelect[] charRef; // temporary
    public static string[] charList; // tells which player is who, -1 if the player isn't active
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name.Equals("Start"))
        {
            DontDestroyOnLoad(this);
        }
    }
    private void Update()
    {
        // temp
        if (Input.GetKeyDown(KeyCode.E) && SceneManager.GetActiveScene().name.Equals("Start"))
        {
            for (int i = 0; i < 4; i++)
            {
                if (i >= charRef.Length) charList[i] = "";
                else charList[i] = charRef[i].currentPlayer.name;
            }
            SceneManager.LoadScene("Main");
        }
    }
}
