using R3;
using TMPro;
using UnityEngine;

namespace _Project.Game.Gameplay.UI
{
    public sealed class KillCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counterText;

        private readonly CompositeDisposable _disposables = new();

        public void Construct(KillCounter killCounter)
        {
            killCounter.Kills
                .Subscribe(UpdateText)
                .AddTo(_disposables);

            UpdateText(killCounter.Kills.Value);
        }

        private void UpdateText(int kills)
        {
            _counterText.text = $"Kills: {kills}";
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}