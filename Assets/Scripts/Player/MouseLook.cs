using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // static으로 변경하여 어디서든 접근 가능하게 합니다.
    public static float sensitivityMultiplier = 1f; // UI 슬라이더에서 조절할 최종 감도 계수

    // 이 변수는 인스펙터에 노출되어 기본 감도를 설정합니다.
    [Header("Base Sensitivity")]
    public float baseMouseSensitivity = 50f;

    // (기존 코드 유지)
    public Transform playerBody;
    int speed = 5;
    float xRotation = 0f;

    void Start()
    {
        // 💾 저장된 감도 값을 불러옵니다.
        // "MouseSensitivity" 키로 저장된 값이 있으면 불러오고, 없으면 1.0f를 기본값으로 사용합니다.
        sensitivityMultiplier = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 💡 핵심: 기본 감도에 UI에서 조절된 계수(Multiplier)를 곱합니다.
        // float finalSensitivity = baseMouseSensitivity * sensitivityMultiplier; 

        // float mouseX = Input.GetAxis("Mouse X") * finalSensitivity * Time.deltaTime * speed;
        // float mouseY = Input.GetAxis("Mouse Y") * finalSensitivity * Time.deltaTime * speed;

        // (기존 코드에서 mouseSensitivity를 baseMouseSensitivity로 변경)
        float mouseX = Input.GetAxis("Mouse X") * baseMouseSensitivity * Time.deltaTime * speed * sensitivityMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * baseMouseSensitivity * Time.deltaTime * speed * sensitivityMultiplier;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}