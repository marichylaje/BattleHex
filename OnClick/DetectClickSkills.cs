using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DetectClickSkills : MonoBehaviour
{
    public SpellCastManager spellCastManager;
    private void Start() {
        // Busca el componente Button en la imagen de Canvas
        Button button = GetComponent<Button>();
        // Sprite button = gameObject.GetComponent<Image>().sprite;

        // Agrega una función de escucha para el evento onClick
        button.onClick.AddListener(() => onClickBtn());
    }

    private void onClickBtn() {
        Debug.Log("3");
        // obtain the name of the ImageRender asociated with the button
        Sprite image = GetComponent<Image>().sprite;
        if(image != null && image.name != "skill box"){
            Debug.Log("IMAGE: " + image.name);
            spellCastManager.isCasting = true;
            spellCastManager.isChangingSpell = true;
            spellCastManager.spellNameClicked = image.name;
        }
    }
}
