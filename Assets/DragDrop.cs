using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class DragDrop : MonoBehaviour
{
    [SerializeField]
    private InputAction mouseClick;
    [SerializeField]
    private float mouseDragPhysicsSpeed= 10;

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    [SerializeField]
    private float mouseDragSpeed = .1f;
    private Vector3 velocity = Vector3.zero;

    private void OnEnable(){
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }

    private void OnDisable(){
        mouseClick.performed -= MousePressed;
        mouseClick.Disable();
    }

    private void MousePressed(InputAction.CallbackContext context){
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && (hit.collider.gameObject.CompareTag("Draggable"))){
            StartCoroutine(DragUpdate(hit.collider.gameObject));
        }
     }

    private IEnumerator DragUpdate(GameObject clickedObject){
        float initialDistance = Vector3.Distance(clickedObject.transform.position, Camera.main.transform.position);
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        
        while (mouseClick.ReadValue<float>() != 0) {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if(rb != null){
                Vector3 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
                rb.velocity = direction * mouseDragPhysicsSpeed;

                yield return waitForFixedUpdate;
            } else {
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
                yield return null;
            }
        }
    }
}
