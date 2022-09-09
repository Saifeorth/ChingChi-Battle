using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;


[RequireComponent(typeof(TextMeshProUGUI))]
public class TextHoverTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI _text;

    /// <summary>
    /// The selectable Component.
    /// </summary>
    public Selectable Selectable;

    /// <summary>
    /// The color of the normal of the transition state.
    /// </summary>
    public Color NormalColor = Color.white;

    /// <summary>
    /// The color of the hover of the transition state.
    /// </summary>
    public Color HoverColor = Color.black;

    public void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void OnEnable()
    {
        _text.color = NormalColor;
    }

    public void OnDisable()
    {
        _text.color = NormalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Selectable == null || Selectable.IsInteractable())
        {
            _text.color = HoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Selectable == null || Selectable.IsInteractable())
        {
            _text.color = NormalColor;
        }
    }
}
