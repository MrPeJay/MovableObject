using System;
using System.Collections.Generic;
using System.Linq;
using MovableObject.Actions;
using MovableObject.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace MovableObject.Staging
{
    public static class MovableStageBase
    {
        public struct ComponentData
        {
            public List<Component> Components;
            public MovableComponentExtension.ComponentType ComponentType;

            public ComponentData(List<Component> components, MovableComponentExtension.ComponentType componentType)
            {
                Components = components;
                ComponentType = componentType;
            }
        }

        /// <summary>
        /// Returns a specified type action. Automatically assigns a specific component type action.
        /// </summary>
        /// <param name="movableObjectBehaviour"></param>
        /// <param name="actionType"></param>
        public static MovableAction GetAction(MonoBehaviour movableObjectBehaviour, MovableAction.ActionType actionType)
        {
            var components = GetObjectComponentTypes(movableObjectBehaviour);

            var types = MovableComponentExtension.GetIMovableActionTypes();

            foreach (var type in types)
            {
                //Create an instant with a default constructor.
                var instance = (IMovableActionComponent) Activator.CreateInstance(type);

                if (instance.Type() != actionType) continue;

                var componentCount = components.Length;

                for (var i = 0; i < componentCount; i++)
                {
                    var component = components[i];

                    if (instance.GetComponentType() != component.ComponentType) continue;

                    var movableAction = (MovableAction)Activator.CreateInstance(type);

                    if (movableAction is IMovableComponent movableComponent)
                    {
                        movableComponent.AssignComponents(component.Components.ToArray());

                        return movableComponent as MovableAction;
                    }

                    Debug.LogError(
                        $"Action of type {movableAction.GetType()} doesn't implement {typeof(IMovableComponent)} interface." +
                        $" Please make sure that it does.");
                }
            }

            return null;
        }

        /// <summary>
        /// Returns all available components assigned on the object with their specified component types
        /// which are described in MovableComponentExtension class.
        /// </summary>
        /// <param name="movableObjectBehaviour"></param>
        /// <returns></returns>
        private static ComponentData[] GetObjectComponentTypes(
            Component movableObjectBehaviour)
        {
            var availableComponentTypes = MovableComponentExtension.GetMovableComponentTypes();

            var componentDataList = new List<ComponentData>();

            //Check for object components.
            AddComponents(ref componentDataList, movableObjectBehaviour.GetComponents<Component>(),
                availableComponentTypes, false);

            //Check for parent components.
            AddComponents(ref componentDataList, movableObjectBehaviour.GetComponentsInParent<Component>(),
                availableComponentTypes, true);

            //Force add Transform component as it is a part of every game object.
            componentDataList.Add(new ComponentData(
                new List<Component>() {movableObjectBehaviour.GetComponent<Transform>()},
                MovableComponentExtension.ComponentType.Transform));

            return componentDataList.ToArray();
        }

        /// <summary>
        /// Adds all specified object components that match the criteria.
        /// </summary>
        /// <param name="componentDataList"></param>
        /// <param name="objectComponents"></param>
        /// <param name="movableObjectTypes"></param>
        /// <param name="retrieveFromParent"></param>
        private static void AddComponents(ref List<ComponentData> componentDataList, IReadOnlyList<Component> objectComponents,
            Type[] movableObjectTypes, bool retrieveFromParent)
        {
            var objectComponentCount = objectComponents.Count;

            for (var i = 0; i < objectComponentCount; i++)
            {
                var currentComponent = objectComponents[i];

                if (!movableObjectTypes.Contains(currentComponent.GetType()))
                    continue;

                //Get component data using reflection.
                var method =
                    typeof(MovableComponentExtension).GetMethod("GetComponentData",
                        new Type[] {currentComponent.GetType()});

                if (method == null)
                {
                    Debug.LogError(
                        $"Couldn't find extension method GetComponentType of type: {currentComponent.GetType()}." +
                        $" Please implement extension method in class {typeof(MovableComponentExtension)}");
                }
                else
                {
                    var componentData =
                        (MovableComponentExtension.ComponentData) method.Invoke(currentComponent,
                            new object[] {currentComponent});

                    if (retrieveFromParent != componentData.RetrievedFromParent)
                        continue;

                    var componentDataCount = componentDataList.Count;

                    if (componentDataCount == 0)
                    {
                        componentDataList.Add(new ComponentData(new List<Component>() {currentComponent},
                            componentData.ComponentType));
                    }
                    else
                    {
                        var added = false;

                        for (var j = 0; j < componentDataCount; j++)
                        {
                            var currentComponentData = componentDataList[j];

                            if (currentComponentData.ComponentType != componentData.ComponentType) continue;

                            currentComponentData.Components.Add(currentComponent);
                            added = true;
                        }

                        if (!added)
                        {
                            componentDataList.Add(new ComponentData(new List<Component>() {currentComponent},
                                componentData.ComponentType));
                        }
                    }
                }
            }
        }

        public static Component[] GetActionComponents(MonoBehaviour movableObjectBehaviour,
            MovableComponentExtension.ComponentType componentType)
        {
            switch (componentType)
            {
                case MovableComponentExtension.ComponentType.None:
                    return null;
                case MovableComponentExtension.ComponentType.Transform:
                    return new Component[] {movableObjectBehaviour.GetComponent<Transform>()};
                case MovableComponentExtension.ComponentType.RectTransform:
                    return new Component[]
                    {
                        movableObjectBehaviour.GetComponent<RectTransform>(),
                        movableObjectBehaviour.GetComponentInParent<CanvasScaler>()
                    };
                case MovableComponentExtension.ComponentType.Image:
                    return new Component[] {movableObjectBehaviour.GetComponent<Image>()};
                case MovableComponentExtension.ComponentType.CanvasGroup:
                    return new Component[] {movableObjectBehaviour.GetComponent<CanvasGroup>()};
                case MovableComponentExtension.ComponentType.MeshRenderer:
                    return new Component[] {movableObjectBehaviour.GetComponent<MeshRenderer>()};
                case MovableComponentExtension.ComponentType.SpriteRenderer:
                    return new Component[] {movableObjectBehaviour.GetComponent<SpriteRenderer>()};
                default:
                    throw new ArgumentOutOfRangeException(nameof(componentType), componentType, null);
            }
        }

        /// <summary>
        /// Returns an action based on the specified action and component types.
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public static MovableAction GetAction(MovableAction.ActionType actionType,
            MovableComponentExtension.ComponentType componentType)
        {
            var implementedInterface = typeof(IMovableActionComponent);

            //Get all the types that implement IMovableActionComponent interface which aren't abstract.
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes()).Where(p =>
                    implementedInterface.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            var neededComponentList = new List<string>();

            foreach (var type in types)
            {
                var movableComponentInstance = (IMovableActionComponent) Activator.CreateInstance(type);

                if (movableComponentInstance.Type() != actionType) continue;

                var instanceComponentType = movableComponentInstance.GetComponentType();

                if (!neededComponentList.Contains(instanceComponentType.ToString()))
                    neededComponentList.Add(instanceComponentType.ToString());

                //Return the action class instance which matches the specified criteria.
                if (instanceComponentType == componentType)
                    return (MovableAction) Activator.CreateInstance(type);
            }

            Debug.LogError($"Wrong component type specified: {componentType}." +
                           $" Expected: {string.Join(", ", neededComponentList.ToArray())}");
            return null;
        }

        /// <summary>
        /// Adds a specified action to the list.
        /// Checks for override possibilities.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actionList"></param>
        public static void AddAction(MovableAction action, ref List<MovableAction> actionList)
        {
            if (action == null)
                return;

            actionList.Add(action);

            switch (action.Type())
            {
                //Check for transparency action to prevent overriding.
                case MovableAction.ActionType.Color:
                    switch (action)
                    {
                        case MovableActionImageColor _:
                            CheckForOverrides<MovableActionImageTransparency>(ref actionList);
                            break;
                        case MovableActionMeshRendererColor _:
                            CheckForOverrides<MovableActionMeshRendererTransparency>(ref actionList);
                            break;
                        case MovableActionSpriteRendererColor _:
                            CheckForOverrides<MovableActionSpriteRendererTransparency>(ref actionList);
                            break;
                    }

                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Check for transparency action available in the list.
        /// If available the method will automatically move Transparency action to the end of the list
        /// to prevent Color action overriding the Transparency one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CheckForOverrides<T>(ref List<MovableAction> actionList) where T : MovableAction
        {
            var actionCount = actionList.Count;

            var transparencyIndex = 0;
            var isFound = false;

            for (var i = 0; i < actionCount; i++)
            {
                var currentAction = actionList[i];

                if (!(currentAction is T)) continue;

                Debug.LogWarning(
                    "It is noted that a Transparency action exists in the list." +
                    " This will cause Transparency action to be overwritten by the Color action." +
                    " To prevent overriding, Transparency action is automatically moved below the added Color action for your comfort :)");

                isFound = true;
                transparencyIndex = i;
            }

            if (!isFound) return;

            var item = actionList[transparencyIndex];
            actionList.RemoveAt(transparencyIndex);
            actionList.Add(item);
        }
    }
}