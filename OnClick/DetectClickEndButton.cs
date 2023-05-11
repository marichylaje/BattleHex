using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectClickEndButton : MonoBehaviour
{
    public TurnManager turnManager;
    void Start() {
        // Busca el componente Button en la imagen de Canvas
        Button button = GetComponent<Button>();

        // Agrega una función de escucha para el evento onClick
        button.onClick.AddListener(turnManager.EndTurn);
    }
    void Update()
    {
        // Verifica si se presiona la tecla Spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Llama a la función EndTurn del turnManager
            turnManager.EndTurn();
        }
    }
}
