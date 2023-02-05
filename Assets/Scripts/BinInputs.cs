using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinInputs : MonoBehaviour
{
    public ContractManager contractUpdate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            ItemStats temp = collision.gameObject.GetComponent<ItemStats>();
            if(temp.type == 5)
            {
                contractUpdate.addItem(temp.seedName);
            }
            Destroy(collision.gameObject);
        }
    }
}
