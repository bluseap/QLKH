namespace EOSCRM.Web.Common
{
    /// <summary>
    /// Summary description for SessionKey
    /// </summary>
    public class SessionKey
    {
        #region Common

        public const string SINGLETON = "singleton";
        public const string MESSAGE = "message";

        public const string SYSTEM_CONFIG = "system_config";
        
        public const string USER_ADMIN = "user_admin";
        public const string USER_LOGIN = "user_login";
        public const string GROUP = "group";

        public const string RESOURCE = "resource";
        public const string PROJECT = "project";
        public const string PROJECT_NAME = "ProjectNameCommon";
        public const string HOANCONG_ID = "HoanCongIDCommon";

        public const string MSG_WARNING = "msg_warning";
        public const string ASSIGN_RESOURCE = "assign_resource";

        public const string MODE = "edit_mode";
        public const string PRINTMODE_SOBOCONGTY = "printmode_sobocongty";
        public const string FILTEREDMODE = "fitlered_mode";


        public const string NHANVIEN_UPDATEMODE = "NHANVIEN_UPDATEMODE_SESSION";
        #endregion


        public enum PrintMode
        {
            SoBoSauThangDauNam = 0,
            SoBoSauThangCuoiNam = 1
        }
        
        #region Session key for resource

        public const string CN_BAOCAO_BANGKECHITIETCHUANTHU = "CN_BAOCAO_BANGKECHITIETCHUANTHU";
        public const string CN_BAOCAO_BANGKECHITIETTHUCTHU = "CN_BAOCAO_BANGKECHITIETTHUCTHU";
        public const string CN_BAOCAO_BANGKETHUTIENNUOC = "CN_BAOCAO_BANGKETHUTIENNUOC";
        public const string CN_BAOCAO_BANGKETONHOADONTHEODUONG = "CN_BAOCAO_BANGKETONHOADONTHEODUONG";
        public const string CN_BAOCAO_TINHHINHTHUCTHU = "CN_BAOCAO_TINHHINHTHUCTHU";
        public const string CN_BAOCAO_CHITIETHOADONTON = "CN_BAOCAO_CHITIETHOADONTON";
        public const string CN_BAOCAO_CHITIETTHUNOTHEOTHOIGIAN = "CN_BAOCAO_CHITIETTHUNOTHEOTHOIGIAN";
        public const string CN_BAOCAO_KHACHHANGTONHOADON = "CN_BAOCAO_KHACHHANGTONHOADON";
        public const string CN_BAOCAO_KHACHHANGTONHOADONNHIEUKY = "CN_BAOCAO_KHACHHANGTONHOADONNHIEUKY";
        public const string CN_BAOCAO_TINHHINHTHUTIENNHANVIEN = "CN_BAOCAO_TINHHINHTHUTIENNHANVIEN";
        public const string CN_BAOCAO_TONGHOPCONGNOTHEONHANVIEN = "CN_BAOCAO_TONGHOPCONGNOTHEONHANVIEN";

        public const string GCS_BAOCAO_THTRANGTHAITHEODUONG = "GCS_BAOCAO_THTRANGTHAITHEODUONG";
        public const string GCS_BAOCAO_BANGKECHITIETCHUANTHU = "GCS_BAOCAO_BANGKECHITIETCHUANTHU";
        public const string GCS_BAOCAO_BANGKECHITIETCHUANTHUKV = "GCS_BAOCAO_BANGKECHITIETCHUANTHUKV";
        public const string GCS_BAOCAO_DSKHTIEUTHU = "GCS_BAOCAO_DSKHTIEUTHU";
        public const string GCS_BAOCAO_TONGHOPCHUANTHU = "GCS_BAOCAO_TONGHOPCHUANTHU";
        public const string GCS_BAOCAO_TONGHOPCHUANTHUBIEUDO = "GCS_BAOCAO_TONGHOPCHUANTHUBIEUDO";
        public const string GCS_BAOCAO_SANLUONGTHUCTE = "GCS_BAOCAO_SANLUONGTHUCTE";


        public const string DM_BAOCAO_DUONGPHO = "DM_BAOCAO_DUONGPHO";
        public const string DM_BAOCAO_VATTU = "DM_BAOCAO_VATTU";

        public const string KH_COBIENNUOC = "KH_COBIENNUOC";
        public const string KH_THONGTINKH = "KH_THONGTINKH";
        public const string KH_THONGTINKHPO = "KH_THONGTINKHPO";

        public const string KH_BAOCAO_DSTDCTN = "KH_BAOCAO_DSTDCTN";
        public const string KH_BAOCAO_TONGHOPDK = "KH_BAOCAO_TONGHOPDK";

        public const string KH_BAOCAOPO_XOABOKHPO = "KH_BAOCAOPO_XOABOKHPO";
        public const string KH_BAOCAOPO_TONGHOPDKPO = "KH_BAOCAOPO_TONGHOPDKPO";

        public const string GCS_BAOCAO_KIEMDO3T = "GCS_BAOCAO_KIEMDO3T";

        public const string TK_BAOCAO_COBIEN = "TK_BAOCAO_COBIEN";
        public const string TK_BAOCAO_CHIETTINHKOKHAITHAC = "TK_BAOCAO_CHIETTINHKOKHAITHAC";
        public const string TK_BAOCAO_CHOHOPDONG = "TK_BAOCAO_CHOHOPDONG";
        public const string TK_BAOCAO_CHOCHIETTINH = "TK_BAOCAO_CHOCHIETTINH";
        public const string TK_BAOCAO_CHOTHICONG = "TK_BAOCAO_CHOTHICONG";
        public const string TK_BAOCAO_CHOTHIETKE = "TK_BAOCAO_CHOTHIETKE";
        public const string TK_BAOCAO_TUCHOITKPO = "TK_BAOCAO_TUCHOITKPO";
        public const string TK_BAOCAO_TUCHOITK = "TK_BAOCAO_TUCHOITK";
        public const string TK_BAOCAO_CHOTK = "TK_BAOCAO_CHOTK";
        public const string TK_BAOCAO_BVTCHUADUYETTKPO = "TK_BAOCAO_BVTCHUADUYETTKPO";
        public const string TK_BAOCAO_TED27BENPHAI = "TK_BAOCAO_TED27BENPHAI";

        public const string TK_BAOCAO_CHUAHOPDONG = "TK_BAOCAO_CHUAHOPDONG";
        public const string TK_BAOCAO_CHUAHOPDONGPO = "TK_BAOCAO_CHUAHOPDONGPO";
        public const string TK_BAOCAO_DAHOPDONGPO = "TK_BAOCAO_DAHOPDONGPO";
        public const string TK_BAOCAO_CHUACHIETTINH = "TK_BAOCAO_CHUACHIETTINH";
        public const string TK_BAOCAO_CHUATHICONG = "TK_BAOCAO_CHUATHICONG";
        public const string TK_BAOCAO_CHUATHIETKE = "TK_BAOCAO_CHUATHIETKE";
        public const string TK_BAOCAO_DAHOPDONG = "TK_BAOCAO_DAHOPDONG";
        public const string TK_BAOCAO_DACHIETTINH = "TK_BAOCAO_DACHIETTINH";
        public const string TK_BAOCAO_DATHICONG = "TK_BAOCAO_DATHICONG";
        public const string TK_BAOCAO_DATHIETKE = "TK_BAOCAO_DATHIETKE";
        public const string TK_BAOCAO_DONDANGKY = "TK_BAOCAO_DONDANGKY";
        public const string TK_BAOCAO_DONDANGKYNGAYN = "TK_BAOCAO_DONDANGKYNGAYN";
        public const string TK_BAOCAO_LAPCHIETTINH = "TK_BAOCAO_LAPCHIETTINH";
        public const string TK_BAOCAO_BANGDENGHIXUATVATTU = "TK_BAOCAO_BANGDENGHIXUATVATTU";
        public const string TK_BAOCAO_BANGTONGHOPQUYETTOAN = "TK_BAOCAO_BANGTONGHOPQUYETTOAN";
        public const string TK_BAOCAO_LAPQUYETTOAN = "TK_BAOCAO_LAPQUYETTOAN";
        public const string TK_BAOCAO_BOCVATTULX = "TK_BAOCAO_BOCVATTULX";

        public const string TC_BAOCAO_BBNTN = "TC_BAOCAO_BBNTN";
        public const string TC_BAOCAO_BBNTPO = "TC_BAOCAO_BBNTPO";
        public const string TC_BAOCAO_BBCHUANTN = "TC_BAOCAO_BBCHUANTN";
        public const string TC_BAOCAO_BBCHUANTPO = "TC_BAOCAO_BBCHUANTPO";
        public const string TC_BAOCAO_CHUANTCN = "TC_BAOCAO_CHUANTCN";
        public const string TC_BAOCAO_CHUANTCPO = "TC_BAOCAO_CHUANTPO";


        #endregion
    }
}