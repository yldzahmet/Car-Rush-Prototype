using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("car"))
        {
            Follower follower = other.GetComponent<Follower>();
            if (follower.isMoving)
            {
                follower.DestroySelf();
            }

            int carIndex = other.transform.root.GetComponent<ShuffleCars>().carList.IndexOf(other.transform.parent.gameObject);
            int carNumber;
            Follower.Side side = other.GetComponent<Follower>().SideName;

            if (side == Follower.Side.Left)
                carNumber = ShuffleCars.leftCarCount - carIndex;
            else
                carNumber = carIndex - ShuffleCars.leftCarCount + 2;

            other.GetComponent<Follower>().DestroyCars(carNumber, side.ToString());
        }

    }
}
