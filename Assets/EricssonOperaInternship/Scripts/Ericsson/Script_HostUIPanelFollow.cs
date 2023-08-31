using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_HostUIPanelFollow : MonoBehaviour
{

    public GameObject CameraObject;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float step;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = CameraObject.transform.position + new Vector3(-0.2f, -0.1f, 0.7f);
        step = 8.0f * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        step = 8.0f * Time.deltaTime;

        targetPosition = CameraObject.transform.position + new Vector3(-0.3f, -0.5f, 0.2f);
        targetRotation = Quaternion.LookRotation(transform.position - CameraObject.transform.position);

        transform.position = Vector3.Lerp(transform.position, targetPosition, step);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    }


}