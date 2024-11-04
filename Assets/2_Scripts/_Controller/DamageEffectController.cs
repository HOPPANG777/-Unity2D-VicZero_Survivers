using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectController : MonoBehaviour
{
    public int poolSize = 100; // 미리 생성할 데미지 이펙트 수
    private GameObject[] damageEffects;
    private int currentIndex = 0;
    private float damageEffectCooldown = 0.2f; // 데미지 이펙트 간 최소 간격 (초 단위)
    private float lastDamageEffectTime = -Mathf.Infinity;

    // 객체 풀 초기화
    public void DamageEffectPool()
    {
        damageEffects = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject damageEffect = new GameObject("DamageEffect" + i);
            TextMesh damageText = damageEffect.AddComponent<TextMesh>();

            damageText.fontSize = 50; // 폰트 사이즈
            damageText.characterSize = 0.1f; // 캐릭터 크기를 작게 설정
            damageText.color = Color.white;

            damageEffect.transform.parent = this.transform;
            damageEffect.SetActive(false);

            damageEffects[i] = damageEffect;
        }
    }

    // 데미지 이펙트 생성 및 위치 지정
    public void ShowDamageEffect(Vector3 position, string damageValue)
    {
        // 쿨다운 체크
        if (Time.time - lastDamageEffectTime < damageEffectCooldown)
        {
            // 아직 쿨다운이 끝나지 않았다면 이펙트를 생성하지 않음
            return;
        }

        lastDamageEffectTime = Time.time; // 마지막 데미지 이펙트 생성 시간 갱신

        GameObject damageEffect = GetNextAvailableEffect();

        if (damageEffect != null)
        {
            damageEffect.transform.position = position;
            damageEffect.SetActive(true);

            TextMesh textMesh = damageEffect.GetComponent<TextMesh>();
            textMesh.text = damageValue;

            // 일정 시간 후에 비활성화
            StartCoroutine(HideDamageEffect(damageEffect, 1.0f)); // 1초 후 비활성화
        }
    }

    // 풀에서 다음 사용 가능한 이펙트를 가져오기
    private GameObject GetNextAvailableEffect()
    {
        for (int i = 0; i < poolSize; i++)
        {
            currentIndex = (currentIndex + 1) % poolSize;
            if (!damageEffects[currentIndex].activeInHierarchy)
            {
                return damageEffects[currentIndex];
            }
        }

        // 모든 이펙트가 사용 중일 경우 null 반환
        return null;
    }

    // 일정 시간 후 이펙트를 비활성화
    private IEnumerator HideDamageEffect(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        effect.SetActive(false);
    }
}

