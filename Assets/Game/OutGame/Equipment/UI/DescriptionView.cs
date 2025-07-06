using UnityEngine;
using UnityEngine.UI;

namespace Confront.Equipment.UI
{
    public class DescriptionView : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _nameText;
        [SerializeField]
        private Image _iconImage;
        [SerializeField]
        private TMPro.TextMeshProUGUI _descriptionText;

        public void SetView(EquipmentItem item)
        {
            if (item != null)
            {
                _nameText.text = item.Data.EquipmentName;
                _iconImage.sprite = item.Data.Icon;
                _descriptionText.text = item.Data.Description;
            }
            else
            {
                _nameText.text = string.Empty;
                _iconImage.sprite = null;
                _descriptionText.text = string.Empty;
            }
        }
    }
}