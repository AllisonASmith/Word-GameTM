using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target; // the focus for the camera. Will most often be the player
    public bool isSmoothed; // true if the camera isn't directly following a player or enemy (no constant motion)
    public static bool shake; // for shaking the camera
    public float shakeTime; // shake duration
    [Range(1, 10)]
    public int magnitude; // shake intensity
    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKey(KeyCode.F)) shake = true;
        // camera is shaking
        float x = 0;
        float y = 0;
        if (shake) {
            shakeTime = .125f;
            shake = false;
        }
        //shaking
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            x = Random.Range(-.02f, .02f) * magnitude;
            y = Random.Range(-.02f, .02f) * magnitude;
        }
        transform.position = isSmoothed ? Vector3.Lerp(transform.position, new Vector3(target.transform.position.x + x, target.transform.position.y, -10), .025f) : new Vector3(target.transform.position.x + x, target.transform.position.y, -10);
    }
}
