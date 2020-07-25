using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MesoGames.GUIUtils
{
    [ExecuteInEditMode]
    public class DynamicGridLayout : MonoBehaviour
    {
        protected void Awake()
        {
            _uiGrid = GetComponent< GridLayoutGroup >() as GridLayoutGroup;
            _uiRectTx = GetComponent< RectTransform >() as RectTransform;

            _uiGrid.cellSize = new Vector2( _uiRectTx.rect.width / _col, _uiRectTx.rect.height / _row );
        }
	
        protected void Update()
        {
	
        }

        [SerializeField] protected int _col = 1;
        [SerializeField] protected int _row = 1;

        private GridLayoutGroup _uiGrid = null;
        private RectTransform _uiRectTx = null;
        private float _height = 0.0f;
    }
}
