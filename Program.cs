using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace GlobalProtect_KeyGen {
    class Program
    {
        static void Main(string[] args)

        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: GlobalProtect_KeyGen.exe Computer_SID_VALUE\n");
                Console.WriteLine("Get Computer SID using PowerShell:");
                Console.WriteLine("((get-wmiobject -query \"Select * from Win32_UserAccount Where LocalAccount = TRUE\").SID -replace \"\\d+$\",\"\" -replace \".$\")[0]");
                return;
            }

            string SID = args[0];
            byte[] aesKey = GetKey(SID);

        }

        public static byte[] GetComputerSID(string sidValue)
        {
            SecurityIdentifier sid = new SecurityIdentifier(sidValue);
            byte[] sidBytes = new byte[sid.BinaryLength];
            sid.GetBinaryForm(sidBytes, 0);
            Console.WriteLine($"\t[*] Computer SID : {sid}");

            StringBuilder sidString = new StringBuilder(sidBytes.Length * 2);

            foreach (byte b in sidBytes)
            {
                sidString.Append(b.ToString("X2"));
            }

            Console.WriteLine($"\t[*] Computer SID (Hex) : {sidString}");

            return sidBytes;
        }

        private static byte[] GetMD5Hash(byte[] input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(input);
                return hashBytes;

            }
        }

        private static byte[] ConcatByteArrays(params byte[][] arrays)
        {
            return arrays.SelectMany(x => x).ToArray();
        }

        private static string BytesToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();

        }

        public static byte[] GetKey(string sidValue)
        {
            Console.WriteLine($"[*] Deriving AES key from computer SID");

            byte[] panMD5 = GetMD5Hash(Encoding.ASCII.GetBytes("pannetwork"));
            byte[] sidBytes = GetComputerSID(sidValue);
            byte[] combinedBytes = ConcatByteArrays(sidBytes, panMD5);

            byte[] md5Key = GetMD5Hash(combinedBytes);
            byte[] finalKey = ConcatByteArrays(md5Key, md5Key);

            Console.WriteLine($"\t[*] Derived AES Key: {BytesToString(finalKey)}");

            return finalKey;
        }

    }
}