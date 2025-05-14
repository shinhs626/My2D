using TMPro;
using UnityEngine;

namespace My2D
{
    //Damage가 위쪽으로 이동
    //Damage 페이드 아웃 효과, 페이드 아웃 효과 후 킬 - text의 컬러값으로 페이드 효과
    public class HealthText : MonoBehaviour
    {
        #region Variables
        //이동
        private RectTransform textTransform;
        [SerializeField]private float moveSpeed = 10f;

        //페이드 아웃 효과
        private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private float fadeTime = 1f;
        private float fadeCount = 0f;

        private Color startColor;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            textTransform = this.GetComponent<RectTransform>();
            textMeshProUGUI = textTransform.transform.GetComponent<TextMeshProUGUI>();

            //초기화
            startColor = textMeshProUGUI.color;
        }

        private void Update()
        {
            //이동
            textTransform.position += Vector3.up * Time.deltaTime * moveSpeed;

            fadeCount += Time.deltaTime;
            float newAlpha = 1 - fadeCount / fadeTime;

            textMeshProUGUI.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            if(fadeCount >= fadeTime)
            {
                Destroy(this.gameObject);
            }
        }
        #endregion
    }

}
