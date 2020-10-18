using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Devboys.SharedObjects.Variables;

[RequireComponent(typeof(Text))]
public class TestUIScript : MonoBehaviour
{
    public FloatReference playerHealth;

    Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    private void Update()
    {
        _text.text = playerHealth.CurrentValue.ToString();
    }
}
