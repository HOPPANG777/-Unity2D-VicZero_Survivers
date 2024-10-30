using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TODO
- Bound시 일어날 일(탄환 비활성화, 장판 생성 등) 구현
- 화면 밖으로 나가면 사라지는거 넣을거면 넣기
 */

public class FireBullet : MonoBehaviour
{
    Vector3 shootDir;
    float speed;
    bool isActive;
    public const float boundSize = 0.0f;

    // 발사시 사용할 것들 초기화
    public void Shoot(Vector3 _shootDir)
    {
        isActive = true;
        shootDir = _shootDir;
    }
    
    // 정해준 방향의 주변으로 비틀비틀 날아가는 용도의 코드
        
    private void Move()
    {
        Vector3 moveDir = new Vector3();
        moveDir.x = shootDir.x * Random.Range(0.0f, 1.0f);
        moveDir.y = shootDir.y * Random.Range(0.0f, 1.0f);
        moveDir.Normalize();
        transform.position += moveDir * speed * Time.deltaTime;
    }

    bool Bounded()
    {
        for (int i = 0; i < All_Manager.instance.GameMode_M.Enemy_List.Count; i++)
        {
            float dis = Vector3.Distance(this.transform.position, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position);
            
            if(dis < boundSize)
            {
                isActive = false;
                return true;
            }
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            Move();
            if(isActive)
            {
                // 화염 장판깔기
                // 혹은 폭발시 효과
            }
        }
    }
}
