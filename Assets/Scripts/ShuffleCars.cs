using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleCars : MonoBehaviour
{
    public List<GameObject> carList = new List<GameObject>();
    internal enum SlidingSide { Left, Right};
    internal SlidingSide slidingSide;
    public static int leftCarCount = 2; // need to set up at game start
    internal int indexToRignt = 0;
    internal int indexToLeft = 0;
    public float waitingTime;
    public static bool coroutineRunning = false;
    public int RightCarCount { get => (carList.Count - leftCarCount) ;}

    public void MoveCarsToLeft()
    {
        StartCoroutine(MoveCarsToLeft_Enum());
    }

    public void MoveCarsToRight()
    {
        StartCoroutine(MoveCarsToRight_Enum());
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }

    public IEnumerator MoveCarsToLeft_Enum()
    {
        slidingSide = SlidingSide.Left;
        coroutineRunning = true;
        for (indexToLeft = leftCarCount; indexToLeft < carList.Count; indexToLeft++)
        {
            carList[indexToLeft].GetComponentInChildren<Follower>().MoveCarToLeft();
            yield return new WaitForSeconds(waitingTime);
        }
    }
    public IEnumerator MoveCarsToRight_Enum()
    {
        slidingSide = SlidingSide.Right;
        coroutineRunning = true;
        for (indexToRignt = leftCarCount - 1; indexToRignt >= 0; indexToRignt--)
        {
            carList[indexToRignt].GetComponentInChildren<Follower>().MoveCarToRight();
            yield return new WaitForSeconds(waitingTime);
        }
    }
    
}
