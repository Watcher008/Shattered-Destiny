using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SD.Combat.WeaponArts;


/// <summary>
/// A class to handle displaying the player's currently-equipped weapon arts
/// </summary>
public class ActiveWeaponArtElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [SerializeField] private Image _image;

    private WeaponManager _manager;
    private WeaponArt _art;
    private bool _isLocked = false;

    public Hand Hand { get; private set; }
    public int Index { get; private set; }

    public void SetValue(WeaponManager manager, Hand hand, int index, WeaponArt art)
    {
        _manager = manager;
        Index = index;
        Hand = hand;

        _art = art;
        _isLocked = false;
        _image.sprite = art == null ? null : art.Sprite;
    }

    public void SetLocked(Sprite sprite)
    {
        _art = null;
        _image.sprite = sprite;
        _isLocked = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (_isLocked) return; // can't equip here
        if (eventData.pointerDrag == null) return; // not dragging anything
        // not dragging a weapon art element
        if (!eventData.pointerDrag.TryGetComponent<WeaponArtElement>(out var element)) return;
        // need to check that the art is the correct weapon type

        // Probably change _manager to the WeaponManager so I can call a function there
        element.OnValidDrop();
        _manager.OnTryEquipArt(this, element.Art);
    }

    #region - Pointer -
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_art == null) return;
        _manager?.ShowTooltip(_art);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_art == null) return;
        _manager?.HideTooltip();
    }
    #endregion
}
