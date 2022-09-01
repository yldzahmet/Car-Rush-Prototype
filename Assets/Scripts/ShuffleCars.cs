using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleCars : MonoBehaviour
{

    public List<GameObject> carList = new List<GameObject>();
    public static int leftCarCount = 2; // need to set up at game start
    internal enum SlidingSide { Left, Right};
    internal SlidingSide slidingSide;
    private bool coroutineRunning = false;
    public float waitingTime;
    public int RightCarCount { get => (carList.Count - leftCarCount) ;}

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!coroutineRunning)
            {
                StartCoroutine(MoveCarToRight()); 
                print("key Down");
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            coroutineRunning = false;
            StopAllCoroutines();
            print("key Up");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!coroutineRunning)
            {
                StartCoroutine(MoveCarToLeft());
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            coroutineRunning = false;
            StopAllCoroutines();
        }
    }
    internal int indexToRignt = 0;
    IEnumerator MoveCarToRight()
    {
        slidingSide = SlidingSide.Right;
        coroutineRunning = true;
        for (indexToRignt = leftCarCount - 1; indexToRignt >= 0; indexToRignt--)
        {
            Debug.Log("to right index:" + indexToRignt);
            carList[indexToRignt].GetComponentInChildren<Follower>().MoveCarToRight();
            yield return new WaitForSeconds(waitingTime);
        }
    }

    internal int indexToLeft = 0;
    IEnumerator MoveCarToLeft()
    {
        slidingSide = SlidingSide.Left;
        coroutineRunning = true;
        for (indexToLeft = leftCarCount; indexToLeft < carList.Count; indexToLeft++)
        {
            Debug.Log("to left index:" + indexToLeft);
            carList[indexToLeft].GetComponentInChildren<Follower>().MoveCarToLeft();
            yield return new WaitForSeconds(waitingTime);
        }
    }
}
