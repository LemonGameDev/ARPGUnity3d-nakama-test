using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private CharacterMovement characterMovement;
    
    [SerializeField] private LayerMask _layer;

    [SerializeField] private GameObject markPointer;
    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnMouseLeftClick();
        }

    }


    private void OnMouseLeftClick()
    {
        RaycastHit hit;
        var mainCamera = Camera.main;
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, _layer))
        {
            characterMovement.MovePlayer(hit.point);
             var markPos = hit.point;
             markPos.y = 0.5f;
             markPointer.transform.position = markPos;
             markPointer.SetActive(true);
        }
    }
}