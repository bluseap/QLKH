using System;
using System.Drawing;
using System.Text;

namespace EOSCRM.Controls
{
    public class Barcode128 : Barcode
    {
        private static IntHashtable ais = new IntHashtable();
        private static readonly byte[][] BARS = new byte[][] { 
            new byte[] { 2, 1, 2, 2, 2, 2 }, new byte[] { 2, 2, 2, 1, 2, 2 }, new byte[] { 2, 2, 2, 2, 2, 1 }, new byte[] { 1, 2, 1, 2, 2, 3 }, new byte[] { 1, 2, 1, 3, 2, 2 }, new byte[] { 1, 3, 1, 2, 2, 2 }, new byte[] { 1, 2, 2, 2, 1, 3 }, new byte[] { 1, 2, 2, 3, 1, 2 }, new byte[] { 1, 3, 2, 2, 1, 2 }, new byte[] { 2, 2, 1, 2, 1, 3 }, new byte[] { 2, 2, 1, 3, 1, 2 }, new byte[] { 2, 3, 1, 2, 1, 2 }, new byte[] { 1, 1, 2, 2, 3, 2 }, new byte[] { 1, 2, 2, 1, 3, 2 }, new byte[] { 1, 2, 2, 2, 3, 1 }, new byte[] { 1, 1, 3, 2, 2, 2 }, 
            new byte[] { 1, 2, 3, 1, 2, 2 }, new byte[] { 1, 2, 3, 2, 2, 1 }, new byte[] { 2, 2, 3, 2, 1, 1 }, new byte[] { 2, 2, 1, 1, 3, 2 }, new byte[] { 2, 2, 1, 2, 3, 1 }, new byte[] { 2, 1, 3, 2, 1, 2 }, new byte[] { 2, 2, 3, 1, 1, 2 }, new byte[] { 3, 1, 2, 1, 3, 1 }, new byte[] { 3, 1, 1, 2, 2, 2 }, new byte[] { 3, 2, 1, 1, 2, 2 }, new byte[] { 3, 2, 1, 2, 2, 1 }, new byte[] { 3, 1, 2, 2, 1, 2 }, new byte[] { 3, 2, 2, 1, 1, 2 }, new byte[] { 3, 2, 2, 2, 1, 1 }, new byte[] { 2, 1, 2, 1, 2, 3 }, new byte[] { 2, 1, 2, 3, 2, 1 }, 
            new byte[] { 2, 3, 2, 1, 2, 1 }, new byte[] { 1, 1, 1, 3, 2, 3 }, new byte[] { 1, 3, 1, 1, 2, 3 }, new byte[] { 1, 3, 1, 3, 2, 1 }, new byte[] { 1, 1, 2, 3, 1, 3 }, new byte[] { 1, 3, 2, 1, 1, 3 }, new byte[] { 1, 3, 2, 3, 1, 1 }, new byte[] { 2, 1, 1, 3, 1, 3 }, new byte[] { 2, 3, 1, 1, 1, 3 }, new byte[] { 2, 3, 1, 3, 1, 1 }, new byte[] { 1, 1, 2, 1, 3, 3 }, new byte[] { 1, 1, 2, 3, 3, 1 }, new byte[] { 1, 3, 2, 1, 3, 1 }, new byte[] { 1, 1, 3, 1, 2, 3 }, new byte[] { 1, 1, 3, 3, 2, 1 }, new byte[] { 1, 3, 3, 1, 2, 1 }, 
            new byte[] { 3, 1, 3, 1, 2, 1 }, new byte[] { 2, 1, 1, 3, 3, 1 }, new byte[] { 2, 3, 1, 1, 3, 1 }, new byte[] { 2, 1, 3, 1, 1, 3 }, new byte[] { 2, 1, 3, 3, 1, 1 }, new byte[] { 2, 1, 3, 1, 3, 1 }, new byte[] { 3, 1, 1, 1, 2, 3 }, new byte[] { 3, 1, 1, 3, 2, 1 }, new byte[] { 3, 3, 1, 1, 2, 1 }, new byte[] { 3, 1, 2, 1, 1, 3 }, new byte[] { 3, 1, 2, 3, 1, 1 }, new byte[] { 3, 3, 2, 1, 1, 1 }, new byte[] { 3, 1, 4, 1, 1, 1 }, new byte[] { 2, 2, 1, 4, 1, 1 }, new byte[] { 4, 3, 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 2, 2, 4 }, 
            new byte[] { 1, 1, 1, 4, 2, 2 }, new byte[] { 1, 2, 1, 1, 2, 4 }, new byte[] { 1, 2, 1, 4, 2, 1 }, new byte[] { 1, 4, 1, 1, 2, 2 }, new byte[] { 1, 4, 1, 2, 2, 1 }, new byte[] { 1, 1, 2, 2, 1, 4 }, new byte[] { 1, 1, 2, 4, 1, 2 }, new byte[] { 1, 2, 2, 1, 1, 4 }, new byte[] { 1, 2, 2, 4, 1, 1 }, new byte[] { 1, 4, 2, 1, 1, 2 }, new byte[] { 1, 4, 2, 2, 1, 1 }, new byte[] { 2, 4, 1, 2, 1, 1 }, new byte[] { 2, 2, 1, 1, 1, 4 }, new byte[] { 4, 1, 3, 1, 1, 1 }, new byte[] { 2, 4, 1, 1, 1, 2 }, new byte[] { 1, 3, 4, 1, 1, 1 }, 
            new byte[] { 1, 1, 1, 2, 4, 2 }, new byte[] { 1, 2, 1, 1, 4, 2 }, new byte[] { 1, 2, 1, 2, 4, 1 }, new byte[] { 1, 1, 4, 2, 1, 2 }, new byte[] { 1, 2, 4, 1, 1, 2 }, new byte[] { 1, 2, 4, 2, 1, 1 }, new byte[] { 4, 1, 1, 2, 1, 2 }, new byte[] { 4, 2, 1, 1, 1, 2 }, new byte[] { 4, 2, 1, 2, 1, 1 }, new byte[] { 2, 1, 2, 1, 4, 1 }, new byte[] { 2, 1, 4, 1, 2, 1 }, new byte[] { 4, 1, 2, 1, 2, 1 }, new byte[] { 1, 1, 1, 1, 4, 3 }, new byte[] { 1, 1, 1, 3, 4, 1 }, new byte[] { 1, 3, 1, 1, 4, 1 }, new byte[] { 1, 1, 4, 1, 1, 3 }, 
            new byte[] { 1, 1, 4, 3, 1, 1 }, new byte[] { 4, 1, 1, 1, 1, 3 }, new byte[] { 4, 1, 1, 3, 1, 1 }, new byte[] { 1, 1, 3, 1, 4, 1 }, new byte[] { 1, 1, 4, 1, 3, 1 }, new byte[] { 3, 1, 1, 1, 4, 1 }, new byte[] { 4, 1, 1, 1, 3, 1 }, new byte[] { 2, 1, 1, 4, 1, 2 }, new byte[] { 2, 1, 1, 2, 1, 4 }, new byte[] { 2, 1, 1, 2, 3, 2 }
         };
        private static readonly byte[] BARS_STOP = new byte[] { 2, 3, 3, 1, 1, 1, 2 };
        public const char CODE_A = '\x00c8';
        public const char CODE_AB_TO_C = 'c';
        public const char CODE_AC_TO_B = 'd';
        public const char CODE_BC_TO_A = 'e';
        public const char CODE_C = '\x00c7';
        public const char DEL = '\x00c3';
        public const char FNC1 = '\x00ca';
        public const char FNC1_INDEX = 'f';
        public const char FNC2 = '\x00c5';
        public const char FNC3 = '\x00c4';
        public const char FNC4 = '\x00c8';
        public const char SHIFT = '\x00c6';
        public const char START_A = 'g';
        public const char START_B = 'h';
        public const char START_C = 'i';
        public const char STARTA = '\x00cb';
        public const char STARTB = '\x00cc';
        public const char STARTC = '\x00cd';

        static Barcode128()
        {
            ais[0] = 20;
            ais[1] = 0x10;
            ais[2] = 0x10;
            ais[10] = -1;
            ais[11] = 9;
            ais[12] = 8;
            ais[13] = 8;
            ais[15] = 8;
            ais[0x11] = 8;
            ais[20] = 4;
            ais[0x15] = -1;
            ais[0x16] = -1;
            ais[0x17] = -1;
            ais[240] = -1;
            ais[0xf1] = -1;
            ais[250] = -1;
            ais[0xfb] = -1;
            ais[0xfc] = -1;
            ais[30] = -1;
            for (int i = 0xc1c; i < 0xe74; i++)
            {
                ais[i] = 10;
            }
            ais[0x25] = -1;
            for (int j = 0xf3c; j < 0xf64; j++)
            {
                ais[j] = -1;
            }
            ais[400] = -1;
            ais[0x191] = -1;
            ais[0x192] = 20;
            ais[0x193] = -1;
            for (int k = 410; k < 0x1a0; k++)
            {
                ais[k] = 0x10;
            }
            ais[420] = -1;
            ais[0x1a5] = -1;
            ais[0x1a6] = 6;
            ais[0x1a7] = -1;
            ais[0x1a8] = 6;
            ais[0x1a9] = 6;
            ais[0x1aa] = 6;
            ais[0x1b59] = 0x11;
            ais[0x1b5a] = -1;
            for (int m = 0x1b76; m < 0x1b80; m++)
            {
                ais[m] = -1;
            }
            ais[0x1f41] = 0x12;
            ais[0x1f42] = -1;
            ais[0x1f43] = -1;
            ais[0x1f44] = -1;
            ais[0x1f45] = 10;
            ais[0x1f46] = 0x16;
            ais[0x1f47] = -1;
            ais[0x1f48] = -1;
            ais[0x1f52] = 0x16;
            ais[0x1f54] = -1;
            ais[0x1fa4] = 10;
            ais[0x1fa5] = 14;
            ais[0x1fa6] = 6;
            for (int n = 90; n < 100; n++)
            {
                ais[n] = -1;
            }
        }

        public Barcode128()
        {
            base.x = 0.8f;
            base.size = 8f;
            base.baseline = base.size;
            base.barHeight = base.size * 3f;
            base.textAlignment = 1;
            base.codeType = 9;
        }

        public override Image CreateDrawingImage(Color foreground, Color background)
        {
            
            code = GetRawText(base.code, base.codeType == 10);
            int width = ((code.Length + 2) * 11) + 2;
            byte[] buffer = GetBarsCode128Raw(code);
            int barHeight = (int) base.barHeight;
            Bitmap bitmap = new Bitmap(width, barHeight);
            for (int i = 0; i < barHeight; i++)
            {
                bool flag = true;
                int num6 = 0;
                for (int j = 0; j < buffer.Length; j++)
                {
                    int num8 = buffer[j];
                    Color color = background;
                    if (flag)
                    {
                        color = foreground;
                    }
                    flag = !flag;
                    for (int k = 0; k < num8; k++)
                    {
                        bitmap.SetPixel(num6++, i, color);
                    }
                }
            }
            return bitmap;
        }

        public static byte[] GetBarsCode128Raw(string text)
        {
            int num;
            int num3 = text[0];
            for (num = 1; num < text.Length; num++)
            {
                num3 += num * text[num];
            }
            num3 = num3 % 0x67;
            text = text + ((char) num3);
            byte[] destinationArray = new byte[((text.Length + 1) * 6) + 7];
            num = 0;
            while (num < text.Length)
            {
                Array.Copy(BARS[text[num]], 0, destinationArray, num * 6, 6);
                num++;
            }
            Array.Copy(BARS_STOP, 0, destinationArray, num * 6, 7);
            return destinationArray;
        }

        public static string GetHumanReadableUCCEAN(string code)
        {
            StringBuilder builder = new StringBuilder();
            string str = '\x00ca'.ToString();
            try
            {
            Label_0015:
                while (code.StartsWith(str))
                {
                    code = code.Substring(1);
                }
                int length = 0;
                int num2 = 0;
                for (int i = 2; i < 5; i++)
                {
                    if (code.Length < i)
                    {
                        break;
                    }
                    length = ais[int.Parse(code.Substring(0, i))];
                    if (length != 0)
                    {
                        num2 = i;
                        break;
                    }
                }
                if (num2 != 0)
                {
                    builder.Append('(').Append(code.Substring(0, num2)).Append(')');
                    code = code.Substring(num2);
                    if (length > 0)
                    {
                        length -= num2;
                        if (code.Length <= length)
                        {
                            goto Label_00FF;
                        }
                        builder.Append(RemoveFNC1(code.Substring(0, length)));
                        code = code.Substring(length);
                        goto Label_0015;
                    }
                    int index = code.IndexOf('\x00ca');
                    if (index >= 0)
                    {
                        builder.Append(code.Substring(0, index));
                        code = code.Substring(index + 1);
                        goto Label_0015;
                    }
                }
            }
            catch
            {
            }
        Label_00FF:
            builder.Append(RemoveFNC1(code));
            return builder.ToString();
        }

        internal static string GetPackedRawDigits(string text, int textIndex, int numDigits)
        {
            StringBuilder builder = new StringBuilder();
            int num = textIndex;
            while (numDigits > 0)
            {
                if (text[textIndex] == '\x00ca')
                {
                    builder.Append('f');
                    textIndex++;
                }
                else
                {
                    numDigits -= 2;
                    int num2 = text[textIndex++] - '0';
                    int num3 = text[textIndex++] - '0';
                    builder.Append((char) ((num2 * 10) + num3));
                }
            }
            return (((char) (textIndex - num)) + builder.ToString());
        }

        public static string GetRawText(string text, bool ucc)
        {
            string str = "";
            int length = text.Length;
            if (length == 0)
            {
                str = str + 'h';
                if (ucc)
                {
                    str = str + 'f';
                }
                return str;
            }
            int num2 = 0;
            for (int i = 0; i < length; i++)
            {
                num2 = text[i];
                if ((num2 > 0x7f) && (num2 != 0xca))
                {
                    throw new ArgumentException("there.are.illegal.characters.for.barcode.128.in.1");
                }
            }
            num2 = text[0];
            char ch = 'h';
            int textIndex = 0;
            if (IsNextDigits(text, textIndex, 2))
            {
                ch = 'i';
                str = str + ch;
                if (ucc)
                {
                    str = str + 'f';
                }
                string str2 = GetPackedRawDigits(text, textIndex, 2);
                textIndex += str2[0];
                str = str + str2.Substring(1);
            }
            else if (num2 < 0x20)
            {
                ch = 'g';
                str = str + ch;
                if (ucc)
                {
                    str = str + 'f';
                }
                str = str + ((char) (num2 + 0x40));
                textIndex++;
            }
            else
            {
                str = str + ch;
                if (ucc)
                {
                    str = str + 'f';
                }
                if (num2 == 0xca)
                {
                    str = str + 'f';
                }
                else
                {
                    str = str + ((char) (num2 - 0x20));
                }
                textIndex++;
            }
            while (textIndex < length)
            {
                switch (ch)
                {
                    case 'g':
                    {
                        if (!IsNextDigits(text, textIndex, 4))
                        {
                            break;
                        }
                        ch = 'i';
                        str = str + 'c';
                        string str3 = GetPackedRawDigits(text, textIndex, 4);
                        textIndex += str3[0];
                        str = str + str3.Substring(1);
                        continue;
                    }
                    case 'h':
                    {
                        if (!IsNextDigits(text, textIndex, 4))
                        {
                            goto Label_02AB;
                        }
                        ch = 'i';
                        str = str + 'c';
                        string str4 = GetPackedRawDigits(text, textIndex, 4);
                        textIndex += str4[0];
                        str = str + str4.Substring(1);
                        continue;
                    }
                    case 'i':
                    {
                        if (!IsNextDigits(text, textIndex, 2))
                        {
                            goto Label_034B;
                        }
                        string str5 = GetPackedRawDigits(text, textIndex, 2);
                        textIndex += str5[0];
                        str = str + str5.Substring(1);
                        continue;
                    }
                    default:
                    {
                        continue;
                    }
                }
                num2 = text[textIndex++];
                if (num2 == 0xca)
                {
                    str = str + 'f';
                }
                else if (num2 > 0x5f)
                {
                    ch = 'h';
                    str = str + 'd' + ((char) (num2 - 0x20));
                }
                else if (num2 < 0x20)
                {
                    str = str + ((char) (num2 + 0x40));
                }
                else
                {
                    str = str + ((char) (num2 - 0x20));
                }
                continue;
            Label_02AB:
                num2 = text[textIndex++];
                if (num2 == 0xca)
                {
                    str = str + 'f';
                }
                else if (num2 < 0x20)
                {
                    ch = 'g';
                    str = str + 'e' + ((char) (num2 + 0x40));
                }
                else
                {
                    str = str + ((char) (num2 - 0x20));
                }
                continue;
            Label_034B:
                num2 = text[textIndex++];
                if (num2 == 0xca)
                {
                    str = str + 'f';
                }
                else
                {
                    if (num2 < 0x20)
                    {
                        ch = 'g';
                        str = str + 'e' + ((char) (num2 + 0x40));
                        continue;
                    }
                    ch = 'h';
                    str = str + 'd' + ((char) (num2 - 0x20));
                }
            }
            return str;
        }

        internal static bool IsNextDigits(string text, int textIndex, int numDigits)
        {
            int length = text.Length;
            while ((textIndex < length) && (numDigits > 0))
            {
                char ch;
                if (text[textIndex] == '\x00ca')
                {
                    textIndex++;
                    continue;
                }
                int num2 = Math.Min(2, numDigits);
                if ((textIndex + num2) <= length)
                {
                    goto Label_004C;
                }
                return false;
            Label_002E:
                ch = text[textIndex++];
                if ((ch < '0') || (ch > '9'))
                {
                    return false;
                }
                numDigits--;
            Label_004C:
                if (num2-- > 0)
                {
                    goto Label_002E;
                }
            }
            return (numDigits == 0);
        }

        public static string RemoveFNC1(string code)
        {
            int length = code.Length;
            StringBuilder builder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                char ch = code[i];
                if ((ch >= ' ') && (ch <= '~'))
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }

        public override string Code
        {
            set
            {
                string str = value;
                if ((base.CodeType == 10) && str.StartsWith("("))
                {
                    int startIndex = 0;
                    StringBuilder builder = new StringBuilder();
                    while (startIndex >= 0)
                    {
                        int index = str.IndexOf(')', startIndex);
                        if (index < 0)
                        {
                            throw new ArgumentException("badly.formed.ucc.string.1");
                        }
                        string s = str.Substring(startIndex + 1, index - (startIndex + 1));
                        if (s.Length < 2)
                        {
                            throw new ArgumentException("ai.too.short.1");
                        }
                        int num3 = int.Parse(s);
                        int num4 = ais[num3];
                        if (num4 == 0)
                        {
                            throw new ArgumentException("ai.not.found.1");
                        }
                        s = num3.ToString();
                        if (s.Length == 1)
                        {
                            s = "0" + s;
                        }
                        startIndex = str.IndexOf('(', index);
                        int num5 = (startIndex < 0) ? str.Length : startIndex;
                        builder.Append(s).Append(str.Substring(index + 1, num5 - (index + 1)));
                        if (num4 < 0)
                        {
                            if (startIndex >= 0)
                            {
                                builder.Append('\x00ca');
                            }
                        }
                        else if ((((num5 - index) - 1) + s.Length) != num4)
                        {
                            throw new ArgumentException("invalid.ai.length.1");
                        }
                    }
                    base.Code = builder.ToString();
                }
                else
                {
                    base.Code = str;
                }
            }
        }
    }
}

