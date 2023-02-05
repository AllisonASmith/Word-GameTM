using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public float[] seedStats; // 0 = thirst, 1 = hunger, 2 = pest rate
    public string seedName; // seed name
    public bool isEmpty; // might change to number later, if the watering can has water
    public int type; // [water, feeding, pesticide, selling, tilling, planting, harvesting]
    public bool isStatic; // can the item be moved
    
    public SoundEffectPlayer sounds; // plays thud

    [SerializeField]
    float offset = 1;
    [SerializeField]
    float magnitude = 5;
    Rigidbody2D rb;
    private void Start()
    {
        if(!isStatic) rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (transform.parent != null && transform.parent.name.Equals("Player"))
        {
            transform.position = new Vector2(transform.parent.position.x, transform.parent.position.y + offset);
        }
    }
    public void Throw(float direction) 
    {
        transform.SetParent(null);
        rb.gravityScale = 1;
        float launchPos = -1;
        if (direction > 0) launchPos = 1;
        rb.AddForce(new Vector2(launchPos, 1) * magnitude);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        sounds.play(3);
    }
}
