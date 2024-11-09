using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    [SerializeField] Dictionary<GameObject, float> gizmoTargets = new Dictionary<GameObject, float>(); // 객체와 충돌 반경을 저장하는 딕셔너리
    // 프로퍼티를 통해 외부에서 타겟을 추가/삭제 가능하게 함
    public Dictionary<GameObject, float> GizmoTargets { get { return gizmoTargets; } set { gizmoTargets = value; } }

    void OnDrawGizmos()
    {
        // 기즈모 타겟이 설정되어 있을 경우, 해당 타겟들에 대해 기즈모를 그림
        if (gizmoTargets != null && gizmoTargets.Count > 0)
        {
            Gizmos.color = Color.red; // 충돌 영역을 빨간색으로 표시

            foreach (var target in gizmoTargets)
            {
                GameObject targetObject = target.Key;
                float radius = target.Value;

                if (targetObject != null)
                {
                    // 각 타겟의 위치에 기즈모 그리기
                    Gizmos.DrawWireSphere(targetObject.transform.position, radius);
                }
            }
        }
    }
}
