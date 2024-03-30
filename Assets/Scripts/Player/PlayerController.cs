using UnityEngine;
using SD.Grids;
using System.Collections;
using System.Collections.Generic;
using SD.Characters;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    private readonly float _moveTime = 0.5f;

    private Transform _transform;
    private PathNode _currentNode;
    private Coroutine _pathingCoroutine;

    private bool _canAct = true;
    public bool CanAct
    {
        get => _canAct;
        set => _canAct = value;
    }

    private void Start()
    {
        _transform = transform;
        _currentNode = WorldMap.GetNode(_transform.position);
        _transform.position = WorldMap.GetNodePosition(_currentNode.X, _currentNode.Y);

        GetComponentInChildren<SpriteRenderer>().sprite = _playerData.Sprite;

        WorldMapManager.Instance.onPauseInput += PauseInput;
        WorldMapManager.Instance.onResumeInput += ResumeInput;
    }

    private void OnDestroy()
    {
        WorldMapManager.Instance.onPauseInput -= PauseInput;
        WorldMapManager.Instance.onResumeInput -= ResumeInput;
    }

    /// <summary>
    /// Pauses player input until a UI panel has been closed.
    /// </summary>
    private void PauseInput()
    {
        _canAct = false;
        if (_pathingCoroutine != null) StopCoroutine(_pathingCoroutine);
    }

    /// <summary>
    /// Resumes player control of their character.
    /// </summary>
    private void ResumeInput()
    {
        _canAct = true;
    }

    public void SetDestination(PathNode node)
    {
        if (!_canAct) return;

        if (node == null) return;
        var path = Pathfinding.FindNodePath(_currentNode, node);
        
        if (path == null) return;

        if (path[0] == _currentNode) path.RemoveAt(0);
        if (_pathingCoroutine != null) StopCoroutine(_pathingCoroutine);
        _pathingCoroutine = StartCoroutine(FollowPath(path));
    }

    private IEnumerator FollowPath(List<PathNode> path)
    {
        while (path.Count > 0)
        {
            var next = path[0];
            path.RemoveAt(0);

            var end = WorldMap.GetNodePosition(next.X, next.Y);
            float t = 0f;
            while (t < _moveTime)
            {
                t += Time.deltaTime;
                _transform.position = Vector3.Lerp(_transform.position, end, t / _moveTime);
                yield return null;
            }
            _transform.position = end;

            _currentNode = next;
            _playerData.WorldPos = new Vector2Int(next.X, next.Y);

            WorldMapManager.Instance.OnPlayerActed(_currentNode);

            yield return null;
        }
    }
}
