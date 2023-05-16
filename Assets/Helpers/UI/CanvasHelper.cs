﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Helpers.UI
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasHelper : MonoBehaviour
    {
        public static UnityEvent onOrientationChange = new UnityEvent();

        public static UnityEvent onResolutionChange = new UnityEvent();

        private static List<CanvasHelper> helpers = new List<CanvasHelper>();

        private static ScreenOrientation lastOrientation = ScreenOrientation.Portrait;

        private static Vector2 lastResolution = Vector2.zero;

        private static Rect lastSafeArea = Rect.zero;

        private static bool screenChangeVarsInitialized;

        private Canvas canvas;

        private RectTransform rectTransform;

        private RectTransform safeAreaTransform;

        public static bool isLandscape { get; private set; }

        public static Vector2 GetCanvasSize()
        {
            return helpers[0].rectTransform.sizeDelta;
        }

        public static Vector2 GetSafeAreaSize()
        {
            for (int i = 0; i < helpers.Count; i++)
            {
                if (helpers[i].safeAreaTransform != null)
                {
                    return helpers[i].safeAreaTransform.sizeDelta;
                }
            }

            return GetCanvasSize();
        }

        private static void OrientationChanged()
        {
            // Debug.Log("Orientation changed from " + lastOrientation + " to " + Screen.orientation + " at " + Time.time);
            lastOrientation = Screen.orientation;
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;

            isLandscape = lastOrientation == ScreenOrientation.LandscapeLeft || lastOrientation == ScreenOrientation.LandscapeRight || lastOrientation == ScreenOrientation.LandscapeLeft;
            onOrientationChange.Invoke();
        }

        private static void ResolutionChanged()
        {
            if (lastResolution.x == Screen.width && lastResolution.y == Screen.height)
            {
                return;
            }

            // Debug.Log("Resolution changed from " + lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;

            isLandscape = Screen.width > Screen.height;
            onResolutionChange.Invoke();
        }

        private static void SafeAreaChanged()
        {
            if (lastSafeArea == Screen.safeArea)
            {
                return;
            }

            // Debug.Log("Safe Area changed from " + lastSafeArea + " to " + Screen.safeArea.size + " at " + Time.time);
            lastSafeArea = Screen.safeArea;

            for (int i = 0; i < helpers.Count; i++)
            {
                helpers[i].ApplySafeArea();
            }
        }

        private void ApplySafeArea()
        {
            if (this.safeAreaTransform == null)
            {
                return;
            }

            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= this.canvas.pixelRect.width;
            anchorMin.y /= this.canvas.pixelRect.height;
            anchorMax.x /= this.canvas.pixelRect.width;
            anchorMax.y /= this.canvas.pixelRect.height;

            this.safeAreaTransform.anchorMin = anchorMin;
            this.safeAreaTransform.anchorMax = anchorMax;

            // Debug.Log(
            // "ApplySafeArea:" +
            // "\n Screen.orientation: " + Screen.orientation +
            // #if UNITY_IOS
            // "\n Device.generation: " + UnityEngine.iOS.Device.generation.ToString() +
            // #endif
            // "\n Screen.safeArea.position: " + Screen.safeArea.position.ToString() +
            // "\n Screen.safeArea.size: " + Screen.safeArea.size.ToString() +
            // "\n Screen.width / height: (" + Screen.width.ToString() + ", " + Screen.height.ToString() + ")" +
            // "\n canvas.pixelRect.size: " + canvas.pixelRect.size.ToString() +
            // "\n anchorMin: " + anchorMin.ToString() +
            // "\n anchorMax: " + anchorMax.ToString());
        }

        private void Awake()
        {
            if (!helpers.Contains(this))
            {
                helpers.Add(this);
            }

            this.canvas = this.GetComponent<Canvas>();
            this.rectTransform = this.GetComponent<RectTransform>();

            this.safeAreaTransform = this.transform.Find("SafeArea") as RectTransform;

            if (!screenChangeVarsInitialized)
            {
                lastOrientation = Screen.orientation;
                lastResolution.x = Screen.width;
                lastResolution.y = Screen.height;
                lastSafeArea = Screen.safeArea;

                screenChangeVarsInitialized = true;
            }
        }

        private void OnDestroy()
        {
            if (helpers != null && helpers.Contains(this))
            {
                helpers.Remove(this);
            }
        }

        private void Start()
        {
            this.ApplySafeArea();
        }

        private void Update()
        {
            if (helpers[0] != this)
            {
                return;
            }

            if (Application.isMobilePlatform)
            {
                if (Screen.orientation != lastOrientation)
                {
                    OrientationChanged();
                }

                if (Screen.safeArea != lastSafeArea)
                {
                    SafeAreaChanged();
                }
            }
            else
            {
                // resolution of mobile devices should stay the same always, right?
                // so this check should only happen everywhere else
                if (Screen.width != lastResolution.x || Screen.height != lastResolution.y)
                {
                    ResolutionChanged();
                }
            }
        }
    }
}