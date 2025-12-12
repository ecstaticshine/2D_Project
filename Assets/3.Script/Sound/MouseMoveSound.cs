using UnityEngine;

public class MouseMoveSound : MonoBehaviour
{
    public string moveSound;          // 재생할 소리
    public float minMoveDistance = 5f;   // 이 픽셀 이상 움직였을 때만 재생
    public float soundCooldown = 0.1f;   // 최소 재생 간격(초)

    private Vector3 lastMousePos;
    private float lastPlayTime;

    void Start()
    {
        lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        Vector3 currentPos = Input.mousePosition;
        float distance = Vector3.Distance(currentPos, lastMousePos);

        if (distance > minMoveDistance && Time.time - lastPlayTime > soundCooldown)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(moveSound);

            lastPlayTime = Time.time;
        }

        lastMousePos = currentPos;
    }
}
