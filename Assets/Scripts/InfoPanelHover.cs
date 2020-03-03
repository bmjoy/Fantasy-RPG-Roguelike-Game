﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoPanelHover : MonoBehaviour
{
    [Header("Properties")]
    public StatusIcon currentIconUnderMouse;    

    [Header("Status Info Panel Components")]
    public GameObject statusPanelParent;
    public TextMeshProUGUI statusPanelNameText;
    public TextMeshProUGUI statusPanelDescriptionText;
    public Image statusPanelImage;

    [Header("Rect Transform Panel Components")]
    public RectTransform statusPanelParentRect;
    public RectTransform statusPanelFrameRect;
    public RectTransform statusPanelDescriptionRect;
    public RectTransform statusPanelNameRect;   

    public static InfoPanelHover Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        MoveToIconPosition();
    }
    public void BuildViewFromIconData(StatusIcon data)
    {
        // Set text fields + image
        statusPanelImage.sprite = data.statusSprite;
        statusPanelNameText.text = data.statusName;
        statusPanelDescriptionText.text = data.statusDescriptionText.text;
    }
    public void MoveToIconPosition()
    {
        // set position
        if(currentIconUnderMouse != null)
        {
            statusPanelParent.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, currentIconUnderMouse.gameObject.transform.position);
        }        
    }    
    public void HandleIconMousedEnter(StatusIcon icon)
    {
        currentIconUnderMouse = icon;
        EnableView();
        BuildViewFromIconData(icon);
        RefreshLayoutGroups();
    }
    public void RefreshLayoutGroups()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusPanelParentRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusPanelFrameRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusPanelDescriptionRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusPanelNameRect);
    }
    public void HandleIconMouseExit(StatusIcon icon)
    {
        if(icon == currentIconUnderMouse)
        {
            currentIconUnderMouse = null;
            DisableView();
        }       
    }
    public void EnableView()
    {
        statusPanelParent.SetActive(true);
    }
    public void DisableView()
    {
        statusPanelParent.SetActive(false);
    }
}