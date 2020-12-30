using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace lecture_12
{
    public static class customEncryption
    {
        public static void EncryptText(SymmetricAlgorithm aesAlgorithm, string text, string fileName)
        {
            // Create an encryptor from the AES algorithm instance and pass the aes algorithm key and inialiaztion vector to generate a new random sequence each time for the same text  
            ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);

            // Create a memory stream to save the encrypted data in it  
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(cs))
                    {
                        // Write the text in the stream writer   
                        writer.Write(text);
                    }
                }

                // Get the result as a byte array from the memory stream   
                byte[] encryptedDataBuffer = ms.ToArray();

                // Write the data to a file   
                File.WriteAllBytes(fileName, encryptedDataBuffer);
            }
        }

        // Method to decrypt a data from a specific file and return the result as a string   
        public static string DecryptData(SymmetricAlgorithm aesAlgorithm, string fileName)
        {
            // Create a decryptor from the aes algorithm   
            ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor(aesAlgorithm.Key, aesAlgorithm.IV);

            // Read the encrypted bytes from the file   
            byte[] encryptedDataBuffer = File.ReadAllBytes(fileName);

            // Create a memorystream to write the decrypted data in it   
            using (MemoryStream ms = new MemoryStream(encryptedDataBuffer))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        // Reutrn all the data from the streamreader   
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public static byte[] toByteArray(this string testString)
        { return Encoding.ASCII.GetBytes(testString); }
        public static byte[] to32Bytes(this byte[] myByteArray)
        {
            byte[] returnArray = new byte[32];

            for (int i = 0; i < myByteArray.Length; i++)
            {
                returnArray[i] = myByteArray[i];
            }
            return returnArray;
        }

        readonly static string pubKeyPath = "public.key";//change as needed
        readonly static string priKeyPath = "private.key";//change as needed
        public static void MakeKey()
        {
            //lets take a new CSP with a new 2048 bit rsa key pair
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);

            //how to get the private key
            RSAParameters privKey = csp.ExportParameters(true);

            //and the public key ...
            RSAParameters pubKey = csp.ExportParameters(false);
            //converting the public key into a string representation
            string pubKeyString;
            {
                //we need some buffer
                var sw = new StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, pubKey);
                //get the string from the stream
                pubKeyString = sw.ToString();
                File.WriteAllText(pubKeyPath, pubKeyString);
            }
            string privKeyString;
            {
                //we need some buffer
                var sw = new StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, privKey);
                //get the string from the stream
                privKeyString = sw.ToString();
                File.WriteAllText(priKeyPath, privKeyString);
            }
        }
        public static void EncryptFile_asym(string filePath)
        {
            //converting the public key into a string representation
            string pubKeyString;
            {
                using (StreamReader reader = new StreamReader(pubKeyPath)) { pubKeyString = reader.ReadToEnd(); }
            }
            //get a stream from the string
            var sr = new StringReader(pubKeyString);

            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));

            //get the object back from the stream
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters((RSAParameters)xs.Deserialize(sr));
            byte[] bytesPlainTextData = File.ReadAllBytes(filePath);

            //apply pkcs#1.5 padding and encrypt our data 
            var bytesCipherText = csp.Encrypt(bytesPlainTextData, false);
            //we might want a string representation of our cypher text... base64 will do
            string encryptedText = Convert.ToBase64String(bytesCipherText);
            File.WriteAllText(filePath, encryptedText);
        }
        public static void DecryptFile_asym(string filePath)
        {
            //we want to decrypt, therefore we need a csp and load our private key
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();

            string privKeyString;
            {
                privKeyString = File.ReadAllText(priKeyPath);
                //get a stream from the string
                var sr = new StringReader(privKeyString);
                //we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //get the object back from the stream
                RSAParameters privKey = (RSAParameters)xs.Deserialize(sr);
                csp.ImportParameters(privKey);
            }
            string encryptedText;
            using (StreamReader reader = new StreamReader(filePath)) { encryptedText = reader.ReadToEnd(); }
            byte[] bytesCipherText = Convert.FromBase64String(encryptedText);

            //decrypt and strip pkcs#1.5 padding
            byte[] bytesPlainTextData = csp.Decrypt(bytesCipherText, false);

            //get our original plainText back...
            File.WriteAllBytes(filePath, bytesPlainTextData);
        }

    }
}
