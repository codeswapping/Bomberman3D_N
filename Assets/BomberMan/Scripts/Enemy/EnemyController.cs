using System.Collections;
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
        public Renderer render;

        private Vector3 _nextPos;
        private float _currentWalkTime = 0f;
        private Direction _direction;
        private bool isDead = false;

        private void Start()
        {
            var triggers = GetComponentsInChildren<TiggerManager>();
            //Debug.Log("Triggers : " + triggers.Length);
            foreach (var trigger in triggers)
            {
                trigger.onTriggered += OnTriggered;
            }

            SetRandomStartPos();
            SetNextPos();
        }

        private void Update()
        {
            if (isDead)
                return;
            if (!IsWalking)
            {
                CheckCanWalk();
                return;
            }

            _currentWalkTime -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _nextPos, walkSpeed * Time.deltaTime);
            if (!(Vector3.Distance(transform.position, _nextPos) <= 0.05)) return;
            transform.position = _nextPos;
            SetNextPos();
        }

        private bool CheckCanWalk(Direction d = Direction.UNSPECIFIED)
        {
            var canwalk = false;
            var dirc = "";
            var dint = (int)d;
            var tempNextPos = _nextPos;
            if (_currentWalkTime <= 0)
            {
                _currentWalkTime = Random.Range(minWalkTime, maxWalkTime);
            }
            if(dint == -1)
            {
                dint = Random.Range(0, 4);
            }
            dirc += dint.ToString();
            _direction = (Direction)dint;

            while (dirc.Length < 4)
            {
                switch (_direction)
                {
                    case Direction.LEFT:
                        tempNextPos.x -= 1;

                        break;
                    case Direction.RIGHT:
                        tempNextPos.x+=1;

                        break;
                    case Direction.TOP:
                        tempNextPos.z += 1;

                        break;
                    case Direction.BOTTOM:
                        tempNextPos.z -= 1;

                        break;
                }
                if (IsWalkablePosition(tempNextPos))
                {
                    _nextPos = tempNextPos;
                    canwalk = true;
                    break;
                }
                else
                {
                    while (dirc.Contains(dint.ToString()))
                    {
                        dint = Random.Range(0, 4);
                    }
                    dirc += dint.ToString();
                    _direction = (Direction)dint;
                    tempNextPos = _nextPos;
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
            //Debug.Log("Random Start Pos : " + GameManager.Instance.walkablePath[p].position);
        }

        private void SetNextPos(Direction direction = Direction.UNSPECIFIED)
        {
            IsWalking = CheckCanWalk(direction);
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
                return true;
            }

            //Debug.Log("Path is : " + v);
            return false;
        }

        private void OnTriggered(TiggerManager.TriggerDirection direction)
        {
            IsWalking = false;
            //Debug.Log("Trigger entered : " + direction);
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

        public void StartDissolve()
        {
            isDead = true;
            StartCoroutine("Dissolve");
        }

        IEnumerator Dissolve()
        {
            var dis = 0f;
            while (dis < 1f)
            {
                dis += Time.deltaTime * 2f;
                render.material.SetFloat("_Level", dis);
                yield return 0;
            }
            Destroy(gameObject);
        }
    }
}