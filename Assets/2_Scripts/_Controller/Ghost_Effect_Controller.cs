using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Effect_Controller : MonoBehaviour
{
    // 트레일을 생성할 대상 리스트
    private List<Transform> Ghost_Target = new List<Transform>();

    // 트레일 지속 시간과 생성 간격 조정 가능
    public float Ghost_Duration = 0.1f;
    public float Spawn_Interval = 2.0f;

    private float Time_Since_Last_Spawn = 0f;

    // 트레일 색상 조정 가능
    public Color Ghost_Color;

    private void Start()
    {
        Ghost_Color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    }

    private void Update()
    {
        Time_Since_Last_Spawn += Time.deltaTime;

        if (Time_Since_Last_Spawn >= Spawn_Interval)
        {
            Spawn_Ghost();
            Time_Since_Last_Spawn = 0f;
        }
    }

    public void Add_Ghost_Target(Transform target)
    {
        // 트레일 타겟 추가
        if (!Ghost_Target.Contains(target))
        {
            Ghost_Target.Add(target);
        }
    }

    private void Spawn_Ghost()
    {
        // trailTargets 리스트를 순회하며 트레일 생성
        int Target_Count = Ghost_Target.Count;
        for (int i = 0; i < Target_Count; i++)
        {
            Transform Target = Ghost_Target[i];
            if (Target != null)
            {
                GameObject Ghost = new GameObject("Trail");
                Ghost.transform.position = Target.position;
                Ghost.transform.rotation = Target.rotation;

                SpriteRenderer SR = Ghost.AddComponent<SpriteRenderer>();
                SR.sprite = Target.GetComponent<SpriteRenderer>().sprite;
                SR.color = Ghost_Color;
                SR.flipX = Target.GetComponent<SpriteRenderer>().flipX;
                SR.drawMode = SpriteDrawMode.Sliced;
                SR.size = new Vector2(Target.GetComponent<SpriteRenderer>().size.x, Target.GetComponent<SpriteRenderer>().size.y);
                SR.sortingOrder = -1;

                // 트레일을 일정 시간 후에 제거
                Destroy(Ghost, Ghost_Duration);
            }
        }
    }
}
