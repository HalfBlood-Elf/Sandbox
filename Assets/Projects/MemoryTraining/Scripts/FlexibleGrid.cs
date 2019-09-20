using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryTraining
{
    public class FlexibleGrid : MonoBehaviour
    {
        [Range(0, 10)]
        public byte columns, rows;

        private RectTransform rectTransform;
        private GridLayoutGroup grid;
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            grid = GetComponent<GridLayoutGroup>();
            SetGrid();
        }
        public void SetGrid() {
            if (columns != 0 && rows != 0)
            {
                grid.constraintCount = columns;
                grid.cellSize = new Vector2(
                    rectTransform.rect.width / columns - grid.spacing.x,
                    rectTransform.rect.height / rows - grid.spacing.y);

            }
            else if (rows == 0)
            {
                grid.constraintCount = columns;
                grid.cellSize = new Vector2(
                    rectTransform.rect.width / columns - grid.spacing.x,
                    rectTransform.rect.width / columns - grid.spacing.y);
            }
        }
        private void Update()
        {
            if (rows == 0)
            {
                float height = transform.childCount / columns;
                height *= rectTransform.rect.width / columns + grid.spacing.y;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
            }
        }
    }
        
}

