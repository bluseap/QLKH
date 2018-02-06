using System;
using System.Drawing;
using System.Text;

namespace EOSCRM.Controls
{
    public class Barcode39 : Barcode
    {
        private static readonly byte[][] BARS = new byte[][] { 
            new byte[] { 0, 0, 0, 1, 1, 0, 1, 0, 0 }, new byte[] { 1, 0, 0, 1, 0, 0, 0, 0, 1 }, new byte[] { 0, 0, 1, 1, 0, 0, 0, 0, 1 }, new byte[] { 1, 0, 1, 1, 0, 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 1, 1, 0, 0, 0, 1 }, new byte[] { 1, 0, 0, 1, 1, 0, 0, 0, 0 }, new byte[] { 0, 0, 1, 1, 1, 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 1, 0, 0, 1, 0, 1 }, new byte[] { 1, 0, 0, 1, 0, 0, 1, 0, 0 }, new byte[] { 0, 0, 1, 1, 0, 0, 1, 0, 0 }, new byte[] { 1, 0, 0, 0, 0, 1, 0, 0, 1 }, new byte[] { 0, 0, 1, 0, 0, 1, 0, 0, 1 }, new byte[] { 1, 0, 1, 0, 0, 1, 0, 0, 0 }, new byte[] { 0, 0, 0, 0, 1, 1, 0, 0, 1 }, new byte[] { 1, 0, 0, 0, 1, 1, 0, 0, 0 }, new byte[] { 0, 0, 1, 0, 1, 1, 0, 0, 0 }, 
            new byte[] { 0, 0, 0, 0, 0, 1, 1, 0, 1 }, new byte[] { 1, 0, 0, 0, 0, 1, 1, 0, 0 }, new byte[] { 0, 0, 1, 0, 0, 1, 1, 0, 0 }, new byte[] { 0, 0, 0, 0, 1, 1, 1, 0, 0 }, new byte[] { 1, 0, 0, 0, 0, 0, 0, 1, 1 }, new byte[] { 0, 0, 1, 0, 0, 0, 0, 1, 1 }, new byte[] { 1, 0, 1, 0, 0, 0, 0, 1, 0 }, new byte[] { 0, 0, 0, 0, 1, 0, 0, 1, 1 }, new byte[] { 1, 0, 0, 0, 1, 0, 0, 1, 0 }, new byte[] { 0, 0, 1, 0, 1, 0, 0, 1, 0 }, new byte[] { 0, 0, 0, 0, 0, 0, 1, 1, 1 }, new byte[] { 1, 0, 0, 0, 0, 0, 1, 1, 0 }, new byte[] { 0, 0, 1, 0, 0, 0, 1, 1, 0 }, new byte[] { 0, 0, 0, 0, 1, 0, 1, 1, 0 }, new byte[] { 1, 1, 0, 0, 0, 0, 0, 0, 1 }, new byte[] { 0, 1, 1, 0, 0, 0, 0, 0, 1 }, 
            new byte[] { 1, 1, 1, 0, 0, 0, 0, 0, 0 }, new byte[] { 0, 1, 0, 0, 1, 0, 0, 0, 1 }, new byte[] { 1, 1, 0, 0, 1, 0, 0, 0, 0 }, new byte[] { 0, 1, 1, 0, 1, 0, 0, 0, 0 }, new byte[] { 0, 1, 0, 0, 0, 0, 1, 0, 1 }, new byte[] { 1, 1, 0, 0, 0, 0, 1, 0, 0 }, new byte[] { 0, 1, 1, 0, 0, 0, 1, 0, 0 }, new byte[] { 0, 1, 0, 1, 0, 1, 0, 0, 0 }, new byte[] { 0, 1, 0, 1, 0, 0, 0, 1, 0 }, new byte[] { 0, 1, 0, 0, 0, 1, 0, 1, 0 }, new byte[] { 0, 0, 0, 1, 0, 1, 0, 1, 0 }, new byte[] { 0, 1, 0, 0, 1, 0, 1, 0, 0 }
         };
        private const string CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";
        private const string EXTENDED = "%U$A$B$C$D$E$F$G$H$I$J$K$L$M$N$O$P$Q$R$S$T$U$V$W$X$Y$Z%A%B%C%D%E  /A/B/C/D/E/F/G/H/I/J/K/L - ./O 0 1 2 3 4 5 6 7 8 9/Z%F%G%H%I%J%V A B C D E F G H I J K L M N O P Q R S T U V W X Y Z%K%L%M%N%O%W+A+B+C+D+E+F+G+H+I+J+K+L+M+N+O+P+Q+R+S+T+U+V+W+X+Y+Z%P%Q%R%S%T";

        public Barcode39()
        {
            base.x = 0.8f;
            base.n = 2f;
            //base.font = BaseFont.CreateFont("Helvetica", "winansi", false);
            base.size = 8f;
            base.baseline = base.size;
            base.barHeight = base.size * 3f;
            base.textAlignment = 1;
            base.generateChecksum = false;
            base.checksumText = false;
            base.startStopText = true;
            base.extended = false;
        }

        public override System.Drawing.Image CreateDrawingImage(Color foreground, Color background)
        {
            string code = base.code;
            if (base.extended)
            {
                code = GetCode39Ex(base.code);
            }
            if (base.generateChecksum)
            {
                code = code + GetChecksum(code);
            }
            int num = code.Length + 2;
            int n = (int) base.n;
            int width = (num * (6 + (3 * n))) + (num - 1);
            int barHeight = (int) base.barHeight;
            Bitmap bitmap = new Bitmap(width, barHeight);
            byte[] buffer = GetBarsCode39(code);
            for (int i = 0; i < barHeight; i++)
            {
                bool flag = true;
                int num6 = 0;
                for (int j = 0; j < buffer.Length; j++)
                {
                    int num8 = (buffer[j] == 0) ? 1 : n;
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

        public static byte[] GetBarsCode39(string text)
        {
            text = "*" + text + "*";
            byte[] destinationArray = new byte[(text.Length * 10) - 1];
            for (int i = 0; i < text.Length; i++)
            {
                int index = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*".IndexOf(text[i]);
                if (index < 0)
                {
                    throw new ArgumentException("the.character.1.is.illegal.in.code.39");
                }
                Array.Copy(BARS[index], 0, destinationArray, i * 10, 9);
            }
            return destinationArray;
        }

        internal static char GetChecksum(string text)
        {
            int num = 0;
            for (int i = 0; i < text.Length; i++)
            {
                int index = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*".IndexOf(text[i]);
                if (index < 0)
                {
                    throw new ArgumentException("the.character.1.is.illegal.in.code.39");
                }
                num += index;
            }
            return "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*"[num % 0x2b];
        }

        public static string GetCode39Ex(string text)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (ch > '\x007f')
                {
                    throw new ArgumentException("the.character.1.is.illegal.in.code.39.extended");
                }
                char ch2 = "%U$A$B$C$D$E$F$G$H$I$J$K$L$M$N$O$P$Q$R$S$T$U$V$W$X$Y$Z%A%B%C%D%E  /A/B/C/D/E/F/G/H/I/J/K/L - ./O 0 1 2 3 4 5 6 7 8 9/Z%F%G%H%I%J%V A B C D E F G H I J K L M N O P Q R S T U V W X Y Z%K%L%M%N%O%W+A+B+C+D+E+F+G+H+I+J+K+L+M+N+O+P+Q+R+S+T+U+V+W+X+Y+Z%P%Q%R%S%T"[ch * '\x0002'];
                char ch3 = "%U$A$B$C$D$E$F$G$H$I$J$K$L$M$N$O$P$Q$R$S$T$U$V$W$X$Y$Z%A%B%C%D%E  /A/B/C/D/E/F/G/H/I/J/K/L - ./O 0 1 2 3 4 5 6 7 8 9/Z%F%G%H%I%J%V A B C D E F G H I J K L M N O P Q R S T U V W X Y Z%K%L%M%N%O%W+A+B+C+D+E+F+G+H+I+J+K+L+M+N+O+P+Q+R+S+T+U+V+W+X+Y+Z%P%Q%R%S%T"[(ch * '\x0002') + 1];
                if (ch2 != ' ')
                {
                    builder.Append(ch2);
                }
                builder.Append(ch3);
            }
            return builder.ToString();
        }
    }
}

