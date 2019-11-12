﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ArtifactGO : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Gameobject + Component References")]
    public Image artifactImage;
    public GameObject infoPanel;
    public TextMeshProUGUI artifactNameText;
    public TextMeshProUGUI artifactDescriptionText;

    // Initialization + Setup
    #region
    public void InitializeSetup(ArtifactDataSO artifactData)
    {
        artifactImage.sprite = artifactData.sprite;
        artifactDescriptionText.text = artifactData.description;
        artifactNameText.text = artifactData.Name;
    }
    #endregion

    // Mouse / Click events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
    }
    #endregion
}
