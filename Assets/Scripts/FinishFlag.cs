using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishFlag : MonoBehaviour
{
    public static bool pressedStart = false;
    public static bool succesfulFinish = false;
    private int parkCounter = 0;
    public float waitinTime = 0.35f;
    public string congrats;
    public string failedText;
    public Text infoText;
    public GameObject tryAgainCanvas;
    public GameObject StartScreenCanvas;
    public List<Transform> parkAreas;
    public PlayerController PlayerController;
    public ShuffleCars shuffleCars;
    public SpawnManager spawnManager;
    public PlayerController playerController;
    public ScoreManager scoreManager;
    public CameraController cameraController;
    public GameObject[] particles;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("leftTruck"))
        {
            Debug.LogWarning("triggered");
            PlayerController.speed -= 5;
            SuccesCompleted();
        }
    }

    public void StartGameCycle()
    {
        pressedStart = true;
        ScoreManager.updateScore = true;
        PlayerController.allowToGo = true;
    }

    public void SuccesCompleted()
    {
        succesfulFinish = true;
        PlayerController.StopDelayed();
        ShowInfoCanvasDelayed(congrats);
        StartCoroutine(StartParkingIEnum());
        ShowConfetties();
    }

    IEnumerator StartParkingIEnum()
    {
        int lcc = ShuffleCars.leftCarCount;
        int rcc = shuffleCars.RightCarCount;
        int c = shuffleCars.carList.Count;
        int n;
        int l;

        if (lcc < rcc)
        {
            n = c - 2 * lcc;
            l = lcc + n;
            for (int i = lcc; i < l; i++)
            {
                Follower follower = shuffleCars.carList[i].transform.GetChild(0).GetComponent<Follower>();
                follower.PrepareToParking(true, parkAreas[CountParking()]);
                yield return new WaitForSeconds(waitinTime);
            }
            for (int i = lcc - 1, j = lcc + n; i >= 0 && j < c; i--, j++)
            {
                shuffleCars.carList[i].transform.GetChild(0).GetComponent<Follower>().PrepareToParking(true, parkAreas[CountParking()]);
                shuffleCars.carList[j].transform.GetChild(0).GetComponent<Follower>().PrepareToParking(true, parkAreas[CountParking()]);
                yield return new WaitForSeconds(waitinTime);
            }
            yield break;
        }
        else if (lcc > rcc)
        {
            n = c - 2 * rcc;
            l = lcc - n;
            for (int i = lcc - 1; i >= l; i--)
            {
                Follower follower = shuffleCars.carList[i].transform.GetChild(0).GetComponent<Follower>();
                follower.PrepareToParking(true, parkAreas[CountParking()]);
                yield return new WaitForSeconds(waitinTime);
            }
            for (int i = rcc - 1, j = rcc + n; i >= 0 && j < c; i--, j++)
            {
                shuffleCars.carList[i].transform.GetChild(0).GetComponent<Follower>().PrepareToParking(true, parkAreas[CountParking()]);
                shuffleCars.carList[j].transform.GetChild(0).GetComponent<Follower>().PrepareToParking(true, parkAreas[CountParking()]);
                yield return new WaitForSeconds(waitinTime);
            }
            yield break;
        }

        for (int i = lcc - 1, j = lcc; i >= 0 && j < c; i--, j++)
        {
            shuffleCars.carList[i].transform.GetChild(0).GetComponent<Follower>().PrepareToParking(true, parkAreas[CountParking()]);
            shuffleCars.carList[j].transform.GetChild(0).GetComponent<Follower>().PrepareToParking(true, parkAreas[CountParking()]);
            yield return new WaitForSeconds(waitinTime);
        }
    }

    public int CountParking()
    {
        if (parkCounter > 18)
            return 18;
        else
            return parkCounter++;
    }

    public void ShowConfetties()
    {
        Instantiate(particles[0], transform.position + new Vector3(0, 25, 10), particles[0].transform.rotation);
        Instantiate(particles[1], transform.position + new Vector3(0, 25, 10), particles[1].transform.rotation);
    }

    public void RestartGame()
    {
        GameObject go;
        while (shuffleCars.carList.Count > 0)
        {
            go = shuffleCars.carList[0];
            shuffleCars.carList.Remove(go);
            Destroy(go);    
        }

        ShuffleCars.leftCarCount = 0;
        
        succesfulFinish = false;
        pressedStart = false;

        spawnManager.CreatePathWithCar("leftTruck", 2);
        spawnManager.CreatePathWithCar("rightTruck", 2);
        playerController.RestartPos();
        cameraController.RestartOrientation();

        parkCounter = 0;
        ScoreManager.score = 400;
        StartScreenCanvas.SetActive(true);
    }

    public void Failed()
    {
        pressedStart = false;
        ShowInfoCanvas(failedText);
    }

    public void ShowInfoCanvasDelayed(string infoText)
    {
        this.infoText.text = infoText;
        StartCoroutine(ShowtryAgainCanvasEnum());
    }
    IEnumerator ShowtryAgainCanvasEnum()
    {
        yield return new WaitForSeconds(5);
        tryAgainCanvas.SetActive(true);
        yield return null;
    }

    public void ShowInfoCanvas(string infoText)
    {
        this.infoText.text = infoText;
        tryAgainCanvas.SetActive(true);
    }
}
