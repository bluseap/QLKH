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
        DM_UpLoadFile = 323,
        DM_DongHoPo = 324,
        DM_XaPhuong = 325,
        DM_ApKhom = 326,
        DM_TramBienAp = 327,
        DM_DuongPhoPo = 328,

        DM_BaoCao = 350,
        DM_BaoCao_DuongPho = 351,
        DM_BaoCao_VatTu = 352,

        KH_NhapMoi = 400,
        KH_DonLapDatMoi = 401,
        KH_Mobi_DonLapDatMoi = 2000,
        KH_DonLapDatMoiPo = 417,
        KH_TraCuuDonLapMoi = 402,
        KH_TraCuuDonLapMoiPo = 418,
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
        KH_ThongTinKH = 412,
        KH_ThayDoiHD = 413,
        KH_XoaBoKHN = 414,
        KH_GiaHanHopDong = 438,
        KH_HoNgheoN = 439,
        KH_CapNhatSoBoTS = 440,
        KH_DSChuanBiKhaiThac = 441,
        KH_TachDuongPho = 450,
        KH_NhapKHMoiCDoc = 452,   

        //dien
        KH_Dien = 415,
        KH_BaoCaoDien = 416,
        KH_NhapMoiDien = 419,
        KH_TraCuuKhachHangPo = 436,
        KH_ThayDongHoPo = 437,
        KH_NhapKHMoiTSPo = 442,
        KH_ThayHopDongPo = 443,
        KH_XoaBoKHPo = 449,
        KH_TachDuongPhoPo = 451,      
        
        KH_BaoCaoPo_InGiayDeNghiPo = 480,
        KH_BaoCaoPo_DSKHMPo = 487,
        KH_BaoCaoPo_DSDLMPo = 488,
        KH_BaoCaoPo_DSThayDHPo = 489,
        KH_BaoCaoPo_DSThayDoiCTPo = 490,
        KH_BaoCaoPo_DSCBiKT = 491,
        KH_BCPO_DSTONGHOPKHDKPO = 447,

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
        //KH_BaoCao_QLKH_DS_ThongTinThayDhTuKhachHang = 474,
        KH_BaoCao_QLKH_DSDHCSLON = 474,
        KH_SuaNoDHKH = 475,
        KH_NhapKHTSon = 486,
        KH_SuaNoDHKHPo = 444,
        KH_ThongTinKHPo = 445,
        KH_BC_DSTONGHOPKHDK = 446,
        KH_TraCuuDonLapMoiLX = 448,

        KH_BaoCao_QLKH_TinhHinhTieuThuKhachHang = 476,
        KH_BaoCao_QLKH_LichSuSuDungNuoc = 477,
        KH_BaoCao_QLKH_ThayDoiChiTietNuoc = 478,
        KH_BaoCao_DonLapMoi_InGiayDeNghi = 479,

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
        TK_KhaoSatThietKePo = 617,
        TK_QuanLyMauBocVatTu = 602,
        TK_QuanLyMauBocVatTuPo = 618,
        TK_ThietKeVaVatTu = 603,
        TK_ThietKeVaVatTuPo = 619,
        TK_DuyetThietKe = 604,
        TK_DuyetThietKePo = 620,
        TK_TraCuuThietKe = 605,
        TK_TraCuuThietKePo = 621,
        TK_LapChietTinh = 606,
        TK_LapChietTinhPo = 622,
        TK_DuyetChietTinh = 607,
        TK_DuyetChietTinhPo = 623,
        TK_TraCuuChietTinh = 608,
        TK_TraCuuChietTinhPo = 624,
        TK_XuLyDonChoHopDong = 609,
        TK_NhapHopDong = 610,
        TK_NhapHopDongPo = 625,
        TK_TraCuuHopDong = 611,
        TK_TraCuuHopDongPo = 626,
        TK_BocVatTuLX = 635,
        TK_ThietKeVaVatTuLX = 636,
        TK_DuyetDonTKTC = 637,
        TK_DuyetDonTKTCPo = 638,

        TK_BaoCao_BangDeNghiXuatVatTu = 614,
        TK_BaoCao_BangQuyetToanTongHop = 615,
        TK_BaoCao_ChietTinhKoKhaiThac = 616,

        TK_BaoCao = 650,
        TK_Dien = 612,
        TK_BaoCaoDien = 613,
        TK_BaoCaoDien_ThietKePo = 627,
        TK_BaoCaoDien_DSDuyetTKPo = 632,
        TK_BaoCaoDien_DSNOBVTTKPo = 633,
        TK_BaoCaoDien_DSCHUAHDPo = 634,

        TC_XuLyDonChoThiCong = 700,
        TC_NhapDonThiCong = 701,
        TC_NhapDonThiCongPo = 713,
        TC_CapNhatChiTietSauThiCong = 702,
        TC_CapNhatCTSauThiCongPo = 714,
        TC_TraCuuThiCong = 703,
        TC_TraCuuThiCongPo = 715,
        TC_LapQuyetToan = 704,
        TC_DuyetQuyetToan = 705,
        TC_TraCuuQuyetToan = 706,
        TC_BienBanNghiemThu = 707,
        TC_InPhieuLapNuoc = 708,
        TC_InPhieuNiemChiNuoc = 709,
        TC_InBienBanNghiemThu = 710,
        TC_SuaDongHo = 711,

        TC_Dien = 712,
        TC_BBNghiemThuPo = 716,
        TC_BaoCaoPo_InPhieuLDPo = 717,
        TC_BaoCaoPo_InBBNghiemThuPo = 719,
        TC_BaoCaoPo = 722,
        TC_DSBBNTPo = 724,
        TC_DSBBChuaNTPo = 726,
        TC_DSChuaTCPo = 728,
        TC_SuaDongHoPo = 729,

        TC_DSChuaTC = 727,
        TC_BBNTSuaChua = 720,
        TC_BaoCao = 721,
        TC_DSBBNT = 723,
        TC_DSBBChuaNT = 725,

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
        GCS_DieuChinhHDTG = 811,

        GCS_BaoCao = 850,
        GCS_BaoCao_DSKTKY = 851,
        GCS_BaoCao_DSTieuThuDK = 852,
        GCS_BaoCao_DSChuanThuTheoDuong = 853,
        GCS_BaoCao_TongHopChuanThu = 854,
        GCS_BaoCao_SanLuongThucTe = 855,
        GCS_BaoCao_DSKDO = 856,
        GCS_BaoCao_DSDCHD = 857,
        GCS_BaoCao_INSOGHINUOC = 858,

        GCS_Dien = 812,
        GCS_BaoCaoDien = 814,
        GCS_KhoiTaoGhiChiSoPo = 813,
        GCS_GhiChiSoPo = 815,
        GCS_SuaChiSoPo = 816,
        GCS_DieuChinhHoaDonPo = 817, 
        GCS_KhoaGhiChiSoPo = 818,
        GCS_TruyThuVPPo = 821,
        GCS_GhiChiSoTBTT = 819,
        GCS_TruyThuVP = 820,
        GCS_DotInHD = 822,
        GCS_DotInHDPo = 823,

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
        VATTU_TongHopVatTuNuoc = 1152,

        VAY_BaoCao = 1200,
        VAY_NhanVienVay = 1201,
        VAY_NhanVienVayCD = 1202,
        VAY_DSNhanVienVay = 1203,
        VAY_TraCuuNhanVienVay = 1204,

        NV_NVLyLich = 2100
        
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
                               //DM_DuongPhoPo
                               new ListItem("Danh mục / Đường phố (điện)", Functions.DM_DuongPhoPo.GetHashCode().ToString()),
                               new ListItem("Danh mục / Phường", Functions.DM_Phuong.GetHashCode().ToString()),
                               new ListItem("Danh mục / Khu vực", Functions.DM_KhuVuc.GetHashCode().ToString()),
                               new ListItem("Danh mục / Nhân viên", Functions.DM_NhanVien.GetHashCode().ToString()),
                               new ListItem("Danh mục / Vật tư", Functions.DM_VatTu.GetHashCode().ToString()),
                               new ListItem("Danh mục / Đồng hồ nước", Functions.DM_DongHo.GetHashCode().ToString()),
                               //DM_DongHoPo
                               new ListItem("Danh mục / Đồng hồ điện",
                                            Functions.DM_DongHoPo.GetHashCode().ToString()),
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
                               new ListItem("Danh mục / UpLoad file",
                                            Functions.DM_UpLoadFile.GetHashCode().ToString()),
                                            //DM_XaPhuong
                                new ListItem("Danh mục / Xã, phường",
                                            Functions.DM_XaPhuong.GetHashCode().ToString()),
                                            //DM_ApKhom
                                new ListItem("Danh mục / Ấp, khóm",
                                            Functions.DM_ApKhom.GetHashCode().ToString()),
                                    //DM_TramBienAp
                                new ListItem("Danh mục / Trạm biến áp",
                                            Functions.DM_TramBienAp.GetHashCode().ToString()),
                               #endregion

                               #region Khách hàng
                               new ListItem("Khách hàng / Điện", Functions.KH_Dien.GetHashCode().ToString()),                               
                               new ListItem("Khách hàng / Báo cáo điện", Functions.KH_BaoCaoDien.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo", Functions.KH_BaoCao.GetHashCode().ToString()),

                               new ListItem("Khách hàng / Nhập khách hàng",
                                            Functions.KH_NhapMoi.GetHashCode().ToString()),
                                            //KH_NhapMoiDien
                               new ListItem("Khách hàng / Nhập khách hàng điện",
                                            Functions.KH_NhapMoiDien.GetHashCode().ToString()),
                                            //KH_NhapKHMoiTSPo
                               new ListItem("Khách hàng / Nhập khách hàng mới điện (Th.Sơn)",
                                            Functions.KH_NhapKHMoiTSPo.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Tra cứu khách hàng",
                                            Functions.KH_TraCuuKhachHang.GetHashCode().ToString()),
                               //KH_TraCuuKhachHang = 436,
                               new ListItem("Khách hàng / Tra cứu khách hàng điện",
                                            Functions.KH_TraCuuKhachHangPo.GetHashCode().ToString()),                               
                               new ListItem("Khách hàng / Nhập đơn lắp đặt mới",
                                            Functions.KH_DonLapDatMoi.GetHashCode().ToString()),         
                                            //KH_Mobi_DonLapDatMoi = 2000,
                               new ListItem("Khách hàng / Nhập đơn lắp đặt mới mobi",
                                            Functions.KH_Mobi_DonLapDatMoi.GetHashCode().ToString()),         
                               new ListItem("Khách hàng / Nhập đơn lắp đặt mới điện",
                                            Functions.KH_DonLapDatMoiPo.GetHashCode().ToString()),
                               //new ListItem("Khách hàng / Nhập đơn cải tạo công ty",
                               //             Functions.KH_DonCaiTaoCongTy.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Tra cứu đơn lắp đặt mới",
                                            Functions.KH_TraCuuDonLapMoi.GetHashCode().ToString()),
                                            //KH_TraCuuDonLapMoiLX
                              new ListItem("Khách hàng / Tra cứu đơn lắp đặt mới (LX)",
                                            Functions.KH_TraCuuDonLapMoiLX.GetHashCode().ToString()),

                                            //KH_TraCuuDonLapMoiPo
                               new ListItem("Khách hàng / Tra cứu đơn lắp đặt mới điện",
                                            Functions.KH_TraCuuDonLapMoiPo.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Cúp mở nước", Functions.KH_CupMoNuoc.GetHashCode().ToString()),
                               //new ListItem("Khách hàng / Phát sinh tăng", Functions.KH_PhatSinhTang.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Thay đồng hồ", Functions.KH_ThayDongHo.GetHashCode().ToString()),
                               //KH_ThayDongHoPo = 437,
                               new ListItem("Khách hàng / Thay đồng hồ điện", Functions.KH_ThayDongHoPo.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Cập nhật sổ bộ", Functions.KH_CapNhatSoBo.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Cập nhật đoạn", Functions.KH_CapNhatDoan.GetHashCode().ToString()),
                               //new ListItem("Khách hàng / Thay danh số", Functions.KH_ThayDanhSo.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Nhập Khách hàng tạm", Functions.KH_KhachHangTam.GetHashCode().ToString()),
                               new ListItem("Khách hàng / In hợp đồng khách hàng", Functions.KH_InHopDongKH.GetHashCode().ToString()),
                                     //KH_TachDuongPho                
                               new ListItem("Khách hàng / Tách đường phố", Functions.KH_TachDuongPho.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Tách đường phố điện", Functions.KH_TachDuongPhoPo.GetHashCode().ToString()),
                               
                               
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
                               new ListItem("Khách hàng / Báo cáo / QLKH / Danh sách đồng hồ công suất lớn",
                                            Functions.KH_BaoCao_QLKH_DSDHCSLON.GetHashCode().ToString()), 
                               new ListItem("Khách hàng / Báo cáo / QLKH / Tình hình tiêu thụ khách hàng",
                                            Functions.KH_BaoCao_QLKH_TinhHinhTieuThuKhachHang.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / QLKH / Lịch sử sử dụng nước",
                                            Functions.KH_BaoCao_QLKH_LichSuSuDungNuoc.GetHashCode().ToString()),
                               new ListItem("Khách hàng / Báo cáo / QLKH / Thay đổi chi tiết nước",
                                            Functions.KH_BaoCao_QLKH_ThayDoiChiTietNuoc.GetHashCode().ToString()),                                                         
                               new ListItem("Khách hàng / Báo cáo / Đơn lắp đặt mới / In giấy đề nghị",
                                            Functions.KH_BaoCao_DonLapMoi_InGiayDeNghi.GetHashCode().ToString()),  
                                            
                                new ListItem("Khách hàng / Báo cáo / QLKH / DS Tổng hợp đơn ĐK KH",
                                            Functions.KH_BC_DSTONGHOPKHDK.GetHashCode().ToString()),  

                               new ListItem("Khách hàng / Thông tin Khách hàng",
                                            Functions.KH_ThongTinKH.GetHashCode().ToString()), 
                                            //KH_ThongTinKHPo
                                new ListItem("Khách hàng / Thông tin Khách hàng điện",
                                            Functions.KH_ThongTinKHPo.GetHashCode().ToString()), 
                               new ListItem("Khách hàng / Thay đổi hợp đồng",
                                            Functions.KH_ThayDoiHD.GetHashCode().ToString()),  
                                            //KH_GiaHanHopDong
                                new ListItem("Khách hàng / Gia hạn hợp đồng khách hàng",
                                            Functions.KH_GiaHanHopDong.GetHashCode().ToString()),  
                                            //KH_XoaBoKHN
                               new ListItem("Khách hàng / Xoá bộ khách hàng nước",
                                            Functions.KH_XoaBoKHN.GetHashCode().ToString()),   
                               new ListItem("Khách hàng / Xoá bộ khách hàng điện",
                                            Functions.KH_XoaBoKHPo.GetHashCode().ToString()),
                                            
                                            //KH_HoNgheoN
                               new ListItem("Khách hàng / Hộ nghèo nước",
                                            Functions.KH_HoNgheoN.GetHashCode().ToString()),    
                                            //KH_CapNhatSoBoTS
                               new ListItem("Khách hàng / Cập nhật sổ bộ (T.Sơn)",
                                            Functions.KH_CapNhatSoBoTS.GetHashCode().ToString()),    
                               new ListItem("Khách hàng / Sửa số No đồng hồ khách hàng",
                                            Functions.KH_SuaNoDHKH.GetHashCode().ToString()),
                                            //KH_SuaNoDHKHPo
                               new ListItem("Khách hàng / Sửa số No đồng hồ điện khách hàng",
                                            Functions.KH_SuaNoDHKHPo.GetHashCode().ToString()),
                                            //KH_NhapKHTSon
                               new ListItem("Khách hàng / Nhập khách hàng mới (Thoại Sơn)",
                                            Functions.KH_NhapKHTSon.GetHashCode().ToString()),
                                            //KH_NhapKHMoiCDoc = 452,   
                               new ListItem("Khách hàng / Nhập khách hàng mới (Châu Đốc)",
                                            Functions.KH_NhapKHMoiCDoc.GetHashCode().ToString()),
                                            //KH_ThayHopDongPo
                                new ListItem("Khách hàng / Thay hợp đồng điện",
                                            Functions.KH_ThayHopDongPo.GetHashCode().ToString()),

                                            //KH_BaoCaoPo_InGiayDeNghiPo
                               new ListItem("Khách hàng / Báo cáo điện / In giấy đề nghị điện",
                                            Functions.KH_BaoCaoPo_InGiayDeNghiPo.GetHashCode().ToString()),
                                            //KH_BaoCaoPo_DSKHMPo
                                new ListItem("Khách hàng / Báo cáo điện / Ds khách hàng mới điện",
                                            Functions.KH_BaoCaoPo_DSKHMPo.GetHashCode().ToString()),
                                            //KH_BaoCaoPo_DSDLMPo
                                new ListItem("Khách hàng / Báo cáo điện / Ds đơn lắp mới điện",
                                            Functions.KH_BaoCaoPo_DSDLMPo.GetHashCode().ToString()),
                                            //KH_BaoCaoPo_DSThayDHPo
                                 new ListItem("Khách hàng / Báo cáo điện / Ds thay đồng hồ điện",
                                            Functions.KH_BaoCaoPo_DSThayDHPo.GetHashCode().ToString()),
                                            //KH_BaoCaoPo_DSThayDoiCTPo
                                new ListItem("Khách hàng / Báo cáo điện / Ds thay đổi chi tiết điện",
                                            Functions.KH_BaoCaoPo_DSThayDoiCTPo.GetHashCode().ToString()),
                                            //KH_BaoCaoPo_DSCBiKT = 491
                                new ListItem("Khách hàng / Báo cáo điện / DS Khách hàng chuẩn bị khai thác điện",
                                            Functions.KH_BaoCaoPo_DSCBiKT.GetHashCode().ToString()),

                                            //KH_DSChuanBiKhaiThac = 441,
                               new ListItem("Khách hàng / Báo cáo / DS Khách hàng chuẩn bị khai thác",
                                            Functions.KH_DSChuanBiKhaiThac.GetHashCode().ToString()),
                                        //KH_BCPO_DSTONGHOPKHDKPO
                                new ListItem("Khách hàng / Báo cáo điện / DS Tổng hợp đơn ĐK KH điện",
                                            Functions.KH_BCPO_DSTONGHOPKHDKPO.GetHashCode().ToString()),
                                            
                                #endregion

                               #region Thiết kế
                               //new ListItem("Thiết kế / Phân công khảo sát thiết kế",
                               //             Functions.TK_PhanCongKhaoSatThietKe.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Điện", Functions.TK_Dien.GetHashCode().ToString()),
                                            //TK_BaoCaoDien
                                new ListItem("Thiết kế / Báo cáo điện", Functions.TK_BaoCaoDien.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Báo cáo",
                                            Functions.TK_BaoCao.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Khảo sát thiết kế nước",
                                            Functions.TK_KhaoSatThietKe.GetHashCode().ToString()),                                
                                new ListItem("Thiết kế / Khảo sát thiết kế điện",
                                            Functions.TK_KhaoSatThietKePo.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Quản lý mẫu bốc vật tư",
                                            Functions.TK_QuanLyMauBocVatTu.GetHashCode().ToString()),                                 
                                new ListItem("Thiết kế / Quản lý mẫu bốc vật tư điện",
                                            Functions.TK_QuanLyMauBocVatTuPo.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Thiết kế & Bốc vật tư",
                                            Functions.TK_ThietKeVaVatTu.GetHashCode().ToString()),
                                          
                               new ListItem("Thiết kế / Thiết kế & Bốc vật tư (LX)",
                                            Functions.TK_ThietKeVaVatTuLX.GetHashCode().ToString()),

                                         //TK_BocVatTuLX
                               new ListItem("Thiết kế / Bốc vật tư (LX)",
                                            Functions.TK_BocVatTuLX.GetHashCode().ToString()),

                               new ListItem("Thiết kế / Thiết kế & Bốc vật tư điện",
                                            Functions.TK_ThietKeVaVatTuPo.GetHashCode().ToString()),                                            
                               new ListItem("Thiết kế / Duyệt thiết kế",
                                            Functions.TK_DuyetThietKe.GetHashCode().ToString()),
                                            //TK_DuyetDonTKTC
                                new ListItem("Thiết kế / Duyệt đơn, thiết kế (Tân Châu)",
                                            Functions.TK_DuyetDonTKTC.GetHashCode().ToString()),

                                            //TK_DuyetThietKePo
                                new ListItem("Thiết kế / Duyệt thiết kế điện",
                                            Functions.TK_DuyetThietKePo.GetHashCode().ToString()),
                                            //TK_DuyetDonTKTCPo
                                new ListItem("Thiết kế / Duyệt đơn, thiết kế điện (Tân Châu)",
                                            Functions.TK_DuyetDonTKTCPo.GetHashCode().ToString()),

                               new ListItem("Thiết kế / Tra cứu thiết kế",
                                            Functions.TK_TraCuuThietKe.GetHashCode().ToString()),
                                            //TK_TraCuuThietKePo
                               new ListItem("Thiết kế / Tra cứu thiết kế điện",
                                            Functions.TK_TraCuuThietKePo.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Lập chiết tính",
                                            Functions.TK_LapChietTinh.GetHashCode().ToString()),
                                            //TK_LapChietTinhPo
                               new ListItem("Thiết kế / Lập chiết tính điện",
                                            Functions.TK_LapChietTinhPo.GetHashCode().ToString()),
                                            //TK_DuyetChietTinhPo
                               new ListItem("Thiết kế / Duyệt chiết tính",
                                            Functions.TK_DuyetChietTinh.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Duyệt chiết tính điện",
                                            Functions.TK_DuyetChietTinhPo.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Tra cứu chiết tính",
                                            Functions.TK_TraCuuChietTinh.GetHashCode().ToString()),
                                            //TK_TraCuuChietTinhPo
                               new ListItem("Thiết kế / Tra cứu chiết tính điện",
                                            Functions.TK_TraCuuChietTinhPo.GetHashCode().ToString()),
                               //new ListItem("Thiết kế / Xử lý đơn chờ hợp đồng",
                               //             Functions.TK_XuLyDonChoHopDong.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Nhập hợp đồng",
                                            Functions.TK_NhapHopDong.GetHashCode().ToString()),
                                            //TK_NhapHopDongPo
                               new ListItem("Thiết kế / Nhập hợp đồng điện",
                                            Functions.TK_NhapHopDongPo.GetHashCode().ToString()),
                               new ListItem("Thiết kế / Tra cứu hợp đồng",
                                            Functions.TK_TraCuuHopDong.GetHashCode().ToString()),
                                //TK_TraCuuHopDongPo
                                 new ListItem("Thiết kế / Tra cứu hợp đồng điện",
                                            Functions.TK_TraCuuHopDongPo.GetHashCode().ToString()),
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
                                
                                            //TK_BaoCao_ChietTinhKoKhaiThac
                                new ListItem("Thiết kế / Báo cáo / Chiết tính chưa khai thác",
                                            Functions.TK_BaoCao_ChietTinhKoKhaiThac.GetHashCode().ToString()),
                                            //TK_BaoCaoDien_ThietKePo
                                new ListItem("Thiết kế / Báo cáo điện / Thiết kế điện",
                                            Functions.TK_BaoCaoDien_ThietKePo.GetHashCode().ToString()),
                                            //TK_BaoCaoDien_DSDuyetTKPo
                                new ListItem("Thiết kế / Báo cáo điện / DS đã duyệt thiết kế điện",
                                            Functions.TK_BaoCaoDien_DSDuyetTKPo.GetHashCode().ToString()),
                                            //TK_BaoCaoDien_DSNOBVTTKPo
                                new ListItem("Thiết kế / Báo cáo điện / DS chưa bốc vật tư thiết kế điện",
                                            Functions.TK_BaoCaoDien_DSNOBVTTKPo.GetHashCode().ToString()),
                                            //TK_BaoCaoDien_DSCHUAHDPo
                                new ListItem("Thiết kế / Báo cáo điện / DS chưa chưa lập hợp đồng điện (Đã duyệt chiết tính)",
                                            Functions.TK_BaoCaoDien_DSCHUAHDPo.GetHashCode().ToString()),
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
                               new ListItem("Thi công / Điện", Functions.TC_Dien.GetHashCode().ToString()),
                               new ListItem("Thi công / Nhập đơn chờ thi công",
                                            Functions.TC_NhapDonThiCong.GetHashCode().ToString()),
                                            //TC_NhapDonThiCongPo
                                new ListItem("Thi công / Nhập đơn chờ thi công điện",
                                            Functions.TC_NhapDonThiCongPo.GetHashCode().ToString()),
                                new ListItem("Thi công / Tra cứu thi công",
                                            Functions.TC_TraCuuThiCong.GetHashCode().ToString()),
                                            //TC_TraCuuThiCongPo
                                new ListItem("Thi công / Tra cứu thi công điện",
                                            Functions.TC_TraCuuThiCongPo.GetHashCode().ToString()),
                               new ListItem("Thi công / Cập nhật chi tiết sau thi công",
                                            Functions.TC_CapNhatChiTietSauThiCong.GetHashCode().ToString()),    
                                            //TC_CapNhatCTSauThiCongPo
                               new ListItem("Thi công / Cập nhật chi tiết sau thi công điện",
                                            Functions.TC_CapNhatCTSauThiCongPo.GetHashCode().ToString()),    
                               new ListItem("Thi công / Lập quyết toán",
                                            Functions.TC_LapQuyetToan.GetHashCode().ToString()),
                               new ListItem("Thi công / Duyệt quyết toán",
                                            Functions.TC_DuyetQuyetToan.GetHashCode().ToString()),
                                new ListItem("Thi công / Tra cứu quyết toán",
                                            Functions.TC_TraCuuQuyetToan.GetHashCode().ToString()),
                                new ListItem("Thi công / Biên bản nghiệm thu",
                                            Functions.TC_BienBanNghiemThu.GetHashCode().ToString()),
                               //TC_BBNTSuaChua = 720,
                               new ListItem("Thi công / Biên bản nghiệm thu sửa chữa",
                                            Functions.TC_BBNTSuaChua.GetHashCode().ToString()),
                                            //TC_BBNghiemThuPo
                               new ListItem("Thi công / Biên bản nghiệm thu điện",
                                            Functions.TC_BBNghiemThuPo.GetHashCode().ToString()),
                                new ListItem("Thi công / In biên bản nghiệm thu",
                                            Functions.TC_InBienBanNghiemThu.GetHashCode().ToString()),
                               new ListItem("Thi công / In phiếu lắp đặt và niêm chì đồng hồ nước",
                                            Functions.TC_InPhieuLapNuoc.GetHashCode().ToString()),
                               new ListItem("Thi công / In Phiếu niêm chì đồng hồ nước",
                                            Functions.TC_InPhieuNiemChiNuoc.GetHashCode().ToString()),
                               new ListItem("Thi công / Sửa đồng hồ nước sau thi công",
                                            Functions.TC_SuaDongHo.GetHashCode().ToString()),
                               new ListItem("Thi công / Sửa đồng hồ điện sau thi công",
                                            Functions.TC_SuaDongHoPo.GetHashCode().ToString()),

                               //TC_BaoCaoPo = 722, TC_BaoCao = 721,
                               new ListItem("Thi công / Báo cáo điện",
                                            Functions.TC_BaoCaoPo.GetHashCode().ToString()),
                                            //TC_DSChuaTCPo
                                new ListItem("Thi công / DS thưa thi công đồng hồ điện",
                                            Functions.TC_DSChuaTCPo.GetHashCode().ToString()),
                                            //TC_DSBBChuaNTPo
                                new ListItem("Thi công / DS Biên bản chưa nghiệm thu điện",
                                            Functions.TC_DSBBChuaNTPo.GetHashCode().ToString()),
                                            //TC_DSBBNTPo
                                new ListItem("Thi công / DS Biên bản nghiệm thu điện",
                                            Functions.TC_DSBBNTPo.GetHashCode().ToString()),
                               new ListItem("Thi công / Báo cáo nước",
                                            Functions.TC_BaoCao.GetHashCode().ToString()),
                                            //TC_DSChuaTC
                             new ListItem("Thi công / DS thưa thi công đồng hồ nước",
                                            Functions.TC_DSChuaTC.GetHashCode().ToString()),
                                            //TC_DSBBChuaNT
                                new ListItem("Thi công / DS Biên bản chưa nghiệm thu nước",
                                            Functions.TC_DSBBChuaNT.GetHashCode().ToString()),
                                //TC_DSBBNT
                                new ListItem("Thi công / DS Biên bản nghiệm thu nước",
                                            Functions.TC_DSBBNT.GetHashCode().ToString()),                                 

                                            //TC_BaoCaoPo_InPhieuLDPo
                               new ListItem("Thi công điện / In Phiếu lắp đặt điện",
                                            Functions.TC_BaoCaoPo_InPhieuLDPo.GetHashCode().ToString()),
                                            //TC_BaoCaoPo_InBBNghiemThuPo = 719,
                                new ListItem("Thi công điện / In Biên bản nghiệm thu điện",
                                            Functions.TC_BaoCaoPo_InBBNghiemThuPo.GetHashCode().ToString()),
                                #endregion 

                               #region Ghi chỉ số
                              // GCS_Dien = 812,
                               //GCS_BaoCaoDien = 814,                               
                               new ListItem("Ghi chỉ số / Điện", Functions.GCS_Dien.GetHashCode().ToString()),                               
                               new ListItem("Ghi chỉ số / Báo cáo điện", Functions.GCS_BaoCaoDien.GetHashCode().ToString()), 
                               new ListItem("Ghi chỉ số / Khởi tạo ghi chỉ số đồng hồ",
                                            Functions.GCS_KhoiTaoGhiChiSo.GetHashCode().ToString()),
                               //GCS_KhoiTaoGhiChiSoPo = 813,
                               new ListItem("Ghi chỉ số / Khởi tạo ghi chỉ số đồng hồ điện",
                                            Functions.GCS_KhoiTaoGhiChiSoPo.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Khởi tạo ghi chỉ số theo đường",
                                            Functions.GCS_KhoiTaoGhiChiSoLe.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Khởi tạo ghi chỉ số bổ sung",
                                            Functions.GCS_KhoiTaoGhiChiSoBoSung.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Ghi chỉ số đồng hồ",
                                            Functions.GCS_GhiChiSo.GetHashCode().ToString()),
                                //GCS_GhiChiSoPo
                               new ListItem("Ghi chỉ số / Ghi chỉ số đồng hồ điện",
                                            Functions.GCS_GhiChiSoPo.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Chỉnh sửa chỉ số - Hóa đơn",
                                            Functions.GCS_SuaChiSo.GetHashCode().ToString()),
                               //GCS_SuaChiSoPo = 816,
                               new ListItem("Ghi chỉ số / Chỉnh sửa chỉ số - Hóa đơn điện",
                                            Functions.GCS_SuaChiSoPo.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Khóa ghi chỉ số", Functions.GCS_KhoaGhiChiSo.GetHashCode().ToString()),
                               //GCS_KhoaGhiChiSoPo
                               new ListItem("Ghi chỉ số / Khóa ghi chỉ số điện", Functions.GCS_KhoaGhiChiSoPo.GetHashCode().ToString()),
                               //GCS_TruyThuVPPo
                               new ListItem("Ghi chỉ số / Truy thu vi phạm điện", Functions.GCS_TruyThuVPPo.GetHashCode().ToString()),

                               new ListItem("Ghi chỉ số / In hóa đơn theo lô",
                                            Functions.GCS_InHoaDon.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / In hóa đơn lẻ",
                                            Functions.GCS_InHoaDonLe.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / Phát hóa đơn",
                                            Functions.GCS_PhatHoaDon.GetHashCode().ToString()),
                               new ListItem("Ghi chỉ số / In hoá đơn",
                                            Functions.GCS_InHoaDonN.GetHashCode().ToString()),
                                            //GCS_DieuChinhHDTG
                               new ListItem("Ghi chỉ số / Điều chỉnh hoá đơn nước tăng giảm",
                                            Functions.GCS_DieuChinhHDTG.GetHashCode().ToString()),

                               new ListItem("Ghi chỉ số / Bảng kê điều chỉnh hoá đơn nước",
                                            Functions.GCS_DieuChinhHoaDon.GetHashCode().ToString()),
                               //GCS_DieuChinhHoaDonPo = 817,
                               new ListItem("Ghi chỉ số / Bảng kê điều chỉnh hoá đơn điện ",
                                            Functions.GCS_DieuChinhHoaDonPo.GetHashCode().ToString()),
                                            //GCS_GhiChiSoTBTT
                               new ListItem("Ghi chỉ số / Ghi chỉ số thông báo tiêu thụ bất thường ",
                                            Functions.GCS_GhiChiSoTBTT.GetHashCode().ToString()),
                                            //GCS_TruyThuVP
                                new ListItem("Ghi chỉ số / Truy thu vi phạm (ĐH chết, ....) ",
                                            Functions.GCS_TruyThuVP.GetHashCode().ToString()),
                                            //GCS_DotInHD
                               new ListItem("Ghi chỉ số / Chuyển đợt in HĐ ",
                                            Functions.GCS_DotInHD.GetHashCode().ToString()),
                                            //GCS_DotInHDPo
                               new ListItem("Ghi chỉ số / Chuyển đợt in HĐ điện",
                                            Functions.GCS_DotInHDPo.GetHashCode().ToString()),
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
                                
                                #region Nghi phep - Cong tac
                                new ListItem("Nghĩ phép & Công tác / Báo cáo",
                                            Functions.NGHIPHEPCONGTAC_BaoCao.GetHashCode().ToString()),
                                new ListItem("Nghĩ phép & Công tác / Đơn xin nghĩ phép",
                                            Functions.NGHIPHEPCONGTAC_DonNghiPhep.GetHashCode().ToString()),

                                #endregion

                                new ListItem("Vật tư / Nhập vật tư nước",
                                            Functions.VATTU_NhapVatTuNuoc.GetHashCode().ToString()),
                                new ListItem("Vật tư / Tổng hợp vật tư nước",
                                            Functions.VATTU_TongHopVatTuNuoc.GetHashCode().ToString()),
                                

                                #region Vay công đoàn
                                            //VAY_BaoCao = 1200,        VAY_NhanVienVay = 1201
                                 new ListItem("Vay công đoàn / Báo cáo vay", Functions.VAY_BaoCao.GetHashCode().ToString()),
                                 new ListItem("Vay công đoàn / Nhân viên tham gia quỹ tiết kiệm", Functions.VAY_NhanVienVay.GetHashCode().ToString()),
                                 new ListItem("Vay công đoàn / Nhân viên vay công đoàn", Functions.VAY_NhanVienVayCD.GetHashCode().ToString()),
                                 //  VAY_DSNhanVienVay = 1203,       VAY_TraCuuNhanVienVay = 1204
                                 new ListItem("Vay công đoàn / Danh sách nhân viên vay tiết kiệm", Functions.VAY_DSNhanVienVay.GetHashCode().ToString()),
                                 new ListItem("Vay công đoàn / Tra cứu nhân viên vay tiết kiệm", Functions.VAY_TraCuuNhanVienVay.GetHashCode().ToString()),
                                #endregion

                                #region Nhân sự
                                 
                                 new ListItem("Nhân sự / Nhập lý lịch nhân viên", Functions.NV_NVLyLich.GetHashCode().ToString()),

                                  #endregion


                           };
            }
        }
    }
}