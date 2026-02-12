CREATE DATABASE QLBENHVIEN;
GO
USE QLBENHVIEN;
GO

/* 1. DANH MỤC HÀNH CHÍNH */
CREATE TABLE Khoa (
    MaKhoa INT IDENTITY PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL
);

CREATE TABLE ChucVu (
    MaCV INT IDENTITY PRIMARY KEY,
    TenCV NVARCHAR(100) NOT NULL -- Bác sĩ, Y tá, Tiếp tân, Admin
);

/* 2. NHÂN SỰ VÀ TÀI KHOẢN (Gộp chung để quản lý dễ hơn) */
CREATE TABLE NhanVien (
    MaNV INT IDENTITY PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    SDT NVARCHAR(20),
    DiaChi NVARCHAR(200),
    MaCV INT,
    MaKhoa INT,
    TrangThai NVARCHAR(50) DEFAULT N'Đang làm việc',
    FOREIGN KEY (MaCV) REFERENCES ChucVu(MaCV),
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

CREATE TABLE TaiKhoan (
    Username NVARCHAR(50) PRIMARY KEY,
    Password NVARCHAR(255) NOT NULL, -- Độ dài lớn để lưu Hash mật khẩu
    MaNV INT UNIQUE,
    VaiTro INT, -- 1: Admin, 0: Staff (Bác sĩ/Y tá/NV)
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV) ON DELETE CASCADE
);

/* 3. QUẢN LÝ PHÒNG VÀ GIƯỜNG */
CREATE TABLE Phong (
    MaPhong INT IDENTITY PRIMARY KEY,
    SoPhong NVARCHAR(10) NOT NULL,
    LoaiPhong NVARCHAR(50),
    MaKhoa INT,
    GiaPhong DECIMAL(18, 2) DEFAULT 0,
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

CREATE TABLE GiuongBenh (
    MaGiuong INT IDENTITY PRIMARY KEY,
    MaPhong INT,
    TenGiuong NVARCHAR(50),
    TrangThai NVARCHAR(50) DEFAULT N'Trống', -- Trống, Có người
    FOREIGN KEY (MaPhong) REFERENCES Phong(MaPhong)
);

/* 4. BỆNH NHÂN VÀ HỒ SƠ BỆNH ÁN */
CREATE TABLE BenhNhan (
    MaBN INT IDENTITY PRIMARY KEY,
    TenBN NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    DiaChi NVARCHAR(200),
    SDT NVARCHAR(20),
    MaBHYT NVARCHAR(20) -- Thêm BHYT để làm logic giảm giá giống KhuyenMai
);

CREATE TABLE HoSoBenhAn (
    MaHS INT IDENTITY PRIMARY KEY,
    MaBN INT,
    MaBS INT, -- Bác sĩ phụ trách
    NgayVaoVien DATETIME DEFAULT GETDATE(),
    NgayRaVien DATETIME,
    ChanDoanSoBo NVARCHAR(500),
    KetLuan NVARCHAR(500),
    MaGiuong INT NULL, -- Nếu điều trị nội trú
    TrangThai NVARCHAR(50) DEFAULT N'Đang điều trị', -- Đang điều trị, Xuất viện, Chờ thanh toán
    FOREIGN KEY (MaBN) REFERENCES BenhNhan(MaBN),
    FOREIGN KEY (MaBS) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaGiuong) REFERENCES GiuongBenh(MaGiuong)
);

/* 5. THUỐC VÀ ĐƠN THUỐC */
CREATE TABLE Thuoc (
    MaThuoc INT IDENTITY PRIMARY KEY,
    TenThuoc NVARCHAR(100) NOT NULL,
    DonVi NVARCHAR(20),
    Gia DECIMAL(18, 2) NOT NULL,
    SoLuongTon INT DEFAULT 0
);

CREATE TABLE DonThuoc (
    MaDon INT IDENTITY PRIMARY KEY,
    MaHS INT,
    MaBS INT,
    NgayKe DATETIME DEFAULT GETDATE(),
    TrangThai NVARCHAR(50) DEFAULT N'Chưa lấy thuốc',
    FOREIGN KEY (MaHS) REFERENCES HoSoBenhAn(MaHS),
    FOREIGN KEY (MaBS) REFERENCES NhanVien(MaNV)
);

CREATE TABLE ChiTietDonThuoc (
    MaDon INT,
    MaThuoc INT,
    SoLuong INT NOT NULL,
    LieuDung NVARCHAR(200),
    DonGiaLucKe DECIMAL(18, 2), -- Lưu giá tại thời điểm kê đơn
    PRIMARY KEY (MaDon, MaThuoc),
    FOREIGN KEY (MaDon) REFERENCES DonThuoc(MaDon),
    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc)
);

/* 6. DỊCH VỤ VÀ THANH TOÁN */
CREATE TABLE DichVu (
    MaDV INT IDENTITY PRIMARY KEY,
    TenDV NVARCHAR(100) NOT NULL,
    GiaDV DECIMAL(18, 2) NOT NULL
);

CREATE TABLE HoaDon (
    MaHD INT IDENTITY PRIMARY KEY,
    MaHS INT UNIQUE, -- Một hồ sơ bệnh án thường có 1 hóa đơn tổng
    NgayLap DATETIME DEFAULT GETDATE(),
    TongTienGoc DECIMAL(18, 2),
    BHYT_ChiTra DECIMAL(18, 2) DEFAULT 0,
    ThanhToanCuoi DECIMAL(18, 2),
    MaNV_LapHD INT,
    FOREIGN KEY (MaHS) REFERENCES HoSoBenhAn(MaHS),
    FOREIGN KEY (MaNV_LapHD) REFERENCES NhanVien(MaNV)
);

CREATE TABLE ChiTietHoaDon (
    MaHD INT,
    MaDV INT,
    SoLuong INT DEFAULT 1,
    ThanhTien DECIMAL(18, 2),
    PRIMARY KEY (MaHD, MaDV),
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    FOREIGN KEY (MaDV) REFERENCES DichVu(MaDV)
);

INSERT INTO Khoa (TenKhoa) VALUES 
(N'Khoa Nội'), (N'Khoa Ngoại'), (N'Khoa Nhi'), (N'Khoa Sản'), (N'Khoa Cấp Cứu');

/* 2. Dữ liệu cho Chức vụ */
INSERT INTO ChucVu (TenCV) VALUES 
(N'Bác sĩ trưởng'), (N'Bác sĩ điều trị'), (N'Y tá'), (N'Tiếp tân'), (N'Quản trị viên');

/* 3. Dữ liệu cho Nhân viên (Gồm cả Bác sĩ) */
INSERT INTO NhanVien (HoTen, GioiTinh, NgaySinh, SDT, DiaChi, MaCV, MaKhoa) VALUES 
(N'Nguyễn Văn A', N'Nam', '1980-05-15', '0901234567', N'Hà Nội', 1, 1),
(N'Trần Thị B', N'Nữ', '1985-10-20', '0912345678', N'TP.HCM', 2, 2),
(N'Lê Văn C', N'Nam', '1990-01-10', '0923456789', N'Đà Nẵng', 3, 1),
(N'Phạm Thị D', N'Nữ', '1995-12-25', '0934567890', N'Cần Thơ', 4, NULL);

/* 4. Dữ liệu cho Tài khoản (Password mẫu là '123') */
INSERT INTO TaiKhoan (Username, Password, MaNV, VaiTro) VALUES 
('admin', '123', 4, 1), -- Admin (Tiếp tân/Quản lý)
('bacsiA', '123', 1, 0), -- Bác sĩ A
('bacsiB', '123', 2, 0); -- Bác sĩ B

/* 5. Dữ liệu cho Phòng */
INSERT INTO Phong (SoPhong, LoaiPhong, MaKhoa, GiaPhong) VALUES 
('101', N'Phòng khám', 1, 0),
('201', N'Phòng nội trú VIP', 2, 500000),
('202', N'Phòng nội trú thường', 2, 200000);

/* 6. Dữ liệu cho Giường bệnh */
INSERT INTO GiuongBenh (MaPhong, TenGiuong, TrangThai) VALUES 
(2, 'G01', N'Có người'), (2, 'G02', N'Trống'), (3, 'G03', N'Trống');

/* 7. Dữ liệu cho Bệnh nhân */
INSERT INTO BenhNhan (TenBN, GioiTinh, NgaySinh, DiaChi, SDT, MaBHYT) VALUES 
(N'Nguyễn Văn Kha', N'Nam', '1990-03-12', N'Long An', '0888111222', 'GD123456789'),
(N'Lê Mỹ Linh', N'Nữ', '2000-07-20', N'Tiền Giang', '0888333444', 'HT987654321');

/* 8. Dữ liệu cho Hồ sơ bệnh án */
INSERT INTO HoSoBenhAn (MaBN, MaBS, NgayVaoVien, ChanDoanSoBo, MaGiuong, TrangThai) VALUES 
(1, 1, '2025-10-01 08:00:00', N'Viêm loét dạ dày', NULL, N'Đang điều trị'),
(2, 2, '2025-10-02 09:30:00', N'Gãy xương cẳng chân', 1, N'Đang điều trị');

/* 9. Dữ liệu cho Thuốc */
INSERT INTO Thuoc (TenThuoc, DonVi, Gia, SoLuongTon) VALUES 
('Paracetamol', N'Viên', 2000, 1000),
('Amoxicillin', N'Viên', 5000, 500),
('Gastropulgite', N'Gói', 15000, 200);

/* 10. Dữ liệu cho Đơn thuốc */
INSERT INTO DonThuoc (MaHS, MaBS, NgayKe, TrangThai) VALUES 
(1, 1, '2025-10-01 08:30:00', N'Đã lấy thuốc');

/* 11. Dữ liệu cho Chi tiết đơn thuốc */
INSERT INTO ChiTietDonThuoc (MaDon, MaThuoc, SoLuong, LieuDung, DonGiaLucKe) VALUES 
(1, 1, 10, N'Sáng 1 viên, tối 1 viên sau ăn', 2000),
(1, 3, 14, N'Uống trước khi ăn 30 phút', 15000);

/* 12. Dữ liệu cho Dịch vụ */
INSERT INTO DichVu (TenDV, GiaDV) VALUES 
(N'Khám nội khoa', 150000),
(N'Chụp X-quang', 300000),
(N'Xét nghiệm máu', 200000);

/* 13. Dữ liệu cho Hóa đơn (Test logic BHYT chi trả) */
INSERT INTO HoaDon (MaHS, NgayLap, TongTienGoc, BHYT_ChiTra, ThanhToanCuoi, MaNV_LapHD) VALUES 
(1, '2025-10-01 10:00:00', 380000, 304000, 76000, 4); -- Giả sử hưởng 80% BHYT

/* 14. Dữ liệu cho Chi tiết hóa đơn */
INSERT INTO ChiTietHoaDon (MaHD, MaDV, SoLuong, ThanhTien) VALUES 
(1, 1, 1, 150000),
(1, 3, 1, 230000); -- Tổng cộng 380k