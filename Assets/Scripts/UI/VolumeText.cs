using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    private TextMeshProUGUI txt;
    [SerializeField] private string intro;
    [SerializeField] private string volumeName;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float volumne = PlayerPrefs.GetFloat(volumeName, 1) * 100;
        txt.text = intro + " " + volumne;
    }
}

