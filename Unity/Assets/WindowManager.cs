using Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance;

    private List<Window> windows = new List<Window>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        Instance = null;
    }

    private void Awake()
    {
        Instance = this;
    }

    public T GetWindow<T>()
        where T : Window
    {
        T window = windows.OfType<T>().FirstOrDefault();
        if (window == null)
            window = Initialize<T>();

        return window;
    }

    public T Initialize<T>()
         where T : Window
    {
        GameObject windowGameObject = Resources.Load<GameObject>($"Windows/{typeof(T).Name}");
        T window = windowGameObject.GetComponent<T>();
        windows.Add(window);

        return window;
    }
}
