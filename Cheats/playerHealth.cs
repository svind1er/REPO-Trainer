using System;
using System.Collections.Generic;
using UnityEngine;

namespace repoMenu
{
    internal static class playerHealth
    {
        static public object playerHealthInstance;
        static public object playerMaxHealthInstance;

        public static void HealPlayer(int healAmount)
        {
            var playerControllerType = Type.GetType("PlayerController, Assembly-CSharp");
            if (playerControllerType != null)
            {
                cheats.Log("PlayerController found.");

                var playerControllerInstance = Helper.FindObjectOfType(playerControllerType);
                if (playerControllerInstance != null)
                {
                    var playerAvatarScriptField = playerControllerInstance.GetType().GetField("playerAvatarScript", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (playerAvatarScriptField != null)
                    {
                        var playerAvatarScriptInstance = playerAvatarScriptField.GetValue(playerControllerInstance);

                        var playerHealthField = playerAvatarScriptInstance.GetType().GetField("playerHealth", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if (playerHealthField != null)
                        {
                            var playerHealthInstance = playerHealthField.GetValue(playerAvatarScriptInstance);

                            var healMethod = playerHealthInstance.GetType().GetMethod("Heal");
                            if (healMethod != null)
                            {
                                healMethod.Invoke(playerHealthInstance, new object[] { healAmount, true });
                                cheats.Log("Healed player with " + healAmount + " HP.");
                            }
                            else
                            {
                                cheats.Log("Heal method not found in playerHealth.");
                            }
                        }
                        else
                        {
                            cheats.Log("playerHealth field not found in playerAvatarScript.");
                        }
                    }
                    else
                    {
                        cheats.Log("playerAvatarScript field not found in PlayerController.");
                    }
                }
                else
                {
                    cheats.Log("playerControllerInstance not found.");
                }
            }
            else
            {
                cheats.Log("PlayerController type not found.");
            }

        }


        public static void DamagePlayer(int damageAmount)
        {
            var playerControllerType = Type.GetType("PlayerController, Assembly-CSharp");
            if (playerControllerType != null)
            {
                cheats.Log("PlayerController found.");
                var playerControllerInstance = Helper.FindObjectOfType(playerControllerType);
                if (playerControllerInstance != null)
                {
                    cheats.Log("playerControllerInstance found.");
                    var playerAvatarScriptField = playerControllerInstance.GetType().GetField("playerAvatarScript", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (playerAvatarScriptField != null)
                    {
                        cheats.Log("playerAvatarScriptField found.");
                        var playerAvatarScriptInstance = playerAvatarScriptField.GetValue(playerControllerInstance);
                        var playerHealthField = playerAvatarScriptInstance.GetType().GetField("playerHealth", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if (playerHealthField != null)
                        {
                            cheats.Log("playerHealthField found.");
                            var playerHealthInstance = playerHealthField.GetValue(playerAvatarScriptInstance);
                            var damageMethod = playerHealthInstance.GetType().GetMethod("Hurt");
                            if (damageMethod != null)
                            {
                                damageMethod.Invoke(playerHealthInstance, new object[] { damageAmount, true, -1 });
                                cheats.Log("Damaged player with " + damageAmount + " HP.");
                            }
                            else
                            {
                                cheats.Log("Hurt method not found in playerHealth.");
                            }
                        }
                        else
                        {
                            cheats.Log("playerHealth field not found in playerAvatarScript.");
                        }
                    }
                    else
                    {
                        cheats.Log("playerAvatarScript field not found in PlayerController.");
                    }
                }
                else
                {
                    cheats.Log("playerControllerInstance not found.");
                }
            }
            else
            {
                cheats.Log("PlayerController type not found.");
            }
        }

        public static void MaxHealth()
        {
            var playerControllerType = Type.GetType("PlayerController, Assembly-CSharp");
            if (playerControllerType != null)
            {
                cheats.Log("PlayerController found.");
                var playerControllerInstance = Helper.FindObjectOfType(playerControllerType);
                if (playerControllerInstance != null)
                {
                    cheats.Log("playerControllerInstance found.");
                    var playerAvatarScriptField = playerControllerInstance.GetType().GetField("playerAvatarScript", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (playerAvatarScriptField != null)
                    {
                        cheats.Log("playerAvatarScriptField found.");
                        var playerAvatarScriptInstance = playerAvatarScriptField.GetValue(playerControllerInstance);
                        var playerHealthField = playerAvatarScriptInstance.GetType().GetField("playerHealth", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if (playerHealthField != null)
                        {
                            cheats.Log("playerHealthField found.");
                            var playerHealthInstance = playerHealthField.GetValue(playerAvatarScriptInstance);
                            var damageMethod = playerHealthInstance.GetType().GetMethod("UpdateHealthRPC");
                            if (damageMethod != null)
                            {
                                damageMethod.Invoke(playerHealthInstance, new object[] { 999999, 100, true });
                                cheats.Log("Damaged player with " + 999999 + " HP.");
                            }
                            else
                            {
                                cheats.Log("Hurt method not found in playerHealth.");
                            }
                        }
                        else
                        {
                            cheats.Log("playerHealth field not found in playerAvatarScript.");
                        }
                    }
                    else
                    {
                        cheats.Log("playerAvatarScript field not found in PlayerController.");
                    }
                }
                else
                {
                    cheats.Log("playerControllerInstance not found.");
                }
            }
            else
            {
                cheats.Log("PlayerController type not found.");
            }
        }

    }
}
