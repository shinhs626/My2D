using UnityEditor.Tilemaps;
using UnityEngine;

namespace My2D
{
    //이동 방향
    public enum WalkableDirection
    {
        Left,
        Right
    }

    //적 캐릭터를 관리하는 클래스
    [RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody2D rb2D;
        private TouchingDirection touchingDirection;
        public Animator animator;
        public DetectionZone detectionZone; //플레이어 감지
        private Damageable damageable;
        public DetectionZone cliffeDetection;

        //이동 속도
        [SerializeField] private float walkSpeed = 4f;

        //이동 방향 Vector
        private Vector2 directionVector = Vector2.right;

        //현재 이동 방향을 저장
        private WalkableDirection walkDirection = WalkableDirection.Right;

        //정지
        private float stopRate = 0.2f;

        //적 감지
        private bool hasTarget = false;
        #endregion

        #region Property
        public WalkableDirection WalkDirection
        {
            get
            {
                return walkDirection;
            }
            set
            {   
                //방향전환이 일어나는 시점
                if(walkDirection != value)
                {
                    //반대방향을 바라보도록 한다 - 이미지 플립
                    this.transform.localScale *= new Vector2(-1, 1);

                    //반대쪽으로 위치 이동하라
                    if(value == WalkableDirection.Left)
                    {
                        directionVector = Vector2.left;
                    }
                    else if(value == WalkableDirection.Right)
                    {
                        directionVector = Vector2.right;
                    }
                }

                walkDirection = value;
            }
        }

        //이동 제한 - 애니메이터 파라미터값 읽어오기
        public bool CannotMove
        {
            get
            {
                return animator.GetBool(AnimationString.cannotMove);
            }

        }

        //적 감지
        public bool HasTarget
        {
            get
            {
                return hasTarget;
            }
            set
            {
                hasTarget = value;
                animator.SetBool(AnimationString.hasTarget, value);
            }
        }

        //공격 쿨타임 : 읽어들여서 0보다 크면 3초 타이머를 돌려 0으로 다시 파라미터 값을 셋팅
        public float CooldownTime
        {
            get
            {
                return animator.GetFloat(AnimationString.cooldownTime);
            }
            set
            {
                animator.SetFloat(AnimationString.cooldownTime, value);
            }
        }
        #endregion


        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = this.GetComponent<Rigidbody2D>();
            touchingDirection = this.GetComponent<TouchingDirection>();

            damageable = this.GetComponent<Damageable>();
            //델리게이트 함수 등록
            damageable.hitAction += OnHit;

            //cliffeDetection 이벤트 함수 등록
            cliffeDetection.noColliderRamain += Flip;
        }

        private void Update()
        {
            //적 감지
            HasTarget = (detectionZone.detectedColliders.Count > 0);

            //CooldownTimer
            if (CooldownTime > 0)
            {
                CooldownTime -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            //벽을 만났을때 방향전환하여 이동한다
            if(touchingDirection.IsWall && touchingDirection.IsGround)
            {
                Flip();
            }

            //좌우 이동
            if (damageable.LockVelocity == false)
            {
                if (CannotMove)
                {
                    rb2D.linearVelocity = new Vector2(Mathf.Lerp(rb2D.linearVelocityX, 0f, stopRate), rb2D.linearVelocityY);
                }
                else
                {
                    rb2D.linearVelocity = new Vector2(directionVector.x * walkSpeed, rb2D.linearVelocityY);
                }
            }
        }
        #endregion

        #region Custom Method
        //방향전환
        void Flip()
        {
            if(WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else if(WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else
            {
                Debug.Log("방향 전환 에러");
            }
        }

        //데미지 입을때 호출되는 함수 - 데미지 입을때의 속도 셋팅
        public void OnHit(float damage, Vector2 knockback)
        {
            rb2D.linearVelocity = new Vector2(knockback.x, rb2D.linearVelocityY + knockback.y);
        }
        #endregion

    }
}
