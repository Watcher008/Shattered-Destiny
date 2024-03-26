using UnityEngine;

namespace SD.CommandSystem
{
    [CreateAssetMenu(menuName = "Command System/Set Theme Command")]
    public class SetThemeCommand : CommandBase
    {
        [SerializeField] private AudioClip _audioClip;
        protected override bool ExecuteCommand()
        {
            if (_audioClip == null) return false;
            AudioManager.SetTheme(_audioClip);
            return true;
        }
    }
}