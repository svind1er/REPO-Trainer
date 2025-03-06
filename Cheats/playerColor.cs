using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Random = System.Random;

namespace repoMenu
{
    internal static class playerColor
    {
        private static bool isColorChanging = false;
        private static readonly Random random = new Random();
        private static Coroutine colorChangeCoroutine;

        public static void ColorRandomizer()
        {
            var colorControllerType = Type.GetType("PlayerAvatar, Assembly-CSharp");
            if (colorControllerType != null)
            {
                cheats.Log("colorControllerType found.");

                var colorControllerInstance = Helper.FindObjectOfType(colorControllerType);
                if (colorControllerInstance != null)
                {
                    cheats.Log("colorControllerInstance found.");
                    var playerSetColorMethod = colorControllerType.GetMethod("PlayerAvatarSetColor");
                    if (playerSetColorMethod != null)
                    {
                        var colorIndex = random.Next(0, 30);  // Generate a random color index between 0 and 30
                        playerSetColorMethod.Invoke(colorControllerInstance, new object[] { colorIndex });
                    }
                    else
                    {
                        cheats.Log("PlayerAvatarSetColor method not found in PlayerAvatar.");
                    }
                }
                else
                {
                    cheats.Log("colorControllerInstance not found.");
                }
            }
            else
            {
                cheats.Log("colorControllerType not found.");
            }
        }
    }
}