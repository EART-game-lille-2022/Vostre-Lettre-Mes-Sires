using UnityEngine;

public class ontriggerTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        print(other.name);
    }
}
