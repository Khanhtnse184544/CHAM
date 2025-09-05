using UnityEngine;
using System;

[System.Serializable]
public class RewardFlyParams
{
    public Sprite icon;                      // Sprite vật phẩm bay
    public Vector3 originWorldPosition;      // Vị trí bắt đầu (UI hoặc World)
    public Transform destination;            // Điểm đến
    public bool originIsUI = true;           // Nếu false, sẽ convert WorldToScreen
    public float flyDuration = 0.75f;        // Thời gian bay
    public Action onFlyComplete;             // Hàm gọi sau khi bay xong
}
