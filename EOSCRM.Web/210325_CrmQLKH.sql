USE [CRMLX6]
GO
/****** Object:  StoredProcedure [dbo].[Delete_DonDangKy_ByMaddk]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-12-15
-- Description:	Delete by Maddk
-- =============================================

CREATE PROCEDURE [dbo].[Delete_DonDangKy_ByMaddk]	
	@Maddk varchar(11),
	@GhiChuDLM nvarchar(1000)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		insert INTO DONDANGKY_H ([MADDK] ,[MADDKTONG]      ,[TENKH]      ,[DIACHILD]      ,[SONHA]
      ,[DIENTHOAI]      ,[CMND]      ,[MADP]      ,[DUONGPHU]      ,[MAPHUONG]      ,[MAKV]
      ,[TEN_DC_KHAC]      ,[MAMDSD]      ,[NGAYDK]      ,[NGAYHKS]      ,[TTDK]      ,[TTTK]
      ,[TTCT]      ,[TTHD]      ,[TTTC]      ,[TTHC]      ,[LOAIDK]      ,[TENDK]      ,[DACBIET]
      ,[NGAYCD]      ,[NGAYKS]      ,[DTNGOAI]      ,[PASSDUYETTK]      ,[BATAICHO]      ,[CTCTMOI]
      ,[MAPB]      ,[MANV]      ,[SOHODN]      ,[SONK]      ,[DMNK]      ,[DAIDIEN]      ,[NOIDUNG]
      ,[MST]      ,[SDInfo_INHOADON]      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]
      ,[ISTUYENONGCHUNG]      ,[NGAYSINH]      ,[CAPNGAY]      ,[TAI]      ,[MAHTTT]      ,[NOILAPDHHN]
      ,[TTNT]      ,[DONVICAPHN]      ,[MAHN]      ,[NGAYCAPHN]      ,[NGAYKETTHUCHN]
      ,[NGAYKYHN]      ,[KYHOTROHN]      ,[ISHONGHEO]      ,[DIACHINGHEO]      ,[TENCHUCVU]
      ,[NGAYN]      ,[MADPKHGAN]      ,[MADBKHGAN]      ,[SONHA2]      ,[NGAYNHAPHSTRA]
      ,[NGAYDUYETHS]      ,[ISTRAHS]      ,[NGAYDUYETHSTRA]      ,[TIENCOCLX]
      ,[TIENVATTULX]      ,[NGAYNHAPHIS]
	  , [SONHANHAPDON2]      ,[TENDUONG]      ,[MAXA]      ,[TENXA]      ,[MAAPTO]
      , [TENAPTO]      ,[SoDienThoai2]      ,[IsKHMuaVatTu]      ,[IsXoaDLM]      ,[GhiChuXoaDLM]      ,[NgayXoaDLM])
		SELECT [MADDK] ,[MADDKTONG]      ,[TENKH]      ,[DIACHILD]      ,[SONHA]
      ,[DIENTHOAI]      ,[CMND]      ,[MADP]      ,[DUONGPHU]      ,[MAPHUONG]      ,[MAKV]
      ,[TEN_DC_KHAC]      ,[MAMDSD]      ,[NGAYDK]      ,[NGAYHKS]      ,[TTDK]      ,[TTTK]
      ,[TTCT]      ,[TTHD]      ,[TTTC]      ,[TTHC]      ,[LOAIDK]      ,[TENDK]      ,[DACBIET]
      ,[NGAYCD]      ,[NGAYKS]      ,[DTNGOAI]      ,[PASSDUYETTK]      ,[BATAICHO]      ,[CTCTMOI]
      ,[MAPB]      ,[MANV]      ,[SOHODN]      ,[SONK]      ,[DMNK]      ,[DAIDIEN]      ,[NOIDUNG]
      ,[MST]      ,[SDInfo_INHOADON]      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]
      ,[ISTUYENONGCHUNG]      ,[NGAYSINH]      ,[CAPNGAY]      ,[TAI]      ,[MAHTTT]      ,[NOILAPDHHN]
      ,[TTNT]      ,[DONVICAPHN]      ,[MAHN]      ,[NGAYCAPHN]      ,[NGAYKETTHUCHN]
      ,[NGAYKYHN]      ,[KYHOTROHN]      ,[ISHONGHEO]      ,[DIACHINGHEO]      ,[TENCHUCVU]
      ,[NGAYN]      ,[MADPKHGAN]      ,[MADBKHGAN]      ,[SONHA2]      ,[NGAYNHAPHSTRA]
      ,[NGAYDUYETHS]      ,[ISTRAHS]      ,[NGAYDUYETHSTRA]      ,[TIENCOCLX]
      ,[TIENVATTULX]      ,GETDATE()
	  , [SONHANHAPDON2]      ,[TENDUONG]      ,[MAXA]      ,[TENXA]      ,[MAAPTO]
      , [TENAPTO]      ,[SoDienThoai2]      ,[IsKHMuaVatTu]      ,[IsXoaDLM]      ,[GhiChuXoaDLM]      ,[NgayXoaDLM]
		FROM DONDANGKY 
		WHERE MADDK = @Maddk

		update DONDANGKY set TTDK = 'DK_A', TTTK = 'TK_RA', TTCT = 'CT_RA', TTHD = 'HD_RA', TTTC = 'TC_RA', TTNT = 'NT_RA'
			, IsXoaDLM = 1, GhiChuXoaDLM = @GhiChuDLM, NgayXoaDLM = getdate()		
		from DONDANGKY d 
		where d.MADDK = @Maddk
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Delete_DonDangKyPo_ByMaddk]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-12-15
-- Description:	Delete by Maddk
-- =============================================

CREATE PROCEDURE [dbo].[Delete_DonDangKyPo_ByMaddk]	
	@Maddkpo varchar(11),
	@GhiChuDLM nvarchar(1000)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		insert INTO DONDANGKYPO_H ([MADDKPO]      ,[MADDKTONG]      ,[TENKH]      ,[DIACHILD]      ,[SONHA]      ,[DIENTHOAI]
      ,[CMND]      ,[MAPB]      ,[MANV]      ,[MADPPO]      ,[DUONGPHUPO]      ,[MAPHUONG]      ,[MAKVPO]      ,[TEN_DC_KHAC]
      ,[MAMDSDPO]      ,[NGAYDK]      ,[NGAYHKS]      ,[TTDK]      ,[TTTK]      ,[TTCT]      ,[TTHD]      ,[TTTC]
      ,[TTHC]      ,[TTNT]      ,[LOAIDK]      ,[TENDK]      ,[DACBIET]      ,[NGAYCD]      ,[NGAYKS]      ,[DTNGOAI]
      ,[PASSDUYETTK]      ,[BATAICHO]      ,[CTCTMOI]      ,[SOHODN]      ,[SONK]      ,[DMNK]      ,[DAIDIEN]
      ,[NOIDUNG]      ,[MST]      ,[SDInfo_INHOADON]      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[ISTUYENONGCHUNG]
      ,[NGAYSINH]      ,[CAPNGAY]      ,[TAI]      ,[MAHTTT]      ,[NOILAPDHHN]      ,[SOTRUPO]      ,[NGAYN]

      ,[NGAYNHAPHIS]      ,[IsKHMuaVatTu]      ,[IsXoaDLM]      ,[GhiChuXoaDLM]      ,[NgayXoaDLM] )
		SELECT [MADDKPO]      ,[MADDKTONG]      ,[TENKH]      ,[DIACHILD]      ,[SONHA]      ,[DIENTHOAI]
      ,[CMND]      ,[MAPB]      ,[MANV]      ,[MADPPO]      ,[DUONGPHUPO]      ,[MAPHUONG]      ,[MAKVPO]      ,[TEN_DC_KHAC]
      ,[MAMDSDPO]      ,[NGAYDK]      ,[NGAYHKS]      ,[TTDK]      ,[TTTK]      ,[TTCT]      ,[TTHD]      ,[TTTC]
      ,[TTHC]      ,[TTNT]      ,[LOAIDK]      ,[TENDK]      ,[DACBIET]      ,[NGAYCD]      ,[NGAYKS]      ,[DTNGOAI]
      ,[PASSDUYETTK]      ,[BATAICHO]      ,[CTCTMOI]      ,[SOHODN]      ,[SONK]      ,[DMNK]      ,[DAIDIEN]
      ,[NOIDUNG]      ,[MST]      ,[SDInfo_INHOADON]      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[ISTUYENONGCHUNG]
      ,[NGAYSINH]      ,[CAPNGAY]      ,[TAI]      ,[MAHTTT]      ,[NOILAPDHHN]      ,[SOTRUPO]      ,[NGAYN]

      ,getdate()     ,[IsKHMuaVatTu]      ,[IsXoaDLM]      ,[GhiChuXoaDLM]      ,[NgayXoaDLM]    
		FROM DONDANGKYPO
		WHERE MADDKPO = @Maddkpo

		update DONDANGKYPO set TTDK = 'DK_A', TTTK = 'TK_RA', TTCT = 'CT_RA', TTHD = 'HD_RA', TTTC = 'TC_RA', TTNT = 'NT_RA'
			, IsXoaDLM = 1, GhiChuXoaDLM = @GhiChuDLM, NgayXoaDLM = getdate()		
		from DONDANGKYPO d 
		where d.MADDKPO = @Maddkpo
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Delete_MauNhanVien]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2021-01-20
-- Description:	Delete Table
-- =============================================

create PROCEDURE [dbo].[Delete_MauNhanVien]	
	@Id int,	
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		insert into MauNhanVienHis ([Id]      ,[KhuVucId]      ,[ServiceId]      ,[TenMauNhanVien]      ,[GhiChu]
      ,[SortOrder]      ,[Status]      ,[Active]      ,[CreateDate]      ,[CreateBy]      ,[UpdateDate]      ,[UpdateBy]
      ,[NgayNhapHis], [Manvxoa]     )		
		select [Id]      ,[KhuVucId]      ,[ServiceId]      ,[TenMauNhanVien]      ,[GhiChu]
      ,[SortOrder]      ,[Status]      ,[Active]      ,[CreateDate]      ,[CreateBy]      ,[UpdateDate]      ,[UpdateBy]
	  , getdate(), @Manv
		from MauNhanVien ma
		where ma.Id = @Id


		delete from MauNhanVien 
		where Id = @Id

		select 'Ok' as KetQua	
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Delete_MauNhanVienChiTiet]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2021-01-21
-- Description:	Delete Table
-- =============================================

create PROCEDURE [dbo].[Delete_MauNhanVienChiTiet]	
	@Id int,	
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		insert into MauNhanVienChiTietHis ( [Id]      ,[MauNhanVienId]      ,[NhanVienId]      ,[NhanVienHoTen]      ,[GhiChu]
      ,[SortOrder]      ,[Status]      ,[Active]      ,[CreateDate]      ,[CreateBy]      ,[UpdateDate]
      ,[UpdateBy]      ,[NgayNhapHis]      ,[ManvXoa]  )		
		select [Id]      ,[MauNhanVienId]      ,[NhanVienId]      ,[NhanVienHoTen]      ,[GhiChu]
      ,[SortOrder]      ,[Status]      ,[Active]      ,[CreateDate]      ,[CreateBy]      ,[UpdateDate]
      ,[UpdateBy]       , getdate(), @Manv
		from MauNhanVienChiTiet ma
		where ma.Id = @Id


		delete from MauNhanVienChiTiet
		where Id = @Id

		select 'Ok' as KetQua	
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_ApTo_ByAll]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-25
-- Description:	Get by Id Service
-- =============================================

create PROCEDURE [dbo].[Get_ApTo_ByAll]	
	@Id int
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT [MAAPTO]
      ,[MAXA]
      ,[MAKV]
      ,[TENAPTO]
      ,[STT]
      ,[Status]
      ,[Active]
      ,[CreateBy]
      ,[CreateDate]
      ,[UpdateBy]
      ,[UpdateDate]
	  FROM [dbo].[APTO] t
	  Order by t.TENAPTO	
		
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_ApTo_ByPhuongXaId]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-24
-- Description:	Get by Id Service
-- =============================================

create PROCEDURE [dbo].[Get_ApTo_ByPhuongXaId]	
	@PhuongXaId varchar(10)
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT a.[MAAPTO]
		  ,a.[MAXA]
		  ,a.[MAKV]
		  ,a.[TENAPTO]
		  ,a.[STT]
		  ,a.[Status]
		  ,a.[Active]
		  ,a.[CreateBy]
		  ,a.[CreateDate]
		  ,a.[UpdateBy]
		  ,a.[UpdateDate]
		FROM [dbo].[APTO] a
		where a.MAXA = @PhuongXaId
		Order by a.TENAPTO	
		
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_ApTo_ByTinhThanhPhoId]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-24
-- Description:	Get by Id Service
-- =============================================

create PROCEDURE [dbo].[Get_ApTo_ByTinhThanhPhoId]	
	@TinhThanhPhoId int
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT a.[MAAPTO]
		  ,a.[MAXA]
		  ,a.[MAKV]
		  ,a.[TENAPTO]
		  ,a.[STT]
		  ,a.[Status]
		  ,a.[Active]
		  ,a.[CreateBy]
		  ,a.[CreateDate]
		  ,a.[UpdateBy]
		  ,a.[UpdateDate]
		FROM [dbo].[APTO] a inner join PhuongXa px on a.MAXA = Convert(int, px.Id)
			inner join QuanHuyen qh on px.QuanHuyenId = qh.Id
		where qh.ThanhPhoTinhId = @TinhThanhPhoId
		Order by a.TENAPTO	
		
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_DonDangKy_ByMaddk]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-12-15
-- Description:	Get by Maddk
-- =============================================

CREATE PROCEDURE [dbo].[Get_DonDangKy_ByMaddk]	
	@Maddk varchar(11)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		select		d.[MADDK]
					,d.[MADDKTONG]
					,d.[TENKH]
					,d.[DIACHILD]
					,d.[SONHA]
					,d.[DIENTHOAI]
					,d.[SoDienThoai2]
					,d.[CMND]
					,d.[MADP]
					,d.[DUONGPHU]
					,d.[MAPHUONG]
					,d.[MAKV]
					,d.[TEN_DC_KHAC]
					,d.[MAMDSD]
					,d.[NGAYDK]
					,d.[NGAYHKS]
					,d.[TTDK]
					,d.[TTTK]
					,d.[TTCT]
					,d.[TTHD]
					,d.[TTTC]
					,d.[TTHC]
					,d.[LOAIDK]
					,d.[TENDK]
					,d.[DACBIET]
					,d.[NGAYCD]
					,d.[NGAYKS]
					,d.[DTNGOAI]
					,d.[PASSDUYETTK]
					,d.[BATAICHO]
					,d.[CTCTMOI]
					,d.[MAPB]
					,d.[MANV]
					,d.[SOHODN]
					,d.[SONK]
					,d.[DMNK]
					,d.[DAIDIEN]
					,d.[NOIDUNG]
					,d.[MST]
					,d.[SDInfo_INHOADON]
					,d.[TENKH_INHOADON]
					,d.[DIACHI_INHOADON]
					,d.[ISTUYENONGCHUNG]
					,d.[NGAYSINH]
					,d.[CAPNGAY]
					,d.[TAI]
					,d.[MAHTTT]
					,d.[NOILAPDHHN]
					,d.[TTNT]
					,d.[DONVICAPHN]
					,d.[MAHN]
					,d.[NGAYCAPHN]
					,d.[NGAYKETTHUCHN]
					,d.[NGAYKYHN]
					,d.[KYHOTROHN]
					,d.[ISHONGHEO]
					,d.[DIACHINGHEO]
					,d.[TENCHUCVU]
					,d.[NGAYN]
					,d.[MADPKHGAN]
					,d.[MADBKHGAN]
					,d.[SONHA2]
					,d.[NGAYNHAPHSTRA]
					,d.[NGAYDUYETHS]
					,d.[ISTRAHS]
					,d.[NGAYDUYETHSTRA]
					,d.[TIENCOCLX]
					,d.[TIENVATTULX]
					,d.[SONHANHAPDON2]
					,d.[TENDUONG]
					,d.[MAXA]
					,d.[TENXA]
					,d.[MAAPTO]
					,d.[TENAPTO]
					,d.[IsKHMuaVatTu]
					,d.[IsXoaDLM]
					,d.[GhiChuXoaDLM]
		from DONDANGKY d
		where d.MADDK = @Maddk
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_DSTongHopKHDK_ByDuyetCTKoTC]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-12-15
-- Description:	Get Table
-- =============================================

CREATE PROCEDURE [dbo].[Get_DSTongHopKHDK_ByDuyetCTKoTC]	
	@MaKV varchar(10),
	@MaPB varchar(10),
	@TuNgay smalldatetime,
	@DenNgay smalldatetime
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		IF @MaPB='%'
		BEGIN
			select d.[MADDK]				 
				  ,d.[TENKH]
				  ,d.[SONHA]
				  ,d.[DIACHILD]				 
				  ,d.[DIENTHOAI]
				  ,d.[SoDienThoai2]
				  ,d.[CMND]
				  ,d.[MADP]			 
				  ,d.[MAKV]
				  ,d.[TEN_DC_KHAC]
				  ,d.[MAMDSD]				  
			from DONDANGKY d
			where d.MAKV = @MaKV
				and d.TTCT = 'CT_A' and d.TTTC != 'TC_A'
		END
		ELSE
		BEGIN
			select d.[MADDK]				 
				  ,d.[TENKH]
				  ,d.[SONHA]
				  ,d.[DIACHILD]				 
				  ,d.[DIENTHOAI]
				  ,d.[SoDienThoai2]
				  ,d.[CMND]
				  ,d.[MADP]			 
				  ,d.[MAKV]
				  ,d.[TEN_DC_KHAC]
				  ,d.[MAMDSD]				  
			from DONDANGKY d INNER JOIN DUYET_QUYEN DQ ON DQ.MADDK = D.MADDK AND DQ.MAPB = @MAPB
			where d.MAKV = @MaKV
				and d.TTCT = 'CT_A' and d.TTTC != 'TC_A'
		END
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_DSTongHopKHDK_ByDuyetCTKoTCPo]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-12-15
-- Description:	Get Table
-- =============================================

create PROCEDURE [dbo].[Get_DSTongHopKHDK_ByDuyetCTKoTCPo]	
	@MaKV varchar(10),
	@MaPB varchar(10),
	@TuNgay smalldatetime,
	@DenNgay smalldatetime
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		IF @MaPB='%'
		BEGIN
			select d.[MADDKPO]				 
				  ,d.[TENKH]
				  ,d.[SONHA]
				  ,d.[DIACHILD]				 
				  ,d.[DIENTHOAI]				 
				  ,d.[CMND]
				  ,d.[MADPPO]			 
				  ,d.[MAKVPO]
				  ,d.[TEN_DC_KHAC]
				  ,d.[MAMDSDPO]				  
			from DONDANGKYPO d
			where d.MAKVPO = @MaKV
				and d.TTCT = 'CT_A' and d.TTTC != 'TC_A'
		END
		ELSE
		BEGIN
			select d.[MADDKPO]				 
				  ,d.[TENKH]
				  ,d.[SONHA]
				  ,d.[DIACHILD]				 
				  ,d.[DIENTHOAI]				 
				  ,d.[CMND]
				  ,d.[MADPPO]			 
				  ,d.[MAKVPO]
				  ,d.[TEN_DC_KHAC]
				  ,d.[MAMDSDPO]				  
			from DONDANGKYPO d INNER JOIN DUYET_QUYEN DQ ON DQ.MADDK = D.MADDKPO AND DQ.MAPB = @MAPB
			where d.MAKVPO = @MaKV
				and d.TTCT = 'CT_A' and d.TTTC != 'TC_A'
		END
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_DSTongHopKHDK_ByKVToDung7Ngay]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-09-14
-- Description:	Get Table
-- =============================================

CREATE PROCEDURE [dbo].[Get_DSTongHopKHDK_ByKVToDung7Ngay]	
	@MaKV varchar(10),
	@MaPB varchar(10),
	@TuNgay smalldatetime,
	@DenNgay smalldatetime
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		IF @MaPB='%'
		BEGIN
			CREATE TABLE [dbo].[#temp]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]

			insert into #temp(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)
			SELECT D.MADDK, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYDTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK=D.MADDK), -- 
				(SELECT NGAYTAO FROM HOPDONG WHERE MADDK=D.MADDK),
				(SELECT NGAYHT FROM THICONG WHERE MADDK=D.MADDK), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHU WHERE MADDK=D.MADDK)
			FROM DONDANGKY D 
			WHERE D.MAKV = @MaKV 				
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)			
				
		--	[Get_DSTongHopKHDK_ByKVToDung7Ngay]	'M','%','2020-6-1','2020-12-1'        

			select t.MADDK, t.TENKH,
				CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,
				CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETDONDANGKY' order by hi.NGAYN) as SoNgayDK
				, d.NOIDUNG  as GhiChuDK				
				, CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE
				, CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETTHIETKE' order by hi.NGAYN)  as SoNgayTK
				, tk.CHUTHICH as GhiChuTK				
				, CONVERT(VARCHAR(20), CHIETTINH, 103) AS CHIETTINH
				, case when (select SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETCTKH') is null then 1 else
					(select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETCTKH' order by hi.NGAYN) end
					as SoNgayCT
				, ct.GHICHU as GhiChuCT
				, CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'INHOPDONG' order by hi.NGAYN) as SoNgayHD
				, hd.GhiChu as GhiChuHD
				, CONVERT(VARCHAR(20),THICONG,103)AS THICONG
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETTHICONG' order by hi.NGAYN) as SoNgayTC
				, tc.GHICHU as GhiChuTC				
				, CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'INBBNT' order by hi.NGAYN) as SoNgayNT
				, nt.GHICHU as GhiChu
				, ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) as TongNgay
			from #temp t inner join DONDANGKY d on t.MADDK = d.MADDK
				inner join THIETKE tk on t.MADDK = tk.MADDK
				inner join CHIETTINH ct on t.MADDK = ct.MADDK
				inner join HOPDONG hd on t.MADDK = hd.MADDK
				inner join THICONG tc on t.MADDK = tc.MADDK
				inner join BBNGHIEMTHU nt on t.MADDK = nt.MADDK
			where ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) < 8
			ORDER BY MADDK			
			 
			drop table #temp 
		END
		ELSE
		BEGIN
			CREATE TABLE [dbo].[#tempMAPB]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]
			
			insert into #tempMAPB(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)
			SELECT D.MADDK, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYDTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK=D.MADDK), -- 
				(SELECT NGAYTAO FROM HOPDONG WHERE MADDK=D.MADDK),
				(SELECT NGAYHT FROM THICONG WHERE MADDK=D.MADDK), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHU WHERE MADDK=D.MADDK)
			FROM DONDANGKY D INNER JOIN DUYET_QUYEN DQ ON DQ.MADDK=D.MADDK
			WHERE D.MAKV = @MaKV AND DQ.MAPB=@MaPB			
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)	
		
			select t.MADDK, t.TENKH,
				CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,
				CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETDONDANGKY' order by hi.NGAYN) as SoNgayDK
				, d.NOIDUNG  as GhiChuDK				
				, CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE
				, CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETTHIETKE' order by hi.NGAYN)  as SoNgayTK
				, tk.CHUTHICH as GhiChuTK				
				, CONVERT(VARCHAR(20), CHIETTINH, 103) AS CHIETTINH
				, case when (select SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETCTKH') is null then 1 else
					(select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETCTKH' order by hi.NGAYN) end
					as SoNgayCT
				, ct.GHICHU as GhiChuCT
				, CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'INHOPDONG' order by hi.NGAYN) as SoNgayHD
				, hd.GhiChu as GhiChuHD
				, CONVERT(VARCHAR(20),THICONG,103)AS THICONG
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETTHICONG' order by hi.NGAYN) as SoNgayTC
				, tc.GHICHU as GhiChuTC				
				, CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'INBBNT' order by hi.NGAYN) as SoNgayNT
				, nt.GHICHU as GhiChu
				, ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) as TongNgay
			from #tempMAPB t inner join DONDANGKY d on t.MADDK = d.MADDK
				inner join THIETKE tk on t.MADDK = tk.MADDK
				inner join CHIETTINH ct on t.MADDK = ct.MADDK
				inner join HOPDONG hd on t.MADDK = hd.MADDK
				inner join THICONG tc on t.MADDK = tc.MADDK
				inner join BBNGHIEMTHU nt on t.MADDK = nt.MADDK
			where ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) < 8
			ORDER BY MADDK			
			 
			drop table #tempMAPB 
		END
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_DSTongHopKHDK_ByKVToDung7NgayPo]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-12-14
-- Description:	Get Table
-- =============================================

CREATE PROCEDURE [dbo].[Get_DSTongHopKHDK_ByKVToDung7NgayPo]	
	@MaKV varchar(10),
	@MaPB varchar(10),
	@TuNgay smalldatetime,
	@DenNgay smalldatetime
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		IF @MaPB='%'
		BEGIN
			CREATE TABLE [dbo].[#temp]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]

			insert into #temp(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)
			SELECT D.MADDKPO, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYDTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK = D.MADDKPO), -- 
				(SELECT NGAYTAO FROM HOPDONGPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYHT FROM THICONG WHERE MADDK = D.MADDKPO), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHUPO WHERE MADDKPO = D.MADDKPO)
			FROM DONDANGKYPO D 
			WHERE D.MAKVPO = @MaKV 				
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)			
				
		--	[Get_DSTongHopKHDK_ByKVToDung7Ngay]	'M','%','2020-6-1','2020-12-1'        

			select t.MADDK, t.TENKH,
				CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,
				CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETDONDANGKY' order by hi.NGAYN) as SoNgayDK
				, d.NOIDUNG  as GhiChuDK				
				, CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE
				, CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETTHIETKE' order by hi.NGAYN)  as SoNgayTK
				, tk.CHUTHICH as GhiChuTK				
				, CONVERT(VARCHAR(20), CHIETTINH, 103) AS CHIETTINH
				, case when (select SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETCTKH') is null then 1 else
					(select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETCTKH' order by hi.NGAYN) end
					as SoNgayCT
				, ct.GHICHU as GhiChuCT
				, CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'INHOPDONG' order by hi.NGAYN) as SoNgayHD
				, hd.GhiChu as GhiChuHD
				, CONVERT(VARCHAR(20),THICONG,103)AS THICONG
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETTHICONG' order by hi.NGAYN) as SoNgayTC
				, tc.GHICHU as GhiChuTC				
				, CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'INBBNT' order by hi.NGAYN) as SoNgayNT
				, nt.GHICHU as GhiChu
				, ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) as TongNgay
			from #temp t inner join DONDANGKYPO d on t.MADDK = d.MADDKPO
				inner join THIETKEPO tk on t.MADDK = tk.MADDKPO
				inner join CHIETTINH ct on t.MADDK = ct.MADDK
				inner join HOPDONGPO hd on t.MADDK = hd.MADDKPO
				inner join THICONG tc on t.MADDK = tc.MADDK
				inner join BBNGHIEMTHUPO nt on t.MADDK = nt.MADDKPO
			where ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) < 8
			ORDER BY MADDK			
			 
			drop table #temp 
		END
		ELSE
		BEGIN
			CREATE TABLE [dbo].[#tempMAPB]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]
			
			insert into #tempMAPB(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)

			SELECT D.MADDKPO, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYDTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK = D.MADDKPO), -- 
				(SELECT NGAYTAO FROM HOPDONGPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYHT FROM THICONG WHERE MADDK = D.MADDKPO), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHUPO WHERE MADDKPO = D.MADDKPO)
			FROM DONDANGKYPO D INNER JOIN DUYET_QUYEN DQ ON DQ.MADDK = D.MADDKPO
			WHERE D.MAKVPO = @MaKV AND DQ.MAPB=@MaPB			
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)				
		
			select t.MADDK, t.TENKH,
				CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,
				CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETDONDANGKY' order by hi.NGAYN) as SoNgayDK
				, d.NOIDUNG  as GhiChuDK				
				, CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE
				, CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETTHIETKE' order by hi.NGAYN)  as SoNgayTK
				, tk.CHUTHICH as GhiChuTK				
				, CONVERT(VARCHAR(20), CHIETTINH, 103) AS CHIETTINH
				, case when (select SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETCTKH') is null then 1 else
					(select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETCTKH' order by hi.NGAYN) end
					as SoNgayCT
				, ct.GHICHU as GhiChuCT
				, CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'INHOPDONG' order by hi.NGAYN) as SoNgayHD
				, hd.GhiChu as GhiChuHD
				, CONVERT(VARCHAR(20),THICONG,103)AS THICONG
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETTHICONG' order by hi.NGAYN) as SoNgayTC
				, tc.GHICHU as GhiChuTC				
				, CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'INBBNT' order by hi.NGAYN) as SoNgayNT
				, nt.GHICHU as GhiChu
				, ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) as TongNgay
			from #tempMAPB t inner join DONDANGKYPO d on t.MADDK = d.MADDKPO
				inner join THIETKEPO tk on t.MADDK = tk.MADDKPO
				inner join CHIETTINH ct on t.MADDK = ct.MADDK
				inner join HOPDONGPO hd on t.MADDK = hd.MADDKPO
				inner join THICONG tc on t.MADDK = tc.MADDK
				inner join BBNGHIEMTHUPO nt on t.MADDK = nt.MADDKPO
			where ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) < 8
			ORDER BY MADDK			
			 
			drop table #tempMAPB 
		END
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_DSTongHopKHDK_ByKVToNgay]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-09-14
-- Description:	Get Table
-- =============================================

CREATE PROCEDURE [dbo].[Get_DSTongHopKHDK_ByKVToNgay]	--	[dbo].[Get_DSTongHopKHDK_ByKVToNgay] 'M', '%', '2020/08/23 12:00:00', '2020/10/23 12:00:00'
	@MaKV varchar(10),
	@MaPB varchar(10),
	@TuNgay smalldatetime,
	@DenNgay smalldatetime
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		IF @MaPB='%'
		BEGIN
			CREATE TABLE [dbo].[#temp]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]

			insert into #temp(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)
			SELECT D.MADDK, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYDTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK=D.MADDK), -- 
				(SELECT NGAYTAO FROM HOPDONG WHERE MADDK=D.MADDK),
				(SELECT NGAYHT FROM THICONG WHERE MADDK=D.MADDK), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHU WHERE MADDK=D.MADDK)
			FROM DONDANGKY D 
			WHERE D.MAKV = @MaKV 				
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)	
				--	[dbo].[Get_DSTongHopKHDK_ByKVToNgay] 'M', '%', '23/08/2020 12:00:00', '23/10/2020 12:00:00'
		
			select MADDK,TENKH,CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON,
				CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE,CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK,
				CONVERT(VARCHAR(20),CHIETTINH,103)AS CHIETTINH,CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG,
				CONVERT(VARCHAR(20),THICONG,103)AS THICONG,CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
			from #temp
			ORDER BY MADDK			
			 
			drop table #temp 
		END
		ELSE
		BEGIN
--	DSTongHopDDK '2017/01/01','2017/05/01','X','','','','DSTHDDKN'	
			CREATE TABLE [dbo].[#tempMAPB]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]
			
			insert into #tempMAPB(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)
			SELECT D.MADDK, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYDTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK=D.MADDK), -- 
				(SELECT NGAYTAO FROM HOPDONG WHERE MADDK=D.MADDK),
				(SELECT NGAYHT FROM THICONG WHERE MADDK=D.MADDK), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHU WHERE MADDK=D.MADDK)
			FROM DONDANGKY D INNER JOIN DUYET_QUYEN DQ ON DQ.MADDK=D.MADDK AND DQ.MAPB=@MaPB
			WHERE D.MAKV = @MaKV 				
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)	
		
			select MADDK,TENKH,CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON,
				CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE,CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK,
				CONVERT(VARCHAR(20),CHIETTINH,103)AS CHIETTINH,CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG,
				CONVERT(VARCHAR(20),THICONG,103)AS THICONG,CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
			from #tempMAPB
			ORDER BY MADDK			
			 
			drop table #tempMAPB 
		END

			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_DSTongHopKHDK_ByKVToTre7Ngay]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-09-14
-- Description:	Get Table
-- =============================================

CREATE PROCEDURE [dbo].[Get_DSTongHopKHDK_ByKVToTre7Ngay]	
	@MaKV varchar(10),
	@MaPB varchar(10),
	@TuNgay smalldatetime,
	@DenNgay smalldatetime
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		IF @MaPB='%'
		BEGIN
			CREATE TABLE [dbo].[#temp]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]

			insert into #temp(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)
			SELECT D.MADDK, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYDTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK=D.MADDK), -- 
				(SELECT NGAYTAO FROM HOPDONG WHERE MADDK=D.MADDK),
				(SELECT NGAYHT FROM THICONG WHERE MADDK=D.MADDK), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHU WHERE MADDK=D.MADDK)
			FROM DONDANGKY D 
			WHERE D.MAKV = @MaKV 				
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)					
		
			select t.MADDK, t.TENKH,
				CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,
				CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETDONDANGKY' order by hi.NGAYN) as SoNgayDK
				, d.NOIDUNG  as GhiChuDK				
				, CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE
				, CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETTHIETKE' order by hi.NGAYN)  as SoNgayTK
				, tk.CHUTHICH as GhiChuTK				
				, CONVERT(VARCHAR(20), CHIETTINH, 103) AS CHIETTINH
				, case when (select SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETCTKH') is null then 1 else
					(select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETCTKH' order by hi.NGAYN) end
					as SoNgayCT
				, ct.GHICHU as GhiChuCT
				, CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'INHOPDONG' order by hi.NGAYN) as SoNgayHD
				, hd.GhiChu as GhiChuHD
				, CONVERT(VARCHAR(20),THICONG,103)AS THICONG
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETTHICONG' order by hi.NGAYN) as SoNgayTC
				, tc.GHICHU as GhiChuTC				
				, CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'INBBNT' order by hi.NGAYN) as SoNgayNT
				, nt.GHICHU as GhiChu
				, ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) as TongNgay
			from #temp t inner join DONDANGKY d on t.MADDK = d.MADDK
				inner join THIETKE tk on t.MADDK = tk.MADDK
				inner join CHIETTINH ct on t.MADDK = ct.MADDK
				inner join HOPDONG hd on t.MADDK = hd.MADDK
				inner join THICONG tc on t.MADDK = tc.MADDK
				inner join BBNGHIEMTHU nt on t.MADDK = nt.MADDK
			where ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) > 7
			ORDER BY MADDK			
			 
			drop table #temp 
		END
		ELSE
		BEGIN
			CREATE TABLE [dbo].[#tempMAPB]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]
			
			insert into #tempMAPB(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)
			SELECT D.MADDK, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYDTK FROM THIETKE WHERE MADDK=D.MADDK),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK=D.MADDK), -- 
				(SELECT NGAYTAO FROM HOPDONG WHERE MADDK=D.MADDK),
				(SELECT NGAYHT FROM THICONG WHERE MADDK=D.MADDK), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHU WHERE MADDK=D.MADDK)
			FROM DONDANGKY D INNER JOIN DUYET_QUYEN DQ ON DQ.MADDK=D.MADDK AND DQ.MAPB=@MaPB
			WHERE D.MAKV = @MaKV 				
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)	
		
			select t.MADDK, t.TENKH,
				CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,
				CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETDONDANGKY' order by hi.NGAYN) as SoNgayDK
				, d.NOIDUNG  as GhiChuDK				
				, CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE
				, CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETTHIETKE' order by hi.NGAYN)  as SoNgayTK
				, tk.CHUTHICH as GhiChuTK				
				, CONVERT(VARCHAR(20), CHIETTINH, 103) AS CHIETTINH
				, case when (select SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETCTKH') is null then 1 else
					(select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETCTKH' order by hi.NGAYN) end
					as SoNgayCT
				, ct.GHICHU as GhiChuCT
				, CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'INHOPDONG' order by hi.NGAYN) as SoNgayHD
				, hd.GhiChu as GhiChuHD
				, CONVERT(VARCHAR(20),THICONG,103)AS THICONG
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'DUYETTHICONG' order by hi.NGAYN) as SoNgayTC
				, tc.GHICHU as GhiChuTC				
				, CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
				, (select top 1 SoNgay From HISNGAYDANGKY hi where hi.MADDK = t.MADDK and MOTATTDON = 'INBBNT' order by hi.NGAYN) as SoNgayNT
				, nt.GHICHU as GhiChu
				, ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) as TongNgay
			from #tempMAPB t inner join DONDANGKY d on t.MADDK = d.MADDK
				inner join THIETKE tk on t.MADDK = tk.MADDK
				inner join CHIETTINH ct on t.MADDK = ct.MADDK
				inner join HOPDONG hd on t.MADDK = hd.MADDK
				inner join THICONG tc on t.MADDK = tc.MADDK
				inner join BBNGHIEMTHU nt on t.MADDK = nt.MADDK
			where ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) > 7
			ORDER BY MADDK			
			 
			drop table #tempMAPB 
		END
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_DSTongHopKHDK_ByKVToTre7NgayPo]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-12-14
-- Description:	Get Table
-- =============================================
CREATE PROCEDURE [dbo].[Get_DSTongHopKHDK_ByKVToTre7NgayPo]	
	@MaKV varchar(10),
	@MaPB varchar(10),
	@TuNgay smalldatetime,
	@DenNgay smalldatetime
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		IF @MaPB='%'
		BEGIN
			CREATE TABLE [dbo].[#temp]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]

			insert into #temp(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)
			SELECT D.MADDKPO, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYDTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK = D.MADDKPO), -- 
				(SELECT NGAYTAO FROM HOPDONGPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYHT FROM THICONG WHERE MADDK = D.MADDKPO), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHUPO WHERE MADDKPO = D.MADDKPO)
			FROM DONDANGKYPO D 
			WHERE D.MAKVPO = @MaKV 				
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)			
				
		--	[Get_DSTongHopKHDK_ByKVToDung7Ngay]	'M','%','2020-6-1','2020-12-1'        

			select t.MADDK, t.TENKH,
				CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,
				CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETDONDANGKY' order by hi.NGAYN) as SoNgayDK
				, d.NOIDUNG  as GhiChuDK				
				, CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE
				, CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETTHIETKE' order by hi.NGAYN)  as SoNgayTK
				, tk.CHUTHICH as GhiChuTK				
				, CONVERT(VARCHAR(20), CHIETTINH, 103) AS CHIETTINH
				, case when (select SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETCTKH') is null then 1 else
					(select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETCTKH' order by hi.NGAYN) end
					as SoNgayCT
				, ct.GHICHU as GhiChuCT
				, CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'INHOPDONG' order by hi.NGAYN) as SoNgayHD
				, hd.GhiChu as GhiChuHD
				, CONVERT(VARCHAR(20),THICONG,103)AS THICONG
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETTHICONG' order by hi.NGAYN) as SoNgayTC
				, tc.GHICHU as GhiChuTC				
				, CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'INBBNT' order by hi.NGAYN) as SoNgayNT
				, nt.GHICHU as GhiChu
				, ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) as TongNgay
			from #temp t inner join DONDANGKYPO d on t.MADDK = d.MADDKPO
				inner join THIETKEPO tk on t.MADDK = tk.MADDKPO
				inner join CHIETTINH ct on t.MADDK = ct.MADDK
				inner join HOPDONGPO hd on t.MADDK = hd.MADDKPO
				inner join THICONG tc on t.MADDK = tc.MADDK
				inner join BBNGHIEMTHUPO nt on t.MADDK = nt.MADDKPO
			where ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) > 7
			ORDER BY MADDK			
			 
			drop table #temp 
		END
		ELSE
		BEGIN
			CREATE TABLE [dbo].[#tempMAPB]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]
			
			insert into #tempMAPB(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)

			SELECT D.MADDKPO, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYDTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK = D.MADDKPO), -- 
				(SELECT NGAYTAO FROM HOPDONGPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYHT FROM THICONG WHERE MADDK = D.MADDKPO), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHUPO WHERE MADDKPO = D.MADDKPO)
			FROM DONDANGKYPO D INNER JOIN DUYET_QUYEN DQ ON DQ.MADDK = D.MADDKPO
			WHERE D.MAKVPO = @MaKV AND DQ.MAPB=@MaPB			
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)				
		
			select t.MADDK, t.TENKH,
				CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,
				CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETDONDANGKY' order by hi.NGAYN) as SoNgayDK
				, d.NOIDUNG  as GhiChuDK				
				, CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE
				, CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETTHIETKE' order by hi.NGAYN)  as SoNgayTK
				, tk.CHUTHICH as GhiChuTK				
				, CONVERT(VARCHAR(20), CHIETTINH, 103) AS CHIETTINH
				, case when (select SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETCTKH') is null then 1 else
					(select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETCTKH' order by hi.NGAYN) end
					as SoNgayCT
				, ct.GHICHU as GhiChuCT
				, CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'INHOPDONG' order by hi.NGAYN) as SoNgayHD
				, hd.GhiChu as GhiChuHD
				, CONVERT(VARCHAR(20),THICONG,103)AS THICONG
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'DUYETTHICONG' order by hi.NGAYN) as SoNgayTC
				, tc.GHICHU as GhiChuTC				
				, CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
				, (select top 1 SoNgay From HISNGAYDANGKYPO hi where hi.MADDKPO = t.MADDK and MOTATTDON = 'INBBNT' order by hi.NGAYN) as SoNgayNT
				, nt.GHICHU as GhiChu
				, ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) as TongNgay
			from #tempMAPB t inner join DONDANGKYPO d on t.MADDK = d.MADDKPO
				inner join THIETKEPO tk on t.MADDK = tk.MADDKPO
				inner join CHIETTINH ct on t.MADDK = ct.MADDK
				inner join HOPDONGPO hd on t.MADDK = hd.MADDKPO
				inner join THICONG tc on t.MADDK = tc.MADDK
				inner join BBNGHIEMTHUPO nt on t.MADDK = nt.MADDKPO
			where ((DATEDIFF(dd, t.NHAPDON, t.NGHIEMTHU) + 1) 
					- (DATEDIFF(wk, t.NHAPDON, t.NGHIEMTHU) * 2) 
					- (CASE WHEN DATENAME(dw, t.NHAPDON) = 'Sunday' THEN 1 ELSE 0 END)
					- (CASE WHEN DATENAME(dw, t.NGHIEMTHU) = 'Saturday' THEN 1 ELSE 0 END)
					) > 7
			ORDER BY MADDK			
			 
			drop table #tempMAPB 
		END
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_DSTongHopKHDKPO_ByKVToNgay]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-10-23
-- Description:	Get Table
-- =============================================

CREATE PROCEDURE [dbo].[Get_DSTongHopKHDKPO_ByKVToNgay]	--	[dbo].[Get_DSTongHopKHDK_ByKVToNgay] 'M', '%', '2020/08/23 12:00:00', '2020/10/23 12:00:00'
	@MaKV varchar(10),
	@MaPB varchar(10),
	@TuNgay smalldatetime,
	@DenNgay smalldatetime
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		IF @MaPB='%'
		BEGIN
			CREATE TABLE [dbo].[#temp]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]

			insert into #temp(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)
			SELECT D.MADDKPO, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYDTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK = D.MADDKPO), -- 
				(SELECT NGAYTAO FROM HOPDONGPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYHT FROM THICONG WHERE MADDK = D.MADDKPO), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHUPO WHERE MADDKPO = D.MADDKPO)
			FROM DONDANGKYPO D 
			WHERE D.MAKVPO = @MaKV 				
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)	
				--	[dbo].[Get_DSTongHopKHDK_ByKVToNgay] 'M', '%', '23/08/2020 12:00:00', '23/10/2020 12:00:00'
		
			select MADDK,TENKH,CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON,
				CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE,CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK,
				CONVERT(VARCHAR(20),CHIETTINH,103)AS CHIETTINH,CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG,
				CONVERT(VARCHAR(20),THICONG,103)AS THICONG,CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
			from #temp
			ORDER BY MADDK			
			 
			drop table #temp 
		END
		ELSE
		BEGIN
--	DSTongHopDDK '2017/01/01','2017/05/01','X','','','','DSTHDDKN'	
			CREATE TABLE [dbo].[#tempMAPB]
			(
				[ID] [int] IDENTITY ,
				[MADDK] [varchar](20)  ,
				[TENKH] [Nvarchar](500)  ,
				[NHAPDON] smalldatetime,[DUYETDON] smalldatetime,[THIETKE] smalldatetime,[DUYETTK] smalldatetime,
				[CHIETTINH] smalldatetime,[HOPDONG] smalldatetime,[THICONG] smalldatetime,[NGHIEMTHU] smalldatetime
			)
			ON [PRIMARY]
			
			insert into #tempMAPB(MADDK,TENKH,NHAPDON,DUYETDON,THIETKE,DUYETTK,CHIETTINH,HOPDONG,
				THICONG,NGHIEMTHU
				)
			SELECT D.MADDKPO, D.TENKH, D.NGAYDK, D.NGAYHKS, --D.NGAYDUYETHS,
				(SELECT NGAYLTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYDTK FROM THIETKEPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYN FROM CHIETTINH WHERE MADDK = D.MADDKPO), -- 
				(SELECT NGAYTAO FROM HOPDONGPO WHERE MADDKPO = D.MADDKPO),
				(SELECT NGAYHT FROM THICONG WHERE MADDK = D.MADDKPO), -- NGAY GAN DONG HO
				(SELECT NGAYLAPBB FROM BBNGHIEMTHUPO WHERE MADDKPO = D.MADDKPO)
			FROM DONDANGKYPO D 
			WHERE D.MAKVPO = @MaKV 				
				and d.NGAYDK >= dateadd(day, -1, @tungay )
				AND D.NGAYDK <= dateadd(day, 1, @denngay)	
		
			select MADDK,TENKH,CONVERT(VARCHAR(20),NHAPDON,103)AS NHAPDON,CONVERT(VARCHAR(20),DUYETDON,103)AS DUYETDON,
				CONVERT(VARCHAR(20),THIETKE,103)AS THIETKE,CONVERT(VARCHAR(20),DUYETTK,103)AS DUYETTK,
				CONVERT(VARCHAR(20),CHIETTINH,103)AS CHIETTINH,CONVERT(VARCHAR(20),HOPDONG,103)AS HOPDONG,
				CONVERT(VARCHAR(20),THICONG,103)AS THICONG,CONVERT(VARCHAR(20),NGHIEMTHU,103)AS NGHIEMTHU
			from #tempMAPB
			ORDER BY MADDK			
			 
			drop table #tempMAPB 
		END

			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_DuongPho_ByAll]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2020-09-10
-- Description:	Get Table
-- =============================================

create PROCEDURE [dbo].[Get_DuongPho_ByAll]		
	
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		select dp.MADP, dp.DUONGPHU, dp.TENDP, dp.MAKV, kv.TENKV
			, dp.IDMADOTIN, dp.DOT
		from DUONGPHO dp inner join KHUVUC kv on dp.MAKV = kv.MAKV

			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_HisNgayDangKy_ByMaDDKMoTaTTDON]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-11-04
-- Description:	Get Table one row
-- =============================================

CREATE PROCEDURE [dbo].[Get_HisNgayDangKy_ByMaDDKMoTaTTDON]	
	@MADDK varchar(11),
	@MOTATTDON varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		declare @countHisNgayDangKy int = (select count(n.MALUOCSU) from HISNGAYDANGKY n
			where n.MADDK = @MADDK and n.MOTATTDON = @MOTATTDON)

		if @countHisNgayDangKy > 0
		begin
			select top 1 n.[MALUOCSU]
			  ,n.[MADDK]
			  ,n.[MAKV]
			  ,n.[TENKH]
			  ,n.[NGAYNHAP]
			  ,n.[NGAYDUYET]
			  ,n.[SoNgay]
			  ,n.[NGAYNHAPTRA]
			  ,n.[NGAYDUYETTRA]
			  ,n.[MOTATTDON]
			  ,n.[GHICHU]
			  ,n.[NGAYN]
			  ,n.[MANV]
			from HISNGAYDANGKY n
			where n.MADDK = @MADDK and n.MOTATTDON = @MOTATTDON
			order by NGAYN
		end
		else begin
			if @MOTATTDON = 'DUYETCTKH'
			begin
				insert into HISNGAYDANGKY ( [MADDK]      ,[MAKV]      ,[TENKH]  ,[NGAYNHAP]   --  ,[NGAYDUYET]  
					,[SoNgay]    		--,[NGAYNHAPTRA]      ,[NGAYDUYETTRA]
				  ,[MOTATTDON]      ,[GHICHU]      ,[NGAYN]      ,[MANV])
				select @MADDK, d.MAKV, d.TENKH		, ct.NGAYN
					, (SELECT (DATEDIFF(dd, tk.NGAYDTK, ct.NGAYN) + 1) 
						- (DATEDIFF(wk, tk.NGAYDTK, ct.NGAYN) * 2) 
						- (CASE WHEN DATENAME(dw, tk.NGAYDTK) = 'Sunday' THEN 1 ELSE 0 END)
						- (CASE WHEN DATENAME(dw, ct.NGAYN) = 'Saturday' THEN 1 ELSE 0 END))
					, 'DUYETCTKH', N'Duyệt chiết tính vật tư', getdate(), 'admin'
				from DONDANGKY d inner join CHIETTINH ct on d.MADDK = ct.MADDK
					inner join THIETKE tk on ct.MADDK = tk.MADDK
				where d.MADDK = @MADDK

				select top 1 n.[MALUOCSU]
				  ,n.[MADDK]
				  ,n.[MAKV]
				  ,n.[TENKH]
				  ,n.[NGAYNHAP]
				  ,n.[NGAYDUYET]
				  ,n.[SoNgay]
				  ,n.[NGAYNHAPTRA]
				  ,n.[NGAYDUYETTRA]
				  ,n.[MOTATTDON]
				  ,n.[GHICHU]
				  ,n.[NGAYN]
				  ,n.[MANV]
				from HISNGAYDANGKY n
				where n.MADDK = @MADDK and n.MOTATTDON = @MOTATTDON
				order by NGAYN

			end
			else begin
				select top 1 n.[MALUOCSU]
				  ,n.[MADDK]
				  ,n.[MAKV]
				  ,n.[TENKH]
				  ,n.[NGAYNHAP]
				  ,n.[NGAYDUYET]
				  ,n.[SoNgay]
				  ,n.[NGAYNHAPTRA]
				  ,n.[NGAYDUYETTRA]
				  ,n.[MOTATTDON]
				  ,n.[GHICHU]
				  ,n.[NGAYN]
				  ,n.[MANV]
				from HISNGAYDANGKY n
				where n.MADDK = @MADDK and n.MOTATTDON = @MOTATTDON
				order by NGAYN
			end			
		end
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-11-17
-- Description:	Get Table one row
-- =============================================

create PROCEDURE [dbo].[Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON]	
	@MADDKPO varchar(11),
	@MOTATTDON varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		declare @countHisNgayDangKy int = (select count(n.MALUOCSUPO) from HISNGAYDANGKYPO n
			where n.MADDKPO = @MADDKPO and n.MOTATTDON = @MOTATTDON)

		if @countHisNgayDangKy > 0
		begin
			select top 1 n.[MALUOCSUPO]
			  ,n.[MADDKPO]
			  ,n.[MAKVPO]
			  ,n.[TENKH]
			  ,n.[NGAYNHAP]
			  ,n.[NGAYDUYET]
			  ,n.[SoNgay]
			  ,n.[NGAYNHAPTRA]
			  ,n.[NGAYDUYETTRA]
			  ,n.[MOTATTDON]
			  ,n.[GHICHU]
			  ,n.[NGAYN]
			  ,n.[MANV]
			from HISNGAYDANGKYPO n
			where n.MADDKPO = @MADDKPO and n.MOTATTDON = @MOTATTDON
			order by NGAYN
		end
		else begin
			if @MOTATTDON = 'DUYETCTKH'
			begin
				insert into HISNGAYDANGKYPO ( [MADDKPO]      ,[MAKVPO]      ,[TENKH]  ,[NGAYNHAP]   --  ,[NGAYDUYET]  
					,[SoNgay]    		--,[NGAYNHAPTRA]      ,[NGAYDUYETTRA]
				  ,[MOTATTDON]      ,[GHICHU]      ,[NGAYN]      ,[MANV])
				select @MADDKPO, d.MAKVPO, d.TENKH		, ct.NGAYN
					, (SELECT (DATEDIFF(dd, tk.NGAYDTK, ct.NGAYN) + 1) 
						- (DATEDIFF(wk, tk.NGAYDTK, ct.NGAYN) * 2) 
						- (CASE WHEN DATENAME(dw, tk.NGAYDTK) = 'Sunday' THEN 1 ELSE 0 END)
						- (CASE WHEN DATENAME(dw, ct.NGAYN) = 'Saturday' THEN 1 ELSE 0 END))
					, 'DUYETCTKH', N'Duyệt chiết tính vật tư', getdate(), 'admin'
				from DONDANGKYPO d inner join CHIETTINH ct on d.MADDKPO = ct.MADDK
					inner join THIETKEPO tk on ct.MADDK = tk.MADDKPO
				where d.MADDKPO = @MADDKPO

				select top 1 n.[MALUOCSUPO]
				  ,n.[MADDKPO]
				  ,n.[MAKVPO]
				  ,n.[TENKH]
				  ,n.[NGAYNHAP]
				  ,n.[NGAYDUYET]
				  ,n.[SoNgay]
				  ,n.[NGAYNHAPTRA]
				  ,n.[NGAYDUYETTRA]
				  ,n.[MOTATTDON]
				  ,n.[GHICHU]
				  ,n.[NGAYN]
				  ,n.[MANV]
				from HISNGAYDANGKYPO n
				where n.MADDKPO = @MADDKPO and n.MOTATTDON = @MOTATTDON
				order by NGAYN

			end
			else begin
				select top 1 n.[MALUOCSUPO]
				  ,n.[MADDKPO]
				  ,n.[MAKVPO]
				  ,n.[TENKH]
				  ,n.[NGAYNHAP]
				  ,n.[NGAYDUYET]
				  ,n.[SoNgay]
				  ,n.[NGAYNHAPTRA]
				  ,n.[NGAYDUYETTRA]
				  ,n.[MOTATTDON]
				  ,n.[GHICHU]
				  ,n.[NGAYN]
				  ,n.[MANV]
				from HISNGAYDANGKYPO n
				where n.MADDKPO = @MADDKPO and n.MOTATTDON = @MOTATTDON
				order by NGAYN
			end			
		end
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_HopDong_ByMaddk]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-11-04
-- Description:	Get by Maddk
-- =============================================

create PROCEDURE [dbo].[Get_HopDong_ByMaddk]	
	@Maddk varchar(11)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		select hd.[MADDK]
			  ,hd.[MADP]
			  ,hd.[DUONGPHU]
			  ,hd.[MADB]
			  ,hd.[LOTRINH]
			  ,hd.[NGAYTAO]
			  ,hd.[NGAYKT]
			  ,hd.[NGAYHL]
			  ,hd.[MAPHUONG]
			  ,hd.[MAKV]
			  ,hd.[CODH]
			  ,hd.[LOAIONG]
			  ,hd.[MAHTTT]
			  ,hd.[MAMDSD]
			  ,hd.[DINHMUCSD]
			  ,hd.[SOHO]
			  ,hd.[SONHANKHAU]
			  ,hd.[LOAIHD]
			  ,hd.[TRANGTHAI]
			  ,hd.[SOHD]
			  ,hd.[CMND]
			  ,hd.[MST]
			  ,hd.[SDInfo_INHOADON]
			  ,hd.[TENKH_INHOADON]
			  ,hd.[DIACHI_INHOADON]
			  ,hd.[DACAPDB]
			  ,hd.[SONHA]
			  ,hd.[NGAYN]
			  ,hd.[GhiChu]
			  ,hd.[Manv]
		from HOPDONG hd
		where hd.MADDK = @Maddk
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_HopDongPo_ByMaddk]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2020-11-17
-- Description:	Get by Maddk
-- =============================================

create PROCEDURE [dbo].[Get_HopDongPo_ByMaddk]	
	@Maddk varchar(11)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		select hd.[MADDKPO]
			  ,hd.[MADPPO]
			  ,hd.[DUONGPHUPO]
			  ,hd.[MADB]
			  ,hd.[LOTRINH]
			  ,hd.[NGAYTAO]
			  ,hd.[NGAYKT]
			  ,hd.[NGAYHL]
			  ,hd.[SONHA]
			  ,hd.[MAPHUONGPO]
			  ,hd.[MAKVPO]
			  ,hd.[CODH]
			  ,hd.[LOAIONG]
			  ,hd.[MAHTTT]
			  ,hd.[MAMDSDPO]
			  ,hd.[DINHMUCSD]
			  ,hd.[SOHO]
			  ,hd.[SONHANKHAU]
			  ,hd.[LOAIHD]
			  ,hd.[TRANGTHAI]
			  ,hd.[SOHD]
			  ,hd.[CMND]
			  ,hd.[MST]
			  ,hd.[SDInfo_INHOADON]
			  ,hd.[TENKH_INHOADON]
			  ,hd.[DIACHI_INHOADON]
			  ,hd.[DACAPDB]
			  ,hd.[NGAYN]
			  ,hd.[GhiChu]
			  ,hd.[Mavn]
		from HOPDONGPO hd
		where hd.MADDKPO = @Maddk
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_KhachHangPo_ByDuongPhoId]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-02
-- Description:	Get by DuongPhoId
-- =============================================

create PROCEDURE [dbo].[Get_KhachHangPo_ByDuongPhoId]	
	@KhuVucId varchar(10),
	@DuongPhoId varchar(10),
	@KyKhaiThac Datetime
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT K.IDKHPO AS IDKH, K.TENKH, K.MADPPO, k.MADBPO
			, case when k.SOTRU != null then k.SOTRU + ',' + (case when k.SOTRUKD != null then k.SOTRUKD else '' end)
				else (case when k.SOTRUKD != null then k.SOTRUKD else '' end) end as SOTRU
		FROM KHACHHANGPO K INNER JOIN TIEUTHUPO T ON T.IDKHPO = K.IDKHPO AND T.THANG = MONTH(@KyKhaiThac)
				AND K.MAKVPO = @KhuVucId AND T.NAM = YEAR(@KyKhaiThac)
			inner join DUONGPHOPO dp on dp.MADPPO = K.MADPPO		
		WHERE K.MADPPO = @DuongPhoId AND K.KYKHAITHAC != CONVERT(DATE, @KyKhaiThac)	

	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_MauNhanVien_ById]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-01-19
-- Description:	Get by Makv Service
-- =============================================

CREATE PROCEDURE [dbo].[Get_MauNhanVien_ById]	
	@Id int
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT ma.[Id]
      ,ma.[KhuVucId]
      ,ma.[ServiceId]
      ,ma.[TenMauNhanVien]
	  ,ma.MaSoKimM1
	  ,ma.MaSoKimM2
      ,ma.[SortOrder]
      ,ma.[Status]
      ,ma.[Active]
      ,ma.[CreateDate]
      ,ma.[CreateBy]
      ,ma.[UpdateDate]
      ,ma.[UpdateBy]
		FROM [dbo].[MauNhanVien] ma
		where ma.Id = @Id
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_MauNhanVien_ByMakvService]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-01-19
-- Description:	Get by Makv Service
-- =============================================

CREATE PROCEDURE [dbo].[Get_MauNhanVien_ByMakvService]	
	@Makv varchar(10),
	@Serviceid int
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT ma.[Id]
      ,ma.[KhuVucId]
      ,ma.[ServiceId]
      ,ma.[TenMauNhanVien]
	  ,ma.MaSoKimM1
	  ,ma.MaSoKimM2
      ,ma.[SortOrder]
      ,ma.[Status]
      ,ma.[Active]
      ,ma.[CreateDate]
      ,ma.[CreateBy]
      ,ma.[UpdateDate]
      ,ma.[UpdateBy]
		FROM [dbo].[MauNhanVien] ma
		where ma.KhuVucId = @Makv and ma.ServiceId = @Serviceid
		order by ma.CreateDate desc
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_MauNhanVienChiTiet_ById]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-01-20
-- Description:	Get by Makv Service
-- =============================================

CREATE PROCEDURE [dbo].[Get_MauNhanVienChiTiet_ById]	
	@Id int
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT mct.[Id]
      ,mct.[MauNhanVienId]
      ,mct.[NhanVienId]
      ,mct.[NhanVienHoTen]
	  , (select pb.TENPB from NHANVIEN nv inner join PHONGBAN pb on nv.MAPB = pb.MAPB where nv.MANV = mct.NhanVienId) as TenPhong
      ,mct.[GhiChu]
      ,mct.[SortOrder]
      ,mct.[Status]
      ,mct.[Active]
      ,mct.[CreateDate]
      ,mct.[CreateBy]
      ,mct.[UpdateDate]
      ,mct.[UpdateBy]
		FROM [dbo].[MauNhanVienChiTiet] mct
		where mct.Id = @Id
		order by mct.SortOrder
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_MauNhanVienChiTiet_ByMauNhanVienId]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-01-20
-- Description:	Get by Makv Service
-- =============================================

create PROCEDURE [dbo].[Get_MauNhanVienChiTiet_ByMauNhanVienId]	
	@MauNhanVienId int
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT mct.[Id]
      ,mct.[MauNhanVienId]
      ,mct.[NhanVienId]
      ,mct.[NhanVienHoTen]
	  , (select pb.TENPB from NHANVIEN nv inner join PHONGBAN pb on nv.MAPB = pb.MAPB where nv.MANV = mct.NhanVienId) as TenPhong
      ,mct.[GhiChu]
      ,mct.[SortOrder]
      ,mct.[Status]
      ,mct.[Active]
      ,mct.[CreateDate]
      ,mct.[CreateBy]
      ,mct.[UpdateDate]
      ,mct.[UpdateBy]
		FROM [dbo].[MauNhanVienChiTiet] mct
		where mct.MauNhanVienId = @MauNhanVienId
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_PhuongXa_ByAll]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-24
-- Description:	Get by Id Service
-- =============================================

create PROCEDURE [dbo].[Get_PhuongXa_ByAll]	
	@Id int
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT px.[Id]
		  ,px.[QuanHuyenId]
		  ,px.[TenPhuongXa]
		  ,px.[LoaiXa]
		  ,px.[KinhDoViDo]
		  ,px.[Status]
		  ,px.[Stt]
		  ,px.[Active]
		  ,px.[CreateBy]
		  ,px.[CreateDate]
		  ,px.[UpdateBy]
		  ,px.[UpdateDate]
	  FROM [dbo].[PhuongXa] px
	  Order by px.QuanHuyenId	
		
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_PhuongXa_ByQuanHuyenId]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-24
-- Description:	Get by Id Service
-- =============================================

create PROCEDURE [dbo].[Get_PhuongXa_ByQuanHuyenId]	
	@QuanHuyenId int
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT px.[Id]
		  ,px.[QuanHuyenId]
		  ,px.[TenPhuongXa]
		  ,px.[LoaiXa]
		  ,px.[KinhDoViDo]
		  ,px.[Status]
		  ,px.[Stt]
		  ,px.[Active]
		  ,px.[CreateBy]
		  ,px.[CreateDate]
		  ,px.[UpdateBy]
		  ,px.[UpdateDate]
	  FROM [dbo].[PhuongXa] px
	  where px.QuanHuyenId = @QuanHuyenId
	  Order by px.TenPhuongXa	
		
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_PhuongXa_ByTinhThanhPhoId]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-25
-- Description:	Get by Id Service
-- =============================================

create PROCEDURE [dbo].[Get_PhuongXa_ByTinhThanhPhoId]	
	@TinhThanhPhoId int
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT px.[Id]
		  ,px.[QuanHuyenId]
		  ,px.[TenPhuongXa]
		  ,px.[LoaiXa]
		  ,px.[KinhDoViDo]
		  ,px.[Status]
		  ,px.[Stt]
		  ,px.[Active]
		  ,px.[CreateBy]
		  ,px.[CreateDate]
		  ,px.[UpdateBy]
		  ,px.[UpdateDate]
	  FROM [dbo].[PhuongXa] px inner join QuanHuyen qh on px.QuanHuyenId = qh.Id
	  where qh.ThanhPhoTinhId = @TinhThanhPhoId
	  Order by px.TenPhuongXa	
		
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_QuanHuyen_ByAll]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-24
-- Description:	Get by Id Service
-- =============================================

create PROCEDURE [dbo].[Get_QuanHuyen_ByAll]	
	@Id int
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT qh.[Id]
		  ,qh.[ThanhPhoTinhId]
		  ,qh.[TenQuan]
		  ,qh.[LoaiQuanHuyen]
		  ,qh.[KinhDoViDo]
		  ,qh.[Description]
		  ,qh.[ORDERS]
		  ,qh.[STARTCODE]
		  ,qh.[DACBIET]
		  ,qh.[MAKV]
		  ,qh.[ProvinceId]
		  ,qh.[Status]
		  ,qh.[Stt]
		  ,qh.[Active]
		  ,qh.[CreateBy]
		  ,qh.[CreateDate]
		  ,qh.[UpdateBy]
		  ,qh.[UpdateDate]
	  FROM [dbo].[QuanHuyen] qh
	  Order by qh.ThanhPhoTinhId	
		
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_QuanHuyen_ByThanhPhoTinhId]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-24
-- Description:	Get by Id Service
-- =============================================

CREATE PROCEDURE [dbo].[Get_QuanHuyen_ByThanhPhoTinhId]	
	@ThanhPhoTinhId int
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT qh.[Id]
		  ,qh.[ThanhPhoTinhId]
		  ,qh.[TenQuan]
		  ,qh.[LoaiQuanHuyen]
		  ,qh.[KinhDoViDo]
		  ,qh.[Description]
		  ,qh.[ORDERS]
		  ,qh.[STARTCODE]
		  ,qh.[DACBIET]
		  ,qh.[MAKV]
		  ,qh.[ProvinceId]
		  ,qh.[Status]
		  ,qh.[Stt]
		  ,qh.[Active]
		  ,qh.[CreateBy]
		  ,qh.[CreateDate]
		  ,qh.[UpdateBy]
		  ,qh.[UpdateDate]
	  FROM [dbo].[QuanHuyen] qh
	  where  qh.ThanhPhoTinhId = @ThanhPhoTinhId
	  Order by qh.TenQuan	
		
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_ThanhPhoTinh_ByAll]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-24
-- Description:	Get by Makv Service
-- =============================================

create PROCEDURE [dbo].[Get_ThanhPhoTinh_ByAll]	
	@Id int
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT tp.[Id]
		  ,tp.[TenTinh]
		  ,tp.[LoaiTinh]
		  ,tp.[MaTinh]
		  ,tp.[Status]
		  ,tp.[Stt]
		  ,tp.[Active]
		  ,tp.[CreateBy]
		  ,tp.[CreateDate]
		  ,tp.[UpdateBy]
		  ,tp.[UpdateDate]
		FROM [dbo].[ThanhPhoTinh] tp		
		order by tp.TenTinh
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_ThanhPhoTinh_ById]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-24
-- Description:	Get by Id Service
-- =============================================

create PROCEDURE [dbo].[Get_ThanhPhoTinh_ById]	
	@Id int
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT tp.[Id]
		  ,tp.[TenTinh]
		  ,tp.[LoaiTinh]
		  ,tp.[MaTinh]
		  ,tp.[Status]
		  ,tp.[Stt]
		  ,tp.[Active]
		  ,tp.[CreateBy]
		  ,tp.[CreateDate]
		  ,tp.[UpdateBy]
		  ,tp.[UpdateDate]
		FROM [dbo].[ThanhPhoTinh] tp		
		where tp.Id = @Id
		order by tp.TenTinh
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Get_UploadFile_ByMakv]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-03
-- Description:	Get by Makv Service
-- =============================================

CREATE PROCEDURE [dbo].[Get_UploadFile_ByMakv]	
	@Makv varchar(10)
as
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		SELECT convert(int, up.[MAUPLOAD]) as ID
		  , up.[MAUPLOAD]
		  ,up.[MANV]
		  ,up.[MAKV]
		  ,up.[TENFILE]
		  ,up.[TENPATH]
		  ,up.[DATE]
		FROM [dbo].[UPLOADFILE] up
		where up.MAKV = @Makv 
		order by up.[DATE] desc
			
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Insert_MauNhanVien]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2020-10-23
-- Description:	Insert Table
-- =============================================

CREATE PROCEDURE [dbo].[Insert_MauNhanVien]	
	@TenMauNhanVien nvarchar(100),
	@MaSoKimM1 nvarchar(50),
	@MaSoKimM2 nvarchar(50),
	@Makv varchar(10),
	@Serviceid int,
	@SortOrder int,
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		declare @maunhanvienid int = (select max(Id) from MauNhanVien)
		if @maunhanvienid is null
		begin
			set @maunhanvienid = 1
		end
		else
		begin
			set @maunhanvienid = @maunhanvienid + 1
		end

		insert into MauNhanVien ( [Id]      ,[KhuVucId]      ,[ServiceId]      ,[TenMauNhanVien]
			,[MaSoKimM1]      ,[MaSoKimM2]
			,[GhiChu]      ,[SortOrder]      ,[Status]      ,[Active]      ,[CreateDate]      ,[CreateBy]      )
		select @maunhanvienid     , @Makv     , @Serviceid      , @TenMauNhanVien      
			,@MaSoKimM1, @MaSoKimM2
			, '', @SortOrder, 1, 1, getdate(), @Manv	

		select 'Ok' as KetQua	
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Insert_MauNhanVienChiTiet]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2021-01-20
-- Description:	Insert Table
-- =============================================

CREATE PROCEDURE [dbo].[Insert_MauNhanVienChiTiet]	
	@ManvMauNhanVienChiTiet nvarchar(100),
	@MauNhanVienId int,
	@SortOrder int,
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		declare @isMauNhanVienChiTietByIdNhanVienId int = (select count(Id) from MauNhanVienChiTiet where MauNhanVienId = @MauNhanVienId
			and NhanVienId = @ManvMauNhanVienChiTiet)

		if @isMauNhanVienChiTietByIdNhanVienId > 0
		begin		
			select 'Sai' as KetQua			
		end
		else
		begin
			declare @maunhanvienchitietid int = (select max(Id) from MauNhanVienChiTiet)
			if @maunhanvienchitietid is null
			begin
				set @maunhanvienchitietid = 1
			end
			else
			begin
				set @maunhanvienchitietid = @maunhanvienchitietid + 1
			end

			insert into MauNhanVienChiTiet ( [Id]      ,[MauNhanVienId]      ,[NhanVienId]     
				,[NhanVienHoTen]
				,[GhiChu]      ,[SortOrder]      ,[Status]      ,[Active]      ,[CreateDate]      ,[CreateBy]          )
			select @maunhanvienchitietid     , @MauNhanVienId     , @ManvMauNhanVienChiTiet      
			, (select HOTEN from NHANVIEN where MANV = @ManvMauNhanVienChiTiet) 
				, '', @SortOrder, 1, 1, getdate(), @Manv	

			select 'Ok' as KetQua	
		end
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Update_DonDangKy_SoDienThoai2MuaVatTu]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2020-12-15
-- Description:	Update Table
-- =============================================

create PROCEDURE [dbo].[Update_DonDangKy_SoDienThoai2MuaVatTu]	
	@Maddk varchar(11),
	@SoDienThoai2 varchar(20),
	@IsMuaVatTu bit,
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		update DONDANGKY set SoDienThoai2 = @SoDienThoai2, IsKHMuaVatTu = @IsMuaVatTu 
		where MADDK = @Maddk

		select 'Ok' as KetQua	

	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Update_DonDangKyHopDongThietKe_NamSinhMadpMadb]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-17
-- Description:	Update Table
-- =============================================

create PROCEDURE [dbo].[Update_DonDangKyHopDongThietKe_NamSinhMadpMadb]	
	@Maddk varchar(11),
	@NamSinh varchar(50),
	@Madp varchar(50),
	@Madb varchar(50),
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		insert into DONDANGKY_H ([MADDK]      ,[MADDKTONG]      ,[TENKH]      ,[DIACHILD]
      ,[SONHA]      ,[DIENTHOAI]      ,[CMND]      ,[MADP]  , [MADB]    ,[DUONGPHU]      ,[MAPHUONG]
      ,[MAKV]      ,[TEN_DC_KHAC]      ,[MAMDSD]      ,[NGAYDK]      ,[NGAYHKS]      ,[TTDK]
      ,[TTTK]      ,[TTCT]      ,[TTHD]      ,[TTTC]      ,[TTHC]      ,[LOAIDK]      ,[TENDK]
      ,[DACBIET]      ,[NGAYCD]      ,[NGAYKS]      ,[DTNGOAI]      ,[PASSDUYETTK]
      ,[BATAICHO]      ,[CTCTMOI]      ,[MAPB]      ,[MANV]      ,[SOHODN]      ,[SONK]
      ,[DMNK]      ,[DAIDIEN]      ,[NOIDUNG]      ,[MST]      ,[SDInfo_INHOADON]
      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[ISTUYENONGCHUNG]      ,[NGAYSINH]
      ,[CAPNGAY]      ,[TAI]      ,[MAHTTT]      ,[NOILAPDHHN]
      ,[TTNT]      ,[DONVICAPHN]      ,[MAHN]      ,[NGAYCAPHN]      ,[NGAYKETTHUCHN]      ,[NGAYKYHN]      ,[KYHOTROHN]      ,[ISHONGHEO]      ,[DIACHINGHEO]
      ,[TENCHUCVU]      ,[NGAYN]      ,[MADPKHGAN]      ,[MADBKHGAN]      ,[SONHA2]      ,[NGAYNHAPHSTRA]
      ,[NGAYDUYETHS]      ,[ISTRAHS]      ,[NGAYDUYETHSTRA]      ,[TIENCOCLX]      ,[TIENVATTULX]
      ,[NGAYNHAPHIS]      ,[SONHANHAPDON2]      ,[TENDUONG]      ,[MAXA]
      ,[TENXA]      ,[MAAPTO]      ,[TENAPTO]      ,[SoDienThoai2]      ,[IsKHMuaVatTu]      ,[IsXoaDLM]      ,[GhiChuXoaDLM]
      ,[NgayXoaDLM])
		select [MADDK]      ,[MADDKTONG]      ,[TENKH]      ,[DIACHILD]
      ,[SONHA]      ,[DIENTHOAI]      ,[CMND]      ,[MADP]   , [MADB]   ,[DUONGPHU]      ,[MAPHUONG]
      ,[MAKV]      ,[TEN_DC_KHAC]      ,[MAMDSD]      ,[NGAYDK]      ,[NGAYHKS]      ,[TTDK]
      ,[TTTK]      ,[TTCT]      ,[TTHD]      ,[TTTC]      ,[TTHC]      ,[LOAIDK]      ,[TENDK]
      ,[DACBIET]      ,[NGAYCD]      ,[NGAYKS]      ,[DTNGOAI]      ,[PASSDUYETTK]
      ,[BATAICHO]      ,[CTCTMOI]      ,[MAPB]      ,[MANV]      ,[SOHODN]      ,[SONK]
      ,[DMNK]      ,[DAIDIEN]      ,[NOIDUNG]      ,[MST]      ,[SDInfo_INHOADON]
      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[ISTUYENONGCHUNG]      ,[NGAYSINH]
      ,[CAPNGAY]      ,[TAI]      ,[MAHTTT]      ,[NOILAPDHHN]
      ,[TTNT]      ,[DONVICAPHN]      ,[MAHN]      ,[NGAYCAPHN]      ,[NGAYKETTHUCHN]      ,[NGAYKYHN]      ,[KYHOTROHN]      ,[ISHONGHEO]      ,[DIACHINGHEO]
      ,[TENCHUCVU]      ,[NGAYN]      ,[MADPKHGAN]      ,[MADBKHGAN]      ,[SONHA2]      ,[NGAYNHAPHSTRA]
      ,[NGAYDUYETHS]      ,[ISTRAHS]      ,[NGAYDUYETHSTRA]      ,[TIENCOCLX]      ,[TIENVATTULX]
      ,getdate()      ,[SONHANHAPDON2]      ,[TENDUONG]      ,[MAXA]
      ,[TENXA]      ,[MAAPTO]      ,[TENAPTO]      ,[SoDienThoai2]      ,[IsKHMuaVatTu]      ,[IsXoaDLM]      ,[GhiChuXoaDLM]
      ,[NgayXoaDLM]
		from DONDANGKY
		where MADDK = @Maddk

		update DONDANGKY set NGAYSINH = '11/11/' + @NamSinh, MADP = @Madp, MADB = @Madb
		where MADDK = @Maddk

		insert into HopDongHis ([MADDK]      ,[MADP]      ,[DUONGPHU]      ,[MADB]      ,[LOTRINH]
      ,[NGAYTAO]      ,[NGAYKT]      ,[NGAYHL]      ,[MAPHUONG]      ,[MAKV]      ,[CODH]      ,[LOAIONG]
      ,[MAHTTT]      ,[MAMDSD]      ,[DINHMUCSD]      ,[SOHO]      ,[SONHANKHAU]      ,[LOAIHD]      ,[TRANGTHAI]
      ,[SOHD]      ,[CMND]      ,[MST]      ,[SDInfo_INHOADON]      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]
      ,[DACAPDB]      ,[SONHA]      ,[NGAYN]      ,[GhiChu]      ,[Manv]      ,[NgayNhapHis])
		select [MADDK]      ,[MADP]      ,[DUONGPHU]      ,[MADB]      ,[LOTRINH]
      ,[NGAYTAO]      ,[NGAYKT]      ,[NGAYHL]      ,[MAPHUONG]      ,[MAKV]      ,[CODH]      ,[LOAIONG]
      ,[MAHTTT]      ,[MAMDSD]      ,[DINHMUCSD]      ,[SOHO]      ,[SONHANKHAU]      ,[LOAIHD]      ,[TRANGTHAI]
      ,[SOHD]      ,[CMND]      ,[MST]      ,[SDInfo_INHOADON]      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]
      ,[DACAPDB]      ,[SONHA]      ,[NGAYN]      ,[GhiChu]      ,@Manv     , getdate()
		from HOPDONG
		where MADDK = @Maddk

		update HOPDONG set MADP = @Madp, MADB = @Madb, NGAYN = getdate(), Manv = @Manv  
		where MADDK = @Maddk

		insert into THIETKEHIS ([MADDK]      ,[TENTK]      ,[FILETK]      ,[FILETK_HC]      ,[CHUTHICH]
      ,[NGAYLTK]      ,[MANVLTK]      ,[NGAYGUI_CT]      ,[NGAYNHAN_CT]      ,[NGAYDTK]      ,[MANVDTK]
      ,[SOBCT]      ,[THECHAP]      ,[THAMGIAONGCAI]      ,[MANVTK]      ,[TENNVTK]      ,[SODB]      ,[MAMAUTK]
      ,[TENKHPHAI]      ,[TENKHTRAI]      ,[DANHSOPHAI]      ,[DANHSOTRAI]      ,[NGAYN]      ,[HINHTK1]
      ,[HINHTK2]      ,[TONGTIENTK]      ,[DIACHITK]      ,[DUONGHEMTK]      ,[PHUONGTK]      ,[SDTTK]
      ,[VITRIDHTK]      ,[DANHSOTK]      ,[NGAYTRAHSKD]      ,[LYDOTRAHSKD]      ,[NGAYDUYETN]      ,[NGAYTRAKH]
      ,[ISTRAKEHOACH]      ,[NGAYTRATC]      ,[ISTRATHICONG]      ,[ISKHTT100]      ,[MADPLX]
      ,[NGAYDUYETKD]      ,[NGAYDUYETKDN]      ,[NGAYTRATK]      ,[MANVTRAKD]      ,[MANVDUYETKD]
      ,[NOIDUNGTRAKD]      ,[NGAYUP]      ,[NGAYNHAPHIS])
		select [MADDK]      ,[TENTK]      ,[FILETK]      ,[FILETK_HC]      ,[CHUTHICH]
      ,[NGAYLTK]      ,[MANVLTK]      ,[NGAYGUI_CT]      ,[NGAYNHAN_CT]      ,[NGAYDTK]      ,[MANVDTK]
      ,[SOBCT]      ,[THECHAP]      ,[THAMGIAONGCAI]      ,[MANVTK]      ,[TENNVTK]      ,[SODB]      ,[MAMAUTK]
      ,[TENKHPHAI]      ,[TENKHTRAI]      ,[DANHSOPHAI]      ,[DANHSOTRAI]      ,[NGAYN]      ,[HINHTK1]
      ,[HINHTK2]      ,[TONGTIENTK]      ,[DIACHITK]      ,[DUONGHEMTK]      ,[PHUONGTK]      ,[SDTTK]
      ,[VITRIDHTK]      ,[DANHSOTK]      ,[NGAYTRAHSKD]      ,[LYDOTRAHSKD]      ,[NGAYDUYETN]      ,[NGAYTRAKH]
      ,[ISTRAKEHOACH]      ,[NGAYTRATC]      ,[ISTRATHICONG]      ,[ISKHTT100]      ,[MADPLX]
      ,[NGAYDUYETKD]      ,[NGAYDUYETKDN]      ,[NGAYTRATK]      ,[MANVTRAKD]      ,[MANVDUYETKD]
      ,[NOIDUNGTRAKD]      ,[NGAYUP]      ,getdate()
		from THIETKE
		where MADDK = @Maddk

		update THIETKE set SODB = @Madp + @Madb, NGAYUP = getdate()
		where MADDK = @Maddk

		
		select 'Ok' as KetQua	

	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Update_DonDangKyHopDongThietKePo_NamSinhMadpMadb]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2021-02-18
-- Description:	Update Table
-- =============================================

create PROCEDURE [dbo].[Update_DonDangKyHopDongThietKePo_NamSinhMadpMadb]	
	@Maddk varchar(11),
	@NamSinh varchar(50),
	@Madp varchar(50),
	@Madb varchar(50),
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		insert into DONDANGKYPO_H ([MADDKPO]      ,[MADDKTONG]      ,[TENKH]      ,[DIACHILD]      ,[SONHA]      ,[DIENTHOAI]
      ,[CMND]      ,[MAPB]      ,[MANV]      ,[MADPPO]      ,[MADBPO]      ,[DUONGPHUPO]      ,[MAPHUONG]      ,[MAKVPO]
      ,[TEN_DC_KHAC]      ,[MAMDSDPO]      ,[NGAYDK]      ,[NGAYHKS]      ,[TTDK]      ,[TTTK]      ,[TTCT]      ,[TTHD]      ,[TTTC]
      ,[TTHC]      ,[TTNT]      ,[LOAIDK]      ,[TENDK]      ,[DACBIET]      ,[NGAYCD]      ,[NGAYKS]      ,[DTNGOAI]      ,[PASSDUYETTK]
      ,[BATAICHO]      ,[CTCTMOI]      ,[SOHODN]      ,[SONK]      ,[DMNK]      ,[DAIDIEN]      ,[NOIDUNG]      ,[MST]      ,[SDInfo_INHOADON]
      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[ISTUYENONGCHUNG]      ,[NGAYSINH]      ,[CAPNGAY]      ,[TAI]      ,[MAHTTT]
      ,[NOILAPDHHN]      ,[SOTRUPO]      ,[NGAYN]      ,[NGAYNHAPHIS]      ,[IsKHMuaVatTu]      ,[IsXoaDLM]      ,[GhiChuXoaDLM]
      ,[NgayXoaDLM])
		select [MADDKPO]      ,[MADDKTONG]      ,[TENKH]      ,[DIACHILD]      ,[SONHA]      ,[DIENTHOAI]
      ,[CMND]      ,[MAPB]      ,[MANV]      ,[MADPPO]      ,[MADBPO]      ,[DUONGPHUPO]      ,[MAPHUONG]      ,[MAKVPO]
      ,[TEN_DC_KHAC]      ,[MAMDSDPO]      ,[NGAYDK]      ,[NGAYHKS]      ,[TTDK]      ,[TTTK]      ,[TTCT]      ,[TTHD]      ,[TTTC]
      ,[TTHC]      ,[TTNT]      ,[LOAIDK]      ,[TENDK]      ,[DACBIET]      ,[NGAYCD]      ,[NGAYKS]      ,[DTNGOAI]      ,[PASSDUYETTK]
      ,[BATAICHO]      ,[CTCTMOI]      ,[SOHODN]      ,[SONK]      ,[DMNK]      ,[DAIDIEN]      ,[NOIDUNG]      ,[MST]      ,[SDInfo_INHOADON]
      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[ISTUYENONGCHUNG]      ,[NGAYSINH]      ,[CAPNGAY]      ,[TAI]      ,[MAHTTT]
      ,[NOILAPDHHN]      ,[SOTRUPO]      ,[NGAYN]      ,getdate()   ,[IsKHMuaVatTu]      ,[IsXoaDLM]      ,[GhiChuXoaDLM]
      ,[NgayXoaDLM]
		from DONDANGKYPO
		where MADDKPO = @Maddk

		update DONDANGKYPO set NGAYSINH = '11/11/' + @NamSinh, MADPPO = @Madp, MADBPO = @Madb
		where MADDKPO = @Maddk

		insert into HopDongPoHis ([MADDKPO]      ,[MADPPO]      ,[DUONGPHUPO]      ,[MADB]      ,[LOTRINH]      ,[NGAYTAO]      ,[NGAYKT]
      ,[NGAYHL]      ,[SONHA]      ,[MAPHUONGPO]      ,[MAKVPO]      ,[CODH]      ,[LOAIONG]      ,[MAHTTT]      ,[MAMDSDPO]      ,[DINHMUCSD]
      ,[SOHO]      ,[SONHANKHAU]      ,[LOAIHD]      ,[TRANGTHAI]      ,[SOHD]      ,[CMND]      ,[MST]      ,[SDInfo_INHOADON]
      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[DACAPDB]      ,[NGAYN]      ,[GhiChu]      ,[Mavn]      ,[NgayHis])
		select [MADDKPO]      ,[MADPPO]      ,[DUONGPHUPO]      ,[MADB]      ,[LOTRINH]      ,[NGAYTAO]      ,[NGAYKT]
      ,[NGAYHL]      ,[SONHA]      ,[MAPHUONGPO]      ,[MAKVPO]      ,[CODH]      ,[LOAIONG]      ,[MAHTTT]      ,[MAMDSDPO]      ,[DINHMUCSD]
      ,[SOHO]      ,[SONHANKHAU]      ,[LOAIHD]      ,[TRANGTHAI]      ,[SOHD]      ,[CMND]      ,[MST]      ,[SDInfo_INHOADON]
      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[DACAPDB]      ,[NGAYN]      ,[GhiChu]      ,[Mavn]      , getdate()
		from HOPDONGPO
		where MADDKPO = @Maddk

		update HOPDONGPO set MADPPO = @Madp, MADB = @Madb, NGAYN = getdate(), Mavn = @Manv  
		where MADDKPO = @Maddk

		insert into ThietKePoHis ([MADDKPO]      ,[TENTK]      ,[FILETK]      ,[FILETK_HC]      ,[CHUTHICH]      ,[NGAYLTK]      ,[MANVLTK]
      ,[NGAYGUI_CT]      ,[NGAYNHAN_CT]      ,[NGAYDTK]      ,[MANVDTK]      ,[SOBCT]      ,[THECHAP]      ,[THAMGIAONGCAI]      ,[MANVTK]
      ,[TENNVTK]      ,[SODB]      ,[MAMAUTK]      ,[TENKHPHAI]      ,[TENKHTRAI]      ,[DANHSOPHAI]      ,[DANHSOTRAI]      ,[MANV]
      ,[NGAY]      ,[TENTRUPHAI]      ,[TENTRUTRAI]      ,[DANHSOTRUPHAI]      ,[DANHSOTRUTRAI]      ,[SOTRUKH]      ,[TENTRAMKH]
      ,[TUYENDAYHATHE]      ,[NGAYN]      ,[NGAYDUYETKD]      ,[NGAYDUYETKDN]      ,[NGAYTRATK]      ,[MANVTRAKD]      ,[MANVDUYETKD]
      ,[NOIDUNGTRAKD]      ,[HINHTK1]      ,[HINHTK2]      ,[KETLUANTK]      ,[MAHTTT]      ,[NgayHis])
		select [MADDKPO]      ,[TENTK]      ,[FILETK]      ,[FILETK_HC]      ,[CHUTHICH]      ,[NGAYLTK]      ,[MANVLTK]
      ,[NGAYGUI_CT]      ,[NGAYNHAN_CT]      ,[NGAYDTK]      ,[MANVDTK]      ,[SOBCT]      ,[THECHAP]      ,[THAMGIAONGCAI]      ,[MANVTK]
      ,[TENNVTK]      ,[SODB]      ,[MAMAUTK]      ,[TENKHPHAI]      ,[TENKHTRAI]      ,[DANHSOPHAI]      ,[DANHSOTRAI]      ,[MANV]
      ,[NGAY]      ,[TENTRUPHAI]      ,[TENTRUTRAI]      ,[DANHSOTRUPHAI]      ,[DANHSOTRUTRAI]      ,[SOTRUKH]      ,[TENTRAMKH]
      ,[TUYENDAYHATHE]      ,[NGAYN]      ,[NGAYDUYETKD]      ,[NGAYDUYETKDN]      ,[NGAYTRATK]      ,[MANVTRAKD]      ,[MANVDUYETKD]
      ,[NOIDUNGTRAKD]      ,[HINHTK1]      ,[HINHTK2]      ,[KETLUANTK]      ,[MAHTTT]      ,getdate()
		from THIETKEPO
		where MADDKPO = @Maddk

		update THIETKEPO set SODB = @Madp + @Madb, NGAYN = getdate()
		where MADDKPO = @Maddk

		
		select 'Ok' as KetQua	

	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Update_HopDong_GhiChu]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2020-10-23
-- Description:	Update Table
-- =============================================

CREATE PROCEDURE [dbo].[Update_HopDong_GhiChu]	
	@Maddk varchar(11),
	@GhiChu nvarchar(1000),
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		insert into HopDongHis ([MADDK]      ,[MADP]      ,[DUONGPHU]      ,[MADB]      ,[LOTRINH]      ,[NGAYTAO]
      ,[NGAYKT]      ,[NGAYHL]      ,[MAPHUONG]      ,[MAKV]      ,[CODH]      ,[LOAIONG]      ,[MAHTTT]      ,[MAMDSD]
      ,[DINHMUCSD]      ,[SOHO]      ,[SONHANKHAU]      ,[LOAIHD]      ,[TRANGTHAI]      ,[SOHD]      ,[CMND]
      ,[MST]      ,[SDInfo_INHOADON]      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[DACAPDB]      ,[SONHA]
      ,[NGAYN]      ,[GhiChu]  ,  [Manv]    ,[NgayNhapHis])
		select [MADDK]      ,[MADP]      ,[DUONGPHU]      ,[MADB]      ,[LOTRINH]      ,[NGAYTAO]
      ,[NGAYKT]      ,[NGAYHL]      ,[MAPHUONG]      ,[MAKV]      ,[CODH]      ,[LOAIONG]      ,[MAHTTT]      ,[MAMDSD]
      ,[DINHMUCSD]      ,[SOHO]      ,[SONHANKHAU]      ,[LOAIHD]      ,[TRANGTHAI]      ,[SOHD]      ,[CMND]
      ,[MST]      ,[SDInfo_INHOADON]      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[DACAPDB]      ,[SONHA]
      ,[NGAYN]      ,[GhiChu] , [Manv]  , getdate()
		from HOPDONG
		where MADDK = @Maddk

		update HOPDONG set GhiChu = @GhiChu, Manv = @Manv
		where MADDK = @Maddk

		select 'Ok' as KetQua	
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Update_HopDongPo_GhiChu]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2020-11-17
-- Description:	Update Table
-- =============================================

create PROCEDURE [dbo].[Update_HopDongPo_GhiChu]	
	@Maddk varchar(11),
	@GhiChu nvarchar(1000),
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		

		insert into HopDongPoHis ([MADDKPO]      ,[MADPPO]      ,[DUONGPHUPO]      ,[MADB]      ,[LOTRINH]      ,[NGAYTAO]
      ,[NGAYKT]      ,[NGAYHL]      ,[SONHA]      ,[MAPHUONGPO]      ,[MAKVPO]      ,[CODH]      ,[LOAIONG]      ,[MAHTTT]
      ,[MAMDSDPO]      ,[DINHMUCSD]      ,[SOHO]      ,[SONHANKHAU]      ,[LOAIHD]      ,[TRANGTHAI]      ,[SOHD]
      ,[CMND]      ,[MST]      ,[SDInfo_INHOADON]      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[DACAPDB]
      ,[NGAYN]      ,[GhiChu]      ,[Mavn]      ,[NgayHis])
		select [MADDKPO]      ,[MADPPO]      ,[DUONGPHUPO]      ,[MADB]      ,[LOTRINH]      ,[NGAYTAO]
      ,[NGAYKT]      ,[NGAYHL]      ,[SONHA]      ,[MAPHUONGPO]      ,[MAKVPO]      ,[CODH]      ,[LOAIONG]      ,[MAHTTT]
      ,[MAMDSDPO]      ,[DINHMUCSD]      ,[SOHO]      ,[SONHANKHAU]      ,[LOAIHD]      ,[TRANGTHAI]      ,[SOHD]
      ,[CMND]      ,[MST]      ,[SDInfo_INHOADON]      ,[TENKH_INHOADON]      ,[DIACHI_INHOADON]      ,[DACAPDB]
      ,[NGAYN]      ,[GhiChu]      ,[Mavn]      , getdate()
		from HOPDONGPO
		where MADDKPO = @Maddk

		update HOPDONGPO set GhiChu = @GhiChu, Mavn = @Manv
		where MADDKPO = @Maddk

		select 'Ok' as KetQua	
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Update_MauNhanVien]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2020-10-23
-- Description:	Insert Table
-- =============================================

CREATE PROCEDURE [dbo].[Update_MauNhanVien]	
	@Id int,
	@TenMauNhanVien nvarchar(100),	
	@MaSoKimM1 nvarchar(50),
	@MaSoKimM2 nvarchar(50),
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		insert into MauNhanVienHis ([Id]      ,[KhuVucId]      ,[ServiceId]      ,[TenMauNhanVien], [MaSoKimM1]     ,[MaSoKimM2]     ,[GhiChu]
      ,[SortOrder]      ,[Status]      ,[Active]      ,[CreateDate]      ,[CreateBy]      ,[UpdateDate]      ,[UpdateBy]
      ,[NgayNhapHis]      )		
		select [Id]      ,[KhuVucId]      ,[ServiceId]      ,[TenMauNhanVien]  , [MaSoKimM1]     ,[MaSoKimM2]     ,[GhiChu]
      ,[SortOrder]      ,[Status]      ,[Active]      ,[CreateDate]      ,[CreateBy]      ,[UpdateDate]      ,[UpdateBy]
	  , getdate()
		from MauNhanVien ma
		where ma.Id = @Id


		update MauNhanVien set TenMauNhanVien = @TenMauNhanVien, [MaSoKimM1]  = @MaSoKimM1   ,[MaSoKimM2] = @MaSoKimM2
			, [UpdateDate] = getdate(), [UpdateBy] = @Manv		
		where Id = @Id

		select 'Ok' as KetQua	
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
/****** Object:  StoredProcedure [dbo].[Update_MauNhanVienChiTiet_BySortOrder]    Script Date: 25/03/2021 2:39:08 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		EOSAG
-- Create date: 2021-01-21
-- Description:	Insert Table
-- =============================================

create PROCEDURE [dbo].[Update_MauNhanVienChiTiet_BySortOrder]	
	@MauNhanVienChiTietId int,
	@SortOrder int,
	@Manv varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try		
		
		insert into MauNhanVienChiTietHis ( [Id]      ,[MauNhanVienId]      ,[NhanVienId]      ,[NhanVienHoTen]      ,[GhiChu]
      ,[SortOrder]      ,[Status]      ,[Active]      ,[CreateDate]      ,[CreateBy]      ,[UpdateDate]
      ,[UpdateBy]      ,[NgayNhapHis]        )		
		select [Id]      ,[MauNhanVienId]      ,[NhanVienId]      ,[NhanVienHoTen]      ,[GhiChu]
      ,[SortOrder]      ,[Status]      ,[Active]      ,[CreateDate]      ,[CreateBy]      ,[UpdateDate]
      ,[UpdateBy]       , getdate()
		from MauNhanVienChiTiet ma
		where ma.Id = @MauNhanVienChiTietId


		update MauNhanVienChiTiet set [SortOrder] = @SortOrder
			, [UpdateDate] = getdate(), [UpdateBy] = @Manv		
		where Id = @MauNhanVienChiTietId

		select 'Ok' as KetQua	
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch	
  
END

GO
