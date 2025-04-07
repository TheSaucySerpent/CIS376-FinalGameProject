using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform tr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tr = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if(Input.GetKey(KeyCode.Q)){
            tr.Rotate(0.0f, -1.0f, 0.0f, Space.Self);
        }
        if(Input.GetKey(KeyCode.E)){
            tr.Rotate(0.0f, 1.0f, 0.0f, Space.Self);
        }

        tr.position += tr.forward * v * 0.5f;
    }
}
