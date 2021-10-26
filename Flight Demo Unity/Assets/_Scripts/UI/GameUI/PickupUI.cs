using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.RuntimeSets;
using TMPro;
using UnityEngine.UI;

public class PickupUI : MonoBehaviour
{
    public TransformRuntimeSet pickupSet;

    private bool isFirstUpdate;

    private int maxPickups;

    private TextMeshProUGUI _text;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        isFirstUpdate = true;
    }

    private void Update()
    {
        int numPickups = pickupSet.items.Count;
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            maxPickups = numPickups;
        }

        _text.text = $"{maxPickups - numPickups} / {maxPickups}";



    }
}