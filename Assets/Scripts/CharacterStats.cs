using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    public int[] cooldowns; // what each time takes, load this by prefabs
    // [water, feeding, pesticide, selling, planting, harvesting]
    public int[] rates; // how effective each action is
    [SerializeField]
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
            Debug.Log("Hn");
            Debug.Log("char: " + charRef[0].currentPlayer.name);
            charList[0] = "a";//charRef[0].currentPlayer.name;
            Debug.Log("HONK");
            for (int i = 0; i < 2; i++)
            {
                if (i >= charRef.Length) charList[i] = "";
                else charList[i] = charRef[i].currentPlayer.name;
            }
            SceneManager.LoadScene("Main");
        }

        if (SceneManager.GetActiveScene().name.Equals("Start") && GameObject.Find("Slot 1").GetComponent<CharacterSelect>().canSelect == false && GameObject.Find("Slot 2").GetComponent<CharacterSelect>().canSelect == false)
        {
            for (int i = 0; i < 2; i++)
            {
                if (i >= charRef.Length) charList[i] = "";
                else charList[i] = charRef[i].currentPlayer.name;
            }
            SceneManager.LoadScene("Main");
        }
    }
}
