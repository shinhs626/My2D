using UnityEngine;
using UnityEngine.Events;

namespace My2D
{
    //Health를 관리하는 클래스, dmg, die 구현
    public class Damageable : MonoBehaviour
    {
        #region Variables
        //참조
        public Animator animator;

        //체력
        [SerializeField] private float playerHp;
        //초기 체력
        [SerializeField] private float startPlayerHp = 100f;
        //죽음 체크
        private bool isDeath = false;

        //무적 타이머
        private bool isInvincible = false;  //true이면 데미지를 입지 않는다
        [SerializeField]
        private float invincibleTime = 1f;
        private float countdown = 0f;

        //델리게이트 이벤트 함수 - 매개변수로 float, Vector2가 있는 함수 등록 가능
        public UnityAction<float, Vector2> hitAction;
        #endregion

        #region Property
        //체력
        public float PlayerHp
        {
            get
            {
                return playerHp;
            }
            private set
            {
                playerHp = value;

                if(playerHp <= 0)
                {
                    Die();
                }
            }
        }
        //초기 체력
        public float StartPlayerHp
        {
            get
            {
                return startPlayerHp;
            }
            private set
            {
                startPlayerHp = value;
            }
        }
        //죽음 체크
        public bool IsDeath
        {
            get
            {
                return isDeath;
            }
            set
            {
                isDeath = value;
            }
        }

        public bool LockVelocity
        {
            get
            {
                return animator.GetBool(AnimationString.lockVelocity);
            }
            set
            {
                animator.SetBool(AnimationString.lockVelocity, value);
            }
        }

        public bool IsHealthFull => PlayerHp >= StartPlayerHp;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            PlayerHp = StartPlayerHp;
        }

        private void Update()
        {
            if (isInvincible)
            {
                countdown += Time.deltaTime;
                if (countdown >= invincibleTime)
                {
                    isInvincible = false;
                    Debug.Log("무적 끝");
                }
            }
        }
        #endregion

        #region Custom Method
        //매개변수로 데미지와 넉백 값을 받아온다
        public bool TakeDamage(float damage, Vector2 knockback)
        {
            if (IsDeath || isInvincible)
            {
                return false;
            }

            PlayerHp -= damage;
            Debug.Log($"남은 체력 : {PlayerHp}");

            //무적 모드 셋팅
            isInvincible = true;
            countdown = 0f;

            //애니메이션
            animator.SetTrigger(AnimationString.hitTrigger);
            LockVelocity = true;

            //효과 : SFX, VFX, 넉백효과, UI 효과
            //델리게이트 함수에 등록된 함수 호출 : 효과 연출에 필요한 함수 등록
            //if(hitAction != null)
            //{
            //    hitAction.Invoke(damage, knockback);
            //}
            hitAction?.Invoke(damage, knockback);

            //UI 효과 - 데미지 text 프리펩 생성하는 이벤트 함수 호출
            CharacterEvents.characterDamaged?.Invoke(gameObject, damage);

            return true;
        }

        public void Die()
        {
            IsDeath = true;

            animator.SetBool(AnimationString.isDeath, true);
        }

        //Health 가산
        //public void Heal(float healAmount, GameObject player)
        //{
        //    //죽음 체크
        //    if (IsDeath)
        //    {
        //        return;
        //    }
        //    if (PlayerHp == StartPlayerHp)
        //        return;
        //    PlayerHp += healAmount;

        //    if (PlayerHp > StartPlayerHp)
        //    {
        //        PlayerHp = StartPlayerHp;
        //    }
        //    Debug.Log($"PlayerHp :{PlayerHp}");

        //    Destroy(player);
        //}

        //참을 반환하면 health를 실질적으로 충전, 거짓반환하면 hp가 풀이어서 충전하지 않았다
        public bool Heal(float healAmount)
        {
            //죽음 체크
            if (IsDeath || IsHealthFull)
            {
                return false;
            }
            float currentHp = PlayerHp;
            PlayerHp += healAmount;

            if (PlayerHp > StartPlayerHp)
            {
                PlayerHp = StartPlayerHp;
            }
            Debug.Log($"PlayerHp :{PlayerHp}");

            //UI 효과 - 힐 text 프리펩 생성하는 이벤트 함수 호출
            //Health  Text, Health Bar 관련 함수 호출
            CharacterEvents.characterHeald?.Invoke(gameObject, (playerHp - currentHp));

            return true;
        }
        #endregion
    }
}

