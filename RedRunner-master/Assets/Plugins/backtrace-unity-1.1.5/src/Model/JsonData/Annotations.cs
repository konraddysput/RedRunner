﻿using Backtrace.Newtonsoft;
using Backtrace.Newtonsoft.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Backtrace.Unity.Model.JsonData
{
    /// <summary>
    /// Get report annotations - environment variables
    /// </summary>
    public class Annotations
    {
        private JToken _serializedAnnotations;

        /// <summary>
        /// Get system environment variables
        /// </summary>
        [JsonProperty(PropertyName = "Environment Variables")]
        public Dictionary<string, string> EnvironmentVariables { get; set; }

        /// <summary>
        /// Get built-in complex attributes
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, object> ComplexAttributes = new Dictionary<string, object>();

        public Annotations()
        {

        }
        /// <summary>
        /// Create new instance of Annotations class
        /// </summary>
        /// <param name="complexAttributes">Built-in complex attributes</param>
        public Annotations(Dictionary<string, object> complexAttributes)
        {
            var environment = new EnvironmentVariables();
            ComplexAttributes = complexAttributes;
            EnvironmentVariables = environment.Variables;
        }

        public void FromJson(JToken jtoken)
        {
            _serializedAnnotations = jtoken;
        }

        public BacktraceJObject ToJson()
        {
            if (_serializedAnnotations != null)
            {
                return _serializedAnnotations as BacktraceJObject;
            }
            var annotations = new BacktraceJObject();
            var envVariables = new BacktraceJObject();

            foreach (var envVariable in EnvironmentVariables)
            {
                envVariables[envVariable.Key] = envVariable.Value?.ToString() ?? string.Empty;
            }
            annotations["Environment Variables"] = envVariables;
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene != null)
            {
                var gameObjects = new JArray();

                var rootObjects = new List<GameObject>();
                activeScene.GetRootGameObjects(rootObjects);
                for (int i = 0; i < rootObjects.Count; ++i)
                {
                    // https://docs.unity3d.com/ScriptReference/GameObject.html
                    // game object properties
                    var gameObject = new BacktraceJObject()
                    {
                        ["name"] = rootObjects[i].name,
                        ["isStatic"] = rootObjects[i].isStatic,
                        ["layer"] = rootObjects[i].layer,
                        ["tag"] = rootObjects[i].tag,
                        ["transform.position"] = rootObjects[i].transform?.position.ToString() ?? "",
                        ["transform.rotation"] = rootObjects[i].transform?.rotation.ToString() ?? "",
                        ["tag"] = rootObjects[i].tag,
                        ["tag"] = rootObjects[i].tag,
                        ["activeInHierarchy"] = rootObjects[i].activeInHierarchy,
                        ["activeSelf"] = rootObjects[i].activeSelf,
                        ["hideFlags"] = (int)rootObjects[i].hideFlags,
                        ["instanceId"] = rootObjects[i].GetInstanceID(),

                    };
                    gameObjects.Add(gameObject);
                }
                annotations["Game objects"] = gameObjects;
            }

            return annotations;
        }

        public static Annotations Deserialize(JToken token)
        {            
            var annotations = new Annotations();
            annotations.FromJson(token);
            return annotations;
        }
    }
}
