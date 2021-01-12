using UnityEngine;
using Random = UnityEngine.Random;

namespace BomberMan.Scripts.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public bool IsWalking;
        public float walkSpeed = 5f;
        public float minWalkTime = 2f, maxWalkTime = 5f;

        private Vector3 _nextPos;
        private float _currentWalkTime = 0f;
        private Direction _direction;

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
            transform.position = GameManager.Instance.walkablePath[
                    Random.Range(0, GameManager.Instance.walkablePath.Count - 1)];
        }

        private void SetNextPos(Direction direction = Direction.UNSPECIFIED)
        {
            if (direction == Direction.UNSPECIFIED)
            {
                if (_currentWalkTime <= 0)
                {
                    _currentWalkTime = Random.Range(minWalkTime, maxWalkTime);
                    _direction = (Direction) Random.Range(0, 4);
                }

                var canwalk = false;
                while (!canwalk)
                {
                    //Debug.Log("Walk Direction : " + _direction);
                    switch (_direction)
                    {
                        case Direction.TOP:
                            if (IsWalkablePosition(new Vector3(transform.position.x, 0.4f, transform.position.z + 1)))
                            {
                                canwalk = true;
                            }

                            break;
                        case Direction.BOTTOM:
                            if (IsWalkablePosition(new Vector3(transform.position.x, 0.4f, transform.position.z - 1)))
                            {
                                canwalk = true;
                            }

                            break;
                        case Direction.LEFT:
                            if (IsWalkablePosition(new Vector3(transform.position.x - 1, 0.4f, transform.position.z)))
                            {
                                canwalk = true;
                            }

                            break;
                        case Direction.RIGHT:
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
                switch (direction)
                {
                    case Direction.TOP:
                        _nextPos = new Vector3(transform.position.x, 0.4f,
                            Mathf.FloorToInt(transform.position.z / GameManager.Instance.rowCount) +
                            1);
                        break;
                    case Direction.BOTTOM:
                        _nextPos = new Vector3(transform.position.x, 0.4f,
                            Mathf.FloorToInt(transform.position.z / GameManager.Instance.rowCount) -
                            1);
                        break;
                    case Direction.LEFT:
                        _nextPos = new Vector3(
                            Mathf.FloorToInt(transform.position.x /
                                             GameManager.Instance.columnCount) - 1, 0.4f,
                            transform.position.z);
                        break;
                    case Direction.RIGHT:
                        _nextPos = new Vector3(
                            Mathf.FloorToInt(transform.position.x /
                                             GameManager.Instance.columnCount) + 1, 0.4f,
                            transform.position.z);
                        break;
                }

                _direction = direction;
            }

            IsWalking = true;
        }

        private bool IsWalkablePosition(Vector3 pos)
        {
            //Debug.Log("Next Position : " + pos);
            if (GameManager.Instance.walkablePath.Contains(pos))
            {
                _nextPos = pos;
                return true;
            }

            _direction = (Direction) Random.Range(0, 4);
            return false;
        }

        private void OnTriggered(TiggerManager.TriggerDirection direction)
        {
            IsWalking = false;
            switch (direction)
            {
                case TiggerManager.TriggerDirection.TOP:
                    SetNextPos(Direction.BOTTOM);
                    break;
                case TiggerManager.TriggerDirection.BOTTOM:
                    break;
                case TiggerManager.TriggerDirection.LEFT:
                    break;
                case TiggerManager.TriggerDirection.RIGHT:
                    break;
            }
        }


    }
}