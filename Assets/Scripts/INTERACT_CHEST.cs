using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
public class INTERACT_CHEST : MonoBehaviour
{
    public Button openButton;
    public Button takeButton;
    public TMP_Text riddleText; // UI text for displaying riddles

    private Animator animator;
    private bool isOpened = false;
    private Action onTreasureTaken;
    private string[] riddles = new string[]
    {
        "Where hearts find peace and spirits rise, Echoes of prayer reach the skies. A place of stillness, pure and deep, Find the chest where faith does sleep.",
        "Where minds are coded and logic takes flight, Machines and humans work day and night. From binary dreams to tech so supreme, Find the chest where the programmers scheme.",
        "Where thunderous cheers break the air, And athletes show strength beyond compare. Run, leap, sweat — no time to rest, Search where athletes give their best.",
        "Robes in black and scrolls in hand, Proud faces gather as families stand. Cheers and tears fill the air so high, Find the chest where dreams touch the sky.",
        "Silent whispers between the shelves, Guarding knowledge like secret elves. Pages turn and minds ignite, Find your prize near wisdom’s light.",
        
    };

    public int riddleIndex = 0; // Set by LocationBased when spawning

    void Start()
    {
        animator = GetComponent<Animator>();

        if (openButton != null)
        {
            openButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(TriggerOpen);
        }

        if (takeButton != null)
        {
            takeButton.onClick.RemoveAllListeners();
            takeButton.onClick.AddListener(TakeTreasure);
            takeButton.gameObject.SetActive(false);
        }

        if (riddleText != null && riddleIndex < riddles.Length)
        {
            riddleText.text = riddles[riddleIndex];
        }
    }

    public void Setup(Button openBtn, Button takeBtn, Action onTaken, TMP_Text riddleOutput, int index)
    {
        openButton = openBtn;
        takeButton = takeBtn;
        onTreasureTaken = onTaken;
        riddleText = riddleOutput;
        riddleIndex = index;

        if (openButton != null)
        {
            openButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(TriggerOpen);
        }

        if (takeButton != null)
        {
            takeButton.onClick.RemoveAllListeners();
            takeButton.onClick.AddListener(TakeTreasure);
            takeButton.gameObject.SetActive(false);
        }

        if (riddleText != null && riddleIndex < riddles.Length)
        {
            riddleText.text = riddles[riddleIndex];
        }
    }

    public void TriggerOpen()
    {
        if (isOpened || animator == null) return;

        animator.SetTrigger("Open");
        isOpened = true;

        if (openButton != null) openButton.gameObject.SetActive(false);
        if (takeButton != null) takeButton.gameObject.SetActive(true);

        Debug.Log("✅ Chest opened.");
    }

    public void TakeTreasure()
    {
        Debug.Log("✅ TakeTreasure() called.");

        if (takeButton != null)
            takeButton.gameObject.SetActive(false);

        onTreasureTaken?.Invoke();

        Destroy(this.gameObject);
    }
}