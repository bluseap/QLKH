using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

public class EncryptUtil
{
    #region Fields

    /// Passphrase from which a pseudo-random password will be derived. The
    /// derived password will be used to generate the encryption key.
    /// Passphrase can be any string. In this example we assume that this
    /// passphrase is an ASCII string.
    /// can be any string.
    private const string passPhrase = "Met@lm1l1t1@";

    /// Salt value used along with passphrase to generate password. Salt can
    /// be any string. In this example we assume that salt is an ASCII string.
    private const string saltValue = "metalfoetus@y.c";

    /// Hash algorithm used to generate password. Allowed values are: "MD5" and
    /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
    private const string hashAlgorithm = "SHA1";

    /// Number of iterations used to generate password. One or two iterations
    /// should be enough.
    /// can be any number.
    private const int passwordIterations = 2;

    /// Initialization vector (or IV). This value is required to encrypt the
    /// first block of plaintext data. For RijndaelManaged class IV must be 
    /// exactly 16 ASCII characters long.
    private const string initVector = "@1B2c3D4e5F6g7H8";

    /// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
    /// Longer keys are more secure than shorter keys.
    private const int keySize = 256;

    #endregion

    #region Methods

    /// <summary>
    /// Encrypts specified plaintext using Rijndael symmetric key algorithm
    /// and returns a base64-encoded result.
    /// </summary>
    /// <param name="plainText">
    /// Plaintext value to be encrypted.
    /// </param>
    /// <returns>
    /// Encrypted value formatted as a base64-encoded string.
    /// </returns>
    public static string Encrypt(string plainText)
    {
        // Convert strings into byte arrays.
        // Let us assume that strings only contain ASCII codes.
        // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
        // encoding.
        var initVectorBytes = Encoding.ASCII.GetBytes(initVector);
        var saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

        // Convert our plaintext into a byte array.
        // Let us assume that plaintext contains UTF8-encoded characters.
        var plainTextBytes = Encoding.UTF8.GetBytes(HttpUtility.UrlDecode(plainText));

        // First, we must create a password, from which the key will be derived.
        // This password will be generated from the specified passphrase and 
        // salt value. The password will be created using the specified hash 
        // algorithm. Password creation can be done in several iterations.
        var password = new PasswordDeriveBytes(passPhrase, saltValueBytes,
                                                               hashAlgorithm, passwordIterations);

        // Use the password to generate pseudo-random bytes for the encryption
        // key. Specify the size of the key in bytes (instead of bits).
#pragma warning disable 618,612
        var keyBytes = password.GetBytes(keySize / 8);
#pragma warning restore 618,612

        // Create uninitialized Rijndael encryption object.
        var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};

        // It is reasonable to set encryption mode to Cipher Block Chaining
        // (CBC). Use default options for other symmetric key parameters.

        // Generate encryptor from the existing key bytes and initialization 
        // vector. Key size will be defined based on the number of the key 
        // bytes.
        var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

        // Define memory stream which will be used to hold encrypted data.
        var memoryStream = new MemoryStream();

        // Define cryptographic stream (always use Write mode for encryption).
        var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        // Start encrypting.
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

        // Finish encrypting.
        cryptoStream.FlushFinalBlock();

        // Convert our encrypted data from a memory stream into a byte array.
        var cipherTextBytes = memoryStream.ToArray();

        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();

        // Convert encrypted data into a base64-encoded string.
        return HttpUtility.UrlEncode(Convert.ToBase64String(cipherTextBytes));
    }

    /// <summary>
    /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
    /// </summary>
    /// <param name="cipherText">
    /// Base64-formatted ciphertext value.
    /// </param>
    /// <returns>
    /// Decrypted string value.
    /// </returns>
    /// <remarks>
    /// Most of the logic in this function is similar to the Encrypt
    /// logic. In order for decryption to work, all parameters of this function
    /// - except cipherText value - must match the corresponding parameters of
    /// the Encrypt function which was called to generate the
    /// ciphertext.
    /// </remarks>
    public static string Decrypt(string cipherText)
    {
        // Convert strings defining encryption key characteristics into byte
        // arrays. Let us assume that strings only contain ASCII codes.
        // If strings include Unicode characters, use Unicode, UTF7, or UTF8
        // encoding.
        var initVectorBytes = Encoding.ASCII.GetBytes(initVector);
        var saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

        // Convert our ciphertext into a byte array.
        var cipherTextBytes = Convert.FromBase64String(HttpUtility.UrlDecode(cipherText));

        // First, we must create a password, from which the key will be 
        // derived. This password will be generated from the specified 
        // passphrase and salt value. The password will be created using
        // the specified hash algorithm. Password creation can be done in
        // several iterations.
        var password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

        // Use the password to generate pseudo-random bytes for the encryption
        // key. Specify the size of the key in bytes (instead of bits).
#pragma warning disable 618,612
        var keyBytes = password.GetBytes(keySize / 8);
#pragma warning restore 618,612

        // Create uninitialized Rijndael encryption object.
        var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};

        // It is reasonable to set encryption mode to Cipher Block Chaining
        // (CBC). Use default options for other symmetric key parameters.

        // Generate decryptor from the existing key bytes and initialization 
        // vector. Key size will be defined based on the number of the key 
        // bytes.
        var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

        // Define memory stream which will be used to hold encrypted data.
        var memoryStream = new MemoryStream(cipherTextBytes);

        // Define cryptographic stream (always use Read mode for encryption).
        var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        // Since at this point we don't know what the size of decrypted data
        // will be, allocate the buffer long enough to hold ciphertext;
        // plaintext is never longer than ciphertext.
        var plainTextBytes = new byte[cipherTextBytes.Length];

        // Start decrypting.
        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();

        // Convert decrypted data into a string. 
        // Let us assume that the original plaintext string was UTF8-encoded.
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
    }

    #endregion
}

public class ParameterWrapper
{
    /// <summary>
    /// Get string contains encoded parameters
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public static string EncodedParam(Hashtable param)
    {
        var result = "";
        if (param.Count == 0) return "";
        foreach (DictionaryEntry de in param)
            result += string.Format("{0}={1}&", de.Key, de.Value);

        return EncryptUtil.Encrypt(result.Remove(result.Length - 1));
    }

    /// <summary>
    /// Get hashtable contains decoded parameter pairs
    /// </summary>
    /// <returns></returns>
    public static Hashtable DecodedParam()
    {
        var query = HttpContext.Current.Request.Url.Query;
        var queryParams = new Hashtable();

        if (query.Equals(String.Empty))
            return queryParams;

        // Decrypt query string
        query = query.Substring(1);
        var index = query.IndexOf('&');
        query = index == -1 ? query.Substring(0) : query.Substring(0, index);

        query = EncryptUtil.Decrypt(query);

        // Split params
        var items = query.Split(new[] { '&' });
        foreach (var item in items)
        {
            var param = item.Split(new[] { '=' });
            if (param.Count() == 2)
                queryParams.Add(param[0], param[1]);
            else
                return new Hashtable();
        }

        return queryParams;
    }

    /// <summary>
    /// Get string contains parameters
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public static string CreateParams(Hashtable param)
    {
        var result = "";
        if (param.Count == 0) return "";
        foreach (DictionaryEntry de in param)
            result += string.Format("{0}={1}&", de.Key, de.Value);

        return result.Remove(result.Length - 1);
    }

    /// <summary>
    /// Get hashtable contains parameter pairs
    /// </summary>
    /// <returns></returns>
    public static Hashtable GetParams()
    {
        var query = HttpContext.Current.Request.Url.Query;
        var queryParams = new Hashtable();

        if (query.Equals(String.Empty))
            return queryParams;

        // Decrypt query string
        query = query.Substring(1);

        // Split params
        var items = query.Split(new[] { '&' });
        foreach (var item in items)
        {
            var param = item.Split(new[] { '=' });
            if (param.Count() >= 2)
                queryParams.Add(param[0], item.Substring(param[0].Length + 1));
            else
                return new Hashtable();
        }

        return queryParams;
    }

}
