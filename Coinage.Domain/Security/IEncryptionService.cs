﻿using System;

namespace Coinage.Domain.Security
{
    public interface IEncryptionService
    {
        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <returns>Salt key</returns>
        string CreateSaltKey(int size);

        /// <summary>
        /// Create a password hash
        /// </summary>
        /// <param name="password">{assword</param>
        /// <param name="saltkey">Salk key</param>
        /// <returns>Password hash</returns>
        string CreatePasswordHash(string password, string saltkey);

        ///// <summary>
        ///// Encrypt text
        ///// </summary>
        ///// <param name="plainText">Text to encrypt</param>
        ///// <param name="encryptionPrivateKey">Encryption private key</param>
        ///// <returns>Encrypted text</returns>
        //string EncryptText(string plainText, string encryptionPrivateKey = "");
    }
}
