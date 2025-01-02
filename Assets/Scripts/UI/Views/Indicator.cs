using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Views
{
    public class Indicator : MonoBehaviour
    {
        [SerializeField] Text _pointText;
        [SerializeField] GameObject _slider;

        private Image _sliderImage;

        void Awake()
        {
            _sliderImage = _slider.GetComponent<Image>();
        }

        public void SetPoints(float points, float maxPoints, string textNum)
        {
            _sliderImage.fillAmount = points / maxPoints;
            _pointText.text = textNum;
        }
        public void AddPoints(float points, float maxPoints, string textNum)
        {
            _sliderImage.fillAmount += points / maxPoints;
            _pointText.text = textNum;
        }
        public void TakeAwayPoints(float points, float maxPoints, string textNum)
        {
            _sliderImage.fillAmount -= points / maxPoints;
            _pointText.text = textNum;
        }
        public void Set(string current, string max)
        {
            _pointText.text = current + "/" + max;
        }
        public void Set(string current)
        {
            _pointText.text = current;
        }
    }
}