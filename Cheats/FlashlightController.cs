using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Random = System.Random;

namespace repoMenu
{
    internal static class FlashLightController
    {
        private static bool isColorChanging = false;
        private static readonly Random random = new Random();
        private static Coroutine colorChangeCoroutine;

        public static void FlashlightTrigger()
        {
            var lightControllerType = Type.GetType("ItemLight, Assembly-CSharp");
            if (lightControllerType != null)
            {
                cheats.Log("lightControllerType found.");

                var lightControllerInstance = Helper.FindObjectOfType(lightControllerType);
                if (lightControllerInstance != null)
                {
                    cheats.Log("lightControllerInstance found.");
                    var lightControllerShowField = lightControllerInstance.GetType().GetField("showLight");
                    if (lightControllerShowField != null)
                    {
                        lightControllerShowField.SetValue(lightControllerInstance, false);
                    }
                    else
                    {
                        cheats.Log("lightControllerShowField method not found in ItemLight.");
                    }
                }
                else
                {
                    cheats.Log("lightControllerInstance not found.");
                }
            }
            else
            {
                cheats.Log("lightControllerType not found.");
            }
        }
    }
}