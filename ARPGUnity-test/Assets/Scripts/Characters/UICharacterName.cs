using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UICharacterName : MonoBehaviour
{
    [SerializeField] private Transform canvasTransform;

    [SerializeField] private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        canvasTransform.LookAt(Camera.main.transform);
    }

    public void  SetPlayerName(string name)
    {
        textMeshPro.text = name;
    }
}