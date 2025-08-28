using UnityEngine;

namespace My2D
{
    //발사체를 관리하는 클래스
    //기능 : 이동하기(rd2D, LinearVelocity), 충돌하기, 데미지 입히기
    public class Projectile : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody2D rb2D;

        //화살의 이동속도 - 좌우로 이동, 위아래로 이동하지 않는다,
        [SerializeField] private Vector2 moveVelocity;

        //데미지 주기
        [SerializeField] private float projectileDamage = 20f;
        [SerializeField] private Vector2 knockback;

        //데미지 효과 Sfx, Vfx
        public GameObject projectEffectPrefab;
        public Transform effectPos;
        
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = this.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            //초기화
            rb2D.linearVelocity = new Vector2(moveVelocity.x * this.transform.localScale.x, moveVelocity.y);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("적을 맞혔습니다.");
            Damageable damageable = collision.GetComponent<Damageable>();
            if(damageable)
            {
                //밀리는 방향 구하기
                Vector2 deliveredKnokback = this.transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                bool isHit = damageable.TakeDamage(projectileDamage, knockback);

                //데미지를 주었으면
                if(isHit)
                {
                    //SFX, VFX
                    GameObject effectGo = Instantiate(projectEffectPrefab, effectPos.position, Quaternion.identity);
                    Destroy(effectGo, 0.4f);
                }
            }

            //화살 킬
            Destroy(gameObject);
        }
        #endregion
    }
}
