using UnityEngine;

public class TriggerColission : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        var player = other.CompareTag("Player");
        if (player)
        {
            gameObject.SetActive(false);
        }
    }
}
