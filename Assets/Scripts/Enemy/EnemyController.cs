using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public bool IsWalking;
        public float walkSpeed = 5f;
        public float minWalkTime = 2f, maxWalkTime = 5f;

        private Vector3 _nextPos;
        private float _currentWalkTime = 0f;
        private WalkDirection _direction;

        private void Start()
        {
            var triggers = GetComponentsInChildren<TiggerManager>();
            foreach (var trigger in triggers)
            {
                trigger.onTriggered += OnTriggered;
            }

            SetRandomStartPos();
            SetNextPos();
        }

        private void Update()
        {
            if (!IsWalking) return;
            _currentWalkTime -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _nextPos, walkSpeed * Time.deltaTime);
            if (!(Vector3.Distance(transform.position, _nextPos) <= 0.01)) return;
            transform.position = _nextPos;
            SetNextPos();
        }

        private void SetRandomStartPos()
        {
            transform.position =
                LevelGenerator.LevelGenerator.Instance.walkablePath[
                    Random.Range(0, LevelGenerator.LevelGenerator.Instance.walkablePath.Count - 1)];
        }

        private void SetNextPos(WalkDirection walkDirection = WalkDirection.UNSPECIFIED)
        {
            if (walkDirection == WalkDirection.UNSPECIFIED)
            {
                if (_currentWalkTime <= 0)
                {
                    _currentWalkTime = Random.Range(minWalkTime, maxWalkTime);
                    _direction = (WalkDirection) Random.Range(0, 4);
                }

                var canwalk = false;
                while (!canwalk)
                {
                    //Debug.Log("Walk Direction : " + _direction);
                    switch (_direction)
                    {
                        case WalkDirection.TOP:
                            if (IsWalkablePosition(new Vector3(transform.position.x, 0.4f, transform.position.z + 1)))
                            {
                                canwalk = true;
                            }

                            break;
                        case WalkDirection.BOTTOM:
                            if (IsWalkablePosition(new Vector3(transform.position.x, 0.4f, transform.position.z - 1)))
                            {
                                canwalk = true;
                            }

                            break;
                        case WalkDirection.LEFT:
                            if (IsWalkablePosition(new Vector3(transform.position.x - 1, 0.4f, transform.position.z)))
                            {
                                canwalk = true;
                            }

                            break;
                        case WalkDirection.RIGHT:
                            if (IsWalkablePosition(new Vector3(transform.position.x + 1, 0.4f, transform.position.z)))
                            {
                                canwalk = true;
                            }

                            break;
                    }

                    //Debug.Log("Walkable : " + canwalk);
                }
            }
            else
            {
                switch (walkDirection)
                {
                    case WalkDirection.TOP:
                        _nextPos = new Vector3(transform.position.x, 0.4f,
                            Mathf.FloorToInt(transform.position.z / LevelGenerator.LevelGenerator.Instance.rowCount) +
                            1);
                        break;
                    case WalkDirection.BOTTOM:
                        _nextPos = new Vector3(transform.position.x, 0.4f,
                            Mathf.FloorToInt(transform.position.z / LevelGenerator.LevelGenerator.Instance.rowCount) -
                            1);
                        break;
                    case WalkDirection.LEFT:
                        _nextPos = new Vector3(
                            Mathf.FloorToInt(transform.position.x /
                                             LevelGenerator.LevelGenerator.Instance.columnCount) - 1, 0.4f,
                            transform.position.z);
                        break;
                    case WalkDirection.RIGHT:
                        _nextPos = new Vector3(
                            Mathf.FloorToInt(transform.position.x /
                                             LevelGenerator.LevelGenerator.Instance.columnCount) + 1, 0.4f,
                            transform.position.z);
                        break;
                }

                _direction = walkDirection;
            }

            IsWalking = true;
        }

        private bool IsWalkablePosition(Vector3 pos)
        {
            //Debug.Log("Next Position : " + pos);
            if (LevelGenerator.LevelGenerator.Instance.walkablePath.Contains(pos))
            {
                _nextPos = pos;
                return true;
            }

            _direction = (WalkDirection) Random.Range(0, 4);
            return false;
        }

        private void OnTriggered(TiggerManager.TriggerDirection direction)
        {
            IsWalking = false;
            switch (direction)
            {
                case TiggerManager.TriggerDirection.TOP:
                    SetNextPos(WalkDirection.BOTTOM);
                    break;
                case TiggerManager.TriggerDirection.BOTTOM:
                    break;
                case TiggerManager.TriggerDirection.LEFT:
                    break;
                case TiggerManager.TriggerDirection.RIGHT:
                    break;
            }
        }

        private enum WalkDirection
        {
            UNSPECIFIED = -1,
            TOP = 0,
            BOTTOM = 1,
            LEFT = 2,
            RIGHT = 3
        }
    }
}