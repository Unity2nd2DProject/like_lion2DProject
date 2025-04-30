using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private const string TAG = "Singleton";

    private static T instance; // Singleton 인스턴스를 저장할 static 변수 
    private static bool isQuitting = false; // 게임 종료 여부 확인


    /// <summary>
    /// Singleton 인스턴스를 가져오는 속성.
    /// 인스턴스가 없으면 새로 생성하고, 이미 있으면 기존 인스턴스를 반환.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (isQuitting) return null;
            if (instance == null) // 인스턴스가 null이면 새로 찾거나 생성
            {
                instance = FindFirstObjectByType<T>(); // 씬에서 해당 타입의 첫 번째 오브젝트를 찾음

                if (instance == null) // 찾지 못하면 새로운 GameObject를 만들어서 추가
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name); // 게임 오브젝트를 새로 생성
                    instance = singletonObject.AddComponent<T>(); // 해당 타입의 컴포넌트를 추가하여 인스턴스 할당
                    DontDestroyOnLoad(singletonObject); // 씬 전환 시에도 오브젝트가 파괴되지 않도록 설정
                }
            }

            return instance; // 인스턴스 반환
        }
    }

    /// <summary>
    /// MonoBehaviour의 Awake 메서드.
    /// 인스턴스가 이미 존재하면 다른 게임 오브젝트를 파괴하고, 그렇지 않으면 현재 오브젝트를 싱글톤으로 설정.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance != null && instance != this) // 인스턴스가 존재하고, 현재 오브젝트가 아니라면 중복된 싱글톤이므로 파괴
        {
            // if(GameManager.Instance.logOn) Debug.Log($"[{TAG}] Destroy");
            Destroy(gameObject); // 중복된 싱글톤을 파괴
            return;
        }

        instance = this as T; // 인스턴스가 null이면 현재 오브젝트를 인스턴스로 설정

        //if (this as BaseUI || this as TimeImageUI)
        //{
        //    Debug.Log(this);
        //    DontDestroyOnLoad(gameObject);
        //    return;
        //}

        if (!gameObject.transform.parent) DontDestroyOnLoad(gameObject); // 최상위 오브젝트일때만 씬 전환 시에도 이 게임 오브젝트가 파괴되지 않도록 설정
    }

    private void OnApplicationQuit()
    {
        isQuitting = true; // 게임 종료 시 플래그 설정
    }
}
