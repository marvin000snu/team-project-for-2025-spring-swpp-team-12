using UnityEngine;

public class RotatingAxe : MonoBehaviour
{
    public float rotationSpeed = 45f; // 초당 회전 속도 (도 단위)
    public float maxAngle = 45f;      // 최대 회전 각도
    private float currentAngle = 0f;
    private int direction = 1;

    void Update()
    {
        float delta = rotationSpeed * Time.deltaTime * direction;
        currentAngle += delta;

        if (Mathf.Abs(currentAngle) > maxAngle)
        {
            direction *= -1;
            currentAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);
        }

        transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);
    }
}
