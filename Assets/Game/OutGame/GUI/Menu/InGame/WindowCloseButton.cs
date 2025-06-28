using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.GUI
{
    public class WindowCloseButton : DeviceSpecificUI
    {
        protected override void Start()
        {
            base.Start();
            GetComponent<Button>().onClick.AddListener(CloseWindow);
        }

        private void CloseWindow()
        {
            MenuController.Instance.CloseMenu();
        }
    }
}