using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTarget : MonoBehaviour
{
    public GameObject target = null;
    // Update is called once per frame
    void Update()
    {
        if (target != null) transform.position = Vector2.Lerp(transform.position, target.transform.position, .125f);
        else transform.position = Vector2.Lerp(transform.position, transform.parent.transform.position, .125f);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }
    public void setTarget(GameObject obj)
    {
        target = obj;
    }
}
