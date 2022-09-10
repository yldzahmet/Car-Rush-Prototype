using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum Swipes { None, Up, Down, Left, Right };
    public static Swipes swipeDirection;
    public float minSwipeLength;
    Vector2 firstPressPos;
    Vector2 currentSwipe;

    private Vector3 carManagerStartPos;
    public static bool allowToGo = false;
    public float speed;

    private ShuffleCars shuffleCars;
    private void Start()
    {
        shuffleCars = GetComponent<ShuffleCars>();
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
            DetectSwipe();
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

    public void DetectSwipe()
    {
        
        // First touch
        if (Input.GetMouseButtonDown(0))
        {
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (Input.GetMouseButton(0))
        {
            currentSwipe = (Vector2)Input.mousePosition - firstPressPos;

            // Swipe left
            if (currentSwipe.x < -minSwipeLength)
            {
                if (swipeDirection == Swipes.Right)
                {
                    ShuffleCars.coroutineRunning = false;
                    shuffleCars.StopCoroutines();

                }

                if (!ShuffleCars.coroutineRunning)
                {
                    swipeDirection = Swipes.Left;
                    shuffleCars.MoveCarsToLeft();
                }
            }
            // Swipe right
            else if (currentSwipe.x > minSwipeLength)
            {
                if (swipeDirection == Swipes.Left) 
                { 
                    ShuffleCars.coroutineRunning = false;
                    shuffleCars.StopCoroutines();
                }

                if (!ShuffleCars.coroutineRunning)
                {
                    swipeDirection = Swipes.Right;
                    shuffleCars.MoveCarsToRight();
                }
            }
        }
        // Release touch
        else if (Input.GetMouseButtonUp(0))
        {
            shuffleCars.StopCoroutines();
            ShuffleCars.coroutineRunning = false;
            swipeDirection = Swipes.None;
        }
    }
}
