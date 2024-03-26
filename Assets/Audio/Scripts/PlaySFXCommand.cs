using UnityEngine;

namespace SD.CommandSystem
{
    [CreateAssetMenu(menuName = "Command System/Play SFX Command")]
    public class PlaySFXCommand : CommandBase
    {
        [SerializeField] private AudioClip _audioClip;
        protected override bool ExecuteCommand()
        {
            if (_audioClip == null) return false;
            AudioManager.PlaySFX(_audioClip);
            return true;
        }
    }
}