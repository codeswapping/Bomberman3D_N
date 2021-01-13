using System.Linq;
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
            Debug.Log("Triggers : " + triggers.Length);
            foreach (var trigger in triggers)
            {
                trigger.onTriggered += OnTriggered;
            }

            SetRandomStartPos();
            SetNextPos();
        }

        private void Update()
        {
            if (!IsWalking)
            {
                CheckCanWalk();
                return;
            }

            _currentWalkTime -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _nextPos, walkSpeed * Time.deltaTime);
            if (!(Vector3.Distance(transform.position, _nextPos) <= 0.01)) return;
            transform.position = _nextPos;
            SetNextPos();
        }

        private bool CheckCanWalk()
        {
            var canwalk = false;
            var dirc = "";
            var dint = -1;
            if (_currentWalkTime <= 0)
            {
                _currentWalkTime = Random.Range(minWalkTime, maxWalkTime);
                dint = Random.Range(0, 4);
                dirc += dint;
            }

            while (dirc.Length < 4)
            {
                while (dirc.Contains(dint.ToString()))
                {
                    dint = Random.Range(0, 4);
                }

                dirc += dint.ToString();
                _direction = (Direction) dint;

                switch (_direction)
                {
                    case Direction.LEFT:
                        _nextPos.x -= 1;

                        break;
                    case Direction.RIGHT:
                        _nextPos.x+=1;

                        break;
                    case Direction.TOP:
                        _nextPos.z += 1;


                        break;
                    case Direction.BOTTOM:
                        _nextPos.z -= 1;
                        if (IsWalkablePosition(_nextPos))
                        {
                            canwalk = true;
                        }

                        break;
                }
            }

            return canwalk;
        }

        private void SetRandomStartPos()
        {
            var p = Random.Range(10, GameManager.Instance.walkablePath.Count - 1);
            while (GameManager.Instance.walkablePath[p].isBrickWall)
            {
                p = Random.Range(10, GameManager.Instance.walkablePath.Count - 1);
            }

            transform.position = _nextPos = GameManager.Instance.walkablePath[p].position;
        }

        private void SetNextPos(Direction direction = Direction.UNSPECIFIED)
        {
            var canwalk = false;
            if (direction == Direction.UNSPECIFIED)
            {
                canwalk = CheckCanWalk();
            }
            else
            {
                switch (direction)
                {
                    case Direction.TOP:
                        _nextPos.z += 1;
                        
                        break;
                    case Direction.BOTTOM:
                        _nextPos.z -= 1;

                        break;
                    case Direction.LEFT:
                        _nextPos.x -= 1;
                        
                        break;
                    case Direction.RIGHT:
                        _nextPos.z += 1;

                        break;
                }

                if (IsWalkablePosition(_nextPos))
                {
                    canwalk = true;
                }

                _direction = direction;
            }

            IsWalking = canwalk;
        }

        private bool IsWalkablePosition(Vector3 pos)
        {
            //Debug.Log("Next Position : " + pos);
            WalkablePathInfo? v = null;
            foreach (var path in GameManager.Instance.walkablePath)
            {
                if (path.position.Equals(pos) && !path.isBrickWall)
                {
                    v = path;
                }
            }

            if (v != null)
            {
                _nextPos = pos;
                return true;
            }

            Debug.Log("Path is : " + v);

            _direction = (Direction) Random.Range(0, 4);
            return false;
        }

        private void OnTriggered(TiggerManager.TriggerDirection direction)
        {
            IsWalking = false;
            Debug.Log("Trigger entered : " + direction);
            switch (direction)
            {
                case TiggerManager.TriggerDirection.TOP:
                    SetNextPos(Direction.BOTTOM);
                    break;
                case TiggerManager.TriggerDirection.BOTTOM:
                    SetNextPos(Direction.TOP);
                    break;
                case TiggerManager.TriggerDirection.LEFT:
                    SetNextPos(Direction.RIGHT);
                    break;
                case TiggerManager.TriggerDirection.RIGHT:
                    SetNextPos(Direction.LEFT);
                    break;
            }
        }
    }
}