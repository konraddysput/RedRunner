﻿using Backtrace.Unity;
using Backtrace.Unity.Model;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BacktraceDatabaseTests
    {
        private GameObject _gameObject;
        private BacktraceDatabase database;
        private BacktraceClient client;

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _gameObject.SetActive(false);
            client = _gameObject.AddComponent<BacktraceClient>();
            client.Configuration = null;
            database = _gameObject.AddComponent<BacktraceDatabase>();
            database.Configuration = null;

            _gameObject.SetActive(true);
        }

        [UnityTest]
        public IEnumerator TestDbCreation_EmptyBacktraceConfiguration_ValidDbCreation()
        {
            Assert.IsTrue(!database.Enable);
            yield return null;
        }


        [UnityTest]
        public IEnumerator TestDbCreation_ValidConfiguration_EnabledDb()
        {
            var configuration = new BacktraceConfiguration()
            {
                ServerUrl = "https://test.sp.backtrace.io:6097/",
                //backtrace configuration require 64 characters
                Token = "1234123412341234123412341234123412341234123412341234123412341234",
                DatabasePath = Application.dataPath,
                CreateDatabase = false,
                AutoSendMode = false,
                Enabled = true
            };
            database.Configuration = configuration;
            database.Reload();
            Assert.IsTrue(database.Enable);
            yield return null;
        }
    }
}
