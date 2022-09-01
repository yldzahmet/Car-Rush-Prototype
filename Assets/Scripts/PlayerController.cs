using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 carManagerStartPos;
    public static bool allowToGo = false;
    public float speed;
    private void Start()
    {
        carManagerStartPos = transform.position;
    }
    public void RestartPos()
    {
        transform.position = carManagerStartPos;
    }
    // Update is called once per frame
    void Update()
    {
        if (FinishFlag.pressedStart && allowToGo)
        {
            MoveTrucksForward();
        }
    }
    public void Stop()
    {
        allowToGo = false;
    }
    public void StopDelayed()
    {
        Invoke("Stop", speed / 4);
    }
    public void MoveTrucksForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
