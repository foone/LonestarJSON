using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

class LonestarJson
{
    private readonly SymmetricAlgorithm? des3;
    private readonly UTF8Encoding utf8;

    static void Main(string[] args)
    {
        if (args.Length == 2)
        {
            LonestarJson lonestar = new("777");
            if (args[0] == "-e")
            {
                Console.Write(lonestar.EncryptString(File.ReadAllText(args[1])));
            }
            else
            {
                Console.Write(lonestar.DecryptString(File.ReadAllText(args[1])));
            }
            
        }
        else {
            Console.WriteLine("Usage: LonestarJSON -e/-d <filename.json>");
        }        
    }
    LonestarJson(string key)
    {
        HashAlgorithm md5Provider = (HashAlgorithm)CryptoConfig.CreateFromName("MD5");
        des3 = CryptoConfig.CreateFromName("3DES") as SymmetricAlgorithm;
        utf8 = new UTF8Encoding();
        byte[] hash = md5Provider.ComputeHash(utf8.GetBytes(key));
        des3.Key = hash;
        des3.Mode = CipherMode.ECB;
        des3.Padding = PaddingMode.PKCS7;


    }
    public string DecryptString(string str)
    {
        
        byte[] inputBuffer = Convert.FromBase64String(str);
        byte[] bytes = des3.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
        return utf8.GetString(bytes);
    }
    public string EncryptString(string str)
    {
        byte[] bytes = utf8.GetBytes(str);
        return Convert.ToBase64String(des3.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
    }
}