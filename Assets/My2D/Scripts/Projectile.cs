using UnityEngine;

namespace My2D
{
    //발사체를 관리하는 클래스
    //발사체 기능 : 이동하기(rb2D, LinearVelocity), 충돌하기. 데미지 입히기
    public class Projectile : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody2D rb2D;

        public DetectionZone detectionZone;

        //이동속도  -   좌, 우로 이동
        [SerializeField] private Vector2 moveSpeed;

        //데미지
        [SerializeField] private float arrowDamage = 10f;
        //넉백
        [SerializeField] private Vector2 knockback = Vector2.zero;

        //데미지 효과
        public GameObject effectTile;

        //화살 끝지점
        public Transform arrowEnd;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = this.transform.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            //초기화
            rb2D.linearVelocity = new Vector2(moveSpeed.x * this.transform.localScale.x, moveSpeed.y);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //if (animator.GetBool(AnimationString.isWall, ))
            //{
            //    Destroy(gameObject);
            //}

            //Debug.Log("화살 충돌");
            Damageable damageable = collision.GetComponent<Damageable>();

            if (damageable != null)
            {
                Vector2 deliveredKnockback = this.transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                bool isHit = damageable.TakeDamage(arrowDamage, deliveredKnockback);

                if (isHit)
                {
                    //Debug.Log(collision.name + " hit for " + arrowDamage);
                    ArrowEffect();
                }
                Kill();
            }
        }
        #endregion

        #region Custom Method
        public void ArrowEffect()
        {
            GameObject effectGameObject = Instantiate(effectTile, arrowEnd.position, Quaternion.identity);
            Destroy(effectGameObject, 0.4f);
        }

        public void Kill()
        {
            Destroy(gameObject, 3f);
        }
        #endregion
    }

}
