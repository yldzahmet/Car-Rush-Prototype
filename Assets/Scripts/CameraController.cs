using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Quaternion startOrientation;
    public Vector3 targetPos;
    public float camRotX = 60;
    public GameObject trucks;
    Vector3 offsetVector;
    // Use this for initialization
    private void Start()
    {
        startOrientation = transform.rotation;
        offsetVector = transform.position - trucks.transform.position;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!FinishFlag.succesfulFinish)
            FollowRegular();
        else
            MoveUpperLook();
    }
    public void MoveUpperLook()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 15 * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(camRotX, 0, 0), 0.6f * Time.deltaTime);
    }
    public void FollowRegular()
    {
        transform.position = trucks.transform.position + offsetVector;
    }
    public void RestartOrientation()
    {
        transform.rotation = startOrientation;
    }
}
