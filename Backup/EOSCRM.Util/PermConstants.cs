#region Imports

using System.Collections.Generic;
using System.Web.UI.WebControls;

#endregion

namespace EOSCRM.Util
{
    /// <summary>
    /// Enum Permission
    /// </summary>
    public enum Permission
    {
        Read = 1,
        Insert = 2,
        Update = 3,
        Delete = 4
    }

    /// <summary>
    /// Enum Functions
    /// </summary>
    public enum Functions
    {
        SYS_Group = 1,
        SYS_UserAdmin = 2,

        DM_PhongBan = 300,
        DM_DuongPho = 301,
        DM_Phuong = 302,
        DM_KhuVuc = 303,
        DM_NhanVien = 304,
        DM_VatTu = 305,
        DM_DongHo = 306,
        DM_CoQuanTt = 307,
        DM_NganHang = 308,
        DM_LoaiDongHo = 309,
        DM_LoaiKhachHangDb = 310,
        DM_LoaiPhanHoi = 311,
        DM_NhomPhanHoi = 312,
        DM_ThongTinPhanHoi = 313,
        DM_ThongTinXuLy = 314,
        DM_NhomVatTu = 315,
        DM_DonViTinh = 316,
        DM_CongViec = 317,
        DM_CapBac = 318,
        DM_TrinhDo = 319,
        DM_HeSo = 320,
        DM_HeSoKH = 321,
        DM_Doan = 322,

        DM_BaoCao = 350,
        DM_BaoCao_DuongPho = 351,
        DM_BaoCao_VatTu = 352,

        KH_NhapMoi = 400,
        KH_DonLapDatMoi = 401,
        KH_TraCuuDonLapMoi = 402,
        KH_TraCuuKhachHang = 403,
        //KH_DonCaiTaoCongTy = 404,
        KH_CupMoNuoc = 405,
        //KH_PhatSinhTang = 406,
        KH_ThayDongHo = 407,
        KH_CapNhatSoBo = 408,
        KH_CapNhatDoan = 409,
        //KH_ThayDanhSo = 410,
        KH_KhachHangTam = 410,
        KH_InHopDongKH = 411,

        KH_BaoCao = 420,
        KH_BaoCao_DonLapMoi = 421,
        KH_BaoCao_QLKH = 422,
        KH_BaoCao_DonLapMoi_DS_DonDangKy = 423,
        KH_BaoCao_DonLapMoi_DS_ChuaThietKe = 424,
        KH_BaoCao_DonLapMoi_DS_ChoThietKe = 425,
        KH_BaoCao_DonLapMoi_DS_DaThietKe = 426,
        KH_BaoCao_DonLapMoi_DS_ChuaLapCT = 427,
        KH_BaoCao_DonLapMoi_DS_ChoLapCT = 428,
        KH_BaoCao_DonLapMoi_DS_DaLapCT = 429,
        KH_BaoCao_DonLapMoi_DS_ChuaHopDong = 430,
        KH_BaoCao_DonLapMoi_DS_ChoHopDong = 431,
        KH_BaoCao_DonLapMoi_DS_DaHopDong = 432,
        KH_BaoCao_DonLapMoi_DS_ChuaThiCong = 433,
        KH_BaoCao_DonLapMoi_DS_ChoThiCong = 434,
        KH_BaoCao_DonLapMoi_DS_DaThiCong = 435,
        KH_BaoCao_QLKH_DS_KhachHang = 470,
        KH_BaoCao_QLKH_DS_KhachHangMoi = 471,
        KH_BaoCao_QLKH_DS_SoBoCongTy = 472,
        KH_BaoCao_QLKH_DS_ThayDongHo = 473,
        KH_BaoCao_QLKH_DS_ThongTinThayDhTuKhachHang = 474,
        KH_BaoCao_QLKH_DS_ThongTinThayDhChuaCo = 475,
        KH_BaoCao_QLKH_TinhHinhTieuThuKhachHang = 476,
        KH_BaoCao_QLKH_LichSuSuDungNuoc = 477,
        KH_BaoCao_QLKH_ThayDoiChiTietNuoc = 478,

        SC_NhapThongTinSuaChua = 500,
        SC_PhanCongSuaChua = 501,
        SC_GiaiQuyetDonSuaChua = 502,
        SC_CapNhatChiTietSauSuaChua = 503,
        SC_QuanLyThongTinDonSuaChua = 504,
        SC_LapChietTinhSuaChua = 506,
        SC_DuyetChietTinhSuaChua = 507,
        SC_TraCuuChietTinhSuaChua = 510,

        SC_BaoCao = 530,
        SC_BaoCao_DanhSachDon = 531,
        SC_BaoCao_ChuaLapCT = 532,
        SC_BaoCao_ChoLapCT = 533,
        SC_BaoCao_DaLapCT = 534,
        SC_BaoCao_DanhSachDonHoanThanhPhanLoai = 535,

        TK_PhanCongKhaoSatThietKe = 600,
        TK_KhaoSatThietKe = 601,
        TK_QuanLyMauBocVatTu = 602,
        TK_ThietKeVaVatTu = 603,
        TK_DuyetThietKe = 604,
        TK_TraCuuThietKe = 605,
        TK_LapChietTinh = 606,
        TK_DuyetChietTinh = 607,
        TK_TraCuuChietTinh = 608,
        TK_XuLyDonChoHopDong = 609,
        TK_NhapHopDong = 610,
        TK_TraCuuHopDong = 611,
        TK_BaoCao_BangDeNghiXuatVatTu = 614,
        TK_BaoCao_BangQuyetToanTongHop = 615,

        TK_BaoCao = 650,
        
        TC_XuLyDonChoThiCong = 700,
        TC_NhapDonThiCong = 701,
        TC_CapNhatChiTietSauThiCong = 702,
        TC_TraCuuThiCong = 703,
        TC_LapQuyetToan = 704,
        TC_DuyetQuyetToan = 705,
        TC_TraCuuQuyetToan = 706,
        TC_BienBanNghiemThu = 707,

        GCS_KhoiTaoGhiChiSo = 800,
        GCS_GhiChiSo = 801,
        GCS_KhoaGhiChiSo = 802,
        GCS_InHoaDon = 803,
        GCS_SuaChiSo = 804,
        GCS_InHoaDonLe = 805,
        GCS_PhatHoaDon = 806,
        GCS_KhoiTaoGhiChiSoLe = 807,
        GCS_KhoiTaoGhiChiSoBoSung = 808,
        GCS_DieuChinhHoaDon = 809,
        GCS_InHoaDonN = 810,

        GCS_BaoCao = 850,
        GCS_BaoCao_DSKTKY = 851,
        GCS_BaoCao_DSTieuThuDK = 852,
        GCS_BaoCao_DSChuanThuTheoDuong = 853,
        GCS_BaoCao_TongHopChuanThu = 854,
        GCS_BaoCao_SanLuongThucTe = 855,
        GCS_BaoCao_DSKDO = 856,
        GCS_BaoCao_DSDCHD = 857,
        GCS_BaoCao_INSOGHINUOC = 858,


        CN_BaoCao = 950,
        CN_NhapCongNo = 901,
        CN_ThuTaiQuay = 902,
        CN_PhanCongGhiThu = 903,
        CN_TraCuuCongNo = 904,
        CN_KiemTraHoaDonTon = 905,
        CN_NhapCongNoNguoc = 906,

        CN_BaoCao_ChiTietHoaDonTon = 951,
        CN_BaoCao_TongHopCongNoTheoNhanVien = 952,
        CN_BaoCao_BangKeChiTietThucThu = 953,
        CN_BaoCao_ChiTietThuNoTheoThoiGian = 954,
        CN_BaoCao_KhachHangTonHoaDon = 955,
        //CN_BaoCao_MoiMoCupSoVoiThangTruoc = 956,
        //CN_BaoCao_DanhSachKhachHangGhiSot = 957,
        CN_BaoCao_TinhHinhThucThu = 958,
        CN_BaoCao_BangChiTietChuanThu = 959,
        CN_BaoCao_TinhHinhThuTienNhanVien = 960,
        CN_BaoCao_BangKeThuTienNuoc = 961,
        CN_BaoCao_BangKeTonHoaDonTheoDuong = 962,
        CN_BaoCao_KhachHangTonHoaDonNhieuKy = 963,

        TRUYTHU_GhiSaiChiSo = 1001,
        TRUYTHU_DoViPham = 1002,     
        TRUYTHU_BaoCao = 1000,

        TRAMBIENAP_NhapTBA=1052,
        TRAMBIENAP_GhiSoHoTBA = 1053,
        TRAMBIENAP_GhiChiSoTBA = 1054,
        TRAMBIENAP_CapNhatChiSoTBA = 1055,
        TRAMBIENAP_KhoiTaoChiSoTBA = 1056,
        TRAMBIENAP_BaoCao = 1051,

        NGHIPHEPCONGTAC_BaoCao = 1100,
        NGHIPHEPCONGTAC_DonNghiPhep = 1101,

        VATTU_BaoCao = 1150,
        VATTU_NhapVatTuNuoc = 1151,
        VATTU_TongHopVatTuNuoc = 1152
        
    }

    /// <summary>
    /// Permission Constants
    /// </summary>
    public class PermConstants
    {
        /// <summary>
        /// Get Permission List
        /// </summary>
        public static List<ListItem> ListPerms
        {
            get
            {
                return new List<ListItem>
                           {
                               #region Hệ thống
                               new ListItem("Hệ thống / Nhóm người dùng", Functions.SYS_Group.GetHashCode().ToString()),
                               new ListItem("Hệ thống / Người dùng", Functions.SYS_UserAdmin.GetHashCode().ToString()),
                                #endregion

                               #region Danh mục
                               new ListItem("Danh mục / Phòng ban", Functions.DM_PhongBan.GetHashCode().ToString()),
                               new ListItem("Danh mục / Đường phố", Functions.DM_DuongPho.GetHashCode().ToString()),
                               new ListItem("Danh mục / Phường", Functions.DM_Phuong.GetHashCode().ToString()),
                               new ListItem("Danh mục / Khu vực", Functions.DM_KhuVuc.GetHashCode().ToString()),
                               new ListItem("Danh mục / Nhân viên", Functions.DM_NhanVien.GetHashCode().ToString()),
                               new ListItem("Danh mục / Vật tư", Functions.DM_VatTu.GetHashCode().ToString()),
                               new ListItem("Danh mục / Đồng hồ", Functions.DM_DongHo.GetHashCode().ToString()),
                               new ListItem("Danh mục / Đoạn", Functions.DM_Doan.GetHashCode().ToString()),
                               new ListItem("Danh mục / Cơ quan thanh toán",
                                            Functions.DM_CoQuanTt.GetHashCode().ToString()),
                               new ListItem("Danh mục / Ngân hàng", Functions.DM_NganHang.GetHashCode().ToString()),
                               new ListItem("Danh mục / Loại đồng hồ", Functions.DM_LoaiDongHo.GetHashCode().ToString()),
                               new ListItem("Danh mục / Loại khách hàng đặc biệt",
                                            Functions.DM_LoaiKhachHangDb.GetHashCode().ToString()),
                               new ListItem("Danh mục / Loại phản hồi",
                                            Functions.DM_LoaiPhanHoi.GetHashCode().ToString()),
                               new ListItem("Danh mục / Nhóm phản hồi",
                                            Functions.DM_NhomPhanHoi.GetHashCode().ToString()),
                               new ListItem("Danh mục / Thông tin phản hồi",
                                            Functions.DM_ThongTinPhanHoi.GetHashCode().ToString()),
                               new ListItem("Danh mục / Thông tin xử lý",
                                            Functions.DM_ThongTinXuLy.GetHashCode().ToString()),
                               new ListItem("Danh mục / Nhóm vật tư", Functions.DM_NhomVatTu.GetHashCode().ToString()),
                               new ListItem("Danh mục / Đơn vị tính", Functions.DM_DonViTinh.GetHashCode().ToString()),
                               new ListItem("Danh mục / Công việc", Functions.DM_CongViec.GetHashCode().ToString()),
                               new ListItem("Danh mục / Cấp bậc", Functions.DM_CapBac.GetHashCode().ToString()),
                               new ListItem("Danh mục / Trình độ", Functions.DM_TrinhDo.GetHashCode().ToString()),
                               new ListItem("Danh mục / Hệ số thiết kế", Functions.DM_HeSo.GetHashCode().ToString()),
                               new ListItem("Danh mục / Hệ số khách hàng", Functions.DM_HeSoKH.GetHashCode().ToString()),
                               new ListItem("Danh mục / Báo cáo", Functions.DM_BaoCao.GetHashCode().ToString()),
                               new ListItem("Danh mục / Báo cáo / Danh sách đường phố",
                                            Functions.DM_BaoCao_DuongPho.GetHashCode().ToString()),
                               new ListItem("Danh mục / Báo cáo / Danh sách vật tư",
                                            Functions.DM_BaoCao_VatTu.GetHashCode().ToString()),
                               #endregion

                               #region Khách hàng
                               new ListItem("Khách hàng / Báo cáo", Functions.KH_BaoCao.GetHashCode().ToString()),

                               new ListItem("Khách hàng / Nhập khách hàng",
                                            Functions.KH_NhapMoi.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Tra cứu khách hàng",
                                            Functions.KH_TraCuuKhachHang.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Nhập đơn lắp đặt mới",
                                            Functions.KH_DonLapDatMoi.GetHashCode().ToString()),
                               //new ListItem("Khách hàng / Nhập đơn cải tạo công ty",
                               //             Functions.KH_DonCaiTaoCongTy.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Tra cứu trạng thái đơn lắp đặt mới",
                                            Functions.KH_TraCuuDonLapMoi.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Cúp mở nước", Functions.KH_CupMoNuoc.GetHashCode().ToString()),
                               //new ListItem("Khách hàng / Phát sinh tăng", Functions.KH_PhatSinhTang.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Thay đồng hồ", Functions.KH_ThayDongHo.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Cập nhật sổ bộ", Functions.KH_CapNhatSoBo.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Cập nhật đoạn", Functions.KH_CapNhatDoan.GetHashCode().ToString()),
                               //new ListItem("Khách hàng / Thay danh số", Functions.KH_ThayDanhSo.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Nhập Khách hàng tạm", Functions.KH_KhachHangTam.GetHashCode().ToString()),
                               new ListItem("Khách hàng / In hợp đồng khách hàng", Functions.KH_InHopDongKH.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / Đơn lắp đặt mới",
                                            Functions.KH_BaoCao_DonLapMoi.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / Quản lý khách hàng",
                                            Functions.KH_BaoCao_QLKH.GetHashCode().ToString()),

                               new ListItem("Khách hàng / Báo cáo / QLKH / Báo cáo danh sách khách hàng mới",
                                            Functions.KH_BaoCao_QLKH_DS_KhachHangMoi.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / QLKH / Danh sách khách hàng",
                                            Functions.KH_BaoCao_QLKH_DS_KhachHang.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / QLKH / Sổ bộ công ty",
                                            Functions.KH_BaoCao_QLKH_DS_SoBoCongTy.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / QLKH / Khách hàng thay đồng hồ",
                                            Functions.KH_BaoCao_QLKH_DS_ThayDongHo.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / QLKH / Thông tin thay đồng hồ của khách hàng",
                                            Functions.KH_BaoCao_QLKH_DS_ThongTinThayDhTuKhachHang.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / QLKH / Khách hàng chưa có thông tin thay đồng hồ",
                                            Functions.KH_BaoCao_QLKH_DS_ThongTinThayDhChuaCo.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / QLKH / Tình hình tiêu thụ khách hàng",
                                            Functions.KH_BaoCao_QLKH_TinhHinhTieuThuKhachHang.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / QLKH / Lịch sử sử dụng nước",
                                            Functions.KH_BaoCao_QLKH_LichSuSuDungNuoc.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / QLKH / Thay đổi chi tiết nước",
                                            Functions.KH_BaoCao_QLKH_ThayDoiChiTietNuoc.GetHashCode().ToString()),
                                                         
                           
                               #endregion

                               #region Thiết kế
                               //new ListItem("Thiết kế / Phân công khảo sát thiết kế",
                               //             Functions.TK_PhanCongKhaoSatThietKe.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo",
                                            Functions.TK_BaoCao.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Khảo sát thiết kế",
                                            Functions.TK_KhaoSatThietKe.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Quản lý mẫu bốc vật tư",
                                            Functions.TK_QuanLyMauBocVatTu.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Thiết kế & Bốc vật tư",
                                            Functions.TK_ThietKeVaVatTu.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Duyệt thiết kế",
                                            Functions.TK_DuyetThietKe.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Tra cứu thiết kế",
                                            Functions.TK_TraCuuThietKe.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Lập chiết tính",
                                            Functions.TK_LapChietTinh.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Duyệt chiết tính",
                                            Functions.TK_DuyetChietTinh.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Tra cứu chiết tính",
                                            Functions.TK_TraCuuChietTinh.GetHashCode().ToString()),
                               //new ListItem("Thiết kế / Xử lý đơn chờ hợp đồng",
                               //             Functions.TK_XuLyDonChoHopDong.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Nhập hợp đồng",
                                            Functions.TK_NhapHopDong.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Tra cứu hợp đồng",
                                            Functions.TK_TraCuuHopDong.GetHashCode().ToString()),

                               new ListItem("Thiết kế / Báo cáo / Danh sách đơn đăng ký",
                                            Functions.KH_BaoCao_DonLapMoi_DS_DonDangKy.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách chưa thiết kế",
                                            Functions.KH_BaoCao_DonLapMoi_DS_ChuaThietKe.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách chờ thiết kế",
                                            Functions.KH_BaoCao_DonLapMoi_DS_ChoThietKe.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách đã thiết kế",
                                            Functions.KH_BaoCao_DonLapMoi_DS_DaThietKe.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách chưa lập chiết tính",
                                            Functions.KH_BaoCao_DonLapMoi_DS_ChuaLapCT.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách chờ lập chiết tính",
                                            Functions.KH_BaoCao_DonLapMoi_DS_ChoLapCT.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách đã lập chiết tính",
                                            Functions.KH_BaoCao_DonLapMoi_DS_DaLapCT.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách chưa lập hợp đồng",
                                            Functions.KH_BaoCao_DonLapMoi_DS_ChuaHopDong.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách chờ lập hợp đồng",
                                            Functions.KH_BaoCao_DonLapMoi_DS_ChoHopDong.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách đã lập hợp đồng",
                                            Functions.KH_BaoCao_DonLapMoi_DS_DaHopDong.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách chưa thi công",
                                            Functions.KH_BaoCao_DonLapMoi_DS_ChuaThiCong.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách chờ thi công",
                                            Functions.KH_BaoCao_DonLapMoi_DS_ChoThiCong.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo / Danh sách đã thi công",
                                            Functions.KH_BaoCao_DonLapMoi_DS_DaThiCong.GetHashCode().ToString()),


                                new ListItem("Thiết kế / Báo cáo / Bảng đề nghị xuất vật tư",
                                            Functions.TK_BaoCao_BangDeNghiXuatVatTu.GetHashCode().ToString()),
                                new ListItem("Thiết kế / Báo cáo / Bảng quyết toán tổng hợp",
                                            Functions.TK_BaoCao_BangQuyetToanTongHop.GetHashCode().ToString()),
                               #endregion

                               #region Sữa chữa 
                               new ListItem("Sửa chữa / Nhập thông tin Sửa chữa",
                                            Functions.SC_NhapThongTinSuaChua.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Phân công Sửa chữa",
                                            Functions.SC_PhanCongSuaChua.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Giải quyết đơn Sửa chữa",
                                            Functions.SC_GiaiQuyetDonSuaChua.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Cập nhật chi tiết sau Sửa chữa",
                                            Functions.SC_CapNhatChiTietSauSuaChua.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Quản lý thông tin đơn Sửa chữa",
                                            Functions.SC_QuanLyThongTinDonSuaChua.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Lập chiết tính Sửa chữa",
                                            Functions.SC_LapChietTinhSuaChua.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Duyệt chiết tính Sửa chữa",
                                            Functions.SC_DuyetChietTinhSuaChua.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Tra cứu chiết tính Sửa chữa",
                                            Functions.SC_TraCuuChietTinhSuaChua.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Báo cáo", Functions.SC_BaoCao.GetHashCode().ToString()),

                               new ListItem("Sửa chữa / Báo cáo / Danh sách đơn sửa chữa",
                                            Functions.SC_BaoCao_DanhSachDon.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Báo cáo / Danh sách đơn sửa chữa - phân nhóm",
                                            Functions.SC_BaoCao_DanhSachDonHoanThanhPhanLoai.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Báo cáo / Danh sách đơn chưa lập chiết tính",
                                            Functions.SC_BaoCao_ChuaLapCT.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Báo cáo / Danh sách đơn chờ lập chiết tính",
                                            Functions.SC_BaoCao_ChoLapCT.GetHashCode().ToString()),
                               new ListItem("Sửa chữa / Báo cáo / Danh sách đơn đã lập chiết tính",
                                            Functions.SC_BaoCao_DaLapCT.GetHashCode().ToString()),
                               #endregion

                               #region Thi công
                               //new ListItem("Thi công / Xử lý đơn chờ thi công",
                               //             Functions.TC_XuLyDonChoThiCong.GetHashCode().ToString()),
                               new ListItem("Thi công / Nhập đơn chờ thi công",
                                            Functions.TC_NhapDonThiCong.GetHashCode().ToString()),
                                new ListItem("Thi công / Tra cứu thi công",
                                            Functions.TC_TraCuuThiCong.GetHashCode().ToString()),
                               new ListItem("Thi công / Cập nhật chi tiết sau thi công",
                                            Functions.TC_CapNhatChiTietSauThiCong.GetHashCode().ToString()),
                                
                               new ListItem("Thi công / Lập quyết toán",
                                            Functions.TC_LapQuyetToan.GetHashCode().ToString()),
                               new ListItem("Thi công / Duyệt quyết toán",
                                            Functions.TC_DuyetQuyetToan.GetHashCode().ToString()),
                                new ListItem("Thi công / Tra cứu quyết toán",
                                            Functions.TC_TraCuuQuyetToan.GetHashCode().ToString()),
                                new ListItem("Thi công / Biên bản nghiệm thu",
                                            Functions.TC_BienBanNghiemThu.GetHashCode().ToString()),
                                #endregion 

                               #region Ghi chỉ số
                               new ListItem("Ghi chỉ số / Khởi tạo ghi chỉ số đồng hồ",
                                            Functions.GCS_KhoiTaoGhiChiSo.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Khởi tạo ghi chỉ số theo đường",
                                            Functions.GCS_KhoiTaoGhiChiSoLe.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Khởi tạo ghi chỉ số bổ sung",
                                            Functions.GCS_KhoiTaoGhiChiSoBoSung.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Ghi chỉ số đồng hồ",
                                            Functions.GCS_GhiChiSo.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Chỉnh sửa chỉ số - Hóa đơn",
                                            Functions.GCS_SuaChiSo.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Khóa ghi chỉ số", Functions.GCS_KhoaGhiChiSo.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / In hóa đơn theo lô",
                                            Functions.GCS_InHoaDon.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / In hóa đơn lẻ",
                                            Functions.GCS_InHoaDonLe.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Phát hóa đơn",
                                            Functions.GCS_PhatHoaDon.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / In hoá đơn",
                                            Functions.GCS_InHoaDonN.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Bảng kê điều chỉnh hoá đơn ",
                                            Functions.GCS_DieuChinhHoaDon.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Báo cáo ", Functions.GCS_BaoCao.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Báo cáo / Danh sách kiểm tra ",
                                            Functions.GCS_BaoCao_DSKTKY.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Báo cáo / Chuẩn thu theo đường ",
                                            Functions.GCS_BaoCao_DSChuanThuTheoDuong.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Báo cáo / Tổng hợp chuẩn thu ",
                                            Functions.GCS_BaoCao_TongHopChuanThu.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Báo cáo / Danh sách tiêu thụ  ",
                                            Functions.GCS_BaoCao_DSTieuThuDK.GetHashCode().ToString()),                                            
                               new ListItem("Ghi chỉ số / Báo cáo / Sản lượng thực tế ",
                                            Functions.GCS_BaoCao_SanLuongThucTe.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Báo cáo / Danh sách kiểm dò ",
                                            Functions.GCS_BaoCao_DSKDO.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Báo cáo / Danh sách điều chỉnh Hoá đơn nước ",
                                            Functions.GCS_BaoCao_DSDCHD.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Báo cáo / In sổ ghi thuỷ kế ",
                                            Functions.GCS_BaoCao_INSOGHINUOC.GetHashCode().ToString()),
                               
                               
                               #endregion

                               #region Công nợ
                               new ListItem("Công nợ / Nhập công nợ", Functions.CN_NhapCongNo.GetHashCode().ToString()),
                               new ListItem("Công nợ / Thu tại quầy ", Functions.CN_ThuTaiQuay.GetHashCode().ToString()),
                               new ListItem("Công nợ / Tra cứu công nợ", Functions.CN_TraCuuCongNo.GetHashCode().ToString()),
                               new ListItem("Công nợ / Phân công ghi thu", Functions.CN_PhanCongGhiThu.GetHashCode().ToString()),
                               new ListItem("Công nợ / Kiểm tra hóa đơn tồn", Functions.CN_KiemTraHoaDonTon.GetHashCode().ToString()),
                               new ListItem("Công nợ / Nhập công nợ ngược", Functions.CN_NhapCongNoNguoc.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo ", Functions.CN_BaoCao.GetHashCode().ToString()),

                               new ListItem("Công nợ / Báo cáo / Chi tiết hóa đơn tồn của từng kỳ hóa đơn ",
                                            Functions.CN_BaoCao_ChiTietHoaDonTon.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo / Tổng hợp công nợ theo nhân viên ",
                                            Functions.CN_BaoCao_TongHopCongNoTheoNhanVien.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo / Chi tiết thu nợ theo thời gian ",
                                            Functions.CN_BaoCao_ChiTietThuNoTheoThoiGian.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo / Bảng kê chi tiết thực thu ",
                                            Functions.CN_BaoCao_BangKeChiTietThucThu.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo / Khách hàng tồn hóa đơn ",
                                            Functions.CN_BaoCao_KhachHangTonHoaDon.GetHashCode().ToString()),
                               //new ListItem("Công nợ / Báo cáo /Danh sách Mới/Mở/Cúp so với kỳ trước ",
                               //             Functions.CN_BaoCao_MoiMoCupSoVoiThangTruoc.GetHashCode().ToString()),
                               //new ListItem("Công nợ / Báo cáo / Danh sách khách hàng ghi sót ",
                               //             Functions.CN_BaoCao_DanhSachKhachHangGhiSot.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo / Báo cáo tình hình thực thu ",
                                            Functions.CN_BaoCao_TinhHinhThucThu.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo / Bảng chi tiết chuẩn thu ",
                                            Functions.CN_BaoCao_BangChiTietChuanThu.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo / Tình hình nhân viên thu tiền ",
                                            Functions.CN_BaoCao_TinhHinhThuTienNhanVien.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo / Bảng kê thu tiền nước ",
                                            Functions.CN_BaoCao_BangKeThuTienNuoc.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo / Bảng kê tồn hóa đơn theo đường",
                                            Functions.CN_BaoCao_BangKeTonHoaDonTheoDuong.GetHashCode().ToString()),
                               new ListItem("Công nợ / Báo cáo / Khách hàng tồn hóa đơn nhiều kỳ",
                                            Functions.CN_BaoCao_KhachHangTonHoaDonNhieuKy.GetHashCode().ToString()),
                               #endregion             

                               #region Truy thu
                               new ListItem("Truy thu / Do ghi sai chỉ số đồng hồ",
                                            Functions.TRUYTHU_GhiSaiChiSo.GetHashCode().ToString()),
                               new ListItem("Truy thu / Do khách hàng vi phạm",
                                            Functions.TRUYTHU_DoViPham.GetHashCode().ToString()), 
#endregion
                              #region Tram BA
                               
                               new ListItem("Trạm biến áp / Nhập Trạm biến áp",
                                            Functions.TRAMBIENAP_NhapTBA.GetHashCode().ToString()),
                                new ListItem("Trạm biến áp / Ghi số hộ Trạm biến áp",
                                            Functions.TRAMBIENAP_GhiSoHoTBA.GetHashCode().ToString()),
                                new ListItem("Trạm biến áp / Ghi chỉ số Trạm biến áp",
                                            Functions.TRAMBIENAP_GhiChiSoTBA.GetHashCode().ToString()),
                                new ListItem("Trạm biến áp / Cập nhật Trạm biến áp",
                                            Functions.TRAMBIENAP_CapNhatChiSoTBA.GetHashCode().ToString()),
                                new ListItem("Trạm biến áp / Khởi tạo chỉ số Trạm biến áp",
                                            Functions.TRAMBIENAP_KhoiTaoChiSoTBA.GetHashCode().ToString()),                                                             

#endregion

                                new ListItem("Nghĩ phép & Công tác / Đơn xin nghĩ phép",
                                            Functions.NGHIPHEPCONGTAC_DonNghiPhep.GetHashCode().ToString()),
  
                                new ListItem("Vật tư / Nhập vật tư nước",
                                            Functions.VATTU_NhapVatTuNuoc.GetHashCode().ToString()),
                                new ListItem("Vật tư / Tổng hợp vật tư nước",
                                            Functions.VATTU_TongHopVatTuNuoc.GetHashCode().ToString())
                           };
            }
        }
    }
}