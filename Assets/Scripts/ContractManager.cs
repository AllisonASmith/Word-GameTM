using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractManager : MonoBehaviour
{
    List<Contract> activeContracts = new List<Contract>(); // current contracts offered to the player
    public static string[] keywords; // what types of crops there are
    int numContracts = 1; // how many contracts can be given to the player
    int numItems = 1; // how many items can be made per contract
    float contractTime;
    public static int money; // score
    public Text tex; // score UI

    // Update is called once per frame
    void Update()
    {
        if (activeContracts.Count < numContracts) addContract();
        foreach (Contract i in activeContracts)
        {
            i.countDown();
        }
        tex.text = "" + money;
    }
    public void addContract()
    {
        int numEntries = numItems > 2 ? Random.Range(1, 4) : Random.Range(1, numItems + 1);
        activeContracts.Add(new Contract(numItems, numEntries, contractTime));
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
public class Contract
{
    List<string> itemName; // type of item needed
    List<int> itemReq; // how many items of this type are needed
    float timer; // how long this contract has
    int difficulty; // combination of itemname and itemreq
    public Contract(int numItems, int numEntries, float time)
    {
        timer = time;
        difficulty = numEntries * numItems;
        // makes numEntries with numItems total
        for (int i = 0; i < numEntries && numItems > 0; i++)
        {
            itemName.Add(ContractManager.keywords[Random.Range(0, ContractManager.keywords.Length)]);
            int temp = Random.Range(1, numItems);
            numItems -= temp;
        }
    }
    public bool CheckItem(string name)
    {
        // look for item name and remove one on hit
        for (int i = 0; i < itemName.Count; i++)
        {
            if (name.Equals(itemName[i]))
            {
                itemReq[i]--;
                if (itemReq[i] <= 0) 
                {
                    itemName.RemoveAt(i);
                    itemReq.RemoveAt(i);
                    return true;
                }
            }
        }
        return false;
    }
    public void countDown()
    {
        timer -= Time.deltaTime;
    }
    public int isEmpty() 
    {
        if (itemName.Count == 0) return difficulty;
        else return -1;
    }
}
