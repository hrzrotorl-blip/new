using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    private Slider sensitivitySlider;

    void Start()
    {
        sensitivitySlider = GetComponent<Slider>();

        // 🔄 슬라이더 초기화: 저장된 값(sensitivityMultiplier)으로 슬라이더의 위치를 설정합니다.
        sensitivitySlider.value = MouseLook.sensitivityMultiplier;

        // 📝 슬라이더 값이 변경될 때마다 OnSensitivityChange 함수를 호출하도록 리스너를 추가합니다.
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChange);
    }

    // 슬라이더 값이 변경될 때 호출되는 함수
    public void OnSensitivityChange(float newSensitivity)
    {
        // 1. MouseLook 스크립트의 감도 계수를 업데이트합니다.
        MouseLook.sensitivityMultiplier = newSensitivity;

        // 2. 💾 PlayerPrefs에 새로운 감도 값을 저장하여 다음에 게임을 켤 때도 유지되도록 합니다.
        PlayerPrefs.SetFloat("MouseSensitivity", newSensitivity);
        PlayerPrefs.Save(); // 디스크에 즉시 저장
    }
}