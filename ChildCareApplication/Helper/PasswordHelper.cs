using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace ChildCareApplication.Helper
{
    
        public static class PasswordHelper
        {
        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);
                return salt;
            }
        }

        public static string HashPassword(string password, byte[] salt)
            {
               // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 32));

                byte[] hashBytes = new byte[16 + 32];
                Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
                Buffer.BlockCopy(Convert.FromBase64String(hashed), 0, hashBytes, 16, 32);

                return Convert.ToBase64String(hashBytes);
            }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Convert the stored hash to byte array
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Extract the salt from the stored hash
            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, salt.Length);

            // Extract the stored subkey (hash) from the stored hash
            byte[] storedSubkey = new byte[32];
            Buffer.BlockCopy(hashBytes, salt.Length, storedSubkey, 0, storedSubkey.Length);

            // Hash the entered password with the extracted salt
            string enteredHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));

            // Compare the hashed entered password with the stored subkey
            return Convert.ToBase64String(storedSubkey) == enteredHashed;
        }


    }
}
