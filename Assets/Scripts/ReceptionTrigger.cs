using UnityEngine;

public class ReceptionTrigger : MonoBehaviour
{
	public UIContadorController uiController;

	// Contadores
	public int contadorTransaccion = 0;
	public static int contadorGlobal = 0;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Monto10k"))
		{
			int monto = 10000;
			contadorTransaccion += monto;
			Debug.Log("Objeto recibido: " + monto);
			Debug.Log("Contador transaccion = " + contadorTransaccion);

			if (uiController != null)
			{
				uiController.ActualizarTransaccion(contadorTransaccion);
			}
			else
			{
				Debug.LogError("ReceptionTrigger: uiController no está asignado en el Inspector.");
			}
			
			other.gameObject.tag = "Monto10kProcesado";
		}
		else if (other.gameObject.CompareTag("Monto100"))
		{
			int monto = 100;
			contadorTransaccion += monto;
			Debug.Log("Objeto recibido: " + monto);
			Debug.Log("Contador transaccion = " + contadorTransaccion);

			if (uiController != null)
			{
				uiController.ActualizarTransaccion(contadorTransaccion);
			}
			else
			{
				Debug.LogError("ReceptionTrigger: uiController no está asignado en el Inspector.");
			}
			
			other.gameObject.tag = "Monto100Procesado";
		}
	}


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
