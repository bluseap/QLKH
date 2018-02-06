using System.Drawing;

namespace EOSCRM.Controls
{
    public abstract class Barcode
    {
        protected string altText;
        protected float barHeight;
        protected float baseline;
        protected bool checksumText;
        public const int CODABAR = 12;
        protected string code = "";
        public const int CODE128 = 9;
        public const int CODE128_RAW = 11;
        public const int CODE128_UCC = 10;
        protected int codeType;
        public const int EAN13 = 1;
        public const int EAN8 = 2;
        protected bool extended;
        protected bool generateChecksum;
        protected bool guardBars;
        protected float inkSpreading;
        protected float n;
        public const int PLANET = 8;
        public const int POSTNET = 7;
        protected float size;
        protected bool startStopText;
        public const int SUPP2 = 5;
        public const int SUPP5 = 6;
        protected int textAlignment;
        public const int UPCA = 3;
        public const int UPCE = 4;
        protected float x;

        protected Barcode()
        {
        }

        public abstract System.Drawing.Image CreateDrawingImage(Color foreground, Color background);
        
        
        public string AltText
        {
            get
            {
                return this.altText;
            }
            set
            {
                this.altText = value;
            }
        }

        public float BarHeight
        {
            get
            {
                return this.barHeight;
            }
            set
            {
                this.barHeight = value;
            }
        }

        public float Baseline
        {
            get
            {
                return this.baseline;
            }
            set
            {
                this.baseline = value;
            }
        }

        public bool ChecksumText
        {
            get
            {
                return this.checksumText;
            }
            set
            {
                this.checksumText = value;
            }
        }

        public virtual string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        public int CodeType
        {
            get
            {
                return this.codeType;
            }
            set
            {
                this.codeType = value;
            }
        }

        public bool Extended
        {
            get
            {
                return this.extended;
            }
            set
            {
                this.extended = value;
            }
        }

        public bool GenerateChecksum
        {
            get
            {
                return this.generateChecksum;
            }
            set
            {
                this.generateChecksum = value;
            }
        }

        public bool GuardBars
        {
            get
            {
                return this.guardBars;
            }
            set
            {
                this.guardBars = value;
            }
        }

        public float InkSpreading
        {
            get
            {
                return this.inkSpreading;
            }
            set
            {
                this.inkSpreading = value;
            }
        }

        public float N
        {
            get
            {
                return this.n;
            }
            set
            {
                this.n = value;
            }
        }

        public float Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        public bool StartStopText
        {
            get
            {
                return this.startStopText;
            }
            set
            {
                this.startStopText = value;
            }
        }

        public int TextAlignment
        {
            get
            {
                return this.textAlignment;
            }
            set
            {
                this.textAlignment = value;
            }
        }

        public float X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }
    }
}

