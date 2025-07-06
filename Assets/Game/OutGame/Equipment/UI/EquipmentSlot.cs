using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Confront.Equipment.UI
{
    public class EquipmentSlot : MonoBehaviour, ISelectHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;

        private EquipmentItem target;

        public event System.Action<EquipmentSlot> OnClicked;
        public event System.Action<EquipmentSlot> OnSelected;

        private void Awake() => button.onClick.AddListener(() => OnClicked?.Invoke(this));
        public void OnSelect(BaseEventData eventData) => OnSelected?.Invoke(this);

        public EquipmentItem Target
        {
            get => target;
            set
            {
                target = value;
                if (target != null)
                {
                    image.sprite = target.Data.Icon;
                    image.color = Color.white;
                }
                else
                {
                    image.sprite = null;
                    image.color = new Color(1, 1, 1, 0);
                }
            }
        }
    }
}