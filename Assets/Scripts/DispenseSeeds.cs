using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenseSeeds : MonoBehaviour
{
    GameObject go = null;

    [SerializeField]
    string seedName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (go == null)
        {
            go = (GameObject)Instantiate(Resources.Load("Prefabs/" + seedName));
            go.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
        }
    }
}
