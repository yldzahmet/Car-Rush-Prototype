using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private FinishFlag finishManager;
    public enum Side { Left, Right };
    public Side SideName;
    public PathHandler path;
    public Transform[] controlPoints;
    private Animator animator;
    private Transform parkArea;
    bool isParking = false;
    [SerializeField]
    public static float speedMultipler = 2f;
    [SerializeField]
    private bool allowToDestroy = true;
    float currentPos = 0;
    [SerializeField]
    float finalDest;
    float movementStep = 1f;
    internal bool isMoving = false;

    private void Awake()
    {
        finishManager = GameObject.FindWithTag("finish").GetComponent<FinishFlag>();
    }
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        animator.Play("Base Layer.bounce", 0, 0);
        controlPoints = path.controlPoints;

        if (SideName == Side.Left)
            currentPos = 0;
        else
            currentPos = 1;
    }


    public void MoveGameObjectOnPath()
    {
        currentPos = Mathf.MoveTowards(currentPos, finalDest, movementStep * speedMultipler * Time.deltaTime);
        currentPos = Mathf.Clamp(currentPos, 0, 1);

        transform.position =  Mathf.Pow(1 - currentPos, 3) * controlPoints[0].position +
                3 * Mathf.Pow(1 - currentPos, 2) * currentPos * controlPoints[1].position +
                3 * (1 - currentPos) * Mathf.Pow(currentPos, 2) * controlPoints[2].position +
                Mathf.Pow(currentPos, 3) * controlPoints[3].position;

        // Stop to move
        if (currentPos == finalDest)
        {
            animator.Play("Base Layer.bounce", 0, 0);
            isMoving = false;
        }
    }

    // Triggers move in Update
    public bool MoveCarToRight()
    {
        if (currentPos == 1)
            return false;
        else
        {
            isMoving = true;
            finalDest = 1;
            ShuffleCars.leftCarCount -= 1;
            print("lcc: " + ShuffleCars.leftCarCount);
            SideName = Side.Right;
            animator.Play("Base Layer.SpinToRight", 0, 0);
            return true;
        }
    }
    // Triggers move in Update
    public bool MoveCarToLeft()
    {
        if (currentPos == 0)
            return false;
        else
        {
            isMoving = true;
            finalDest = 0;
            ShuffleCars.leftCarCount += 1;
            SideName = Side.Left;
            animator.Play("Base Layer.SpinToLeft", 0, 0);
            return true;
        }
    }

    public void DestroySelf()
    {
        if (allowToDestroy)
        {
            transform.root.GetComponent<SpawnManager>().RemoveCarFromList(transform.parent.gameObject, SideName);
            allowToDestroy = false;
        }
    }

    public void DestroyCars(int carNumber, string side)
    {
        if (allowToDestroy)
        {
            transform.root.GetComponent<SpawnManager>().RemoveCarFromList(side, carNumber);
            allowToDestroy = false;
        }
    }

    public void SpreadAround()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float xRange = Random.Range(-0.5f, 0.5f);
        float yRange = Random.Range(0, 10);
        float zRange = Random.Range(-10, 0);
        rb.isKinematic = false;
        rb.AddForce(new Vector3(xRange, yRange, zRange)* 10, ForceMode.VelocityChange);

        Destroy(transform.parent.gameObject, 1f);
    }

    public void PrepareToParking(bool isParking, Transform parkArea)
    {
        int indexOfParkArea = finishManager.parkAreas.IndexOf(parkArea);

        switch (indexOfParkArea)
        {
            case 0:
            case 1:
                finishManager.scoreManager.EditScore(2);
                break;
            case 2:
            case 3:
                finishManager.scoreManager.EditScore(4);
                break;
            case 4:
            case 5:
                finishManager.scoreManager.EditScore(6);
                break;
            case 6:
            case 7:
                finishManager.scoreManager.EditScore(8);
                break;
            case 8:
            case 9:
                finishManager.scoreManager.EditScore(10);
                break;
            case 10:
            case 11:
                finishManager.scoreManager.EditScore(12);
                break;
            case 12:
            case 13:
                finishManager.scoreManager.EditScore(16);
                break;
            case 14:
            case 15:
                finishManager.scoreManager.EditScore(20);
                break;
            case 16:
            case 17:
                finishManager.scoreManager.EditScore(24);
                break;
            case 18:
                finishManager.scoreManager.EditScore(100);
                break;
            default:
                break;
        }

        if (indexOfParkArea < 18)
            this.parkArea = parkArea;
        else
            this.parkArea = finishManager.parkAreas[18];
        
        this.isParking = isParking;
    }

    public void GoToParkArea()
    {
        transform.position = Vector3.MoveTowards(transform.position, parkArea.position, 50 * Time.deltaTime);
    }

    void Update()
    {
        if (FinishFlag.pressedStart)
        {
            if (isMoving)
                MoveGameObjectOnPath();
            if (isParking)
                GoToParkArea();
        }
    }
}

