using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractManager : MonoBehaviour
{
    List<Contract> activeContracts = new List<Contract>(); // current contracts offered to the player
    public GameObject canvas;
    public Vector2 position; // base position for element 0
    public int offset; // offset based on how many elements right the contract is
    [SerializeField]
    string[] keywordRef;
    public static string[] keywords; // what types of crops there are
    int numContracts = 1; // how many contracts can be given to the player
    int numItems = 4; // how many items can be made per contract
    float contractTime = 10; // base value per item
    float difficulty; // scale based on score

    public static int money; // score
    public static bool catMultiplier; // tldr when the cat throws a harvestable this goes off because... yea cutting corners. Cat ability
    public Text tex; // score UI
    private void Start()
    {
        keywords = keywordRef;
    }
    void Update()
    {
        if (activeContracts.Count < numContracts) addContract();
        for (int i = 0; i < activeContracts.Count; i++)
        {
            activeContracts[i].countDown();
        }
        tex.text = "" + money;
        difficulty = Mathf.Sqrt(money);
    }
    public void addContract()
    {
        numItems += 1 + (int)difficulty;
        int numEntries = numItems > 2 ? Random.Range(1, 4) : Random.Range(1, numItems + 1);
        activeContracts.Add(new Contract(numItems, numEntries, contractTime, canvas, position, offset, activeContracts.Count));
    }
    public void addItem(string name)
    {
        // when an item is thrown into the container, remove the first item of this instance
        foreach(Contract i in activeContracts)
        {
            if (i.CheckItem(name))
            {
                int temp = i.isEmpty();
                if (temp > -1)
                {
                    activeContracts.Remove(i);
                    money += temp;
                }
                return;
            }
        }
    }
}
public class Contract : MonoBehaviour
{
    List<string> itemName; // type of item needed
    List<int> itemReq; // how many items of this type are needed
    float timer; // how long this contract has
    float maxTime; // for UI
    GameObject UI; // contract UI element
    int difficulty; // combination of itemname and itemreq
    public Contract(int numItems, int numEntries, float time, GameObject canvas, Vector2 position, int offset, int iterator)
    {
        itemName = new List<string>();
        itemReq = new List<int>();
        timer = time * numItems;
        UI = (GameObject)Instantiate(Resources.Load("Contract"));
        UI.transform.parent = canvas.transform;
        UI.transform.localPosition = new Vector2(position.x + (offset * iterator), position.y);
        maxTime = timer;
        difficulty = numEntries * numItems;
        // makes numEntries with numItems total
        for (int i = 0; i < numEntries && numItems > 0; i++)
        {
            itemName.Add(ContractManager.keywords[Random.Range(0, ContractManager.keywords.Length)]);
            int temp = Random.Range(1, numItems);
            numItems -= temp;
            itemReq.Add(temp);
        }
        updateUI();
    }
    public bool CheckItem(string name)
    {
        // look for item name and remove one on hit
        for (int i = 0; i < itemName.Count; i++)
        {
            if (name.Equals(itemName[i]))
            {
                itemReq[i]--;
                updateUI();
                if (itemReq[i] <= 0) 
                {
                    itemName.RemoveAt(i);
                    itemReq.RemoveAt(i);
                    updateUI();
                    return true;
                }
            }
        }
        return false;
    }
    public void updateUI()
    {
        int a = 0;
        foreach (Text i in UI.GetComponentsInChildren<Text>())
        {
            if (itemReq.Count > a)
            {
                i.GetComponentInParent<Image>().sprite = Resources.Load<Sprite>(itemName[a]);
                i.text = "" + itemReq[a];
            }
            else
            {
                i.GetComponentInParent<Image>().enabled = false;
                i.enabled = false;
            }
            a++;
        }
    }
    public void countDown()
    {
        timer -= Time.deltaTime;
        UI.GetComponentInChildren<Image>().fillAmount = Mathf.Clamp(timer / maxTime, 0, 1f);
    }
    public int isEmpty() 
    {
        if (itemName.Count == 0)
        {
            Destroy(UI);
            return difficulty; 
        }
        else return -1;
    }
}
