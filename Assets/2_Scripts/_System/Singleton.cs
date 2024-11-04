using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트에 해당 컴포넌트를 복수개 붙일 수 없게 하는 어트리뷰트
[DisallowMultipleComponent]  // 중복 컴포넌트 추가 방지
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singletonObject = new GameObject();
                instance = singletonObject.AddComponent<T>();
                Debug.Log("singleton Manager has been created " + instance.name);
            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            Debug.Log("singleton Manager has been created " + instance.name);
        }
        else
            Destroy(gameObject);
    }


    private void OnDestroy()
    {
        Debug.Log(instance.name + " 파괴됨");
    }

}


#region
//// 오브젝트에 해당 컴포넌트를 복수개 붙일 수 없게 하는 어트리뷰트
//[DisallowMultipleComponent]  // 중복 컴포넌트 추가 방지
//public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
//{
//    private static T Instance;
//    private static bool isApplicationQuit = false;

//    public static T instance
//    {
//        get
//        {
//            if (isApplicationQuit == true)
//                return null;

//            if (Instance == null)
//            {
//                T[] FindInstance = FindObjectsOfType<T>();
//                if (FindInstance.Length > 0)
//                {
//                    Instance = FindInstance[0];
//                    DontDestroyOnLoad(Instance.gameObject);
//                }

//                if (FindInstance.Length > 1)
//                {
//                    for (int i = 1; i < FindInstance.Length; i++)
//                        Destroy(FindInstance[i].gameObject);
//                }

//                if (Instance == null)
//                {
//                    GameObject CreateGameObject = new GameObject(typeof(T).Name);
//                    DontDestroyOnLoad(CreateGameObject);
//                    Instance = CreateGameObject.AddComponent<T>();
//                }
//            }
//            return Instance;
//        }
//    }

//    private void OnApplicationQuit()
//    {
//        isApplicationQuit = true;
//    }

//}

//public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
//{
//    private static T instance;
//    private static bool isApplicationQuit = false;
//    private static readonly object lockObj = new object();  // 스레드 안전성 추가

//    public static T Instance
//    {
//        get
//        {
//            // 애플리케이션 종료 후에는 null을 반환
//            if (isApplicationQuit)
//                return null;

//            lock (lockObj)  // 스레드 안전성 확보를 위한 lock 사용
//            {
//                if (instance == null)
//                {
//                    T[] foundInstances = FindObjectsOfType<T>();

//                    // 1. 인스턴스가 이미 존재할 경우 첫 번째로 찾은 인스턴스 사용
//                    if (foundInstances.Length > 0)
//                    {
//                        instance = foundInstances[0];
//                        DontDestroyOnLoad(instance.gameObject);
//                    }

//                    // 2. 중복된 인스턴스가 있으면 나머지 삭제
//                    if (foundInstances.Length > 1)
//                    {
//                        for (int i = 1; i < foundInstances.Length; i++)
//                        {
//                            Destroy(foundInstances[i].gameObject);
//                        }
//                    }

//                    // 3. 인스턴스가 아직 없을 경우 새로 생성
//                    if (instance == null)
//                    {
//                        GameObject newSingleton = new GameObject(typeof(T).Name);
//                        instance = newSingleton.AddComponent<T>();
//                        DontDestroyOnLoad(newSingleton);
//                    }
//                }
//                return instance;
//            }
//        }
//    }

//    protected virtual void Awake()
//    {
//        // 중복된 싱글톤 인스턴스가 있으면 파괴
//        if (instance == null)
//        {
//            instance = this as T;
//            DontDestroyOnLoad(gameObject);
//        }
//        else if (instance != this)
//        {
//            Destroy(gameObject);  // 중복 인스턴스 파괴
//        }
//    }

//    private void OnApplicationQuit()
//    {
//        isApplicationQuit = true;
//    }
//}

#endregion