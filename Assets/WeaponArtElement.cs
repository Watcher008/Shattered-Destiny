using UnityEngine;
using UnityEngine.UI;
using SD.Combat.WeaponArts;
using UnityEngine.EventSystems;

public class WeaponArtElement : MonoBehaviour, IInitializePotentialDragHandler,
    IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rect;

    private Canvas _canvas;
    private RectTransform _parent;
    private int siblingIndex;
    private bool _validDrop;

    private WeaponArt _art;
    public WeaponArt Art => _art;

    public void SetValue(WeaponArt art, Canvas canvas)
    {
        _art = art;
        _image.sprite = art == null ? null : art.Sprite;

        _canvas = canvas;
        _parent = transform.parent as RectTransform;
        siblingIndex = transform.GetSiblingIndex();
    }

    public void OnValidDrop()
    {
        _validDrop = true;
        // don't return to parent
    }

    #region - Pointer Events -
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Only allow dragging with LMB
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _validDrop = false;
            _canvasGroup.blocksRaycasts = false;
            transform.SetParent(_canvas.transform, true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Only allow dragging with LMB
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _rect.anchoredPosition += eventData.delta * _canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_validDrop) return;

        _canvasGroup.blocksRaycasts = true;

        // valid drop? IDK destroy self
        // will have to call some function in here

        transform.SetParent(_parent);
        transform.SetSiblingIndex(siblingIndex);
    }
    #endregion
}
