using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public Image backroundImage;
    public Text uiText;

    public Color32 yellow;
    public Color32 red;

    [SerializeField]
    private int evaluateNumber;
    private char signChar;

    enum ProcessType { divide, multiply, subtraction, addition }
    ProcessType equationType;

    private void Start()
    {
        GenerateProcessType();
    }
    public void GenerateProcessType()
    {
        int n =Random.Range(0, 100);
        if (n < 35)
            equationType = ProcessType.addition;
        else if (n >= 35 && n < 60)
            equationType = ProcessType.multiply;
        else if (n >= 60 && n < 80)
            equationType = ProcessType.subtraction;
        else if (n >= 80 && n < 100)
            equationType = ProcessType.divide;

        switch (equationType)
        {
            case ProcessType.divide:
                evaluateNumber = Random.Range(2, 4);
                backroundImage.color = red;
                signChar = '/';
                break;
            case ProcessType.multiply:
                evaluateNumber = Random.Range(2, 4);
                backroundImage.color = yellow;
                signChar = 'x';
                break;
            case ProcessType.subtraction:
                evaluateNumber = Random.Range(2, 7);
                backroundImage.color = red;
                signChar = '-';
                break;
            case ProcessType.addition:
                evaluateNumber = Random.Range(2, 7);
                backroundImage.color = yellow;
                signChar = '+';
                break;
            default:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        int lcc = ShuffleCars.leftCarCount;
        int rcc = other.transform.root.GetComponent<ShuffleCars>().RightCarCount;
        string tag = other.tag;
        if (other.CompareTag("leftTruck") || other.CompareTag("rightTruck"))
        {
            int number = evaluateNumber;
            switch (equationType)
            {
                case ProcessType.divide:
                    if (tag == "leftTruck")
                        number = lcc - lcc / evaluateNumber;
                    else
                        number = rcc - rcc / evaluateNumber;
                    other.transform.root.GetComponent<SpawnManager>().RemoveCarFromList(tag, number);
                    break;
                case ProcessType.multiply:
                    if (tag == "leftTruck")
                        number = lcc * evaluateNumber - lcc;
                    else
                        number = rcc * evaluateNumber - rcc;
                    other.transform.root.GetComponent<SpawnManager>().CreatePathWithCar(tag, number);
                    break;
                case ProcessType.subtraction:
                    other.transform.root.GetComponent<SpawnManager>().RemoveCarFromList(tag, number);
                    break;
                case ProcessType.addition:
                    other.transform.root.GetComponent<SpawnManager>().CreatePathWithCar(tag, number);
                    break;
                default:
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        uiText.text = string.Concat(signChar.ToString(), " ", System.Convert.ToString(evaluateNumber));
    }
}
