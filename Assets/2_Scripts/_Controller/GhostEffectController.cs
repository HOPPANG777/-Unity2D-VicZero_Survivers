using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffectController : MonoBehaviour
{
    // 트레일을 생성할 대상 리스트
    private List<Transform> GhostTarget = new List<Transform>();

    // 트레일 지속 시간과 생성 간격 조정 가능
    public float GhostDuration = 0.1f;
    public float SpawnInterval = 2.0f;

    private float TimeSinceLastSpawn = 0f;

    // 트레일 색상 조정 가능
    public Color GhostColor;

    private void Start()
    {
        GhostColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    }

    private void Update()
    {
        TimeSinceLastSpawn += Time.deltaTime;

        if (TimeSinceLastSpawn >= SpawnInterval)
        {
            SpawnGhost();
            TimeSinceLastSpawn = 0f;
        }
    }

    public void AddGhostTarget(Transform target)
    {
        // 트레일 타겟 추가
        if (!GhostTarget.Contains(target))
        {
            GhostTarget.Add(target);
        }
    }

    private void SpawnGhost()
    {
        // trailTargets 리스트를 순회하며 트레일 생성
        int TargetCount = GhostTarget.Count;
        for (int i = 0; i < TargetCount; i++)
        {
            Transform Target = GhostTarget[i];
            if (Target != null)
            {
                GameObject Ghost = new GameObject("Trail");
                Ghost.transform.position = Target.position;
                Ghost.transform.rotation = Target.rotation;

                SpriteRenderer spriteRenderer = Ghost.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Target.GetComponent<SpriteRenderer>().sprite;
                spriteRenderer.color = GhostColor;
                spriteRenderer.flipX = Target.GetComponent<SpriteRenderer>().flipX;
                spriteRenderer.drawMode = SpriteDrawMode.Sliced;
                spriteRenderer.size = new Vector2(Target.GetComponent<SpriteRenderer>().size.x, Target.GetComponent<SpriteRenderer>().size.y);
                spriteRenderer.sortingOrder = -1;

                // 트레일을 일정 시간 후에 제거
                Destroy(Ghost, GhostDuration);
            }
        }
    }
}
