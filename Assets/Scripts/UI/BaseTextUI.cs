using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTextUI : MonoBehaviour
{
    private Individual individual;
    private Text text;

    private void Awake()
    {
        individual = GameObject.FindGameObjectWithTag("Base").GetComponent<Individual>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "基地血量"+individual.health.ToString();
    }
}