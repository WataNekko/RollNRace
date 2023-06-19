using UnityEngine;

/// <summary>
/// Derive this class to get a singleton MonoBehavior, whose instance can be accessed with <see cref="Instance"/>.
/// </summary>
/// <typeparam name="T">This should be the class deriving from <see cref="Singleton{T}"/> so that the returned type of <see cref="Instance"/> is the derived class (<see cref="T"/>) itself.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
    }
}
