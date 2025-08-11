using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfoPopupPoint : MonoBehaviour
{
    [SerializeField] private Transform popupPoint;

    public Vector2 GetPoint()
    {
        return popupPoint.position;
    }
}