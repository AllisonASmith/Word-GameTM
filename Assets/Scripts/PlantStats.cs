using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantStats : MonoBehaviour
{
    float scale = .5f;
    [SerializeField]
    float thirstRate, hungerRate, pestAttraction; // constant rates
    [SerializeField]
    float maxTime, maxThirst, maxHunger, maxPest; // runtime values, used in UI
    public float currentTime, currentThirst, currentHunger, pestTime; // runtime values, used in UI
    bool hasPests; // does this plant have pests
    string state; // determines the current state of the plant (Soil, Wilted, Plant___[Seed, Sapling, etc])
    void Start()
    {
        currentThirst = maxThirst;
        currentHunger = maxHunger;
        currentTime = maxTime;
        pestTime = 0;
        state = "Soil";
    }
    void Update()
    {
        if (!state.Equals("Soil") && (currentHunger < 0 || currentThirst < 0 || pestTime > maxPest))
        {
            // dying
            state = "Wilted";
        }
        else if(state.Contains("Plant"))
        {
            currentThirst -= thirstRate * Time.deltaTime;
            currentHunger -= hungerRate * Time.deltaTime;
            currentTime -= Time.deltaTime;
            if (hasPests) pestTime += pestAttraction * Time.deltaTime;
            else if(Random.Range(0, 10) == 1)
            {
                hasPests = true;
            }
            
        }
    }
    public void UseManager(float amount, int index, GameObject holding)
    {
        if (index == 0) Water(amount, holding);
        else if (index == 1) Feed(amount);
        else if (index == 2) Pesticide();
    }
    public void Water(float amount, GameObject wateringCan)
    {
        if (wateringCan.GetComponent<ItemStats>().isEmpty) return;
        currentThirst += amount;
        if (currentThirst > maxThirst) currentThirst = maxThirst;
    }
    public void Feed(float amount) 
    {
        currentHunger += amount;
        if (currentHunger > maxHunger) currentHunger = maxHunger;
    }
    public void Pesticide() 
    {
        hasPests = false;
        pestTime = 0;
    }
    public void Sow(float[] stats, GameObject holding) 
    {
        thirstRate = stats[0];
        hungerRate = stats[1];
        pestAttraction = stats[2];
        state = "PlantSeed";
        name = holding.name;
    }
    public string Harvest() 
    {
        if (!state.Contains("Done")) return "X";
        state = "Plot";
        name = "Soil";
        return gameObject.name;
    }
}
