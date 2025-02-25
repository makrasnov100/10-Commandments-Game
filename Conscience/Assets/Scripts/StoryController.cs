﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{

    //Content
    public List<string> goodChoises = new List<string>()
    {
        "Make a clay statue of depicting Jesus and put it in your car's cup holder for good luck.",
        "Go to a Christian church for some spiritual leadership.",
        "Stay silent. Like normal people.",
        "Read the Bible, pray and spend some quality time with family.",
        "Earn just enough money for a 20 year-old Corvette by doing side gigs for 10 years.",
        "Call 911 and tell them you are in need of finacial advice.",
        "Repent for even thinking about the other option and invite your girlfriend instead.",
        "Throw $1000 dollars worth of dollar coins into a wishing well for 'investment' purposes.",
        "Talk about last night's sports game and how happy you are that you have a 1 week backlog of homework to do.",
        "Enjoy the physical buttons on your 10-year old nokia phone."
    };

    public List<string> badChoises = new List<string>()
    {
        "Listen to an audiobook of Leviticus on repeat while driving.",
        "Dance around a golden statue as if it was your god.",
        "Say 'OH MY G*D' many many times for no reason.",
        "Work overtime on a Saturday to get enough money for next friday's date night.",
        "Take your dad's new BMW for a spin, despite him saying that it was off limits.",
        "Shoot a running away thief that has stole your million dollar lottery ticket. ",
        "Enjoy a fun weekend in Greece with your past roomate's wife.",
        "Pickup a quarter someone just dropped and naruto run to the nearest arcade to use it.",
        "Tell your class friends how horrible your roomate is even though they are just fine.",
        "Stare at your classmate's golden iPhone imagining how nice it would be to have it."
    };

    //UI
    public GameObject BackStory;
    public GameObject Option1GO;
    public GameObject Option2GO;
    public GameObject RestartGo;
    public TMP_Text Option1;
    public TMP_Text Option2;
    public Animator bgAnim;

    //In-Game
    public GameObject leftPlatform;
    public GameObject rightPlatform;
    public SpriteRenderer bg;
    public SmoothFollow follow;
    public List<EffectSpawner> topEffects = new List<EffectSpawner>();
    public List<EffectSpawner> bottomEffects = new List<EffectSpawner>();

    //Go to hell options
    public float durationToOpenDoor;
    public float angleToGoDown;

    //Instance
    HashSet<int> usedScenarios = new HashSet<int>();
    int curGoodSide;

    //Utility
    public Button tempBtn;


    public void OnBeginStory()
    {
        ShowNextScenario();
        BackStory.SetActive(false);
        Option1GO.SetActive(true);
        Option2GO.SetActive(true);
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void OnChooseOption(int side)
    {
        tempBtn.Select();
        if (side == curGoodSide)    //good side is one that make player have a "GameOver"
        {
            bgAnim.SetBool("isWrong", true);
            ShowGameOver(false);
        }
        else
        {
            bgAnim.SetBool("isCorrect", true);
            ShowNextScenario();
            foreach (EffectSpawner effect in topEffects)
            {
                effect.SpawnEffect();
            }
        }
    }

    private void ShowNextScenario()
    {
        int scenario = GetNextScenario();

        if (scenario == -1)
        {
            ShowGameOver(true);
            return;
        }

        curGoodSide = Random.Range(0, 2);
        if (curGoodSide == 1)
        {
            Option1.text = badChoises[scenario];
            Option2.text = goodChoises[scenario];
        }
        else
        {
            Option1.text = goodChoises[scenario];
            Option2.text = badChoises[scenario];
        }
    }


    private void ShowGameOver(bool isSuccess)
    {
        if (isSuccess)
            Debug.Log("You Won!");
        else
            StartCoroutine(SendToHell());

        Option1GO.SetActive(false);
        Option2GO.SetActive(false);
    }

    IEnumerator SendToHell()
    {
        Debug.Log("You Lost!");

        //Smoothly change color of sky
        bg.color = new Color(0.3867925f,0,0,1);
        follow.enabled = true;

        //Spawn in bad items on top of person

        //Start effect show
        foreach (EffectSpawner effect in bottomEffects)
        {
            effect.SpawnEffect();
        }

        //Open the gates
        float startTime = Time.time;
        float changeAngle = 0;
        while ((Time.time - startTime) <= durationToOpenDoor)
        {
            changeAngle = Mathf.SmoothStep(0, angleToGoDown, (Time.time - startTime) / durationToOpenDoor);
            leftPlatform.transform.eulerAngles = new Vector3(0,0, -changeAngle);
            rightPlatform.transform.eulerAngles = new Vector3(0, 0, changeAngle);
            yield return new WaitForEndOfFrame();
        }


        //Wait a bit
        yield return new WaitForSeconds(3f);

        //Show Restart Button
        RestartGo.SetActive(true);
    }


    //[UTILITY]
    private int GetNextScenario()
    {
        //TODO: improve efficiency
        if (usedScenarios.Count >= badChoises.Count)
            return -1;

        int scenario = Random.Range(0, goodChoises.Count);
        while (usedScenarios.Contains(scenario))
        {
            scenario = Random.Range(0, goodChoises.Count);
        }

        usedScenarios.Add(scenario);
        return scenario;
    }
}
