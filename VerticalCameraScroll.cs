using Cinemachine;
using Cinemachine.Utility;
using UnityEngine;

public class VerticalCameraScroll : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineDollyCart dollyCart; // DollyCart 참조
    public float scrollSpeed = 5f;
    public float minPosition = 0f;
    public float maxPosition = 10f;

    private bool isDragging = false;
    private float lastMouseY;

    void Update()
    {
        if (dollyCart == null) return;

        // 마우스 드래그 시작
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMouseY = Input.mousePosition.y;
        }
        // 마우스 드래그 종료
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
        // 드래그 중일 때 DollyCart 위치 이동
        if (isDragging)
        {
            float mouseY = Input.mousePosition.y;
            float deltaY = mouseY - lastMouseY;
            lastMouseY = mouseY;

            float newPos = dollyCart.m_Position + deltaY * scrollSpeed * Time.deltaTime * 0.1f;
            dollyCart.m_Position = Mathf.Clamp(newPos, minPosition, maxPosition);
        }
    }
}
