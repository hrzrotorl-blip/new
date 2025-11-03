using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요합니다.

public class SceneChangeTrigger : MonoBehaviour
{
    // 이동할 씬의 이름을 Inspector 창에서 설정할 수 있게 합니다.
    public string targetSceneName = "IntroAnimationScene";

    // 유니티의 OnTriggerEnter 함수는 다른 Collider가 이 Trigger Collider에 진입했을 때 호출됩니다.
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 'Player' 태그를 가지고 있는지 확인합니다.
        // (플레이어 오브젝트에 'Player' 태그를 반드시 설정해 주세요!)
        if (other.CompareTag("Player"))
        {
            // IntroAnimationScene으로 이동
            SceneManager.LoadScene(targetSceneName);
        }
    }
}