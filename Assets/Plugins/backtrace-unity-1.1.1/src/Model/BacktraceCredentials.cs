﻿using System;
using System.Text;
using UnityEngine;

namespace Backtrace.Unity.Model
{
    /// <summary>
    /// Bactrace credentials informations
    /// </summary>
    public class BacktraceCredentials
    {
        private const string _configurationHostRecordName = "HostUrl";
        private const string _configurationTokenRecordName = "Token";

        private readonly Uri _backtraceHostUri;
        private readonly byte[] _accessToken;

        /// <summary>
        /// Get a Uri to Backtrace servcie
        /// </summary>
        public Uri BacktraceHostUri
        {
            get
            {
                return _backtraceHostUri;
            }
        }

        /// <summary>
        /// Get an access token
        /// </summary>
        public string Token
        {
            get
            {
                return Encoding.UTF8.GetString(_accessToken);
            }
        }

        /// <summary>
        /// Create submission url to Backtrace API
        /// </summary>
        /// <returns></returns>
        public Uri GetSubmissionUrl()
        {
            if (_backtraceHostUri == null)
            {
                throw new ArgumentException(nameof(BacktraceHostUri));
            }

            var uriBuilder = new UriBuilder(BacktraceHostUri);
            if (_validUrl)
            {
                return uriBuilder.Uri;
            }
            if (string.IsNullOrEmpty(Token))
            {
                throw new ArgumentException(nameof(Token));
            }

            if (!uriBuilder.Scheme.StartsWith("http"))
            {
                uriBuilder.Scheme = $"https://{uriBuilder.Scheme}";
            }
            if (!uriBuilder.Path.EndsWith("/") && !string.IsNullOrEmpty(uriBuilder.Path))
            {
                uriBuilder.Path += "/";
            }
            uriBuilder.Path = $"{uriBuilder.Path}post";
            uriBuilder.Query = $"format=json&token={Token}";
            return uriBuilder.Uri;
        }
        private readonly bool _validUrl = false;

        /// <summary>
        /// Initialize Backtrace credentials with Backtrace submit url. 
        /// If you pass backtraceSubmitUrl you have to make sure url to API is valid and contains token
        /// </summary>
        /// <param name="backtraceSubmitUrl">Backtrace submit url</param>
        public BacktraceCredentials(string backtraceSubmitUrl)
            : this(new Uri(backtraceSubmitUrl))
        { }

        /// <summary>
        /// Initialize Backtrace credentials with Backtrace submit url. 
        /// If you pass backtraceSubmitUrl you have to make sure url to API is valid and contains token
        /// </summary>
        /// <param name="backtraceSubmitUrl">Backtrace submit url</param>
        public BacktraceCredentials(Uri backtraceSubmitUrl)
        {
            var hostToCheck = backtraceSubmitUrl.Host;
            if (!hostToCheck.StartsWith("www."))
            {
                hostToCheck = $"www.{hostToCheck}";
            }
            _validUrl = hostToCheck.StartsWith("www.submit.backtrace.io") || hostToCheck.Contains("sp.backtrace.io");
            if (!_validUrl)
            {                
                throw new ArgumentException(nameof(backtraceSubmitUrl));
            }
            _backtraceHostUri = backtraceSubmitUrl;
        }

        /// <summary>
        /// Initialize Backtrace credencials
        /// </summary>
        /// <param name="backtraceHostUri">Uri to Backtrace host</param>
        /// <param name="accessToken">Access token to Backtrace services</param>
        /// <exception cref="ArgumentException">Thrown when uri to backtrace is invalid or accessToken is null or empty</exception>
        public BacktraceCredentials(
            Uri backtraceHostUri,
            byte[] accessToken)
        {
            if (!IsValid(backtraceHostUri, accessToken))
            {
                throw new ArgumentException($"{nameof(backtraceHostUri)} or {nameof(accessToken)} is not valid.");
            }
            _backtraceHostUri = backtraceHostUri;
            _accessToken = accessToken;
        }
        /// <summary>
        /// Initialize Backtrace credencials
        /// </summary>
        /// <param name="backtraceHostUrl">Url to Backtrace Url</param>
        /// <param name="accessToken">Access token to Backtrace services</param>
        public BacktraceCredentials(
            string backtraceHostUrl,
            byte[] accessToken)
        : this(new Uri(backtraceHostUrl), accessToken)
        { }

        /// <summary>
        /// Initialize Backtrace credencials
        /// </summary>
        /// <param name="backtraceHostUrl">Url to Backtrace Url</param>
        /// <param name="accessToken">Access token to Backtrace services</param>
        public BacktraceCredentials(
            string backtraceHostUrl,
            string accessToken)
        : this(backtraceHostUrl, Encoding.UTF8.GetBytes(accessToken))
        { }

        /// <summary>
        /// Initialize Backtrace credencials
        /// </summary>
        /// <param name="backtraceHostUri">Url to Backtrace Url</param>
        /// <param name="accessToken">Access token to Backtrace services</param>
        public BacktraceCredentials(
            Uri backtraceHostUri,
            string accessToken)
        : this(backtraceHostUri, Encoding.UTF8.GetBytes(accessToken))
        { }
        /// <summary>
        /// Check if model passed to constructor is valid
        /// </summary>
        /// <param name="uri">Backtrace service uri</param>
        /// <param name="token">Access token to Backtrace services</param>
        /// <returns>validation result</returns>
        internal bool IsValid(Uri uri, byte[] token)
        {
            return token != null && token.Length > 0 && uri.IsWellFormedOriginalString();
        }
    }
}