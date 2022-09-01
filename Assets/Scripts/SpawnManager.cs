using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    ShuffleCars shuffleCars;
    public ScoreManager scoreManager;
    public GameObject carPrefab;
    public Transform carParent;
    public float yOffset = 1.5f;
    public int addNumber;
    public int removeNumber;

    private void Start()
    {
        shuffleCars = GetComponent<ShuffleCars>();
    }

    public void CreatePathWithCar(string truckTag, int carNumber)
    {
        float xMove, yMove;
        int leftCars = ShuffleCars.leftCarCount;
        int rightCars = shuffleCars.carList.Count - ShuffleCars.leftCarCount;

        for (int i = 0; i < carNumber; i++) // loop new car number times
        {
            yMove = Mathf.Abs(carNumber - i - 1) * yOffset;
            var carObj = Instantiate(carPrefab, carParent);    // instantiate carPrefab 

            GameObject car = carObj.transform.GetChild(0).gameObject;
            PathHandler pathHandler = carObj.transform.GetChild(1).gameObject.GetComponent<PathHandler>();

            // Get anchors
            Transform[] leftAnchor = { pathHandler.controlPoints[0], pathHandler.controlPoints[1] };
            Transform[] rightAnchor = { pathHandler.controlPoints[2], pathHandler.controlPoints[3] };

            // Set locals according to side
            if (truckTag == "leftTruck")
            {
                if (shuffleCars.slidingSide == ShuffleCars.SlidingSide.Right)
                {
                    shuffleCars.indexToRignt += 1;
                }   
                xMove = -4.5f;
                car.GetComponent<Follower>().SideName = Follower.Side.Left;
                car.transform.localPosition += new Vector3(xMove, leftCars * yOffset, 0); // put into true position

                leftAnchor[0].transform.localPosition = car.transform.localPosition;
                rightAnchor[1].transform.localPosition += new Vector3(0, rightCars * yOffset + yMove, 0);

                leftCars++;
                shuffleCars.carList.Insert(ShuffleCars.leftCarCount + i, carObj); // add to list
            }
            else
            {
                if (shuffleCars.slidingSide == ShuffleCars.SlidingSide.Left) 
                { 
                    // cars count already increased
                }
                xMove = 4.5f;
                car.GetComponent<Follower>().SideName = Follower.Side.Right;
                car.transform.localPosition += new Vector3(xMove, rightCars * yOffset, 0); // put into true position

                leftAnchor[0].transform.localPosition += new Vector3(0, leftCars * yOffset + yMove, 0);
                rightAnchor[1].transform.localPosition = car.transform.localPosition; 

                rightCars++;

                shuffleCars.carList.Insert(ShuffleCars.leftCarCount , carObj); // add to list
            }
            scoreManager.EditScore(carNumber);
        }
        EditPathHeight(carNumber); // pass positive values for add cars
        ShuffleCars.leftCarCount = leftCars;
        Debug.Log("lcc: " + ShuffleCars.leftCarCount);

    }
    /// <summary>
    /// Removes multiple objects from list
    /// </summary>
    /// <param name="carNumber">Number of car need to extract</param>
    /// <param name="tag_TruckOrCarSide">which side will the cars be remove from</param>
    public void RemoveCarFromList(string tag_TruckOrCarSide, int carNumber)
    {
        int removedCars = 0;
        int index;
        int lastIndex;

        if (tag_TruckOrCarSide == "leftTruck" || tag_TruckOrCarSide == Follower.Side.Left.ToString()) // standing left side
        {
            if (carNumber > ShuffleCars.leftCarCount)
                carNumber = ShuffleCars.leftCarCount;

            index = ShuffleCars.leftCarCount - carNumber;
            index = Mathf.Clamp(index, 0, ShuffleCars.leftCarCount - 1);

            lastIndex = ShuffleCars.leftCarCount;
        }
        else if (tag_TruckOrCarSide == "rightTruck" || tag_TruckOrCarSide == Follower.Side.Right.ToString()) // standing right side
        {
            if (carNumber > shuffleCars.RightCarCount)
                carNumber = shuffleCars.RightCarCount;

            index = ShuffleCars.leftCarCount;

            lastIndex = ShuffleCars.leftCarCount + carNumber; // ?????
            lastIndex = Mathf.Clamp(lastIndex, ShuffleCars.leftCarCount, shuffleCars.carList.Count);
        }
        else return;

        for (int i = index; i < lastIndex; i++)
        {
            GameObject car = shuffleCars.carList[index];
            shuffleCars.carList.Remove(car);
            car.transform.GetChild(0).GetComponent<Follower>().isMoving = false;
            car.transform.GetChild(0).GetComponent<Follower>().SpreadAround();

            removedCars++;
        }

        if (tag_TruckOrCarSide == "leftTruck" || tag_TruckOrCarSide == Follower.Side.Left.ToString()) // standing left side
        {
            ShuffleCars.leftCarCount -= removedCars;
        }

        scoreManager.EditScore(-removedCars);
        EditPathHeight(removedCars * -1);// pass negative value for remove cars
    }

    /// <summary>
    /// Removes single object from list
    /// </summary>
    /// <param name="car"></param>
    public void RemoveCarFromList(GameObject car, Follower.Side side)
    {
        shuffleCars.carList.Remove(car);
        Destroy(car);
        if (side == Follower.Side.Left)
            ShuffleCars.leftCarCount -= 1;

        EditPathHeight(-1);// pass negative value for remove cars
        scoreManager.EditScore(-1);
    }


    public void EditPathHeight(int carNumber)
    {
        Vector3 offsetVector = new Vector3(0, yOffset * carNumber, 0);
        int leftCarCount = ShuffleCars.leftCarCount;

        for (int i = 0; i < shuffleCars.carList.Count; i++)
        {
            if ( i >= leftCarCount && i < leftCarCount + Mathf.Abs(carNumber) && carNumber > 0)
            {
                continue;
            }
            else
            {
                PathHandler pathHandler = shuffleCars.carList[i].transform.GetChild(1).gameObject.GetComponent<PathHandler>();

                if (i < leftCarCount)
                {
                    pathHandler.controlPoints[3].transform.position += offsetVector;
                }

                else
                {
                    pathHandler.controlPoints[0].transform.position += offsetVector;
                }
            }
        }
        
    }

}
