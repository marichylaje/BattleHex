using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // Objeto a seguir
    public float smoothSpeed = 0.125f;  // Velocidad de movimiento de la cámara
    public float dragSpeed = 2f;  // Velocidad de arrastre de la cámara
    public float delayTime = 1f;  // Tiempo de retraso antes de volver a centrar la cámara en el objeto de seguimiento

    private Vector3 dragOrigin;  // Origen del arrastre
    private Vector3 targetPosition;  // Posición objetivo de la cámara
    private bool isDragging;  // Indicador de si se está arrastrando la cámara

    private void Start()
    {
        targetPosition = target.position;
    }

    private void LateUpdate()
    {
        if (!isDragging)
        {
            // Seguir el objeto objetivo con una transición suave
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Centrar el objeto objetivo en el medio de la pantalla
            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        }
    }

    private void Update()
    {
        // Iniciar el arrastre de la cámara en el clic del botón izquierdo del ratón
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
        }

        // Arrastrar la cámara mientras se mantiene presionado el botón izquierdo del ratón
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(dragOrigin);
            transform.position -= difference * dragSpeed;
            dragOrigin = Input.mousePosition;
        }

        // Soltar el arrastre de la cámara en el clic del botón izquierdo del ratón
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // Volver a centrar la cámara en el objeto objetivo después de un retraso
            Invoke("ResetCamera", delayTime);
        }
    }

    private void ResetCamera()
    {
        targetPosition = target.position;
    }
}
