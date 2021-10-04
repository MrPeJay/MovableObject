using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MovableObject.Extensions
{
    public static class MovableComponentExtension
    {
        public struct ComponentData
        {
            public ComponentType ComponentType;

            //Should component be received from object's parent.
            public bool RetrievedFromParent;

            public ComponentData(ComponentType componentType, bool retrievedFromParent)
            {
                ComponentType = componentType;
                RetrievedFromParent = retrievedFromParent;
            }
        }

        public static ComponentData GetComponentData(this Transform transform)
        {
            return new ComponentData(ComponentType.Transform, false);
        }

        public static ComponentData GetComponentData(this RectTransform rectTransform)
        {
            return new ComponentData(ComponentType.RectTransform, false);
        }

        public static ComponentData GetComponentData(this CanvasScaler canvasScaler)
        {
            return new ComponentData(ComponentType.RectTransform, true);
        }

        public static ComponentData GetComponentData(this CanvasGroup canvasGroup)
        {
            return new ComponentData(ComponentType.CanvasGroup, false);
        }

        public static ComponentData GetComponentData(this Image image)
        {
            return new ComponentData(ComponentType.Image, false);
        }

        public static ComponentData GetComponentData(this MeshRenderer meshRenderer)
        {
            return new ComponentData(ComponentType.MeshRenderer, false);
        }

        public static ComponentData GetComponentData(this SpriteRenderer spriteRenderer)
        {
            return new ComponentData(ComponentType.SpriteRenderer, false);
        }

        /// <summary>
        /// Returns all types which are part of the movable components.
        /// To add a component, just add component type in the array, create a component type and assign
        /// component type with an extension method as shown above.
        /// </summary>
        /// <returns></returns>
        public static Type[] GetMovableComponentTypes()
        {
            return new Type[]
            {
                typeof(Transform), typeof(RectTransform), typeof(CanvasScaler), typeof(CanvasScaler),
                typeof(CanvasGroup), typeof(Image), typeof(MeshRenderer), typeof(SpriteRenderer)
            };
        }

        /// <summary>
        /// Returns all types that implement IMovableActionComponent interface that aren't abstract.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetIMovableActionTypes()
        {
            var implementedInterface = typeof(IMovableActionComponent);

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes()).Where(p =>
                    implementedInterface.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
        }

        public enum ComponentType
        {
            None = 0,
            Transform = 1,
            RectTransform = 2,
            Image = 3,
            CanvasGroup = 4,
            MeshRenderer = 5,
            SpriteRenderer = 6
        }
    }
}