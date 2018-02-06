
namespace EOSCRM.Util
{
    public class Constants
    {
        public const string REPORT_FREE_MEM = "REPORT_FREE_MEM";
        public const string PARAM_PAGE = "page";
        public const string PARAM_KEYWORD = "keyword";
        public const string PARAM_FROMDATE = "from";
        public const string PARAM_TODATE = "to";
        public const string PARAM_STATECODE = "state";
        public const string PARAM_AREACODE = "area";
        public const string PARAM_REPORTED = "reported";
        public const string PARAM_MADDK = "id";
        public const string PARAM_IDKH = "idkh";
        public const string PARAM_MADH = "madh";
        public const string PARAM_TENDP = "tendp";
        public const string PARAM_SODIENTHOAI = "sodienthoai";
        public const string PARAM_TENKH = "name";
        public const string PARAM_SONHA = "addr";
        public const string PARAM_DIENTHOAI = "tel";
        public const string PARAM_SOHD = "sohd";
        public const string PARAM_FILTERED = "filtered";

        public const string PARAM_XOABONUOC = "xoabonuoc";        

        public const string SESSION_TIEUTHUKHACHHANG = "SessionTieuThuKhachHang";
        
        public const string SETTINGS_CONNECTION = "eosconnection";
        public const string SETTINGS_CONNECTIONPOWACO = "powacoconnection";

        public const byte HasSub = 1;
        public const byte NoSub = 0;
        public const byte Active = 1;
        public const byte Inactive = 0;
        public const byte NotClose = 0;
        public const byte IsClose = 1;
        public const byte Deleted = 1;
        public const byte NotDelete = 0;
        public const string Yes = "Yes";
        public const string No = "No";
        public const byte Male = 0;
        public const byte Female = 1;

        // Category
        public const int ParentId = 0;
        public const string ParentStr = "Root";
        public const string PrefixStr = "--";
        public const string PrefixArrowStr = " ";


        // Sort Direction
        public const string Desc = "DESC";
        public const string Asc = "ASC";

        // project constants:
        public const string ColorRowError = "#FF6A6A";
        public const string ColorClose = "#E6E6FA";
        public const string ColorPotential = "#9ccef1";
        public const string BtnInsert = "Insert";
        public const string BtnUpdate = "Update";
        public const string BtnAssign = "Assign";
        // Log constant
        public const string LogActionInsert = "Insert";
        public const string LogActionUpdate = "Update";
        public const string LogActionDelete = "Delete";

        // Config Constant:
        public const string ConfigTypeNumber = "Number";
        public const string ConfigTypeString = "String";
        public const string ConfigTypeDatetime = "DateTime";

        // Date format in DB
        public const string DateformatDb = "yyyy-MM-dd";
        public const string DateformatView = "dd/MM/yyyy";
        public const string DateTimeformatView = "dd/MM/yyyy :hh:mmm";

        //Luu vet tac vu
        public const string ObjKhachHang = "ObjKhachHang";
        public const string ObjDonDangKy = "ObjDonDangKy";
        public const string ObjDonSuaChua = "ObjDonSuaChua";
        public const string AddNew = "AddNew";
        public const string Update = "Update";
        public const string DeleteObject = "DeleteObject";
        public const string CongTacDefault = "Lắp đặt hệ thống đồng hồ đo nước 15 ly, L=3m";
        public const string GhiChuThietKeDefault = "Khách hàng tự trám lại sân, nền, tường, vỉa hè sau khi thi công xong công trình";

        public const string CRM_DELIMITER = "";
        public const string CRM_NULL_STRING = "";
    }

    public class AttachType
    {
        public const int HOANCONG = 1;
        public const int HOPDONG = 2;
        public const int CHATLUONGNUOC = 3;
        public const int THIETBI = 4;
        public const int KETOAN = 5;
        public const int DAT = 6;
        public const int HINHANH = 7;
        public const int BANVE = 8;
        public const int VATTU = 9;
        public const int DULIEUKHAC = 10;
        public const int VANBAN = 11;
        public const int CANHAN = 12;
    }

    

    public class MAHS
    {
        /// <summary>
        /// Hệ số chi phí chung
        /// </summary>
        public const string HSCPC = "HSCPC";

        /// <summary>
        /// Hệ số phí khác
        /// </summary>
        public const string HSCPK = "HSCPK";

        /// <summary>
        /// Hệ số nhân công 1
        /// </summary>
        public const string HSNC1 = "HSNC1";

        /// <summary>
        /// Hệ số nhân công 2
        /// </summary>
        public const string HSNC2 = "HSNC2";
        
        /// <summary>
        /// Hệ số thuế
        /// </summary>
        public const string HSTHE = "HSTHE";
        
        /// <summary>
        /// Hệ số thu nhập chịu thuế tính trước
        /// </summary>
        public const string HSTHU = "HSTHU";

        /// <summary>
        /// Hệ số thiết kế 1
        /// </summary>
        public const string HSTK1 = "HSTK1";
        
        /// <summary>
        /// Hệ số thiết kế 2
        /// </summary>
        public const string HSTK2 = "HSTK2";
    }

    public class MAHSKH
    {
        /// <summary>
        /// Định mức nhân khẩu
        /// </summary>
        public const string DMNK = "DMNK";
    }

    public enum MAMDSD
    {
        /// <summary>
        /// Hộ dân
        /// </summary>
        HD = 1,
        /// <summary>
        /// Kinh doanh dịch vụ
        /// </summary>
        KD = 2,
        /// <summary>
        /// Cơ quan
        /// </summary>
        CQ = 3,
        /// <summary>
        /// Hộ nghèo
        /// </summary>
        HN = 4,
        /// <summary>
        /// Sinh hoạt các hộ dân
        /// </summary>
        SH =  5,        
        /// <summary>
        /// Cơ quan HCSN
        /// </summary>
        HC = 6,
        /// <summary>
        /// Sản xuất 100%
        /// </summary>
        XS = 7,
        /// <summary>
        /// Sinh hoạt và sản xuất
        /// </summary>
        CS = 8,
        /// <summary>
        /// Dịch vụ 100%
        /// </summary>
        VD = 9,
        /// <summary>
        /// Kinh doanh và sinh hoạt
        /// </summary>
        DS = 10
    }


    public class DELIMITER
    {
        /// <summary>
        /// Constant value: __crmdelimiter__
        /// </summary>
        public const string Delimiter = "__crmdelimiter__";

        /// <summary>
        /// Constant value: __crmnullstring__
        /// </summary>
        public const string NullString = "__crmnullstring__";

        /// <summary>
        /// Constant value: __crmpassed__
        /// </summary>
        public const string Passed = "__crmpassed__";

        /// <summary>
        /// Constant value: __crmfailed__
        /// </summary>
        public const string Failed = "__crmfailed__";
    }

    public enum Mode
    {
        Create = 0,
        Update = 1
    }

    public enum ReportMode
    {
        Normal = 0,
        Day = 1,
        Month = 2
    }

    public enum FilteredMode
    {
        None = 0,
        Filtered = 1
    }

    public enum LOAIDK
    {
        DK = 0,
        CTCT = 1
    }

    public enum TTDK
    {
        DK_A = 1
    }

    public enum TTTK
    {
        TK_N = 1,
        TK_P = 2,
        TK_A = 3,
        TK_RA = 4
    }

    public enum TTCT
    {
        CT_N = 1,
        CT_P = 2,
        CT_A = 3,
        CT_RA = 4
    }

    public enum TTTC
    {
        TC_N = 1,
        TC_P = 2,
        TC_A = 3,
        TC_RA = 4
    }

    public enum TTHD
    {
        HD_N = 1,
        HD_P = 2,
        HD_A = 3,
        HD_RA = 4
    }

    public enum TT
    {
        CHOKHAOSAT = 1,
        CHODUYETTHIETKE = 2,
        CHODUYETCHIETTINH = 3,
        TRACUUTHIETKE = 4,
        TRACUUCHIETTINH = 5,
        XULYDONCHOHOPDONG = 6
    }

    public enum CT
    {
        CT = 1,
        SC = 2
    }

    public enum TTSC
    {
        SC_P = 1,
        SC_N= 2,
        SC_I= 3,
        SC_F =4

   }

    public enum TTQT
    {
        QT_N = 1,
        QT_P = 2,
        QT_A = 3,
        QT_RA = 4
    }

    public enum CHUCNANGKYDUYET
    {
        /// <summary>
        /// Thiết kế
        /// </summary>
        CT01 = 1,
        /// <summary>
        /// Chiết tính
        /// </summary>
        CT05 = 5,
        /// <summary>
        /// Hợp đồng
        /// </summary>
        HD00 = 10,
        /// <summary>
        /// Lắp mới
        /// </summary>
        KH01 = 21,
        /// <summary>
        /// Khách hàng
        /// </summary>
        KH05 = 25,
        /// <summary>
        /// Hóa đơn
        /// </summary>
        KH31 = 31,
        /// <summary>
        /// Thi công
        /// </summary>
        TC00 = 60,
        /// <summary>
        /// Vật tư
        /// </summary>
        VT00 =70
    }

    public enum TACVUKYDUYET
    {
        /// <summary>
        /// Xử lý
        /// </summary>
        A = 1,
        /// <summary>
        /// Nhập
        /// </summary>
        I =2,
        /// <summary>
        /// Sửa
        /// </summary>
        U =3,
        /// <summary>
        /// Xóa
        /// </summary>
        D=6
    } 
    
    public enum MACV
    {
        /// <summary>
        /// NV-GHI THU
        /// </summary>
        GT = 1
    }

}



