using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace repoMenu
{
    internal static class PlayerController
    {
        public static object playerSpeedInstance;
        private static object enemyDirectorInstance;

        public static void GodMode()
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

                            var godModeField = playerHealthInstance.GetType().GetField("godMode", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            if (godModeField != null)
                            {
                                bool currentGodMode = (bool)godModeField.GetValue(playerHealthInstance);

                                bool newGodModeState = !currentGodMode;
                                godModeField.SetValue(playerHealthInstance, newGodModeState);

                                cheats.godModeEnabled = !newGodModeState;

                                cheats.Log("God Mode " + (newGodModeState ? "enabled" : "disabled"));
                            }
                            else
                            {
                                cheats.Log("godMode field not found in playerHealth.");
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

        public static void RemoveSpeed(float sliderValue)
        {
            var playerInSpeedType = Type.GetType("PlayerController, Assembly-CSharp");
            if (playerInSpeedType != null)
            {
                cheats.Log("playerInSpeedType is not NULL ");
                playerSpeedInstance = Helper.FindObjectOfType(playerInSpeedType);
                if (playerSpeedInstance != null)
                {
                    cheats.Log("playerSpeedInstance is not NULL");
                }
                else
                {
                    cheats.Log("playerSpeedInstance null");
                }
            }
            else
            {
                cheats.Log("playerInSpeedType null");
            }
            if (playerSpeedInstance != null)
            {
                cheats.Log("playerSpeedInstance is not NULL");

                var playerControllerType = playerSpeedInstance.GetType();

                var moveSpeedField1 = playerControllerType.GetField("MoveSpeed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (moveSpeedField1 != null)
                {
                    moveSpeedField1.SetValue(playerSpeedInstance, sliderValue);
                    cheats.Log("MoveSpeed value set to " + sliderValue);
                }
                else
                {
                    cheats.Log("MoveSpeed field not found in PlayerController.");
                }
            }
        }

        public static void Revive()
        {
            var enemyDirectorType = Type.GetType("EnemyDirector, Assembly-CSharp");
            if (enemyDirectorType != null)
            {
                enemyDirectorInstance = Helper.FindObjectOfType(enemyDirectorType);
                if (enemyDirectorInstance != null)
                {
                    cheats.Log("EnemyDirector found.");

                    var setInvestigateMethod = enemyDirectorType.GetMethod("SetInvestigate");
                    if (setInvestigateMethod != null)
                    {
                        Vector3 spawnPosition = new Vector3(0f, 1f, 0f);
                        setInvestigateMethod.Invoke(enemyDirectorInstance, new object[] { spawnPosition, 999f });
                        cheats.Log("SetInvestigate called successfully");
                    }
                    else
                    {
                        cheats.Log("SetInvestigate method not found.");
                    }
                }
                else
                {
                    cheats.Log("enemyDirectorInstance is null.");
                    return;
                }
            }
            else
            {
                cheats.Log("EnemyDirector not found.");
                return;
            }

            var semiFuncType = Type.GetType("SemiFunc, Assembly-CSharp");
            if (semiFuncType != null)
            {
                var playerGetListMethod = semiFuncType.GetMethod("PlayerGetList", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                if (playerGetListMethod != null)
                {
                    var playerList = playerGetListMethod.Invoke(null, null) as IEnumerable<object>;
                    if (playerList != null)
                    {
                        foreach (var playerAvatar in playerList)
                        {
                            var playerDeathHeadField = playerAvatar.GetType().GetField("playerDeathHead", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                            if (playerDeathHeadField != null)
                            {
                                var playerDeathHeadInstance = playerDeathHeadField.GetValue(playerAvatar);
                                if (playerDeathHeadInstance != null)
                                {
                                    var inExtractionPointField = playerDeathHeadInstance.GetType().GetField("inExtractionPoint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                    if (inExtractionPointField != null)
                                    {
                                        inExtractionPointField.SetValue(playerDeathHeadInstance, true);
                                        cheats.Log("inExtractionPoint set to true.");
                                    }
                                    else
                                    {
                                        cheats.Log("inExtractionPoint field not found.");
                                    }

                                    var reviveMethod = playerDeathHeadInstance.GetType().GetMethod("Revive");
                                    if (reviveMethod != null)
                                    {
                                        reviveMethod.Invoke(playerDeathHeadInstance, null);
                                        cheats.Log("Player successfully revived.");
                                    }
                                    else
                                    {
                                        cheats.Log("Revive method not found.");
                                    }
                                }
                                else
                                {
                                    cheats.Log("playerDeathHeadInstance is NULL.");
                                }
                            }
                            else
                            {
                                cheats.Log("playerDeathHead field not found.");
                            }
                        }
                    }
                    else
                    {
                        cheats.Log("PlayerGetList returned null.");
                    }
                }
                else
                {
                    cheats.Log("Method PlayerGetList not found.");
                }
            }
            else
            {
                cheats.Log("SemiFunc não encontrado.");
            }
        }

        public static void SendFirstPlayerToVoid()
        {
            var playerController = Type.GetType("PlayerController, Assembly-CSharp");
            if (playerController != null)
            {
                var playerControllerInstance = Helper.FindObjectOfType(playerController);
                {
                    if (playerControllerInstance != null)
                    {
                        cheats.Log("reviveInstance is not NULL.");
                        var damageMethod1 = playerControllerInstance.GetType().GetMethod("Revive");
                        if (damageMethod1 != null)
                        {
                            Vector3 spawnPosition = new Vector3(-9999f, -9999f, -99999f);
                            damageMethod1.Invoke(spawnPosition, new object[] { });
                            cheats.Log("Player Sent to Void");
                        }
                        else
                        {
                            cheats.Log("void null");
                        }
                    }
                }
            }
            else
            {
                cheats.Log("reviveInstance null");
            }
        }

        public static void MaxStamina()
        {
            var playerControllerType = Type.GetType("PlayerController, Assembly-CSharp");
            if (playerControllerType != null)
            {
                cheats.Log("PlayerController found.");

                var playerControllerInstance = Helper.FindObjectOfType(playerControllerType);
                if (playerControllerInstance != null)
                {
                    var energyCurrentField = playerControllerInstance.GetType().GetField("EnergyCurrent", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (energyCurrentField != null)
                    {
                        energyCurrentField.SetValue(playerControllerInstance, 999999);
                        cheats.Log("EnergyCurrent set to " + 999999);
                    }
                    else
                    {
                        cheats.Log("EnergyCurrent field not found in playerAvatarScript.");
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
