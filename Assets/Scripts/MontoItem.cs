using UnityEngine;

public class MontoItem : MonoBehaviour
{
    [HideInInspector]
    public Vector3 originalPosition;
    [HideInInspector]
    public Quaternion originalRotation;
    public string processedTag = "";   // por ejemplo "Monto10kProcesado"

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void ResetItem()
    {
        // restauramos posición y rotación
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // restauramos el tag original si quieres
        if (!string.IsNullOrEmpty(processedTag))
        {
            // asume que el tag original está guardado o conocido
            // por simplicidad: devolvemos al tag “Monto10k” o “Monto100”
            // podrías guardar un campo originalTag también
            gameObject.tag = processedTag;  // o gameObject.tag = originalTag;
        }

        // restablece físicas si fuerza/gravedad fue cambiada etc.
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;  // si lo usas de esta forma
            rb.useGravity = true;
        }
    }
}