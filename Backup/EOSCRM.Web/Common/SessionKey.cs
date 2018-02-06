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


        public const string GCS_BAOCAO_BANGKECHITIETCHUANTHU = "GCS_BAOCAO_BANGKECHITIETCHUANTHU";
        public const string GCS_BAOCAO_DSKHTIEUTHU = "GCS_BAOCAO_DSKHTIEUTHU";
        public const string GCS_BAOCAO_TONGHOPCHUANTHU = "GCS_BAOCAO_TONGHOPCHUANTHU";
        public const string GCS_BAOCAO_SANLUONGTHUCTE = "GCS_BAOCAO_SANLUONGTHUCTE";


        public const string DM_BAOCAO_DUONGPHO = "DM_BAOCAO_DUONGPHO";
        public const string DM_BAOCAO_VATTU = "DM_BAOCAO_VATTU";


        public const string TK_BAOCAO_CHOHOPDONG = "TK_BAOCAO_CHOHOPDONG";
        public const string TK_BAOCAO_CHOCHIETTINH = "TK_BAOCAO_CHOCHIETTINH";
        public const string TK_BAOCAO_CHOTHICONG = "TK_BAOCAO_CHOTHICONG";
        public const string TK_BAOCAO_CHOTHIETKE = "TK_BAOCAO_CHOTHIETKE";
        public const string TK_BAOCAO_CHUAHOPDONG = "TK_BAOCAO_CHUAHOPDONG";
        public const string TK_BAOCAO_CHUACHIETTINH = "TK_BAOCAO_CHUACHIETTINH";
        public const string TK_BAOCAO_CHUATHICONG = "TK_BAOCAO_CHUATHICONG";
        public const string TK_BAOCAO_CHUATHIETKE = "TK_BAOCAO_CHUATHIETKE";
        public const string TK_BAOCAO_DAHOPDONG = "TK_BAOCAO_DAHOPDONG";
        public const string TK_BAOCAO_DACHIETTINH = "TK_BAOCAO_DACHIETTINH";
        public const string TK_BAOCAO_DATHICONG = "TK_BAOCAO_DATHICONG";
        public const string TK_BAOCAO_DATHIETKE = "TK_BAOCAO_DATHIETKE";
        public const string TK_BAOCAO_DONDANGKY = "TK_BAOCAO_DONDANGKY";

        public const string TK_BAOCAO_LAPCHIETTINH = "TK_BAOCAO_LAPCHIETTINH";

        public const string TK_BAOCAO_BANGDENGHIXUATVATTU = "TK_BAOCAO_BANGDENGHIXUATVATTU";
        public const string TK_BAOCAO_BANGTONGHOPQUYETTOAN = "TK_BAOCAO_BANGTONGHOPQUYETTOAN";

        public const string TK_BAOCAO_LAPQUYETTOAN = "TK_BAOCAO_LAPQUYETTOAN";
        #endregion
    }
}