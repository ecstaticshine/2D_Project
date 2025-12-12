using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    [SerializeField]
    private GameObject SelectUnitPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SelectUnitPanel.activeSelf)
            {
                Button returnbutton = SelectUnitPanel.GetComponentInChildren<Button>();
                returnbutton.onClick.Invoke();
            }

        }
        
    }
}
