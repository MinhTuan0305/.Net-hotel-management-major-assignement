/* =====================================================================
   HOTEL MANAGEMENT - DỮ LIỆU MẪU (chạy SAU file 01)
   ---------------------------------------------------------------------
   - Ngày tháng tính theo ngày chạy script, nên luôn có: lịch sử đã trả phòng
     (kèm hóa đơn, thanh toán, dọn phòng), khách đang ở, phòng trả hôm nay chờ dọn,
     khách đến hôm nay, booking tương lai, booking đã hủy / no-show, 1 booking
     Pending quá 24 giờ (sẽ bị hủy khi gọi sp_ExpirePendingBookings).
   - Lịch sử được INSERT trực tiếp (vì procedure chặn ngày quá khứ); booking từ
     hôm nay trở đi tạo bằng chính procedure để đúng quy tắc nghiệp vụ.
   - Mật khẩu mọi nhân viên mẫu: 123456 (BCrypt).
   ===================================================================== */
USE HotelManagement;
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO
IF EXISTS (SELECT 1 FROM dbo.Bookings) OR EXISTS (SELECT 1 FROM dbo.RoomTypes)
BEGIN
    RAISERROR(N'Database đã có dữ liệu nghiệp vụ. Chạy lại file 01 để tạo DB trống trước khi nạp dữ liệu mẫu.', 16, 1);
    SET NOEXEC ON;
END;
GO

/* ---------- 1. Danh mục ---------- */
BEGIN TRANSACTION;

INSERT INTO dbo.Employees (RoleId, FullName, Username, PasswordHash, Phone, Email, HireDate, IsActive)
SELECT r.RoleId, v.FullName, v.Username, '$2a$11$daKlmKa19waEfjVD44kjOOLeRXk.TGWT0jyumdd/kem9R5NxEY8dW',
       v.Phone, v.Email, v.HireDate, v.IsActive
FROM (VALUES
    (N'Manager',      N'Lê Minh Quân',     'quanlm', '0900000002', 'quan.le@hotel.local',    '2022-01-10', 1),
    (N'Receptionist', N'Phạm Thu Hà',      'hapt',   '0900000003', 'ha.pham@hotel.local',    '2022-03-01', 1),
    (N'Receptionist', N'Trần Thị Lan',     'lantt',  '0900000004', 'lan.tran@hotel.local',   '2022-06-15', 1),
    (N'Receptionist', N'Nguyễn Hoàng Nam', 'namnh',  '0900000005', 'nam.nguyen@hotel.local', '2023-02-20', 1),
    (N'Housekeeping', N'Vũ Thị Mai',       'maivt',  '0900000006', NULL,                     '2022-03-01', 1),
    (N'Housekeeping', N'Đỗ Văn Hùng',      'hungdv', '0900000007', NULL,                     '2023-05-08', 1),
    (N'Receptionist', N'Hoàng Văn Tùng',   'tunghv', '0900000009', NULL,                     '2022-09-01', 0)
) v (RoleName, FullName, Username, Phone, Email, HireDate, IsActive)
JOIN dbo.Roles r ON r.RoleName = v.RoleName;

INSERT INTO dbo.Customers (FullName, Gender, DateOfBirth, IdNumber, Phone, Email, Address, Nationality) VALUES
(N'Nguyễn Văn An',     N'Nam', '1988-03-15', '001088012345', '0912345678',     'an.nguyen@gmail.com',       N'25 Láng Hạ, Đống Đa, Hà Nội',                   N'Việt Nam'),
(N'Trần Thị Bích',     N'Nữ',  '1993-07-22', '079193004521', '0987654321',     'bich.tran@gmail.com',       N'112 Nguyễn Trãi, Quận 1, TP. Hồ Chí Minh',      N'Việt Nam'),
(N'Lê Hoàng Long',     N'Nam', '1985-11-02', '048085001234', '0905112233',     'longlh@yahoo.com',          N'45 Bạch Đằng, Hải Châu, Đà Nẵng',               N'Việt Nam'),
(N'Phạm Thị Hương',    N'Nữ',  '1991-01-30', '031191007788', '0934556677',     NULL,                        N'8 Lạch Tray, Ngô Quyền, Hải Phòng',             N'Việt Nam'),
(N'Võ Minh Tuấn',      N'Nam', '1979-09-09', '092079003321', '0918223344',     'tuan.vo@outlook.com',       N'77 Ninh Kiều, Cần Thơ',                         N'Việt Nam'),
(N'Đặng Thu Trang',    N'Nữ',  '1996-05-18', '001196009876', '0977001122',     'trang.dang@gmail.com',      N'12 Trần Duy Hưng, Cầu Giấy, Hà Nội',            N'Việt Nam'),
(N'Bùi Quang Huy',     N'Nam', '1990-12-25', '038090004455', '0868990011',     'huybq@gmail.com',           N'34 Lê Lợi, TP. Thanh Hóa',                      N'Việt Nam'),
(N'Hoàng Thị Lan Anh', N'Nữ',  '1987-04-08', '001187002233', '0903778899',     'lananh.hoang@gmail.com',    N'56 Kim Mã, Ba Đình, Hà Nội',                    N'Việt Nam'),
(N'Ngô Đức Thịnh',     N'Nam', NULL,         '079082005566', '0912009988',     NULL,                        N'90 Cách Mạng Tháng 8, Quận 3, TP. Hồ Chí Minh', N'Việt Nam'),
(N'Đinh Thị Mỹ Linh',  N'Nữ',  '1998-02-27', '052198001100', '0946112233',     'mylinh.dinh@gmail.com',     N'15 Trần Phú, TP. Quy Nhơn, Bình Định',          N'Việt Nam'),
(N'John Smith',        N'Nam', '1975-06-12', 'US523847196',  '+14155550134',   'john.smith@example.com',    N'742 Market Street, San Francisco, CA',          N'Hoa Kỳ'),
(N'Emily Johnson',     N'Nữ',  '1990-10-03', 'GB912736405',  '+447700900123',  'emily.j@example.com',       N'14 Baker Street, London',                       N'Anh'),
(N'Kim Min-jun',       N'Nam', '1983-03-21', 'M83920417',    '+821012345678',  'minjun.kim@example.com',    N'25 Gangnam-daero, Seoul',                       N'Hàn Quốc'),
(N'Tanaka Yuki',       N'Nữ',  '1994-09-16', 'TK4471280',    '+819012345678',  'yuki.tanaka@example.com',   N'2-1 Shibuya, Tokyo',                            N'Nhật Bản'),
(N'Wang Wei',          N'Nam', '1980-01-05', 'E38475612',    '+8613800138000', 'wei.wang@example.com',      N'88 Nanjing Road, Shanghai',                     N'Trung Quốc'),
(N'Sophie Martin',     N'Nữ',  '1989-07-29', '19FA84720',    '+33612345678',   'sophie.martin@example.com', N'10 Rue de Rivoli, Paris',                       N'Pháp'),
(N'Lý Văn Khánh',      N'Nam', '1992-11-11', '075092008877', '0932445566',     'khanh.ly@gmail.com',        N'21 Nguyễn Huệ, TP. Biên Hòa, Đồng Nai',         N'Việt Nam'),
(N'Mai Thị Thanh Tâm', N'Nữ',  '1995-06-06', '001195003344', '0965778800',     'tam.mai@gmail.com',         N'3 Hàng Bài, Hoàn Kiếm, Hà Nội',                 N'Việt Nam'),
(N'Trịnh Văn Dũng',    N'Nam', NULL,         '040077001199', '0913556677',     NULL,                        N'60 Quang Trung, TP. Vinh, Nghệ An',             N'Việt Nam'),
(N'Cao Thị Yến',       N'Nữ',  '2000-08-19', '001200005522', '0355667788',     'yen.cao@gmail.com',         N'27 Giảng Võ, Ba Đình, Hà Nội',                  N'Việt Nam');

INSERT INTO dbo.RoomTypes (TypeName, Description, BasePrice, MaxOccupancy, BedType) VALUES
(N'Standard Single', N'Phòng đơn tiêu chuẩn, phù hợp khách công tác',    400000, 1, N'1 giường đơn'),
(N'Standard Double', N'Phòng đôi tiêu chuẩn',                            550000, 2, N'1 giường đôi'),
(N'Deluxe',          N'Phòng cao cấp, rộng, có ban công',                850000, 3, N'1 giường đôi + 1 giường đơn'),
(N'Family',          N'Phòng gia đình, 2 giường đôi, bếp nhỏ',          1200000, 4, N'2 giường đôi'),
(N'Suite',           N'Phòng hạng sang, phòng khách riêng, view thành phố', 1800000, 4, N'1 giường King');

INSERT INTO dbo.Rooms (RoomTypeId, RoomNumber, Floor, Note)
SELECT rt.RoomTypeId, v.RoomNumber, v.Floor, v.Note
FROM (VALUES
    ('101', 1, N'Standard Single', N'Gần thang máy'),
    ('102', 1, N'Standard Single', NULL),
    ('103', 1, N'Standard Single', NULL),
    ('104', 1, N'Standard Single', NULL),
    ('201', 2, N'Standard Double', NULL),
    ('202', 2, N'Standard Double', NULL),
    ('203', 2, N'Standard Double', NULL),
    ('204', 2, N'Standard Double', NULL),
    ('205', 2, N'Standard Double', NULL),
    ('301', 3, N'Deluxe', NULL),
    ('302', 3, N'Deluxe', NULL),
    ('303', 3, N'Deluxe', NULL),
    ('304', 3, N'Deluxe', N'Ban công hướng hồ'),
    ('305', 3, N'Deluxe', N'Ban công hướng hồ'),
    ('401', 4, N'Family', NULL),
    ('402', 4, N'Family', NULL),
    ('403', 4, N'Family', N'Cửa thông với phòng 404'),
    ('404', 4, N'Family', N'Cửa thông với phòng 403'),
    ('501', 5, N'Suite', NULL),
    ('502', 5, N'Suite', NULL),
    ('503', 5, N'Suite', NULL)
) v (RoomNumber, Floor, TypeName, Note)
JOIN dbo.RoomTypes rt ON rt.TypeName = v.TypeName;

INSERT INTO dbo.Amenities (AmenityName, Description) VALUES
(N'Wifi miễn phí', N'Wifi tốc độ cao'), (N'Điều hòa', N'Điều hòa 2 chiều'), (N'TV màn hình phẳng', N'TV 43 inch'),
(N'Minibar', N'Tủ lạnh mini'), (N'Bồn tắm', NULL), (N'Ban công', NULL), (N'Két an toàn', NULL),
(N'Máy sấy tóc', NULL), (N'Bàn làm việc', NULL), (N'Ấm đun nước', N'Kèm trà và cà phê'),
(N'View thành phố', NULL), (N'Bếp nhỏ', N'Bếp từ và dụng cụ cơ bản');

INSERT INTO dbo.RoomTypeAmenities (RoomTypeId, AmenityId)
SELECT rt.RoomTypeId, a.AmenityId
FROM (VALUES
    (N'Standard Single', N'Wifi miễn phí'), (N'Standard Single', N'Điều hòa'), (N'Standard Single', N'TV màn hình phẳng'),
    (N'Standard Single', N'Máy sấy tóc'), (N'Standard Single', N'Ấm đun nước'),
    (N'Standard Double', N'Wifi miễn phí'), (N'Standard Double', N'Điều hòa'), (N'Standard Double', N'TV màn hình phẳng'),
    (N'Standard Double', N'Máy sấy tóc'), (N'Standard Double', N'Ấm đun nước'),
    (N'Deluxe', N'Wifi miễn phí'), (N'Deluxe', N'Điều hòa'), (N'Deluxe', N'TV màn hình phẳng'), (N'Deluxe', N'Minibar'),
    (N'Deluxe', N'Ban công'), (N'Deluxe', N'Két an toàn'), (N'Deluxe', N'Bàn làm việc'), (N'Deluxe', N'Ấm đun nước'),
    (N'Family', N'Wifi miễn phí'), (N'Family', N'Điều hòa'), (N'Family', N'TV màn hình phẳng'), (N'Family', N'Minibar'),
    (N'Family', N'Két an toàn'), (N'Family', N'Bếp nhỏ'),
    (N'Suite', N'Wifi miễn phí'), (N'Suite', N'Điều hòa'), (N'Suite', N'TV màn hình phẳng'), (N'Suite', N'Minibar'),
    (N'Suite', N'Bồn tắm'), (N'Suite', N'Két an toàn'), (N'Suite', N'Bàn làm việc'), (N'Suite', N'View thành phố')
) v (TypeName, AmenityName)
JOIN dbo.RoomTypes rt ON rt.TypeName = v.TypeName
JOIN dbo.Amenities a ON a.AmenityName = v.AmenityName;

INSERT INTO dbo.Services (ServiceName, Category, Unit, Price, IsActive) VALUES
(N'Ăn sáng buffet',          N'Ăn uống',   N'Suất',  150000, 1),
(N'Ăn tối set menu',         N'Ăn uống',   N'Suất',  280000, 1),
(N'Nước suối',               N'Minibar',   N'Chai',   15000, 1),
(N'Bia Tiger',               N'Minibar',   N'Lon',    35000, 1),
(N'Nước ngọt',               N'Minibar',   N'Lon',    25000, 1),
(N'Giặt ủi',                 N'Giặt là',   N'Kg',     40000, 1),
(N'Giặt khô',                N'Giặt là',   N'Bộ',     90000, 1),
(N'Đưa đón sân bay',         N'Di chuyển', N'Lượt',  300000, 1),
(N'Thuê xe máy',             N'Di chuyển', N'Ngày',  150000, 1),
(N'Massage toàn thân',       N'Spa',       N'Giờ',   400000, 1),
(N'Xông hơi',                N'Spa',       N'Lượt',  200000, 1),
(N'Tour tham quan thành phố', N'Tour',     N'Người', 350000, 1),
(N'Thuê phòng họp',          N'Khác',      N'Giờ',   500000, 1),
(N'Phụ thu thêm giường',     N'Khác',      N'Đêm',   200000, 1),
(N'Karaoke',                 N'Giải trí',  N'Giờ',   300000, 0);

COMMIT TRANSACTION;
GO

/* ---------- 2. Lịch sử, khách đang ở, booking đã hủy (INSERT trực tiếp) ---------- */
BEGIN TRANSACTION;

DECLARE @Today DATE = CAST(GETDATE() AS DATE);
DECLARE @Now DATETIME2(0) = SYSDATETIME();

-- Mỗi dòng là một lượt lưu trú. Kind: H = đã trả, T = trả hôm nay, I = đang ở, C = đã hủy
DECLARE @Stay TABLE (
    K INT PRIMARY KEY, Bk INT, Kind CHAR(1), CustIdNumber VARCHAR(20), RoomNumber VARCHAR(10),
    InOff INT, OutOff INT, Adults TINYINT, Children TINYINT, Rate TINYINT, Emp VARCHAR(50),
    InMin INT, OutMin INT NULL, DiscPct INT, DiscReason NVARCHAR(200), Method VARCHAR(20),
    CancelOff INT NULL, CancelReason NVARCHAR(200) NULL);
DECLARE @Svc TABLE (K INT, ServiceName NVARCHAR(100), Qty INT, DayOff INT, HourOfDay INT);

INSERT INTO @Stay VALUES
(1, 1, 'H', '001088012345', '301', -60, -58, 2, 0, 50, 'hapt', 20, -90, 0, NULL, 'Cash', NULL, NULL),
(2, 2, 'H', 'US523847196', '501', -55, -51, 2, 0, 100, 'lantt', 30, -30, 5, N'Khách quen', 'Card', NULL, NULL),
(3, 3, 'H', '079193004521', '101', -50, -49, 1, 0, 50, 'hapt', 5, -120, 0, NULL, 'EWallet', NULL, NULL),
(4, 4, 'H', '092079003321', '401', -45, -42, 2, 1, 50, 'namnh', 40, 45, 0, NULL, 'BankTransfer', NULL, NULL),
(5, 5, 'H', 'M83920417', '302', -40, -37, 2, 0, 50, 'lantt', 30, -60, 10, N'Đoàn công ty', 'BankTransfer', NULL, NULL),
(6, 5, 'H', 'M83920417', '303', -40, -37, 2, 0, 50, 'lantt', 35, -45, 10, N'Đoàn công ty', 'BankTransfer', NULL, NULL),
(7, 5, 'H', 'M83920417', '304', -40, -36, 2, 0, 50, 'lantt', 40, 130, 10, N'Đoàn công ty', 'Card', NULL, NULL),
(8, 6, 'H', '001196009876', '102', -35, -34, 1, 0, 50, 'hapt', 10, -150, 0, NULL, 'Cash', NULL, NULL),
(9, 7, 'H', 'TK4471280', '305', -30, -27, 2, 0, 100, 'namnh', 60, -20, 0, NULL, 'Card', NULL, NULL),
(10, 8, 'H', '038090004455', '201', -25, -23, 2, 0, 50, 'lantt', 15, 95, 0, NULL, 'Cash', NULL, NULL),
(11, 9, 'H', 'E38475612', '502', -20, -16, 2, 0, 100, 'hapt', 50, -60, 0, NULL, 'BankTransfer', NULL, NULL),
(12, 10, 'H', '19FA84720', '402', -15, -12, 2, 2, 50, 'namnh', 25, -40, 5, N'Khách quen', 'EWallet', NULL, NULL),
(13, 11, 'H', '075092008877', '202', -10, -8, 2, 0, 50, 'lantt', 5, -90, 0, NULL, 'Cash', NULL, NULL),
(14, 12, 'H', '052198001100', '103', -7, -5, 1, 0, 50, 'hapt', 45, -100, 0, NULL, 'Card', NULL, NULL),
(15, 13, 'H', '001195003344', '203', -3, -1, 2, 0, 50, 'namnh', 20, -30, 0, NULL, 'Cash', NULL, NULL),
(16, 14, 'T', '048085001234', '205', -2, 0, 2, 0, 50, 'lantt', 30, NULL, 0, NULL, 'Cash', NULL, NULL),
(17, 15, 'T', '001187002233', '403', -3, 0, 3, 1, 50, 'hapt', 10, NULL, 0, NULL, 'Card', NULL, NULL),
(18, 13, 'I', '001195003344', '204', -3, 2, 2, 0, 50, 'namnh', 25, NULL, 0, NULL, 'Cash', NULL, NULL),
(19, 16, 'I', 'GB912736405', '503', -2, 3, 2, 0, 100, 'hapt', 45, NULL, 0, NULL, 'Cash', NULL, NULL),
(20, 17, 'I', '040077001199', '404', -1, 1, 2, 2, 50, 'lantt', 20, NULL, 0, NULL, 'Cash', NULL, NULL),
(21, 19, 'I', '001088012345', '301', -1, 0, 2, 0, 50, 'namnh', 30, NULL, 0, NULL, 'Cash', NULL, NULL),
(22, 20, 'C', '031191007788', '202', -6, -4, 2, 0, 50, 'lantt', 0, NULL, 0, NULL, 'BankTransfer', -7, N'Khách hủy dưới 48 giờ'),
(23, 21, 'C', '079082005566', '305', -4, -2, 2, 0, 50, 'hapt', 0, NULL, 0, NULL, 'BankTransfer', -2, N'NoShow');
INSERT INTO @Svc VALUES
(1, N'Ăn sáng buffet', 2, 1, 8),
(1, N'Bia Tiger', 2, 0, 21),
(2, N'Đưa đón sân bay', 1, 0, 15),
(2, N'Ăn sáng buffet', 2, 1, 8),
(2, N'Massage toàn thân', 2, 2, 19),
(2, N'Ăn tối set menu', 2, 3, 19),
(4, N'Ăn sáng buffet', 3, 1, 8),
(4, N'Nước suối', 6, 0, 20),
(4, N'Xông hơi', 3, 2, 17),
(5, N'Thuê phòng họp', 2, 1, 14),
(5, N'Ăn sáng buffet', 2, 1, 8),
(6, N'Ăn sáng buffet', 2, 1, 8),
(7, N'Ăn sáng buffet', 2, 1, 8),
(7, N'Giặt ủi', 3, 2, 10),
(8, N'Nước ngọt', 2, 0, 22),
(9, N'Ăn tối set menu', 2, 0, 19),
(9, N'Massage toàn thân', 1, 1, 18),
(9, N'Giặt khô', 2, 1, 9),
(10, N'Ăn sáng buffet', 2, 1, 8),
(11, N'Đưa đón sân bay', 2, 0, 15),
(11, N'Ăn sáng buffet', 2, 1, 8),
(11, N'Massage toàn thân', 2, 1, 18),
(11, N'Thuê xe máy', 1, 2, 9),
(12, N'Ăn sáng buffet', 4, 1, 8),
(12, N'Phụ thu thêm giường', 1, 0, 15),
(12, N'Tour tham quan thành phố', 4, 1, 9),
(13, N'Bia Tiger', 4, 0, 21),
(15, N'Ăn sáng buffet', 2, 1, 8),
(16, N'Nước suối', 2, 1, 21),
(17, N'Ăn sáng buffet', 4, 1, 8),
(17, N'Ăn sáng buffet', 4, 2, 8),
(18, N'Ăn sáng buffet', 2, 1, 8),
(18, N'Giặt ủi', 2, 2, 10),
(19, N'Đưa đón sân bay', 1, 0, 15),
(19, N'Ăn sáng buffet', 2, 1, 8),
(20, N'Ăn tối set menu', 4, 0, 19),
(21, N'Nước suối', 2, 0, 20);

-- Thời điểm thực tế. Trả phòng hôm nay: 10:00 hoặc 1 giờ trước lúc chạy script (lấy mốc sớm hơn).
DECLARE @StayTime TABLE (K INT PRIMARY KEY, InAt DATETIME2(0), OutAt DATETIME2(0) NULL, CreatedAt DATETIME2(0));
INSERT INTO @StayTime (K, InAt, OutAt, CreatedAt)
SELECT s.K,
       DATEADD(MINUTE, 14 * 60 + s.InMin, CAST(DATEADD(DAY, s.InOff, @Today) AS DATETIME2(0))),
       CASE s.Kind
            WHEN 'H' THEN DATEADD(MINUTE, 12 * 60 + s.OutMin, CAST(DATEADD(DAY, s.OutOff, @Today) AS DATETIME2(0)))
            WHEN 'T' THEN CASE WHEN DATEADD(HOUR, 10, CAST(@Today AS DATETIME2(0))) < DATEADD(HOUR, -1, @Now)
                               THEN DATEADD(HOUR, 10, CAST(@Today AS DATETIME2(0)))
                               ELSE DATEADD(HOUR, -1, @Now) END
       END,
       DATEADD(HOUR, 9, CAST(DATEADD(DAY, s.InOff - 10, @Today) AS DATETIME2(0)))
FROM @Stay s;

-- 2.1 Booking (một dòng cho mỗi Bk)
DECLARE @Map TABLE (Bk INT PRIMARY KEY, BookingId INT);
DECLARE @Bk INT = 0, @NewId INT;
WHILE 1 = 1
BEGIN
    SELECT TOP (1) @Bk = Bk FROM @Stay WHERE Bk > @Bk ORDER BY Bk;
    IF @@ROWCOUNT = 0 BREAK;

    INSERT INTO dbo.Bookings (CustomerId, DepositRate, Note, CreatedBy, CreatedAt, ConfirmedAt)
    SELECT TOP (1) c.CustomerId, s.Rate, NULL, e.EmployeeId, t.CreatedAt, DATEADD(HOUR, 1, t.CreatedAt)
    FROM @Stay s
    JOIN @StayTime t ON t.K = s.K
    JOIN dbo.Customers c ON c.IdNumber = s.CustIdNumber
    JOIN dbo.Employees e ON e.Username = s.Emp
    WHERE s.Bk = @Bk
    ORDER BY s.K;
    SET @NewId = SCOPE_IDENTITY();
    INSERT INTO @Map (Bk, BookingId) VALUES (@Bk, @NewId);
END;

-- 2.2 Lượt lưu trú
INSERT INTO dbo.BookingRooms (BookingId, RoomId, CheckInDate, CheckOutDate, Adults, Children, PricePerNight, Status,
                              ActualCheckIn, ActualCheckOut, CheckInBy, CheckOutBy, CancelledAt, CancelReason)
SELECT m.BookingId, r.RoomId, DATEADD(DAY, s.InOff, @Today), DATEADD(DAY, s.OutOff, @Today), s.Adults, s.Children,
       rt.BasePrice,
       CASE s.Kind WHEN 'I' THEN 'CheckedIn' WHEN 'C' THEN 'Cancelled' ELSE 'CheckedOut' END,
       CASE WHEN s.Kind = 'C' THEN NULL ELSE t.InAt END,
       CASE WHEN s.Kind IN ('H', 'T') THEN t.OutAt END,
       CASE WHEN s.Kind = 'C' THEN NULL ELSE e.EmployeeId END,
       CASE WHEN s.Kind IN ('H', 'T') THEN e.EmployeeId END,
       CASE WHEN s.Kind = 'C' THEN DATEADD(HOUR, 10, CAST(DATEADD(DAY, s.CancelOff, @Today) AS DATETIME2(0))) END,
       s.CancelReason
FROM @Stay s
JOIN @StayTime t ON t.K = s.K
JOIN @Map m ON m.Bk = s.Bk
JOIN dbo.Rooms r ON r.RoomNumber = s.RoomNumber
JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
JOIN dbo.Employees e ON e.Username = s.Emp;

DECLARE @StayMap TABLE (K INT PRIMARY KEY, BookingRoomId INT);
INSERT INTO @StayMap (K, BookingRoomId)
SELECT s.K, br.BookingRoomId
FROM @Stay s
JOIN @Map m ON m.Bk = s.Bk
JOIN dbo.Rooms r ON r.RoomNumber = s.RoomNumber
JOIN dbo.BookingRooms br ON br.BookingId = m.BookingId AND br.RoomId = r.RoomId;

-- 2.3 Dịch vụ (giá theo danh mục)
INSERT INTO dbo.ServiceUsages (BookingRoomId, ServiceId, Quantity, UnitPrice, UsedAt, CreatedBy)
SELECT sm.BookingRoomId, sv.ServiceId, x.Qty, sv.Price,
       DATEADD(HOUR, x.HourOfDay, CAST(DATEADD(DAY, s.InOff + x.DayOff, @Today) AS DATETIME2(0))),
       e.EmployeeId
FROM @Svc x
JOIN @Stay s ON s.K = x.K
JOIN @StayMap sm ON sm.K = x.K
JOIN dbo.Services sv ON sv.ServiceName = x.ServiceName
JOIN dbo.Employees e ON e.Username = s.Emp;

-- 2.4 Phụ thu trả trễ (đúng chính sách fn_LateCheckoutFee)
INSERT INTO dbo.ServiceUsages (BookingRoomId, ServiceId, Quantity, UnitPrice, UsedAt, Note, CreatedBy)
SELECT br.BookingRoomId, sv.ServiceId, 1,
       dbo.fn_LateCheckoutFee(br.PricePerNight, br.CheckOutDate, br.ActualCheckOut),
       br.ActualCheckOut,
       CONCAT(N'Trễ ', DATEDIFF(MINUTE, DATEADD(HOUR, 12, CAST(br.CheckOutDate AS DATETIME2(0))), br.ActualCheckOut), N' phút'),
       br.CheckOutBy
FROM dbo.BookingRooms br
JOIN @StayMap sm ON sm.BookingRoomId = br.BookingRoomId
CROSS JOIN dbo.Services sv
WHERE sv.IsSystem = 1 AND sv.ServiceName = N'Phụ thu trả phòng trễ'
  AND br.Status = 'CheckedOut'
  AND dbo.fn_LateCheckoutFee(br.PricePerNight, br.CheckOutDate, br.ActualCheckOut) > 0;

-- 2.5 Tiền cọc (lúc xác nhận booking)
INSERT INTO dbo.Payments (BookingRoomId, Type, Amount, Method, ReferenceNo, PaidAt, EmployeeId, Note)
SELECT br.BookingRoomId, 'Deposit',
       CAST(ROUND(br.PricePerNight * br.Nights * b.DepositRate / 100.0, 0) AS DECIMAL(14,0)),
       'BankTransfer', CONCAT('DEP', RIGHT('00000' + CAST(br.BookingRoomId AS VARCHAR(10)), 5)),
       b.ConfirmedAt, b.CreatedBy, CONCAT(N'Cọc phiếu đặt #', b.BookingId)
FROM dbo.BookingRooms br
JOIN @StayMap sm ON sm.BookingRoomId = br.BookingRoomId
JOIN dbo.Bookings b ON b.BookingId = br.BookingId;

-- 2.6 Hóa đơn cho các phòng đã trả
INSERT INTO dbo.Invoices (BookingRoomId, Nights, RoomCharge, ServiceCharge, Discount, DiscountReason, TaxRate,
                          Status, IssuedAt, IssuedBy, BuyerCompanyName, BuyerTaxCode, BuyerAddress)
SELECT br.BookingRoomId, br.Nights, x.RoomCharge, x.ServiceCharge,
       CAST(ROUND((x.RoomCharge + x.ServiceCharge) * s.DiscPct / 100.0, 0) AS DECIMAL(14,0)),
       CASE WHEN s.DiscPct > 0 THEN s.DiscReason END,
       10, 'Paid', br.ActualCheckOut, br.CheckOutBy,
       CASE WHEN s.Bk = 5 THEN N'Công ty TNHH Hanbit Việt Nam' END,
       CASE WHEN s.Bk = 5 THEN '0312345678' END,
       CASE WHEN s.Bk = 5 THEN N'Tầng 8, 72 Lê Thánh Tôn, Quận 1, TP. Hồ Chí Minh' END
FROM dbo.BookingRooms br
JOIN @StayMap sm ON sm.BookingRoomId = br.BookingRoomId
JOIN @Stay s ON s.K = sm.K
CROSS APPLY (SELECT CAST(br.PricePerNight * br.Nights AS DECIMAL(14,0)) AS RoomCharge,
                    CAST(ISNULL((SELECT SUM(su.Amount) FROM dbo.ServiceUsages su
                                 WHERE su.BookingRoomId = br.BookingRoomId), 0) AS DECIMAL(14,0)) AS ServiceCharge) x
WHERE br.Status = 'CheckedOut';

-- 2.7 Thu phần còn lại khi trả phòng (hoặc hoàn phần dư), gắn mọi khoản vào hóa đơn
INSERT INTO dbo.Payments (BookingRoomId, InvoiceId, Type, Amount, Method, ReferenceNo, PaidAt, EmployeeId, Note)
SELECT i.BookingRoomId, i.InvoiceId,
       CASE WHEN i.TotalAmount > p.NetPaid THEN 'Payment' ELSE 'Refund' END,
       ABS(i.TotalAmount - p.NetPaid),
       s.Method,
       CASE WHEN s.Method IN ('BankTransfer', 'EWallet') THEN CONCAT('TXN', RIGHT('00000' + CAST(i.InvoiceId AS VARCHAR(10)), 5)) END,
       i.IssuedAt, i.IssuedBy,
       CASE WHEN i.TotalAmount > p.NetPaid THEN N'Thu khi lập hóa đơn' ELSE N'Hoàn tiền thừa khi lập hóa đơn' END
FROM dbo.Invoices i
JOIN @StayMap sm ON sm.BookingRoomId = i.BookingRoomId
JOIN @Stay s ON s.K = sm.K
CROSS APPLY (SELECT SUM(SignedAmount) AS NetPaid FROM dbo.Payments WHERE BookingRoomId = i.BookingRoomId) p
WHERE i.TotalAmount <> p.NetPaid;

UPDATE p SET InvoiceId = i.InvoiceId
FROM dbo.Payments p
JOIN dbo.Invoices i ON i.BookingRoomId = p.BookingRoomId AND i.Status = 'Paid'
WHERE p.InvoiceId IS NULL;

-- 2.8 Việc dọn phòng: lịch sử đã xong; phòng trả hôm nay đang chờ dọn
INSERT INTO dbo.HousekeepingTasks (RoomId, BookingRoomId, Source, Status, Result, AssignedTo, CreatedAt, StartedAt, CompletedAt)
SELECT br.RoomId, br.BookingRoomId, 'CheckOut',
       CASE WHEN s.Kind = 'H' THEN 'Done' ELSE 'Pending' END,
       CASE WHEN s.Kind = 'H' THEN 'Clean' END,
       CASE WHEN s.Kind = 'H' THEN hk.EmployeeId END,
       br.ActualCheckOut,
       CASE WHEN s.Kind = 'H' THEN DATEADD(MINUTE, 20, br.ActualCheckOut) END,
       CASE WHEN s.Kind = 'H' THEN DATEADD(MINUTE, 55, br.ActualCheckOut) END
FROM dbo.BookingRooms br
JOIN @StayMap sm ON sm.BookingRoomId = br.BookingRoomId
JOIN @Stay s ON s.K = sm.K
JOIN dbo.Employees hk ON hk.Username = CASE WHEN br.BookingRoomId % 2 = 0 THEN 'maivt' ELSE 'hungdv' END
WHERE br.Status = 'CheckedOut';

-- 2.9 Trạng thái phòng hiện tại
UPDATE r SET Status = 'Occupied'
FROM dbo.Rooms r JOIN dbo.BookingRooms br ON br.RoomId = r.RoomId
WHERE br.Status = 'CheckedIn';

UPDATE r SET Status = 'Cleaning'
FROM dbo.Rooms r JOIN dbo.HousekeepingTasks t ON t.RoomId = r.RoomId
WHERE t.Status <> 'Done';

UPDATE dbo.Rooms SET Status = 'Maintenance', Note = N'Sửa vòi sen phòng tắm' WHERE RoomNumber = '104';

-- 2.10 Tính lại trạng thái các phiếu
SET @Bk = 0;
WHILE 1 = 1
BEGIN
    SELECT TOP (1) @Bk = Bk, @NewId = BookingId FROM @Map WHERE Bk > @Bk ORDER BY Bk;
    IF @@ROWCOUNT = 0 BREAK;
    EXEC dbo.sp_RefreshBookingStatus @BookingId = @NewId;
END;

UPDATE b SET CancelReason = br.CancelReason, CancelledAt = br.CancelledAt
FROM dbo.Bookings b JOIN dbo.BookingRooms br ON br.BookingId = b.BookingId
WHERE b.Status = 'Cancelled' AND b.CancelReason IS NULL;

COMMIT TRANSACTION;
GO

/* ---------- 3. Booking từ hôm nay trở đi: tạo bằng procedure ---------- */
DECLARE @Today DATE = CAST(GETDATE() AS DATE);
DECLARE @Rooms dbo.BookingRoomInput;
DECLARE @BookingId INT, @Amount DECIMAL(14,0), @Emp INT, @Cust INT;

DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children, GuestName, GuestIdNumber) VALUES
    ((SELECT RoomId FROM dbo.Rooms WHERE RoomNumber = '302'), DATEADD(DAY, 0, @Today), DATEADD(DAY, 2, @Today), 2, 0, NULL, NULL),
    ((SELECT RoomId FROM dbo.Rooms WHERE RoomNumber = '303'), DATEADD(DAY, 0, @Today), DATEADD(DAY, 2, @Today), 2, 1, NULL, NULL);
SELECT @Emp = EmployeeId FROM dbo.Employees WHERE Username = 'hapt';
SELECT @Cust = CustomerId FROM dbo.Customers WHERE IdNumber = '001088012345';
EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 50, @Note = N'Khách đến hôm nay, 2 phòng', @EmployeeId = @Emp,
     @Rooms = @Rooms, @BookingId = @BookingId OUTPUT;
SELECT @Amount = RequiredDeposit FROM dbo.vw_BookingSummary WHERE BookingId = @BookingId;
EXEC dbo.sp_RecordBookingDeposit @BookingId = @BookingId, @Amount = @Amount, @Method = 'BankTransfer',
     @ReferenceNo = 'FT22496', @EmployeeId = @Emp;

DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children, GuestName, GuestIdNumber) VALUES
    ((SELECT RoomId FROM dbo.Rooms WHERE RoomNumber = '501'), DATEADD(DAY, 1, @Today), DATEADD(DAY, 4, @Today), 2, 0, NULL, NULL);
SELECT @Emp = EmployeeId FROM dbo.Employees WHERE Username = 'lantt';
SELECT @Cust = CustomerId FROM dbo.Customers WHERE IdNumber = 'US523847196';
EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 100, @Note = N'Đón sân bay lúc 10h', @EmployeeId = @Emp,
     @Rooms = @Rooms, @BookingId = @BookingId OUTPUT;
SELECT @Amount = RequiredDeposit FROM dbo.vw_BookingSummary WHERE BookingId = @BookingId;
EXEC dbo.sp_RecordBookingDeposit @BookingId = @BookingId, @Amount = @Amount, @Method = 'BankTransfer',
     @ReferenceNo = 'FT23865', @EmployeeId = @Emp;

DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children, GuestName, GuestIdNumber) VALUES
    ((SELECT RoomId FROM dbo.Rooms WHERE RoomNumber = '201'), DATEADD(DAY, 3, @Today), DATEADD(DAY, 5, @Today), 2, 0, NULL, NULL);
SELECT @Emp = EmployeeId FROM dbo.Employees WHERE Username = 'hapt';
SELECT @Cust = CustomerId FROM dbo.Customers WHERE IdNumber = '048085001234';
EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 50, @Note = N'Chờ khách chuyển cọc', @EmployeeId = @Emp,
     @Rooms = @Rooms, @BookingId = @BookingId OUTPUT;

DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children, GuestName, GuestIdNumber) VALUES
    ((SELECT RoomId FROM dbo.Rooms WHERE RoomNumber = '401'), DATEADD(DAY, 7, @Today), DATEADD(DAY, 10, @Today), 4, 0, NULL, NULL),
    ((SELECT RoomId FROM dbo.Rooms WHERE RoomNumber = '402'), DATEADD(DAY, 7, @Today), DATEADD(DAY, 10, @Today), 3, 1, NULL, NULL);
SELECT @Emp = EmployeeId FROM dbo.Employees WHERE Username = 'namnh';
SELECT @Cust = CustomerId FROM dbo.Customers WHERE IdNumber = 'M83920417';
EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 50, @Note = N'Đoàn gia đình', @EmployeeId = @Emp,
     @Rooms = @Rooms, @BookingId = @BookingId OUTPUT;
SELECT @Amount = RequiredDeposit FROM dbo.vw_BookingSummary WHERE BookingId = @BookingId;
EXEC dbo.sp_RecordBookingDeposit @BookingId = @BookingId, @Amount = @Amount, @Method = 'BankTransfer',
     @ReferenceNo = 'FT18315', @EmployeeId = @Emp;

DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children, GuestName, GuestIdNumber) VALUES
    ((SELECT RoomId FROM dbo.Rooms WHERE RoomNumber = '305'), DATEADD(DAY, 10, @Today), DATEADD(DAY, 12, @Today), 2, 0, NULL, NULL);
SELECT @Emp = EmployeeId FROM dbo.Employees WHERE Username = 'lantt';
SELECT @Cust = CustomerId FROM dbo.Customers WHERE IdNumber = 'TK4471280';
EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 100, @Note = NULL, @EmployeeId = @Emp,
     @Rooms = @Rooms, @BookingId = @BookingId OUTPUT;
SELECT @Amount = RequiredDeposit FROM dbo.vw_BookingSummary WHERE BookingId = @BookingId;
EXEC dbo.sp_RecordBookingDeposit @BookingId = @BookingId, @Amount = @Amount, @Method = 'BankTransfer',
     @ReferenceNo = 'FT19277', @EmployeeId = @Emp;

GO

/* ---------- 4. Booking Pending quá 24 giờ, chưa cọc (gọi sp_ExpirePendingBookings sẽ hủy) ---------- */
DECLARE @Today DATE = CAST(GETDATE() AS DATE);
DECLARE @BookingId INT;
INSERT INTO dbo.Bookings (CustomerId, DepositRate, Note, CreatedBy, CreatedAt)
SELECT c.CustomerId, 50, N'Khách hẹn chuyển cọc nhưng chưa chuyển', e.EmployeeId, DATEADD(HOUR, -30, SYSDATETIME())
FROM dbo.Customers c CROSS JOIN dbo.Employees e
WHERE c.IdNumber = '001196009876' AND e.Username = 'hapt';
SET @BookingId = SCOPE_IDENTITY();
INSERT INTO dbo.BookingRooms (BookingId, RoomId, CheckInDate, CheckOutDate, Adults, Children, PricePerNight)
SELECT @BookingId, r.RoomId, DATEADD(DAY, 5, @Today), DATEADD(DAY, 7, @Today), 1, 0, rt.BasePrice
FROM dbo.Rooms r JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
WHERE r.RoomNumber = '102';
GO

SET NOEXEC OFF;
GO

/* ---------- 5. Thống kê nhanh ---------- */
SELECT N'Bookings' AS Bang, COUNT(*) AS SoDong FROM dbo.Bookings
UNION ALL SELECT N'BookingRooms', COUNT(*) FROM dbo.BookingRooms
UNION ALL SELECT N'ServiceUsages', COUNT(*) FROM dbo.ServiceUsages
UNION ALL SELECT N'Invoices', COUNT(*) FROM dbo.Invoices
UNION ALL SELECT N'Payments', COUNT(*) FROM dbo.Payments
UNION ALL SELECT N'HousekeepingTasks', COUNT(*) FROM dbo.HousekeepingTasks;

SELECT Status, COUNT(*) AS SoPhieu FROM dbo.Bookings GROUP BY Status;
SELECT * FROM dbo.vw_RoomStatusSummary;
GO
