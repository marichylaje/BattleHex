using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DetectClickSkills : MonoBehaviour
{
    public string spellNameClicked = null;
    private void Start() {
        // Busca el componente Button en la imagen de Canvas
        Button button = GetComponent<Button>();
        // Sprite button = gameObject.GetComponent<Image>().sprite;

        // Agrega una función de escucha para el evento onClick
        button.onClick.AddListener(() => onClickBtn());
    }

    private void onClickBtn() {
        // obtain the name of the ImageRender asociated with the button
        Sprite image = GetComponent<Image>().sprite;
        if(image != null && image.name != "skill box"){
            Debug.Log("IMAGE: " + image.name);
            spellNameClicked = image.name;
        }
    }
}
