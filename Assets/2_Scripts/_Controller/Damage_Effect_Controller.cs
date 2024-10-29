using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Effect_Controller : MonoBehaviour
{
    public int poolSize = 100; // 미리 생성할 데미지 이펙트 수
    private GameObject[] damageEffects;
    private int currentIndex = 0;

    // 객체 풀 초기화
    public void Damage_Effect_Pool()
    {
        damageEffects = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject damageEffect = new GameObject("Damage_Effect_" + i);
            TextMesh damageText = damageEffect.AddComponent<TextMesh>();

            damageText.fontSize = 32;
            damageText.characterSize = 0.1f;
            damageText.color = Color.white;

            damageEffect.transform.parent = this.transform;
            damageEffect.SetActive(false);

            damageEffects[i] = damageEffect;
        }
    }

    // 데미지 이펙트 생성 및 위치 지정
    public void Show_Damage_Effect(Vector3 position, string damageValue)
    {
        GameObject damageEffect = Get_Next_Available_Effect();

        if (damageEffect != null)
        {
            damageEffect.transform.position = position;
            damageEffect.SetActive(true);

            TextMesh textMesh = damageEffect.GetComponent<TextMesh>();
            textMesh.text = damageValue;

            // 일정 시간 후에 비활성화
            StartCoroutine(Hide_Damage_Effect(damageEffect, 1.0f)); // 1초 후 비활성화
        }
    }

    // 풀에서 다음 사용 가능한 이펙트를 가져오기
    private GameObject Get_Next_Available_Effect()
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
    private System.Collections.IEnumerator Hide_Damage_Effect(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        effect.SetActive(false);
    }
}

