using System;
using UnityEngine;
using Photon.Pun;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;

namespace repoMenu
{
    internal static class DebugCheats
    {
        public static bool drawEspBool = false;

        public static void SpawnItem()
        {
            var debugAxelType = Type.GetType("DebugAxel, Assembly-CSharp");
            if (debugAxelType != null)
            {
                var debugAxelInstance = Helper.FindObjectOfType(debugAxelType);
                if (debugAxelInstance != null)
                {
                    var spawnObjectMethod = debugAxelType.GetMethod("SpawnObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    if (spawnObjectMethod != null)
                    {
                        GameObject itemToSpawn = AssetManager.instance.surplusValuableBig;
                        Vector3 spawnPosition = new Vector3(0f, 1f, 0f);
                        string path = "Valuables/";

                        spawnObjectMethod.Invoke(debugAxelInstance, new object[] { itemToSpawn, spawnPosition, path });

                        cheats.Log("Item spawned successfully.");
                    }
                    else
                    {
                        cheats.Log("SpawnObject method not found in DebugAxel.");
                    }
                }
                else
                {
                    cheats.Log("DebugAxel instance not found in the scene.");
                }
            }
            else
            {
                cheats.Log("DebugAxel type not found.");
            }
        }

        private static List<object> enemyList = new List<object>();

        public static void UpdateEnemyList()
        {
            enemyList.Clear();
            cheats.Log("Updating enemy list");

            var enemyDirectorType = Type.GetType("EnemyDirector, Assembly-CSharp");
            if (enemyDirectorType != null)
            {
                cheats.Log("EnemyDirector found");
                var enemyDirectorInstance = enemyDirectorType.GetField("instance", BindingFlags.Public | BindingFlags.Static)?.GetValue(null);

                if (enemyDirectorInstance != null)
                {
                    cheats.Log("EnemyDirector instance obtained");
                    var enemiesSpawnedField = enemyDirectorType.GetField("enemiesSpawned", BindingFlags.Public | BindingFlags.Instance);

                    if (enemiesSpawnedField != null)
                    {
                        var enemies = enemiesSpawnedField.GetValue(enemyDirectorInstance) as IEnumerable<object>;
                        if (enemies != null)
                        {
                            foreach (var enemy in enemies)
                            {
                                if (enemy != null)
                                {
                                    var enemyInstanceField = enemy.GetType().GetField("enemyInstance", BindingFlags.NonPublic | BindingFlags.Instance)
                                                          ?? enemy.GetType().GetField("Enemy", BindingFlags.NonPublic | BindingFlags.Instance)
                                                          ?? enemy.GetType().GetField("childEnemy", BindingFlags.NonPublic | BindingFlags.Instance);
                                    if (enemyInstanceField != null)
                                    {
                                        var enemyInstance = enemyInstanceField.GetValue(enemy) as Enemy;
                                        if (enemyInstance != null && enemyInstance.gameObject != null && enemyInstance.gameObject.activeInHierarchy)
                                        {
                                            enemyList.Add(enemy);
                                        }
                                    }
                                }
                            }
                            cheats.Log($"Enemies found: {enemyList.Count}");
                        }
                        else
                        {
                            cheats.Log("No enemies found in enemies Spawned");
                        }
                    }
                    else
                    {
                        cheats.Log("Field 'enemiesSpawned' not found");
                    }
                }
                else
                {
                    cheats.Log("EnemyDirector instance is null");
                }
            }
            else
            {
                cheats.Log("EnemyDirector not found");
            }
        }

        public static void RectFilled(float x, float y, float width, float height, Texture2D text)
        {
            GUI.DrawTexture(new Rect(x, y, width, height), text);
        }

        public static void RectOutlined(float x, float y, float width, float height, Texture2D text, float thickness = 1f)
        {
            RectFilled(x, y, thickness, height, text);
            RectFilled(x + width - thickness, y, thickness, height, text);
            RectFilled(x + thickness, y, width - thickness * 2f, thickness, text);
            RectFilled(x + thickness, y + height - thickness, width - thickness * 2f, thickness, text);
        }

        public static void Box(float x, float y, float width, float height, Texture2D text, float thickness = 2f)
        {
            RectOutlined(x - width / 2f, y - height, width, height, text, thickness);
        }

        public static Texture2D texture2;

        public static void DrawESP()
        {
            if (!drawEspBool) return;

            Camera playerCamera = Camera.main;
            if (playerCamera == null)
            {
                cheats.Log("Camera.main not found!");
                return;
            }

            float scaleX = (float)Screen.width / playerCamera.pixelWidth;
            float scaleY = (float)Screen.height / playerCamera.pixelHeight;

            int enemyIndex = 0;

            foreach (var enemyParent in enemyList)
            {
                if (enemyParent == null) continue;

                var enemyField = enemyParent.GetType().GetField("enemyInstance", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? enemyParent.GetType().GetField("Enemy", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? enemyParent.GetType().GetField("childEnemy", BindingFlags.NonPublic | BindingFlags.Instance);

                if (enemyField != null)
                {
                    var enemyInstance = enemyField.GetValue(enemyParent) as Enemy;
                    if (enemyInstance != null && enemyInstance.gameObject != null && enemyInstance.gameObject.activeInHierarchy && enemyInstance.CenterTransform != null)
                    {
                        Vector3 footPosition = enemyInstance.transform.position;
                        float enemyHeightEstimate = 2f;
                        Vector3 headPosition = enemyInstance.transform.position + Vector3.up * enemyHeightEstimate;

                        Vector3 screenFootPos = playerCamera.WorldToScreenPoint(footPosition);
                        Vector3 screenHeadPos = playerCamera.WorldToScreenPoint(headPosition);

                        if (screenFootPos.z > 0 && screenHeadPos.z > 0)
                        {
                            float footX = screenFootPos.x * scaleX;
                            float footY = Screen.height - (screenFootPos.y * scaleY);
                            float headY = Screen.height - (screenHeadPos.y * scaleY);

                            float height = Mathf.Abs(footY - headY);
                            float enemyScale = enemyInstance.transform.localScale.y;
                            float baseWidth = enemyScale * 200f;
                            float distance = screenFootPos.z;
                            float width = (baseWidth / distance) * scaleX;

                            width = Mathf.Clamp(width, 30f, height * 1.2f);
                            height = Mathf.Clamp(height, 40f, 400f);

                            float x = footX;
                            float y = footY;

                            Box(x, y, width, height, texture2, 1f);

                            float labelWidth = 100f;
                            float labelHeight = 20f;
                            float labelX = x - labelWidth / 2f;
                            float labelY = y - height - labelHeight - 5f;
                            GUI.Label(new Rect(labelX + 28f, labelY, labelWidth, labelHeight), "Enemy");

                            enemyIndex++;
                        }
                        else
                        {
                            cheats.Log("Enemy or part of it behind the camera");
                        }
                    }
                }
            }
        }

        public static void KillAllEnemies()
        {
            cheats.Log("Trying to kill all enemies");

            foreach (var enemyParent in enemyList)
            {
                if (enemyParent == null) continue;

                var enemyField = enemyParent.GetType().GetField("enemyInstance", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? enemyParent.GetType().GetField("Enemy", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? enemyParent.GetType().GetField("childEnemy", BindingFlags.NonPublic | BindingFlags.Instance);

                if (enemyField != null)
                {
                    var enemyInstance = enemyField.GetValue(enemyParent);
                    if (enemyInstance != null)
                    {
                        try
                        {
                            var healthField = enemyInstance.GetType().GetField("Health", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            if (healthField != null)
                            {
                                var healthComponent = healthField.GetValue(enemyInstance);
                                if (healthComponent != null)
                                {
                                    var healthType = healthComponent.GetType();

                                    var hurtMethod = healthType.GetMethod("Hurt", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    if (hurtMethod != null)
                                    {
                                        hurtMethod.Invoke(healthComponent, new object[] { 9999, Vector3.zero });
                                        cheats.Log($"Enemy hurt with 9999 damage via Hurt");
                                    }
                                    else
                                    {
                                        cheats.Log("Method 'Hurt' not found in EnemyHealth");
                                    }
                                }
                                else
                                {
                                    cheats.Log("EnemyHealth component is null");
                                }
                            }
                            else
                            {
                                cheats.Log("Field 'Health' not found in Enemy");
                            }
                        }
                        catch (Exception e)
                        {
                            cheats.Log($"Error killing enemy: {e.Message}");
                        }
                    }
                    else
                    {
                        cheats.Log("Invalid or inactive enemy");
                    }
                }
            }

            UpdateEnemyList();
        }
    }
}