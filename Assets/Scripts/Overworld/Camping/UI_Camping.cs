using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SD.Characters;

public class UI_Camping : MonoBehaviour
{
    public bool IsOpen => _panel.activeSelf;

    [SerializeField] private PlayerData _playerData;

    [Space]

    [SerializeField] private GameObject _panel;

    [SerializeField] private Button _restButton;
    [SerializeField] private Button _trainButton;
    [SerializeField] private Button _patrolButton;
    [SerializeField] private Button _guildButton;
    [SerializeField] private Button _huntFishButton;
    [SerializeField] private Button _leaveButton;

    private Camp _currentCamp;

    private bool _resting = false;
    private TMP_Text _restText;
    private Coroutine _restCoroutine;
    private readonly int[] _recoveryRate = { 2, 4, 8 };
    private readonly int[] _exhaustionRecoveryTime = { 12, 6, 3 };

    private bool _training = false;

    private void Awake()
    {
        _restButton.onClick.AddListener(OnRest);
        _trainButton.onClick.AddListener(OnTrain);
        _patrolButton.onClick.AddListener(OnPatrol);
        _guildButton.onClick.AddListener(OnGuild);
        _huntFishButton.onClick.AddListener(OnHuntFish);
        _leaveButton.onClick.AddListener(ClosePanel);

        _restText = _restButton.GetComponentInChildren<TMP_Text>();
    }

    private void OnDestroy()
    {
        _restButton.onClick.RemoveAllListeners();
        _trainButton.onClick.RemoveAllListeners();
        _patrolButton.onClick.RemoveAllListeners();
        _guildButton.onClick.RemoveAllListeners();
        _huntFishButton.onClick.RemoveAllListeners();
        _leaveButton.onClick.RemoveAllListeners();
    }

    public void OpenPanel(Camp camp)
    {
        Debug.Log($"Setting up a {camp} camp");
        _currentCamp = camp;
        _panel.SetActive(true);
    }

    private void ClosePanel()
    {
        _panel.SetActive(false);
        WorldMapManager.Instance.onResumeInput?.Invoke();
    }

    private void OnRest()
    {
        _resting = !_resting;

        if (_resting)
        {
            _restText.text = "Stop Resting";
            Debug.Log("Resting");
            // Time begins moving at a normal rate
            // Party begins healing
            // Each hour? roll for a chance of enemy attack

            if (_restCoroutine != null) StopCoroutine(_restCoroutine);
            _restCoroutine = StartCoroutine(RestCoroutine());
        }
        else
        {
            _restText.text = "Rest";
        }
    }

    private void OnTrain()
    {
        Debug.Log("Training");
    }

    private void OnPatrol()
    {
        Debug.Log("Patrolling");
    }

    private void OnGuild()
    {
        Debug.Log("Guilding");
    }

    private void OnHuntFish()
    {
        Debug.Log("Hunting fish");
    }

    private IEnumerator RestCoroutine()
    {
        int hoursPassed = 0;

        while (_resting)
        {
            yield return new WaitForSeconds(2.5f);

            DateTime.IncrementHour();
            hoursPassed++;

            // Roll for chance of random encounter
            if (Random.value <= 0.10f)
            {
                Debug.Log("The party has been ambushed!");
                _resting = false;
                // This should then also lead right into combat
                yield break;
            }

            // health regenerates each hour, 2x/4x/8x
            _playerData.OnRest(2);

            // exhaustion regenerates based on camp style
            if (hoursPassed % _exhaustionRecoveryTime[(int)_currentCamp] == 0)
            {
                // Decrease exhaustion by 1
                _playerData.Exhaustion--;
            }

            yield return null;
        }
    }

    private IEnumerator TrainStatCoroutine(Attributes stat)
    {
        while (_training)
        {
            yield return new WaitForSeconds(2.5f);

            DateTime.IncrementHour();

            // Roll for chance of random encounter
            if (Random.value <= 0.10f)
            {
                Debug.Log("The party has been ambushed!");
                _training = false;
                // This should then also lead right into combat
                yield break;
            }

            // Grant some exp to the relevant stat
            _playerData.PlayerStats.GainXP(stat, 10);

            yield return null;
        }
    }

    private IEnumerator TrainSkillCoroutine()
    {
        while (_training)
        {
            yield return new WaitForSeconds(2.5f);

            DateTime.IncrementHour();

            // Roll for chance of random encounter
            if (Random.value <= 0.10f)
            {
                Debug.Log("The party has been ambushed!");
                _training = false;
                // This should then also lead right into combat
                yield break;
            }

            // Grant some exp to the relevant skill

            yield return null;
        }
    }
}

public enum Camp
{
    Makeshift,
    Basic,
    Advanced,
}
