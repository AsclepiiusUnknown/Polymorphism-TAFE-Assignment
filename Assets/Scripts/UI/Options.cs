using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    public GameObject predator;
    public GameObject prey;

    public GameObject explanation;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Predator(bool isPredator)
    {
        isPredator = !isPredator;

        predator.SetActive(!isPredator);
    }

    public void Prey(bool isPrey)
    {
        isPrey = !isPrey;

        prey.SetActive(!isPrey);
    }

    public void ExplanationShow(bool isShowing)
    {
        isShowing = !isShowing;

        if (!isShowing)
        {
            explanation.SetActive(true);
        }
        else
        {
            explanation.SetActive(false);
        }
    }
}
