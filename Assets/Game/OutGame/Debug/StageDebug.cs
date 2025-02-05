using Confront.GameUI;
using Confront.Stage;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.Debugger
{
    public class StageDebug : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_Dropdown _stageDropdown;
        [SerializeField]
        private TMPro.TMP_InputField _startPointIndex;
        [SerializeField]
        private Button _stageChangeButton;

        private void Start()
        {
            _stageDropdown.ClearOptions();
            _stageDropdown.AddOptions(StageManager.StageNames);
            _stageChangeButton.onClick.AddListener(ChangeStage);
        }

        private void ChangeStage()
        {
            var stageName = _stageDropdown.options[_stageDropdown.value].text;
            var startPointIndex = int.Parse(_startPointIndex.text);

            StageManager.ChangeStage(
                stageName,
                startPointIndex,
                ScreenFader.Instance.FadeOut,
                ScreenFader.Instance.FadeIn).Forget();
        }
    }
}