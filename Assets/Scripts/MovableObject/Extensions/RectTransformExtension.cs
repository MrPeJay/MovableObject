using System;
using UnityEngine;
using UnityEngine.UI;

namespace MovableObject.Extensions
{
    public static class RectTransformExtension
    {
        /// <summary>
        /// Default screen positions
        /// </summary>
        /// <param name="window"></param>
        /// <param name="pos"></param>
        /// <param name="parentCanvas"></param>
        /// <returns></returns>
        public static Vector2 GetScreenPosition(this RectTransform window, ScreenPosition pos,
            CanvasScaler parentCanvas = null)
        {
            switch (pos)
            {
                case ScreenPosition.Left:
                    return new Vector2(
                        parentCanvas == null ? -Screen.width : -Screen.width / parentCanvas.GetScale(),
                        window.anchoredPosition.y);
                case ScreenPosition.Right:
                    return new Vector2(
                        parentCanvas == null ? Screen.width : Screen.width / parentCanvas.GetScale(),
                        window.anchoredPosition.y);
                case ScreenPosition.Center:
                    return new Vector2(0, 0);
                case ScreenPosition.Current:
                    return window.anchoredPosition;
                case ScreenPosition.Down:
                    return new Vector2(window.anchoredPosition.x,
                        parentCanvas == null
                            ? -Screen.height
                            : -Screen.height / parentCanvas.GetScale());
                case ScreenPosition.Up:
                    return new Vector2(window.anchoredPosition.x,
                        parentCanvas == null
                            ? Screen.height
                            : Screen.height / parentCanvas.GetScale());
                default:
                    throw new ArgumentOutOfRangeException(nameof(pos), pos, null);
            }
        }
    }

    public static class CanvasScalerExtension
    {
        public static float GetScale(this CanvasScaler scaler)
        {
            return Mathf.Pow(Screen.width / scaler.referenceResolution.x, 1f - scaler.matchWidthOrHeight) *
                   Mathf.Pow(Screen.height / scaler.referenceResolution.y, scaler.matchWidthOrHeight);
        }
    }

    public enum ScreenPosition
    {
        Left,
        Right,
        Center,
        Current,
        Up,
        Down
    }
}