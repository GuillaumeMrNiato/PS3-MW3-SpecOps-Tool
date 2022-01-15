using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;
using PS3Lib.NET;
using System.Drawing;
using System.Runtime.CompilerServices; //Using For CCAPI
using System.Windows.Media.Media3D; //Vector3D
using System.Globalization;
using System.Threading;

namespace CoD_Public_Cheater
{
    public static class Lib
    {
        private static PS3API PS3 = new PS3API();
        public static TMAPI TMAPI = new TMAPI();
        public static PS3TMAPI PS3TMAPI = new PS3TMAPI();
        public static CCAPI CCAPI = new CCAPI();
        public static double Pi = 3.141592653589793;
        public static int maxIntValue = 2147483647;
        public static int minIntValue = -2147483647;
        public static byte[] stringToBytesASCII(String str)
        {
            char[] buffer = str.ToCharArray();
            byte[] b = new byte[buffer.Length];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)buffer[i];
            }
            return b;
        }
        public static String ReturnInfo(UInt32 g_gametype, Int32 Length, Int32 Index)
        {
            try
            {
                return Encoding.ASCII.GetString(GetBytes(g_gametype, Length)).Replace(@"\", "|").Split('|')[Index];
            }
            catch
            {
                return null;
            }
        }
        public static String GetTextBetween(String source, String leftWord, String rightWord)
        {
            return
          System.Text.RegularExpressions.Regex.Match(source, String.Format(@"{0}(?<words>[-\\^\w\s]+){1}", leftWord, rightWord),
                              System.Text.RegularExpressions.RegexOptions.IgnoreCase).Groups["words"].Value;
        }
        public static void Sleep(Int32 millisecondsTimeout)
        {
            System.Threading.Thread.Sleep(millisecondsTimeout);
        }
        public static sbyte[] ReadSBytes(UInt32 address, int length)
        {
            byte[] memory = GetMemoryL(address, length);
            sbyte[] numArray = new sbyte[length];
            for (int i = 0; i < length; i++)
            {
                numArray[i] = (sbyte)memory[i];
            }
            return numArray;
        }
        public static bool WriteBytesToggle(UInt32 Offset, byte[] On, byte[] Off)
        {
            bool flag = ReadByte(Offset) == On[0];
            WriteBytes(Offset, !flag ? On : Off);
            return flag;
        }

        public static void WriteDouble(UInt32 address, double input, Boolean Reverse = true)
        {
            byte[] array = new byte[8];
            BitConverter.GetBytes(input).CopyTo(array, 0);
            if (Reverse == true) { Array.Reverse(array, 0, 8); }
            SetMemory(address, array);
        }

        public static void WriteDouble(UInt32 address, double[] input, Boolean Reverse = true)
        {
            int length = input.Length;
            byte[] array = new byte[length * 8];
            for (int i = 0; i < length; i++)
            {
                if (Reverse == true) { ReverseBytes(BitConverter.GetBytes(input[i])).CopyTo(array, (int)(i * 8)); }
                else { BitConverter.GetBytes(input[i]).CopyTo(array, (int)(i * 8)); }
            }
            SetMemory(address, array);
        }

        public static double[] ReadDouble(UInt32 address, int length)
        {
            byte[] memory = GetMemoryL(address, length * 8);
            ReverseBytes(memory);
            double[] numArray = new double[length];
            for (int i = 0; i < length; i++)
            {
                numArray[i] = BitConverter.ToSingle(memory, ((length - 1) - i) * 8);
            }
            return numArray;
        }
        public static class ControleConsole
        {
            private enum ProcessType
            {
                CURRENTGAME = PS3Lib.CCAPI.ProcessType.CURRENTGAME,
                SYS_AGENT = PS3Lib.CCAPI.ProcessType.SYS_AGENT,
                VSH = PS3Lib.CCAPI.ProcessType.VSH
            }
            public static int AttachProcess() { return CCAPI.AttachProcess(); }
            public static int AttachProcess(uint process) { return CCAPI.AttachProcess(process); }
            public static int AttachProcess(PS3Lib.CCAPI.ProcessType procType) { return CCAPI.AttachProcess(procType); }
            public static bool ConnectTarget() { return CCAPI.ConnectTarget(); }
            public static int ConnectTarget(string targetIP) { return CCAPI.ConnectTarget(targetIP); }
            public static void ClearTargetInfo() { CCAPI.ClearTargetInfo(); }
        }
        public static class ASCII
        {
            public static byte[] GetBytes(string s)
            {
                return Encoding.ASCII.GetBytes(Lib.DetectButtonsBytes(s));
            }
        }
        public static class UTF8
        {
            public static byte[] GetBytes(string s)
            {
                return Encoding.UTF8.GetBytes(Lib.DetectButtonsBytes(s));
            }
        }
        public static void WriteMemory(UInt32 Address, UInt32 Interval, Int32 Clients, Byte[] Bytes)
        {
            for (UInt32 i = 0; i < Clients; i++)
            {
                Lib.SetMemory(Address + i * Interval, Bytes);
            }
        }
        public static void Wait(Int32 Seconds, Int32 Minutes, Int32 Hours, Int32 Days)
        {
            Int32 seconds = 1000;
            Int32 Results = (Days + seconds) * 86400 + (Hours + seconds) * 3600 + (Minutes + seconds) * 60 + (Seconds * seconds);
            Thread.Sleep(Results);
        }
        public static void Wait(Int32 Seconds, Int32 Minutes, Int32 Hours)
        {
            Int32 seconds = 1000;
            Int32 Results = (Hours + seconds) * 3600 + (Minutes + seconds) * 60 + (Seconds * seconds);
            Thread.Sleep(Results);
        }
        public static void Wait(Int32 Seconds, Int32 Minutes)
        {
            Int32 seconds = 1000;
            Int32 Results = (Minutes + seconds) * 60 + (Seconds * seconds);
            Thread.Sleep(Results);
        }
        public static void Wait(Int32 Seconds)
        {
            Int32 seconds = 1000;
            Int32 Results = (Seconds * seconds);
            Thread.Sleep(Results);
        }
        public static List<Byte> _add_(List<Byte> A, Byte b, Int32 idx = 0, Byte rem = 0)
        {
            short sample = 0;
            if (idx < A.Count)
            {
                sample = (short)((short)A[idx] + (short)b);
                A[idx] = (Byte)(sample % 256);
                rem = (Byte)((sample - A[idx]) % 255);
                if (rem > 0)
                    return _add_(A, (Byte)rem, idx + 1);
            }
            else A.Add(b);

            return A;
        }
        public static Byte[] Multiply(this Byte[] A, Byte[] B)
        {
            List<Byte> ans = new List<Byte>();

            Byte ov, res;
            Int32 idx = 0;
            for (Int32 i = 0; i < A.Length; i++)
            {
                ov = 0;
                for (Int32 j = 0; j < B.Length; j++)
                {
                    short result = (short)(A[i] * B[j] + ov);
                    ov = (Byte)(result >> 8);
                    res = (Byte)result;
                    idx = i + j;
                    if (idx < (ans.Count))
                        ans = _add_(ans, res, idx);
                    else ans.Add(res);
                }
                if (ov > 0)
                    if (idx + 1 < (ans.Count))
                        ans = _add_(ans, ov, idx + 1);
                    else ans.Add(ov);
            }

            return ans.ToArray();
        }
        private static Byte[] Combine(Byte[] Arr1, Byte[] Arr2)
        {
            Byte[] Res = new Byte[Arr1.Length + Arr2.Length];
            Buffer.BlockCopy(Arr1, 0, Res, 0, Arr1.Length);
            Buffer.BlockCopy(Arr2, 0, Res, Arr1.Length, Arr2.Length);
            return Res;
        }
        public static float[] vectoangles(float[] Angles)
        {
            float forward;
            float yaw, pitch;
            float[] angles = new float[3];
            if (Angles[1] == 0 && Angles[0] == 0)
            {
                yaw = 0;
                if (Angles[2] > 0) pitch = 90f;
                else pitch = 270f;
            }
            else
            {
                if (Angles[0] != -1) yaw = (float)(Math.Atan2((double)Angles[1], (double)Angles[0]) * 180f / Math.PI);
                else if (Angles[1] > 0) yaw = 90f;
                else yaw = 270;
                if (yaw < 0) yaw += 360f;

                forward = (float)Math.Sqrt((double)(Angles[0] * Angles[0] + Angles[1] * Angles[1]));
                pitch = (float)(Math.Atan2((double)Angles[2], (double)forward) * 180f / Math.PI);
                if (pitch < 0) pitch += 360f;
            }
            angles[0] = -pitch;
            angles[1] = yaw;
            angles[2] = 0;

            return angles;
        }
        public static float[] vectoanglesAsNum(float[] Angles)
        {
            float num2;
            float num3;
            float[] numArray = new float[3];
            if ((Angles[1] == 0f) && (Angles[0] == 0f))
            {
                num2 = 0f;
                if (Angles[2] > 0f)
                {
                    num3 = 90f;
                }
                else
                {
                    num3 = 270f;
                }
            }
            else
            {
                if (Angles[0] != -1f)
                {
                    num2 = (float)((Math.Atan2((double)Angles[1], (double)Angles[0]) * 180.0) / 3.1415926535897931);
                }
                else if (Angles[1] > 0f)
                {
                    num2 = 90f;
                }
                else
                {
                    num2 = 270f;
                }
                if (num2 < 0f)
                {
                    num2 += 360f;
                }
                float num = (float)Math.Sqrt((double)((Angles[0] * Angles[0]) + (Angles[1] * Angles[1])));
                num3 = (float)((Math.Atan2((double)Angles[2], (double)num) * 180.0) / 3.1415926535897931);
                if (num3 < 0f)
                {
                    num3 += 360f;
                }
            }
            numArray[0] = -num3;
            numArray[1] = num2;
            return numArray;
        }
        public static float[] getVector(float[] point1, float[] point2)
        {
            return new float[] { (point2[0] - point1[0]), (point2[1] - point1[1]), (point2[2] - point1[2]) };
        }

        public static float Distance3D(float[] point1, float[] point2)
        {
            float deltax = point2[0] - point1[0];
            float deltay = point2[1] - point1[1];
            float deltaz = point2[2] - point1[2];
            return Convert.ToSingle(Math.Sqrt((deltax * deltax) + (deltay * deltay) + (deltaz * deltaz)));
        }

        public static float Distance2D(float[] point1, float[] point2)
        {
            float deltax = point2[0] - point1[0];
            float deltaz = point2[1] - point1[1];
            return Convert.ToSingle(Math.Sqrt((deltax * deltax) + (deltaz * deltaz)));
        }

        public static float VecDistance3D(Single[] Vector)
        {
            return Convert.ToSingle(Math.Sqrt((Vector[0] * Vector[0]) + (Vector[1] * Vector[1]) + (Vector[2] * Vector[2])));
        }

        public static Single[] VecMultiply(Single[] Vector, Single Value)
        {
            return new Single[] { Vector[0] *= Value, Vector[1] *= Value, Vector[2] *= Value };
        }

        public static void vec_scale(float[] vec, float scale, out float[] Forward)
        {
            Forward = new float[] { vec[0] * scale, vec[1] * scale, vec[2] * scale };
        }

        public static void lockIntDvarToValue(UInt32 pointer, Byte[] value)
        {
            UInt32 _flag = 0x4;                           // First value is pointer to name ( const char* ), so dvar flag is at 0x4 
            UInt32 _value = 0xB;                          // Default value is at 0x11

            // Get pointer to dvar
            Byte[] buffer = new Byte[4];
            Lib.GetMemory(pointer, buffer);
            Array.Reverse(buffer);
            UInt32 dvar = BitConverter.ToUInt32(buffer, 0);

            // Get current dvar flag
            Byte[] flag = new Byte[2];
            Lib.GetMemory(dvar + _flag, flag);
            Array.Reverse(flag);
            ushort shortFlag = BitConverter.ToUInt16(flag, 0);

            // Check if dvar is already write protected
            if ((shortFlag & 0x800) != 0x800)
            {
                shortFlag |= 0x800;

                flag = BitConverter.GetBytes(shortFlag);
                Array.Reverse(flag);

                // Apply new dvarflag
                Lib.SetMemory(dvar + _flag, flag);
            }

            // Apply new value
            Lib.SetMemory(dvar + _value, value);//FIXED THIS FOR NUMERICUPDOWN --"BaSs_HaXoR"
        }
        public static void WriteVec(UInt32 address, Single[] vec)
        {
            for (uint i = 0, f = 0; i < vec.Length; i++, f += 4)
            {
                Lib.WriteSingle((address) + f, vec[i]);
            }
        }
        public static void WriteIntToByte(UInt32 Address, Int32 Value)
        {
            int num = (int)Value;
            byte num2 = (byte)num;
            byte num3 = (byte)(num >> 8);
            Lib.SetMemory(Address, new byte[] { num3, num2 });
        }
        public static void ReadIntToByte(UInt32 Address, Int32 Value)
        {
            int num = (int)Value;
            byte num2 = (byte)num;
            byte num3 = (byte)(num >> 8);
            Lib.GetMemory(Address, new byte[] { num3, num2 });
        }
        public static Single[] ReadVec(UInt32 address, UInt32 dim)
        {
            Single[] vec = new Single[dim];
            for (uint i = 0, f = 0; i < dim; i++, f += 4)
            {
                vec[i] = Lib.ReadSingle((address) + f);
            }
            return vec;
        }
        public static string ByteArrayToString(Byte[] Bytes)
        {
            //Function to Convert a Byte array Int32o a string
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString(Bytes);
        }
        public static float[] ReadFloatLength(uint Offset, int Length)
        {
            byte[] buffer = new byte[Length * 4];
            PS3.GetMemory(Offset, buffer);
            Array.Reverse(buffer);
            float[] FArray = new float[Length];
            for (int i = 0; i < Length; i++)
            {
                FArray[i] = BitConverter.ToSingle(buffer, (Length - 1 - i) * 4);
            }
            return FArray;
        }
        public static String getDetails(UInt32 g_gametype, String type)
        {
            String Main = Lib.ReadString(g_gametype);
            String[] MainArray = Main.Split('\\');
            Int32 StrLen = Main.Length;
            Int32 strIndex = 0;
            for (Int32 i = 0; i < MainArray.Length; i++)
            {
                if (MainArray[i] == type)
                {
                    i++;
                    strIndex = i;
                    continue;
                }
            }
            return MainArray[strIndex];
        }
        
        public static String DetectButtonsCodes(String TexT)
        {
            String Texting = TexT.Replace("[X]", "[{+gostand}]")
                .Replace("[O]", "[{+stance}]")
                .Replace("[ ]", "[{+usereload}]")
                .Replace("[T]", "[{weapnext}]")
                .Replace("[R1]", "[{+attack}]")
                .Replace("[L1]", "[{+speed_throw}]")
                .Replace("[L2]", "[{+smoke}]")
                .Replace("[R2]", "[{+frag}]")
                .Replace("[R3]", "[{+melee}]")
                .Replace("[L3]", "[{+breath_sprint}]")
                .Replace("[UP]", "[{+actionslot 1}]")
                .Replace("[RIGHT]", "[{+actionslot 2}]")
                .Replace("[DOWN]", "[{+actionslot 3}]")
                .Replace("[LEFT]", "[{+actionslot 4}]")
                .Replace("[LINE]", "\n")
                .Replace("(X)", "[{+gostand}]")
                .Replace("(O)", "[{+stance}]")
                .Replace("( )", "[{+usereload}]")
                .Replace("(T)", "[{weapnext}]")
                .Replace("(R1)", "[{+attack}]")
                .Replace("(L1)", "[{+speed_throw}]")
                .Replace("(L2)", "[{+smoke}]")
                .Replace("(R2)", "[{+frag}]")
                .Replace("(R3)", "[{+melee}]")
                .Replace("(L3)", "[{+breath_sprint}]")
                .Replace("(UP)", "[{+actionslot 1}]")
                .Replace("(RIGHT)", "[{+actionslot 2}]")
                .Replace("(DOWN)", "[{+actionslot 3}]")
                .Replace("(LEFT)", "[{+actionslot 4}]")
                .Replace("(LINE)", "\n");
            return Texting;
        }
        public static String DetectButtonsBytes(String TexT)
        {
            String Texting = TexT.Replace("[X]", "\x0001").Replace("[O]", "\x0002").Replace("[]", "\x0003")
                .Replace("[Y]", "\x0004").Replace("[L1]", "\x0005").Replace("[R1]", "\x0006").Replace("[L3]", "\x0010")
                .Replace("[R3]", "\x0011").Replace("[L2]", "\x0012").Replace("[R2]", "\x0013").Replace("[UP]", "\x0014")
                .Replace("[DOWN]", "\x0015").Replace("[LEFT]", "\x0016").Replace("[RIGHT]", "\x0017").Replace("[START]", "\x000e")
                .Replace("[SELECT]", "\x000f").Replace("[LINE]", "\x000a").Replace("[3D]", "\x000d").Replace("\n", "\x000a").Replace("\r", "\x000d").Replace("[\n]", "\x000a").Replace("null", "\x0000");
            return Texting;
        }
        public static byte[] ReadMemory(UInt32 Address, UInt32 Interval, Int32 Clients, int Length)
        {
            Byte[] buffer = new Byte[Length];
            for (UInt32 i = 0; i < Clients; i++)
            {
                Lib.GetMemory(Address + i * Interval, buffer);
            }
            return buffer;
        }
        public static String char_to_wchar(String text)
        {
            String wchar = text;
            for (Int32 i = 0; i < text.Length; i++)
            {
                wchar = wchar.Insert(i * 2, "\0");
            }
            return wchar;
        }
        public static void WriteInt(UInt32 address, Int32 val, Boolean Reverse = true)
        {
            if (Reverse == true) { Lib.SetMemory(address, ReverseBytes(BitConverter.GetBytes(val))); }
            else { Lib.SetMemory(address, BitConverter.GetBytes(val)); }
        }
        public static Byte[] Reverse(Byte[] buff)
        {
            Array.Reverse(buff);
            return buff;
        }
        public static void WriteShort(UInt32 address, Int32 val, bool dvar = false)
        {
            Byte[] data = BitConverter.GetBytes(val);
            if (!dvar)
                Lib.SetMemory(address, new Byte[] { data[0], data[1] });
            else
                Lib.SetMemory(address, new Byte[] { data[1], data[0] });

        }
        public static void WriteUInt(UInt32 address, UInt32 val, Boolean Reverse = true)
        {
            if (Reverse == true) { Lib.SetMemory(address, ReverseBytes(BitConverter.GetBytes(val))); }
            else { Lib.SetMemory(address, BitConverter.GetBytes(val)); }
        }

        public static bool CompareByteArray(Byte[] a, Byte[] b)
        {
            Int32 num = 0;
            for (Int32 i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i])
                {
                    num++;
                }
            }
            return (num == a.Length);
        }
        public static String centerString(String[] StringArray)
        {
            Int32 num3;
            Int32 length = 0;
            Int32 num2 = 0;
            String str = "";
            for (num3 = 0; num3 < StringArray.Length; num3++)
            {
                if (StringArray[num3].Length > length)
                {
                    length = StringArray[num3].Length;
                }
            }
            for (num3 = 0; num3 < StringArray.Length; num3++)
            {
                str = "";
                if (StringArray[num3].Length < length)
                {
                    num2 = length - StringArray[num3].Length;
                    if (StringArray[num3].Contains("[{+"))
                    {
                        num2 += 13;
                    }
                    for (Int32 i = 0; i < num2; i++)
                    {
                        str = str + " ";
                    }
                }
                StringArray[num3] = str + StringArray[num3];
            }
            String str2 = "";
            for (num3 = 0; num3 < StringArray.Length; num3++)
            {
                str2 = str2 + StringArray[num3] + "\n";
            }
            return ("^7" + str2);
        }
        public static void FillMemory(UInt32 Start, UInt32 Length, Byte[] Bytes)
        {
            UInt32 ArrayLength = (UInt32)Bytes.Length;
            UInt32 End = Start + Length;
            for (UInt32 i = Start; i < End; )
            {
                while (End > i)
                {
                    Lib.SetMemory(i, Bytes);
                    i += ArrayLength;
                }
            }
        }
        public static Int32 ReadInt(UInt32 Offset, Boolean Reverse = true)
        {
            Byte[] buffer = new Byte[4];
            PS3.GetMemory(Offset, buffer);
            if (Reverse == true) { Array.Reverse(buffer); }
            Int32 Value = BitConverter.ToInt32(buffer, 0);
            return Value;
        }
        public static void WriteFloatArray(UInt32 Offset, float[] Array)
        {
            try
            {
                Byte[] buffer = new Byte[Array.Length * 4];
                for (Int32 Lenght = 0; Lenght < Array.Length; Lenght++)
                {
                    ReverseBytes(BitConverter.GetBytes(Array[Lenght])).CopyTo(buffer, Lenght * 4);
                }
                Lib.SetMemory(Offset, buffer);
            }
            catch { }
        }
        public static float[] AnglesToForward(float[] Origin, float[] Angles, uint diff)
        {
            float num = ((float)Math.Sin((Angles[0] * Math.PI) / 180)) * diff;
            float num1 = (float)Math.Sqrt(((diff * diff) - (num * num)));
            float num2 = ((float)Math.Sin((Angles[1] * Math.PI) / 180)) * num1;
            float num3 = ((float)Math.Cos((Angles[1] * Math.PI) / 180)) * num1;
            return new float[] { Origin[0] + num3, Origin[1] + num2, Origin[2] - num };
        }
        public static UInt32 G_Client(UInt32 G_ClientAddress, UInt32 clientIndex, UInt32 offset, UInt32 G_ClientSize)
        {
            return (G_ClientAddress + offset + clientIndex * G_ClientSize);
        }
        public static UInt32 G_Entity(UInt32 G_EntityAddress, UInt32 clientIndex, UInt32 offset, UInt32 G_EntitySize)
        {
            return (G_EntityAddress + offset + clientIndex * G_EntitySize);
        }
        public static void GetMemory(UInt32 Offset, Byte[] Buffer)
        {
            try
            {
                PS3.GetMemory(Offset, Buffer);
            }
            catch { }
        }
        public static void And_Int32(UInt32 address, Int32 input)
        {
            Int32 num = Lib.ReadInt32(address) & input;
            Lib.WriteInt32(address, num);
        }
        public static float ReadSingle(UInt32 address)
        {
            Byte[] memory = GetMemoryL(address, 4);
            Array.Reverse(memory, 0, 4);
            return BitConverter.ToSingle(memory, 0);
        }
        public static float[] ReadSingle(UInt32 address, Int32 length)
        {
            Byte[] mem = Lib.ReadBytes(address, length * 4);
            ReverseBytes(mem);
            float[] numArray = new float[length];
            for (Int32 index = 0; index < length; ++index)
                numArray[index] = BitConverter.ToSingle(mem, (length - 1 - index) * 4);
            return numArray;
        }
        public static Byte[] ArrayReverse(Byte[] Byte_43)
        {
            Array.Reverse((Array)Byte_43);
            return Byte_43;
        }
        public static void Attach()
        {
            try
            {
                PS3.AttachProcess();
            }
            catch { }
        }
        public static void AttachProcess()
        {
            try
            {
                PS3.AttachProcess();
            }
            catch { }
        }
        public static void ConnectTarget()
        {
            try
            {
                PS3.ConnectTarget();
            }
            catch { }
        }
        public static void Connect()
        {
            try
            {
                PS3.ConnectTarget();
            }
            catch { }
        }
        public static void Or_Int32(UInt32 address, Int32 input)
        {
            Int32 or = Lib.ReadInt32(address);
            or |= input;
            Lib.WriteInt32(address, or);
        }
        public static void DisconnectTarget()
        {
            try
            {
                PS3.DisconnectTarget();
            }
            catch { }
        }

        public static void ChangeAPI(SelectAPI API)
        {
            try
            {
                PS3.ChangeAPI(API);
            }
            catch { }
        }
        public static Byte[] GetMemoryL(UInt32 address, Int32 length)
        {
            Byte[] buffer = new Byte[length];
            Lib.GetMemory(address, buffer);
            return buffer;
        }
        public static void SetMemory(UInt32 Address, Byte[] Bytes)
        {
            try
            {
                if (PS3.GetCurrentAPI() == SelectAPI.TargetManager)
                    SetMem(Address, Bytes, SelectAPI.TargetManager);
                else if (PS3.GetCurrentAPI() == SelectAPI.ControlConsole)
                    SetMem(Address, Bytes, SelectAPI.ControlConsole);
            }
            catch
            { }
        }
        private static void getMemR(UInt32 Address, ref byte[] buffer, SelectAPI API)
        {
            try
            {
                if (API == SelectAPI.ControlConsole)
                    CCAPI.GetMemory(Address, buffer);
                else if (API == SelectAPI.TargetManager)
                    TMAPI.GetMemory(Address, buffer);
            }
            catch { }
        }
        public static void GetMemoryR(uint offset, ref byte[] buffer)
        {
            getMemR(offset, ref buffer, CurrentAPI);
        }
        public static SelectAPI CurrentAPI;
        public static Byte[] GetBytes(UInt32 Offset, Int32 Length)
        {
            return PS3.GetBytes(Offset, Length);
        }
        private static Byte[] GetBytes(UInt32 offset, Int32 length, SelectAPI API)
        {
            Byte[] Bytes = new Byte[length];
            if (API == SelectAPI.ControlConsole)
            {
                CurrentAPI = PS3.GetCurrentAPI();
                return PS3.GetBytes(offset, length);
            }
            if (API == SelectAPI.TargetManager)
            {
                CurrentAPI = PS3.GetCurrentAPI();
                Bytes = PS3.GetBytes(offset, length);
            }
            return Bytes;
        }

        private static void GetMem(UInt32 offset, Byte[] buffer, SelectAPI API)
        {
            try
            {
                if (API == SelectAPI.ControlConsole)
                {
                    GetMemoryR(offset, ref buffer);
                }
                else if (API == SelectAPI.TargetManager)
                {
                    GetMemoryR(offset, ref buffer);
                }
            }
            catch { }
        }

        public static bool ReadBool(UInt32 offset)
        {
            Byte[] buffer = new Byte[1];
            GetMem(offset, buffer, CurrentAPI);
            return (buffer[0] != 0);
        }

        public static Byte ReadByte(UInt32 offset)
        {
            return GetBytes(offset, 1, CurrentAPI)[0];
        }
        public static Boolean ReadByte(uint Address, Byte Byte)
        {
            byte[] buffer = new byte[1];
            Lib.GetMemoryR(Address, ref buffer);
            bool flag = new byte[] { Byte }.SequenceEqual<byte>(buffer);
            if (flag)
            { return true; }
            return false;
        }
        public static Byte[] ReadBytes(UInt32 offset, Int32 length)
        {
            return GetBytes(offset, length, CurrentAPI);
        }
        public static Boolean ReadBytes(uint Address, byte[] newBytes)
        {
            byte[] buffer = new byte[newBytes.Length];
            Lib.GetMemoryR(Address, ref newBytes);
            bool Flag = newBytes.SequenceEqual<byte>(buffer);
            if (Flag)
                return true;
            return false;
        }
        public static float ReadFloat(UInt32 offset, Boolean Reverse = true)
        {
            Byte[] array = GetBytes(offset, 4, CurrentAPI);
            if (Reverse == true) { Array.Reverse(array, 0, 4); }
            return BitConverter.ToSingle(array, 0);
        }

        public static short ReadInt16(UInt32 offset, Boolean Reverse = true)
        {
            Byte[] array = GetBytes(offset, 2, CurrentAPI);
            if (Reverse == true) { Array.Reverse(array, 0, 2); }
            return BitConverter.ToInt16(array, 0);
        }

        public static Int32 ReadInt32(UInt32 offset, Boolean Reverse = true)
        {
            Byte[] array = GetBytes(offset, 4, CurrentAPI);
            if (Reverse == true) { Array.Reverse(array, 0, 4); }
            return BitConverter.ToInt32(array, 0);
        }

        public static void WriteSingle(UInt32 address, float input)
        {
            Byte[] array = new Byte[4];
            BitConverter.GetBytes(input).CopyTo(array, 0);
            Array.Reverse(array, 0, 4);
            SetMemory(address, array);
        }

        public static void WriteSingle(UInt32 address, float[] input)
        {
            Int32 length = input.Length;
            Byte[] array = new Byte[length * 4];
            for (Int32 i = 0; i < length; i++)
            {
                ReverseBytes(BitConverter.GetBytes(input[i])).CopyTo(array, (Int32)(i * 4));
            }
            PS3.SetMemory(address, array);
        }

        public static Byte[] ReverseBytes(Byte[] inArray)
        {
            Array.Reverse(inArray);
            return inArray;
        }
        public static string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            if (s == null) return null;
            else
                return new string(charArray);
        }
        public static long ReadInt64(UInt32 offset, Boolean Reverse = true)
        {
            Byte[] array = GetBytes(offset, 8, CurrentAPI);
            if (Reverse == true) { Array.Reverse(array, 0, 8); }
            return BitConverter.ToInt64(array, 0);
        }

        public static sbyte ReadSByte(UInt32 offset)
        {
            Byte[] buffer = new Byte[1];
            GetMem(offset, buffer, CurrentAPI);
            return (sbyte)buffer[0];
        }

        public static string ReadString(uint offset)
        {
            int block = 40;
            int addOffset = 0;
            string str = "";
        repeat:
            byte[] buffer = ReadBytes(offset + (uint)addOffset, block);
            str += Encoding.UTF8.GetString(buffer);
            addOffset += block;
            if (str.Contains('\0'))
            {
                int index = str.IndexOf('\0');
                string final = str.Substring(0, index);
                str = String.Empty;
                return final;
            }
            else
                goto repeat;
        }
        public static Byte[] ReverseArray(float float_0)
        {
            Byte[] Bytes = BitConverter.GetBytes(float_0);
            Array.Reverse(Bytes);
            return Bytes;
        }

        public static Byte[] uintBytes(UInt32 input)
        {
            Byte[] data = BitConverter.GetBytes(input);
            Array.Reverse(data);
            return data;
        }
        public static Byte[] ToHexFloat(float Axis)
        {
            Byte[] Bytes = BitConverter.GetBytes(Axis);
            Array.Reverse(Bytes);
            return Bytes;
        }

        public static ushort ReadUInt16(UInt32 offset, Boolean Reverse = true)
        {
            Byte[] array = GetBytes(offset, 2, CurrentAPI);
            if (Reverse == true) { Array.Reverse(array, 0, 2); }
            return BitConverter.ToUInt16(array, 0);
        }

        public static UInt32 ReadUInt32(UInt32 offset, Boolean Reverse = true)
        {
            Byte[] array = GetBytes(offset, 4, CurrentAPI);
            if (Reverse == true) { Array.Reverse(array, 0, 4); }
            return BitConverter.ToUInt32(array, 0);
        }

        public static ulong ReadUInt64(UInt32 offset, Boolean Reverse = true)
        {
            Byte[] array = GetBytes(offset, 8, CurrentAPI);
            if (Reverse == true) { Array.Reverse(array, 0, 8); }
            return BitConverter.ToUInt64(array, 0);
        }

        private static void SetMem(UInt32 Address, Byte[] buffer, SelectAPI API)
        {
            try
            {
                if (API == SelectAPI.ControlConsole)
                    PS3.CCAPI.SetMemory(Address, buffer);
                else if (API == SelectAPI.TargetManager)
                    PS3.TMAPI.SetMemory(Address, buffer);
            }
            catch { }
        }
        public static void WriteBool(UInt32 offset, bool input)
        {
            try
            {
                Byte[] buffer = new Byte[] { input ? ((Byte)1) : ((Byte)0) };
                SetMem(offset, buffer, CurrentAPI);
            }
            catch { }
        }

        public static void WriteByte(UInt32 offset, Byte input)
        {
            try
            {
                Byte[] buffer = new Byte[] { input };
                SetMem(offset, buffer, CurrentAPI);
            }
            catch { }
        }
        public static void WriteByte(UInt32 offset, Byte input1, Byte input2)
        {
            try
            {
                Byte[] buffer = new Byte[] { input1, input2 };
                SetMem(offset, buffer, CurrentAPI);
            }
            catch { }
        }
        public static void WriteByte(UInt32 offset, Byte input1, Byte input2, Byte input3)
        {
            try
            {
                Byte[] buffer = new Byte[] { input1, input2, input3 };
                SetMem(offset, buffer, CurrentAPI);
            }
            catch { }
        }
        public static void WriteByte(UInt32 offset, Byte input1, Byte input2, Byte input3, Byte input4)
        {
            try
            {
                Byte[] buffer = new Byte[] { input1, input2, input3, input4 };
                SetMem(offset, buffer, CurrentAPI);
            }
            catch { }
        }

        public static void WriteBytes(UInt32 offset, Byte[] input)
        {
            try
            {
                Byte[] buffer = input;
                SetMem(offset, buffer, CurrentAPI);
            }
            catch { }
        }
        public static void WriteBytes(uint address, byte[] ByteArray, int length)
        {
            int i = 0;
            uint Address = Convert.ToUInt32(address + 1 * i);
            while (i < length)
            {
                SetMemory(Address, ByteArray);
            }
        }
        public static void WriteByte(uint address, byte Byte, int length)
        {
            int i = 0;
            uint Address = Convert.ToUInt32(address + 1 * i);
            byte[] BYTE = new byte[] { Byte };
            while (i < length)
            {
                SetMemory(Address, BYTE);
            }
        }
        public static void WriteFloat(UInt32 offset, float input, Boolean Reverse = true)
        {
            try
            {
                Byte[] array = new Byte[4];
                BitConverter.GetBytes(input).CopyTo(array, 0);
                if (Reverse == true) { Array.Reverse(array, 0, 4); }
                SetMem(offset, array, CurrentAPI);
            }
            catch { }
        }
        private static void WriteFloat(UInt32 Offset, Single[] input)
        {
            try
            {
                for (UInt32 i = 0; i < input.Length; i++)
                    WriteFloat(Offset + (i * 4), input[i]);
            }
            catch { }
        }

        private static SelectAPI GetCurrentAPI()
        {
            return PS3.GetCurrentAPI();
        }
        public static void WriteInt16(UInt32 offset, short input, Boolean Reverse = true)
        {
            try
            {
                Byte[] array = new Byte[2];
                BitConverter.GetBytes(input).CopyTo(array, 0);
                if (Reverse == true) { Array.Reverse(array, 0, 2); }
                SetMem(offset, array, CurrentAPI);
            }
            catch { }
        }

        public static void WriteInt32(UInt32 offset, int input, Boolean Reverse = true)
        {
            try
            {
                Byte[] array = new Byte[4];
                BitConverter.GetBytes(input).CopyTo(array, 0);
                if (Reverse == true) { Array.Reverse(array, 0, 4); }
                SetMem(offset, array, CurrentAPI);
            }
            catch { }
        }

        public static void WriteInt64(UInt32 offset, long input, Boolean Reverse = true)
        {
            try
            {
                Byte[] array = new Byte[8];
                BitConverter.GetBytes(input).CopyTo(array, 0);
                if (Reverse == true) { Array.Reverse(array, 0, 8); }
                SetMem(offset, array, CurrentAPI);
            }
            catch { }
        }
        public static void WriteSByte(UInt32 offset, sbyte input)
        {
            try
            {
                Byte[] buffer = new Byte[] { (Byte)input };
                SetMem(offset, buffer, CurrentAPI);
            }
            catch { }
        }
        public static byte[] RGBA(decimal R, decimal G, decimal B, decimal A)
        {
            byte[] RGBA = new byte[4];
            byte[] RVal = BitConverter.GetBytes(Convert.ToInt32(R));
            byte[] GVal = BitConverter.GetBytes(Convert.ToInt32(G));
            byte[] BVal = BitConverter.GetBytes(Convert.ToInt32(B));
            byte[] AVal = BitConverter.GetBytes(Convert.ToInt32(A));
            RGBA[0] = RVal[0]; RGBA[1] = GVal[0]; RGBA[2] = BVal[0];
            RGBA[3] = AVal[0]; return RGBA;
        }
        public static void WriteString(UInt32 offset, String input)
        {
            try
            {
                Byte[] Bytes = Encoding.UTF8.GetBytes(DetectButtonsBytes(input));
                Array.Resize<Byte>(ref Bytes, Bytes.Length + 1);
                SetMem(offset, Bytes, CurrentAPI);
            }
            catch { }
        }

        public static void WriteUInt16(UInt32 offset, ushort input, Boolean Reverse = true)
        {
            try
            {
                Byte[] array = new Byte[2];
                BitConverter.GetBytes(input).CopyTo(array, 0);
                if (Reverse == true) { Array.Reverse(array, 0, 2); }
                SetMem(offset, array, CurrentAPI);
            }
            catch { }
        }
        public static void SetMemoryref(uint Address, ref byte[] buffer)
        {
            SetMemory(Address, buffer);
        }
        public static string EncodingText(byte[] byte_43)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(byte_43);
        }
        public static void WriteUInt32(UInt32 offset, uint input, Boolean Reverse = true)
        {
            try
            {
                Byte[] array = new Byte[4];
                BitConverter.GetBytes(input).CopyTo(array, 0);
                if (Reverse == true) { Array.Reverse(array, 0, 4); }
                SetMem(offset, array, CurrentAPI);
            }
            catch { }
        }

        public static void WriteUInt64(UInt32 offset, ulong input, Boolean Reverse = true)
        {
            try
            {
                Byte[] array = new Byte[8];
                BitConverter.GetBytes(input).CopyTo(array, 0);
                if (Reverse == true) { Array.Reverse(array, 0, 8); }
                SetMem(offset, array, CurrentAPI);
            }
            catch { }
        }
        public static class Builder
        {
            public static class ArrayReader
            {
                private static byte[] buffer;
                private static int size;
                private static int None(uint Address, SByte sByte)
                {
                    size = 1;
                    buffer = new byte[] { (Byte)sByte };
                    SetMemory(Address, buffer);
                    return size;
                }
                public static bool GetBool(int pos)
                {
                    return (buffer[pos] != 0);
                }

                public static byte GetByte(int pos)
                {
                    return buffer[pos];
                }

                public static byte[] GetBytes(int pos, int length)
                {
                    byte[] buffer = new byte[length];
                    for (int i = 0; i < length; i++)
                    {
                        buffer[i] = buffer[pos + i];
                    }
                    return buffer;
                }

                public static char GetChar(int pos)
                {
                    return buffer[pos].ToString()[0];
                }

                public static float GetFloat(int pos)
                {
                    byte[] array = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        array[i] = buffer[pos + i];
                    }
                    Array.Reverse(array, 0, 4);
                    return BitConverter.ToSingle(array, 0);
                }

                public static short GetInt16(int pos, EndianType Type = EndianType.BigEndian)
                {
                    byte[] array = new byte[2];
                    for (int i = 0; i < 2; i++)
                    {
                        array[i] = buffer[pos + i];
                    }
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(array, 0, 2);
                    }
                    return BitConverter.ToInt16(array, 0);
                }

                public static int GetInt32(int pos, EndianType Type = EndianType.BigEndian)
                {
                    byte[] array = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        array[i] = buffer[pos + i];
                    }
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(array, 0, 4);
                    }
                    return BitConverter.ToInt32(array, 0);
                }

                public static long GetInt64(int pos, EndianType Type = EndianType.BigEndian)
                {
                    byte[] array = new byte[8];
                    for (int i = 0; i < 8; i++)
                    {
                        array[i] = buffer[pos + i];
                    }
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(array, 0, 8);
                    }
                    return BitConverter.ToInt64(array, 0);
                }

                public static sbyte GetSByte(int pos)
                {
                    return (sbyte)buffer[pos];
                }

                public static string GetString(int pos)
                {
                    int num = 0;
                    while (true)
                    {
                        if (buffer[pos + num] == 0)
                        {
                            byte[] bytes = new byte[num];
                            for (int i = 0; i < num; i++)
                            {
                                bytes[i] = buffer[pos + i];
                            }
                            return Encoding.UTF8.GetString(bytes);
                        }
                        num++;
                    }
                }

                public static ushort GetUInt16(int pos, EndianType Type = EndianType.BigEndian)
                {
                    byte[] array = new byte[2];
                    for (int i = 0; i < 2; i++)
                    {
                        array[i] = buffer[pos + i];
                    }
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(array, 0, 2);
                    }
                    return BitConverter.ToUInt16(array, 0);
                }

                public static uint GetUInt32(int pos, EndianType Type = EndianType.BigEndian)
                {
                    byte[] array = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        array[i] = buffer[pos + i];
                    }
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(array, 0, 4);
                    }
                    return BitConverter.ToUInt32(array, 0);
                }

                public static ulong GetUInt64(int pos, EndianType Type = EndianType.BigEndian)
                {
                    byte[] array = new byte[8];
                    for (int i = 0; i < 8; i++)
                    {
                        array[i] = buffer[pos + i];
                    }
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(array, 0, 8);
                    }
                    return BitConverter.ToUInt64(array, 0);
                }
            }
            public static class ArrayWriter
            {
                private static byte[] buffer;
                private static int size;
                private static int None(uint Address, SByte sByte)
                {
                    size = 1;
                    buffer = new byte[] { (Byte)sByte };
                    SetMemory(Address, buffer);
                    return size;
                }
                public static void SetBool(int pos, bool value)
                {
                    byte[] buffer = new byte[] { value ? ((byte)1) : ((byte)0) };
                    buffer[pos] = buffer[0];
                }

                public static void SetByte(int pos, byte value)
                {
                    buffer[pos] = value;
                }

                public static void SetBytes(int pos, byte[] value)
                {
                    int length = value.Length;
                    for (int i = 0; i < length; i++)
                    {
                        buffer[i + pos] = value[i];
                    }
                }

                public static void SetChar(int pos, char value)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(value.ToString());
                    buffer[pos] = bytes[0];
                }

                public static void SetFloat(int pos, float value)
                {
                    byte[] bytes = BitConverter.GetBytes(value);
                    Array.Reverse(bytes, 0, 4);
                    for (int i = 0; i < 4; i++)
                    {
                        buffer[i + pos] = bytes[i];
                    }
                }

                public static void SetInt16(int pos, short value, EndianType Type = EndianType.BigEndian)
                {
                    byte[] bytes = BitConverter.GetBytes(value);
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(bytes, 0, 2);
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        buffer[i + pos] = bytes[i];
                    }
                }

                public static void SetInt32(int pos, int value, EndianType Type = EndianType.BigEndian)
                {
                    byte[] bytes = BitConverter.GetBytes(value);
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(bytes, 0, 4);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        buffer[i + pos] = bytes[i];
                    }
                }

                public static void SetInt64(int pos, long value, EndianType Type = EndianType.BigEndian)
                {
                    byte[] bytes = BitConverter.GetBytes(value);
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(bytes, 0, 8);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        buffer[i + pos] = bytes[i];
                    }
                }

                public static void SetSByte(int pos, sbyte value)
                {
                    buffer[pos] = (byte)value;
                }

                public static void SetString(int pos, string value)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(value + "\0");
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        buffer[i + pos] = bytes[i];
                    }
                }

                public static void SetUInt16(int pos, ushort value, EndianType Type = EndianType.BigEndian)
                {
                    byte[] bytes = BitConverter.GetBytes(value);
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(bytes, 0, 2);
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        buffer[i + pos] = bytes[i];
                    }
                }

                public static void SetUInt32(int pos, uint value, EndianType Type = EndianType.BigEndian)
                {
                    byte[] bytes = BitConverter.GetBytes(value);
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(bytes, 0, 4);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        buffer[i + pos] = bytes[i];
                    }
                }

                public static void SetUInt64(int pos, ulong value, EndianType Type = EndianType.BigEndian)
                {
                    byte[] bytes = BitConverter.GetBytes(value);
                    if (Type == EndianType.BigEndian)
                    {
                        Array.Reverse(bytes, 0, 8);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        buffer[i + pos] = bytes[i];
                    }
                }
            }
        }
    }
    public static class Write
    {
        private static String Addr(String Address)
        {
            String TempAddr = Address.Replace("0x", "").Replace("&H", "").Replace("u", "");
            return TempAddr;
        }
        public static string ByteArrayToString(byte[] bytes)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString(bytes);
        }
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16); // exception here
            return bytes;
        }
        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static bool IsValidHexString(IEnumerable<char> hexString)
        {
            return hexString.Select(currentCharacter =>
                        (currentCharacter >= '0' && currentCharacter <= '9') ||
                        (currentCharacter >= 'a' && currentCharacter <= 'f') ||
                        (currentCharacter >= 'A' && currentCharacter <= 'F')).All(isHexCharacter => isHexCharacter);
        }
        public static void Bytes(String Address, String Bytes_)
        {
            String TempBytes = Bytes_.Replace("0x", "").Replace(" ", "").Replace(",", "").Replace("&H", "");
            Lib.WriteBytes(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), HexStringToByteArray(TempBytes));
        }
        public static void Strings(String Address, String Text)
        {
            Lib.WriteString(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), Text);
        }
        public static void Decimal(String Address, Decimal Number)
        {
            Byte[] Buffer = BitConverter.GetBytes(Convert.ToInt32(Number));
            Lib.WriteBytes(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), Buffer);
        }
        public static void WriteUInt(String Address, uint uint_)
        {
            Lib.WriteUInt(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), uint_);
        }
        public static void WriteUInt16(String Address, ushort ushort_)
        {
            Lib.WriteUInt16(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), ushort_);
        }
        public static void WriteUInt32(String Address, UInt32 UInt32_)
        {
            Lib.WriteUInt32(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), UInt32_);
        }
        public static void WriteUInt64(String Address, ulong ulong_)
        {
            Lib.WriteUInt64(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), ulong_);
        }

        public static void WriteInt(String Address, int int_)
        {
            Lib.WriteInt(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), int_);
        }
        public static void WriteInt16(String Address, short short_)
        {
            Lib.WriteInt16(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), short_);
        }
        public static void WriteInt32(String Address, Int32 Int32_)
        {
            Lib.WriteInt32(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), Int32_);
        }
        public static void WriteInt64(String Address, long long_)
        {
            Lib.WriteInt64(UInt32.Parse(Addr(Address), NumberStyles.HexNumber), long_);
        }
        public static int[] numbers = new int[5] { 1, 2, 3, 4, 5 };
        public static string[] names = new string[3] { "Matt", "Joanne", "Robert" };
    }

    public enum EndianType
    {
        LittleEndian,
        BigEndian
    }
    public class Shit
    {
        public Shit()
        {

        }
        public void Hey()
        {

        }
    }
    public class ArrayBuilder
    {
        private byte[] buffer;
        private int size;

        public ArrayBuilder(byte[] BytesArray)
        {
            this.buffer = BytesArray;
            this.size = this.buffer.Length;
        }

        public ArrayReader Read
        {
            get
            {
                return new ArrayReader(this.buffer);
            }
        }

        public ArrayWriter Write
        {
            get
            {
                return new ArrayWriter(this.buffer);
            }
        }

        public class ArrayReader
        {
            private byte[] buffer;
            private int size;

            public ArrayReader(byte[] BytesArray)
            {
                this.buffer = BytesArray;
                this.size = this.buffer.Length;
            }

            public bool GetBool(int pos)
            {
                return (this.buffer[pos] != 0);
            }

            public byte GetByte(int pos)
            {
                return this.buffer[pos];
            }

            public byte[] GetBytes(int pos, int length)
            {
                byte[] buffer = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    buffer[i] = this.buffer[pos + i];
                }
                return buffer;
            }

            public char GetChar(int pos)
            {
                return this.buffer[pos].ToString()[0];
            }

            public float GetFloat(int pos)
            {
                byte[] array = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    array[i] = this.buffer[pos + i];
                }
                Array.Reverse(array, 0, 4);
                return BitConverter.ToSingle(array, 0);
            }

            public short GetInt16(int pos, EndianType Type = EndianType.BigEndian)
            {
                byte[] array = new byte[2];
                for (int i = 0; i < 2; i++)
                {
                    array[i] = this.buffer[pos + i];
                }
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(array, 0, 2);
                }
                return BitConverter.ToInt16(array, 0);
            }

            public int GetInt32(int pos, EndianType Type = EndianType.BigEndian)
            {
                byte[] array = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    array[i] = this.buffer[pos + i];
                }
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(array, 0, 4);
                }
                return BitConverter.ToInt32(array, 0);
            }

            public long GetInt64(int pos, EndianType Type = EndianType.BigEndian)
            {
                byte[] array = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    array[i] = this.buffer[pos + i];
                }
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(array, 0, 8);
                }
                return BitConverter.ToInt64(array, 0);
            }

            public sbyte GetSByte(int pos)
            {
                return (sbyte)this.buffer[pos];
            }

            public string GetString(int pos)
            {
                int num = 0;
                while (true)
                {
                    if (this.buffer[pos + num] == 0)
                    {
                        byte[] bytes = new byte[num];
                        for (int i = 0; i < num; i++)
                        {
                            bytes[i] = this.buffer[pos + i];
                        }
                        return Encoding.UTF8.GetString(bytes);
                    }
                    num++;
                }
            }

            public ushort GetUInt16(int pos, EndianType Type = EndianType.BigEndian)
            {
                byte[] array = new byte[2];
                for (int i = 0; i < 2; i++)
                {
                    array[i] = this.buffer[pos + i];
                }
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(array, 0, 2);
                }
                return BitConverter.ToUInt16(array, 0);
            }

            public uint GetUInt32(int pos, EndianType Type = EndianType.BigEndian)
            {
                byte[] array = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    array[i] = this.buffer[pos + i];
                }
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(array, 0, 4);
                }
                return BitConverter.ToUInt32(array, 0);
            }

            public ulong GetUInt64(int pos, EndianType Type = EndianType.BigEndian)
            {
                byte[] array = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    array[i] = this.buffer[pos + i];
                }
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(array, 0, 8);
                }
                return BitConverter.ToUInt64(array, 0);
            }
        }
        public class ArrayWriter
        {
            private byte[] buffer;
            private int size;

            public ArrayWriter(byte[] BytesArray)
            {
                this.buffer = BytesArray;
                this.size = this.buffer.Length;
            }

            public void SetBool(int pos, bool value)
            {
                byte[] buffer = new byte[] { value ? ((byte)1) : ((byte)0) };
                this.buffer[pos] = buffer[0];
            }

            public void SetByte(int pos, byte value)
            {
                this.buffer[pos] = value;
            }

            public void SetBytes(int pos, byte[] value)
            {
                int length = value.Length;
                for (int i = 0; i < length; i++)
                {
                    this.buffer[i + pos] = value[i];
                }
            }

            public void SetChar(int pos, char value)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value.ToString());
                this.buffer[pos] = bytes[0];
            }

            public void SetFloat(int pos, float value)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes, 0, 4);
                for (int i = 0; i < 4; i++)
                {
                    this.buffer[i + pos] = bytes[i];
                }
            }

            public void SetInt16(int pos, short value, EndianType Type = EndianType.BigEndian)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(bytes, 0, 2);
                }
                for (int i = 0; i < 2; i++)
                {
                    this.buffer[i + pos] = bytes[i];
                }
            }

            public void SetInt32(int pos, int value, EndianType Type = EndianType.BigEndian)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(bytes, 0, 4);
                }
                for (int i = 0; i < 4; i++)
                {
                    this.buffer[i + pos] = bytes[i];
                }
            }

            public void SetInt64(int pos, long value, EndianType Type = EndianType.BigEndian)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(bytes, 0, 8);
                }
                for (int i = 0; i < 8; i++)
                {
                    this.buffer[i + pos] = bytes[i];
                }
            }

            public void SetSByte(int pos, sbyte value)
            {
                this.buffer[pos] = (byte)value;
            }

            public void SetString(int pos, string value)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value + "\0");
                for (int i = 0; i < bytes.Length; i++)
                {
                    this.buffer[i + pos] = bytes[i];
                }
            }

            public void SetUInt16(int pos, ushort value, EndianType Type = EndianType.BigEndian)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(bytes, 0, 2);
                }
                for (int i = 0; i < 2; i++)
                {
                    this.buffer[i + pos] = bytes[i];
                }
            }

            public void SetUInt32(int pos, uint value, EndianType Type = EndianType.BigEndian)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(bytes, 0, 4);
                }
                for (int i = 0; i < 4; i++)
                {
                    this.buffer[i + pos] = bytes[i];
                }
            }

            public void SetUInt64(int pos, ulong value, EndianType Type = EndianType.BigEndian)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (Type == EndianType.BigEndian)
                {
                    Array.Reverse(bytes, 0, 8);
                }
                for (int i = 0; i < 8; i++)
                {
                    this.buffer[i + pos] = bytes[i];
                }
            }
        }
    }
    #region Memory
    public class Memory
    {
        //private static PS3Lib.NET.PS3TMAPI.SNRESULT snr;
        private Int32 target;
        private UInt32 process;
        /*
        public static void PS3Connector()
        {
            snr = PS3Lib.NET.PS3TMAPI.InitTargetComms();
            if (snr == PS3Lib.NET.PS3TMAPI.SNRESULT.SN_S_OK)
            {
                snr = PS3Lib.NET.PS3TMAPI.PickTarget(Handle, out target);
                if (snr == PS3Lib.NET.PS3TMAPI.SNRESULT.SN_S_OK)
                {
                    snr = PS3Lib.NET.PS3TMAPI.Connect(target, null);
                    if (snr == PS3Lib.NET.PS3TMAPI.SNRESULT.SN_S_OK || snr == PS3Lib.NET.PS3TMAPI.SNRESULT.SN_S_NO_ACTION)
                    {
                        PS3Lib.NET.PS3TMAPI.TCPIPConnectProperties tcpip = new PS3Lib.NET.PS3TMAPI.TCPIPConnectProperties();
                        PS3Lib.NET.PS3TMAPI.GetConnectionInfo(target, out tcpip);
                        ipaddr = tcpip.IPAddress;

                        uint[] processIDs;
                        snr = PS3Lib.NET.PS3TMAPI.GetProcessList(target, out processIDs);
                        processID = processIDs[0];
                        snr = PS3Lib.NET.PS3TMAPI.ProcessAttach(target, PS3Lib.NET.PS3TMAPI.UnitType.PPU, processID);
                        if (snr == PS3Lib.NET.PS3TMAPI.SNRESULT.SN_S_OK)
                        {
                            snr = PS3Lib.NET.PS3TMAPI.ProcessContinue(target, processID);
                            if (snr == PS3Lib.NET.PS3TMAPI.SNRESULT.SN_S_OK)
                            {
                                Text = Application.ProductName + "   |   " + ipaddr + "   |   0x" +
                                       processID.ToString("X8") + "   |   " + GetTitleID();
                                Enable();
                            }
                        }
                    }
                }
            }
        }
        */
        public Memory(Int32 _target, UInt32 _process)
        {
            target = _target;
            process = _process;
        }

        public String ReadString(UInt32 address)
        {
            char c;
            UInt32 count = 0;
            List<char> chars = new List<char>();
            while ((c = ReadChar(address + count)) != 0)
            {
                chars.Add(c);
                count++;
            }
            return new String(chars.ToArray());
        }

        public char ReadChar(UInt32 address)
        {
            return (char)ReadByte(address);
        }

        public char[] ReadChars(UInt32 address, Int32 length)
        {
            return Encoding.UTF8.GetChars(ReadBytes(address, length));
        }

        public bool ReadBoolean(UInt32 address)
        {
            return (ReadByte(address) != 0);
        }

        public Byte ReadByte(UInt32 address)
        {
            return ReadBytes(address, 1)[0];
        }

        public Byte[] ReadBytes(UInt32 address, Int32 length)
        {
            Byte[] buffer = new Byte[length];
            Lib.GetMemoryR(address, ref buffer);
            return buffer;
        }

        public Double ReadDouble(UInt32 address)
        {
            return BitConverter.ToDouble(ReadBytes(address, 8).Reverse().ToArray(), 0);
        }

        public Int16 ReadInt16(UInt32 address)
        {
            return BitConverter.ToInt16(ReadBytes(address, 2).Reverse().ToArray(), 0);
        }

        public Int32 ReadInt32(UInt32 address)
        {
            return BitConverter.ToInt32(ReadBytes(address, 4).Reverse().ToArray(), 0);
        }

        public Int64 ReadInt64(UInt32 address)
        {
            return BitConverter.ToInt64(ReadBytes(address, 8).Reverse().ToArray(), 0);
        }

        public sbyte ReadSByte(UInt32 address)
        {
            return (sbyte)(ReadByte(address));
        }

        public float ReadSingle(UInt32 address)
        {
            return BitConverter.ToSingle(ReadBytes(address, 4).Reverse().ToArray(), 0);
        }

        public UInt16 ReadUInt16(UInt32 address)
        {
            return BitConverter.ToUInt16(ReadBytes(address, 2).Reverse().ToArray(), 0);
        }

        public UInt32 ReadUInt32(UInt32 address)
        {
            return BitConverter.ToUInt32(ReadBytes(address, 4).Reverse().ToArray(), 0);
        }

        public UInt64 ReadUInt64(UInt32 address)
        {
            return BitConverter.ToUInt64(ReadBytes(address, 8).Reverse().ToArray(), 0);
        }

        public Vector3D ReadVector(UInt32 address)
        {
            Vector3D vec = new Vector3D();
            vec.X = BitConverter.ToDouble(ReadBytes(address, 4).Reverse().ToArray(), 0);
            vec.Y = BitConverter.ToDouble(ReadBytes(address + 4, 4).Reverse().ToArray(), 0);
            vec.Z = BitConverter.ToDouble(ReadBytes(address + 8, 4).Reverse().ToArray(), 0);
            return vec;
        }

        public Color ReadColor(UInt32 address)
        {
            Byte[] rgba = ReadBytes(address, 4);
            return Color.FromArgb(rgba[3], rgba[0], rgba[1], rgba[2]);
        }

        public void Write(UInt32 address, String value)
        {
            Write(address, Encoding.UTF8.GetBytes(value + "\x00"));
        }

        public void Write(UInt32 address, char value)
        {
            char[] buffer = { value };
            Write(address, Encoding.UTF8.GetBytes(buffer));
        }

        public void Write(UInt32 address, char[] value)
        {
            Write(address, Encoding.UTF8.GetBytes(value));
        }

        public void Write(UInt32 address, bool value)
        {
            Write(address, BitConverter.GetBytes(value));
        }

        public void Write(UInt32 address, Byte value)
        {
            Byte[] buffer = { value };
            Write(address, buffer);
        }

        public void Write(UInt32 address, Byte[] value)
        {
            Lib.SetMemory(address, value);
        }

        public void Write(UInt32 address, Double value)
        {
            Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public void Write(UInt32 address, Int16 value)
        {
            Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public void Write(UInt32 address, Int32 value)
        {
            Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public void Write(UInt32 address, Int64 value)
        {
            Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public void Write(UInt32 address, sbyte value)
        {
            Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public void Write(UInt32 address, float value)
        {
            Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public void Write(UInt32 address, UInt16 value)
        {
            Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public void Write(UInt32 address, UInt32 value)
        {
            Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public void Write(UInt32 address, UInt64 value)
        {
            Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public void Write(UInt32 address, Vector3D vec)
        {
            Write(address, BitConverter.GetBytes((float)vec.X).Reverse().ToArray());
            Write(address + 4, BitConverter.GetBytes((float)vec.Y).Reverse().ToArray());
            Write(address + 8, BitConverter.GetBytes((float)vec.Z).Reverse().ToArray());
        }

        public void Write(UInt32 address, Color color)
        {
            Byte[] rgba =
            {
                BitConverter.GetBytes(color.R)[0], BitConverter.GetBytes(color.G)[0], BitConverter.GetBytes(color.B)[0],
                BitConverter.GetBytes(color.A)[0]
            };
            Write(address, rgba);
        }

    #endregion
        public static class Conversions
        {
            public static Byte[] ReverseBytes(Byte[] input)
            {
                Array.Reverse(input);
                return input;
            }
            public static Byte[] RandomizeRGBA()
            {
                Byte[] RGBA = new Byte[4];
                Random randomize = new Random();
                RGBA[0] = BitConverter.GetBytes(randomize.Next(0, 255))[0];
                RGBA[1] = BitConverter.GetBytes(randomize.Next(0, 255))[0];
                RGBA[2] = BitConverter.GetBytes(randomize.Next(0, 255))[0];
                RGBA[3] = BitConverter.GetBytes(randomize.Next(0, 255))[0];
                return RGBA;
            }
        }

        public static Byte[] GetBytes(UInt32 address, Int32 length)
        {
            return Lib.ReadBytes(address, length);
        }
    }
}