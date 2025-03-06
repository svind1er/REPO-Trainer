using UnityEngine;

public class Helper : MonoBehaviour
{
    private static Helper _instance;

    public static Helper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Helper>();
            }
            return _instance;
        }
    }

    public new static object FindObjectOfType(System.Type type)
    {
        return FindObjectOfType(type, true);
    }
}