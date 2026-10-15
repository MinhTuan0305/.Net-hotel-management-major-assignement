/* =====================================================================
   HOTEL MANAGEMENT - SCRIPT TẠO DATABASE (bản làm lại theo tài liệu
   nghiệp vụ đã rà soát, mục VII)
   ---------------------------------------------------------------------
   Thứ tự: Database -> 14 bảng -> Index -> Kiểu bảng -> Function -> View
           -> Stored procedure -> Dữ liệu khởi tạo
   Chạy trên SQL Server 2016 trở lên (SSMS: mở file, bấm F5).

   CẢNH BÁO: Phần 0 XÓA database HotelManagement cũ (nếu có) rồi tạo lại.
   Sao lưu dữ liệu cũ trước khi chạy nếu cần giữ.

   Quy ước lỗi (THROW) để ứng dụng đổi sang thông báo tiếng Việt:
     50001 Không đủ quyền          50005 Trùng lịch phòng
     50002 Không tìm thấy dữ liệu  50006 Lỗi số tiền / chưa thu đủ
     50003 Sai trạng thái          50007 Sai ngày giờ
     50004 Dữ liệu không hợp lệ
   ===================================================================== */

/* =====================================================================
   0. TẠO DATABASE
   ===================================================================== */
USE master;
GO
IF DB_ID(N'HotelManagement') IS NOT NULL
BEGIN
    ALTER DATABASE HotelManagement SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE HotelManagement;
END;
GO
CREATE DATABASE HotelManagement COLLATE Vietnamese_CI_AS;
GO
USE HotelManagement;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* =====================================================================
   1. BẢNG (14 bảng)
   ===================================================================== */

-- 1. Roles: 4 vai trò cố định
CREATE TABLE dbo.Roles (
    RoleId      INT IDENTITY(1,1) CONSTRAINT PK_Roles PRIMARY KEY,
    RoleName    NVARCHAR(50)  NOT NULL CONSTRAINT UQ_Roles_RoleName UNIQUE,
    Description NVARCHAR(200) NULL,
    CONSTRAINT CK_Roles_RoleName CHECK (RoleName IN (N'Admin', N'Manager', N'Receptionist', N'Housekeeping'))
);
GO

-- 2. Employees: nhân viên, kiêm tài khoản đăng nhập (mật khẩu băm BCrypt ở C#)
CREATE TABLE dbo.Employees (
    EmployeeId   INT IDENTITY(1,1) CONSTRAINT PK_Employees PRIMARY KEY,
    RoleId       INT           NOT NULL CONSTRAINT FK_Employees_Roles REFERENCES dbo.Roles (RoleId),
    FullName     NVARCHAR(100) NOT NULL,
    Username     VARCHAR(50)   NOT NULL CONSTRAINT UQ_Employees_Username UNIQUE,
    PasswordHash VARCHAR(255)  NOT NULL,
    Phone        VARCHAR(20)   NULL,
    Email        VARCHAR(100)  NULL,
    HireDate     DATE          NOT NULL CONSTRAINT DF_Employees_HireDate DEFAULT (CAST(GETDATE() AS DATE)),
    IsActive     BIT           NOT NULL CONSTRAINT DF_Employees_IsActive DEFAULT (1),
    CreatedAt    DATETIME2(0)  NOT NULL CONSTRAINT DF_Employees_CreatedAt DEFAULT (SYSDATETIME())
);
GO

-- 3. Customers: người đặt phòng
CREATE TABLE dbo.Customers (
    CustomerId  INT IDENTITY(1,1) CONSTRAINT PK_Customers PRIMARY KEY,
    FullName    NVARCHAR(100) NOT NULL,
    Gender      NVARCHAR(10)  NULL CONSTRAINT CK_Customers_Gender CHECK (Gender IN (N'Nam', N'Nữ', N'Khác')),
    DateOfBirth DATE          NULL CONSTRAINT CK_Customers_DateOfBirth CHECK (DateOfBirth <= CAST(GETDATE() AS DATE)),
    IdNumber    VARCHAR(20)   NOT NULL CONSTRAINT UQ_Customers_IdNumber UNIQUE,
    Phone       VARCHAR(20)   NOT NULL,
    Email       VARCHAR(100)  NULL,
    Address     NVARCHAR(255) NULL,
    Nationality NVARCHAR(50)  NOT NULL CONSTRAINT DF_Customers_Nationality DEFAULT (N'Việt Nam'),
    CreatedAt   DATETIME2(0)  NOT NULL CONSTRAINT DF_Customers_CreatedAt DEFAULT (SYSDATETIME())
);
GO

-- 4. RoomTypes: loại phòng
CREATE TABLE dbo.RoomTypes (
    RoomTypeId   INT IDENTITY(1,1) CONSTRAINT PK_RoomTypes PRIMARY KEY,
    TypeName     NVARCHAR(50)  NOT NULL CONSTRAINT UQ_RoomTypes_TypeName UNIQUE,
    Description  NVARCHAR(500) NULL,
    BasePrice    DECIMAL(14,0) NOT NULL CONSTRAINT CK_RoomTypes_BasePrice CHECK (BasePrice > 0),
    MaxOccupancy TINYINT       NOT NULL CONSTRAINT CK_RoomTypes_MaxOccupancy CHECK (MaxOccupancy BETWEEN 1 AND 10),
    BedType      NVARCHAR(50)  NULL,
    IsActive     BIT           NOT NULL CONSTRAINT DF_RoomTypes_IsActive DEFAULT (1)
);
GO

-- 5. Rooms: phòng vật lý, trạng thái hiện tại (không có Reserved)
CREATE TABLE dbo.Rooms (
    RoomId     INT IDENTITY(1,1) CONSTRAINT PK_Rooms PRIMARY KEY,
    RoomTypeId INT           NOT NULL CONSTRAINT FK_Rooms_RoomTypes REFERENCES dbo.RoomTypes (RoomTypeId),
    RoomNumber VARCHAR(10)   NOT NULL CONSTRAINT UQ_Rooms_RoomNumber UNIQUE,
    Floor      SMALLINT      NOT NULL,
    Status     VARCHAR(20)   NOT NULL CONSTRAINT DF_Rooms_Status DEFAULT ('Available')
               CONSTRAINT CK_Rooms_Status CHECK (Status IN ('Available', 'Occupied', 'Cleaning', 'Maintenance')),
    Note       NVARCHAR(200) NULL,
    IsActive   BIT           NOT NULL CONSTRAINT DF_Rooms_IsActive DEFAULT (1)
);
GO

-- 6. Amenities: tiện nghi
CREATE TABLE dbo.Amenities (
    AmenityId   INT IDENTITY(1,1) CONSTRAINT PK_Amenities PRIMARY KEY,
    AmenityName NVARCHAR(100) NOT NULL CONSTRAINT UQ_Amenities_AmenityName UNIQUE,
    Description NVARCHAR(200) NULL
);
GO

-- 7. RoomTypeAmenities: N-N, KHÔNG cascade (không xóa tiện nghi đang được gán)
CREATE TABLE dbo.RoomTypeAmenities (
    RoomTypeId INT NOT NULL CONSTRAINT FK_RTA_RoomTypes REFERENCES dbo.RoomTypes (RoomTypeId),
    AmenityId  INT NOT NULL CONSTRAINT FK_RTA_Amenities REFERENCES dbo.Amenities (AmenityId),
    CONSTRAINT PK_RoomTypeAmenities PRIMARY KEY (RoomTypeId, AmenityId)
);
GO

-- 8. Bookings: phiếu đặt của người đặt (trạng thái do hệ thống tính)
CREATE TABLE dbo.Bookings (
    BookingId    INT IDENTITY(1,1) CONSTRAINT PK_Bookings PRIMARY KEY,
    CustomerId   INT           NOT NULL CONSTRAINT FK_Bookings_Customers REFERENCES dbo.Customers (CustomerId),
    DepositRate  TINYINT       NOT NULL CONSTRAINT CK_Bookings_DepositRate CHECK (DepositRate IN (50, 100)),
    Status       VARCHAR(20)   NOT NULL CONSTRAINT DF_Bookings_Status DEFAULT ('Pending')
                 CONSTRAINT CK_Bookings_Status CHECK (Status IN ('Pending', 'Confirmed', 'CheckedIn', 'CheckedOut', 'Cancelled')),
    Note         NVARCHAR(500) NULL,
    CreatedBy    INT           NOT NULL CONSTRAINT FK_Bookings_CreatedBy REFERENCES dbo.Employees (EmployeeId),
    CreatedAt    DATETIME2(0)  NOT NULL CONSTRAINT DF_Bookings_CreatedAt DEFAULT (SYSDATETIME()),
    ConfirmedAt  DATETIME2(0)  NULL,
    CancelledAt  DATETIME2(0)  NULL,
    CancelReason NVARCHAR(200) NULL
);
GO

-- 9. BookingRooms: LƯỢT LƯU TRÚ PHÒNG - đơn vị check-in, dịch vụ, check-out, hóa đơn, thanh toán
CREATE TABLE dbo.BookingRooms (
    BookingRoomId  INT IDENTITY(1,1) CONSTRAINT PK_BookingRooms PRIMARY KEY,
    BookingId      INT            NOT NULL CONSTRAINT FK_BookingRooms_Bookings REFERENCES dbo.Bookings (BookingId),
    RoomId         INT            NOT NULL CONSTRAINT FK_BookingRooms_Rooms REFERENCES dbo.Rooms (RoomId),
    CheckInDate    DATE           NOT NULL,
    CheckOutDate   DATE           NOT NULL,
    Nights         AS (DATEDIFF(DAY, CheckInDate, CheckOutDate)) PERSISTED,
    Adults         TINYINT        NOT NULL CONSTRAINT CK_BookingRooms_Adults CHECK (Adults >= 1),
    Children       TINYINT        NOT NULL CONSTRAINT DF_BookingRooms_Children DEFAULT (0),
    PricePerNight  DECIMAL(14,0)  NOT NULL CONSTRAINT CK_BookingRooms_Price CHECK (PricePerNight > 0),
    Status         VARCHAR(20)    NOT NULL CONSTRAINT DF_BookingRooms_Status DEFAULT ('Booked')
                   CONSTRAINT CK_BookingRooms_Status CHECK (Status IN ('Booked', 'CheckedIn', 'CheckedOut', 'Cancelled')),
    GuestName      NVARCHAR(100)  NULL,
    GuestIdNumber  VARCHAR(20)    NULL,
    ActualCheckIn  DATETIME2(0)   NULL,
    ActualCheckOut DATETIME2(0)   NULL,
    CheckInBy      INT            NULL CONSTRAINT FK_BookingRooms_CheckInBy REFERENCES dbo.Employees (EmployeeId),
    CheckOutBy     INT            NULL CONSTRAINT FK_BookingRooms_CheckOutBy REFERENCES dbo.Employees (EmployeeId),
    CancelledAt    DATETIME2(0)   NULL,
    CancelReason   NVARCHAR(200)  NULL,
    Note           NVARCHAR(1000) NULL,
    CONSTRAINT CK_BookingRooms_Dates CHECK (CheckOutDate > CheckInDate),
    CONSTRAINT CK_BookingRooms_StatusData CHECK (
           (Status = 'Booked'     AND ActualCheckIn IS NULL     AND ActualCheckOut IS NULL)
        OR (Status = 'CheckedIn'  AND ActualCheckIn IS NOT NULL AND ActualCheckOut IS NULL)
        OR (Status = 'CheckedOut' AND ActualCheckIn IS NOT NULL AND ActualCheckOut > ActualCheckIn)
        OR (Status = 'Cancelled'  AND ActualCheckIn IS NULL     AND CancelledAt IS NOT NULL))
);
GO

-- 10. Services: danh mục dịch vụ (IsSystem = dịch vụ do hệ thống ghi, ví dụ phụ thu trả trễ)
CREATE TABLE dbo.Services (
    ServiceId   INT IDENTITY(1,1) CONSTRAINT PK_Services PRIMARY KEY,
    ServiceName NVARCHAR(100) NOT NULL CONSTRAINT UQ_Services_ServiceName UNIQUE,
    Category    NVARCHAR(50)  NULL,
    Unit        NVARCHAR(20)  NOT NULL,
    Price       DECIMAL(14,0) NOT NULL CONSTRAINT CK_Services_Price CHECK (Price >= 0),
    IsActive    BIT           NOT NULL CONSTRAINT DF_Services_IsActive DEFAULT (1),
    IsSystem    BIT           NOT NULL CONSTRAINT DF_Services_IsSystem DEFAULT (0)
);
GO

-- 11. ServiceUsages: dịch vụ dùng theo từng lượt lưu trú
CREATE TABLE dbo.ServiceUsages (
    UsageId       INT IDENTITY(1,1) CONSTRAINT PK_ServiceUsages PRIMARY KEY,
    BookingRoomId INT           NOT NULL CONSTRAINT FK_ServiceUsages_BookingRooms REFERENCES dbo.BookingRooms (BookingRoomId),
    ServiceId     INT           NOT NULL CONSTRAINT FK_ServiceUsages_Services REFERENCES dbo.Services (ServiceId),
    Quantity      INT           NOT NULL CONSTRAINT CK_ServiceUsages_Quantity CHECK (Quantity > 0),
    UnitPrice     DECIMAL(14,0) NOT NULL CONSTRAINT CK_ServiceUsages_UnitPrice CHECK (UnitPrice >= 0),
    Amount        AS (CAST(Quantity * UnitPrice AS DECIMAL(14,0))) PERSISTED,
    UsedAt        DATETIME2(0)  NOT NULL,
    Note          NVARCHAR(200) NULL,
    CreatedBy     INT           NOT NULL CONSTRAINT FK_ServiceUsages_CreatedBy REFERENCES dbo.Employees (EmployeeId),
    CreatedAt     DATETIME2(0)  NOT NULL CONSTRAINT DF_ServiceUsages_CreatedAt DEFAULT (SYSDATETIME())
);
GO

-- 12. Invoices: hóa đơn của từng lượt lưu trú; tối đa 1 hóa đơn Paid / lượt
CREATE TABLE dbo.Invoices (
    InvoiceId         INT IDENTITY(1,1) CONSTRAINT PK_Invoices PRIMARY KEY,
    InvoiceNo         AS ('HD' + RIGHT('000000' + CAST(InvoiceId AS VARCHAR(10)), 6)),
    BookingRoomId     INT           NOT NULL CONSTRAINT FK_Invoices_BookingRooms REFERENCES dbo.BookingRooms (BookingRoomId),
    Nights            SMALLINT      NOT NULL CONSTRAINT CK_Invoices_Nights CHECK (Nights > 0),
    RoomCharge        DECIMAL(14,0) NOT NULL CONSTRAINT CK_Invoices_RoomCharge CHECK (RoomCharge >= 0),
    ServiceCharge     DECIMAL(14,0) NOT NULL CONSTRAINT CK_Invoices_ServiceCharge CHECK (ServiceCharge >= 0),
    Discount          DECIMAL(14,0) NOT NULL CONSTRAINT DF_Invoices_Discount DEFAULT (0),
    DiscountReason    NVARCHAR(200) NULL,
    TaxRate           DECIMAL(5,2)  NOT NULL CONSTRAINT DF_Invoices_TaxRate DEFAULT (10)
                      CONSTRAINT CK_Invoices_TaxRate CHECK (TaxRate BETWEEN 0 AND 100),
    TotalAmount       AS (CAST(ROUND((RoomCharge + ServiceCharge - Discount) * (1 + TaxRate / 100), 0) AS DECIMAL(14,0))) PERSISTED,
    BuyerName         NVARCHAR(150) NULL,
    BuyerCompanyName  NVARCHAR(200) NULL,
    BuyerTaxCode      VARCHAR(20)   NULL,
    BuyerAddress      NVARCHAR(255) NULL,
    Status            VARCHAR(10)   NOT NULL CONSTRAINT DF_Invoices_Status DEFAULT ('Paid')
                      CONSTRAINT CK_Invoices_Status CHECK (Status IN ('Paid', 'Void')),
    IssuedAt          DATETIME2(0)  NOT NULL CONSTRAINT DF_Invoices_IssuedAt DEFAULT (SYSDATETIME()),
    IssuedBy          INT           NOT NULL CONSTRAINT FK_Invoices_IssuedBy REFERENCES dbo.Employees (EmployeeId),
    VoidedAt          DATETIME2(0)  NULL,
    VoidedBy          INT           NULL CONSTRAINT FK_Invoices_VoidedBy REFERENCES dbo.Employees (EmployeeId),
    VoidReason        NVARCHAR(200) NULL,
    ReplacesInvoiceId INT           NULL CONSTRAINT FK_Invoices_Replaces REFERENCES dbo.Invoices (InvoiceId),
    CONSTRAINT CK_Invoices_Discount CHECK (Discount >= 0 AND Discount <= RoomCharge + ServiceCharge),
    CONSTRAINT CK_Invoices_DiscountReason CHECK (Discount = 0 OR DiscountReason IS NOT NULL),
    CONSTRAINT CK_Invoices_Buyer CHECK (BuyerTaxCode IS NULL OR BuyerCompanyName IS NOT NULL),
    CONSTRAINT CK_Invoices_Void CHECK (
           (Status = 'Paid' AND VoidedAt IS NULL AND VoidedBy IS NULL)
        OR (Status = 'Void' AND VoidedAt IS NOT NULL AND VoidedBy IS NOT NULL AND VoidReason IS NOT NULL))
);
GO

-- 13. Payments: cọc / thanh toán / hoàn tiền, gắn với lượt lưu trú
CREATE TABLE dbo.Payments (
    PaymentId     INT IDENTITY(1,1) CONSTRAINT PK_Payments PRIMARY KEY,
    BookingRoomId INT           NOT NULL CONSTRAINT FK_Payments_BookingRooms REFERENCES dbo.BookingRooms (BookingRoomId),
    InvoiceId     INT           NULL CONSTRAINT FK_Payments_Invoices REFERENCES dbo.Invoices (InvoiceId),
    Type          VARCHAR(10)   NOT NULL CONSTRAINT CK_Payments_Type CHECK (Type IN ('Deposit', 'Payment', 'Refund')),
    Amount        DECIMAL(14,0) NOT NULL CONSTRAINT CK_Payments_Amount CHECK (Amount > 0),
    SignedAmount  AS (CAST(CASE WHEN Type = 'Refund' THEN -Amount ELSE Amount END AS DECIMAL(14,0))) PERSISTED,
    Method        VARCHAR(20)   NOT NULL CONSTRAINT CK_Payments_Method CHECK (Method IN ('Cash', 'Card', 'BankTransfer', 'EWallet')),
    ReferenceNo   VARCHAR(50)   NULL,
    PaidAt        DATETIME2(0)  NOT NULL CONSTRAINT DF_Payments_PaidAt DEFAULT (SYSDATETIME()),
    EmployeeId    INT           NOT NULL CONSTRAINT FK_Payments_Employees REFERENCES dbo.Employees (EmployeeId),
    Note          NVARCHAR(200) NULL,
    CONSTRAINT CK_Payments_Reference CHECK (Method NOT IN ('BankTransfer', 'EWallet') OR ReferenceNo IS NOT NULL)
);
GO

-- 14. HousekeepingTasks: việc dọn phòng
CREATE TABLE dbo.HousekeepingTasks (
    TaskId        INT IDENTITY(1,1) CONSTRAINT PK_HousekeepingTasks PRIMARY KEY,
    RoomId        INT           NOT NULL CONSTRAINT FK_HousekeepingTasks_Rooms REFERENCES dbo.Rooms (RoomId),
    BookingRoomId INT           NULL CONSTRAINT FK_HousekeepingTasks_BookingRooms REFERENCES dbo.BookingRooms (BookingRoomId),
    Source        VARCHAR(20)   NOT NULL CONSTRAINT CK_HousekeepingTasks_Source CHECK (Source IN ('CheckOut', 'AfterMaintenance')),
    Status        VARCHAR(20)   NOT NULL CONSTRAINT DF_HousekeepingTasks_Status DEFAULT ('Pending')
                  CONSTRAINT CK_HousekeepingTasks_Status CHECK (Status IN ('Pending', 'InProgress', 'Done')),
    Result        VARCHAR(20)   NULL CONSTRAINT CK_HousekeepingTasks_Result CHECK (Result IN ('Clean', 'NeedMaintenance')),
    AssignedTo    INT           NULL CONSTRAINT FK_HousekeepingTasks_AssignedTo REFERENCES dbo.Employees (EmployeeId),
    CreatedAt     DATETIME2(0)  NOT NULL CONSTRAINT DF_HousekeepingTasks_CreatedAt DEFAULT (SYSDATETIME()),
    StartedAt     DATETIME2(0)  NULL,
    CompletedAt   DATETIME2(0)  NULL,
    Note          NVARCHAR(500) NULL,
    CONSTRAINT CK_HousekeepingTasks_Done CHECK (
           (Status <> 'Done' AND Result IS NULL AND CompletedAt IS NULL)
        OR (Status = 'Done'  AND Result IS NOT NULL AND CompletedAt IS NOT NULL)),
    CONSTRAINT CK_HousekeepingTasks_Note CHECK (Result IS NULL OR Result = 'Clean' OR Note IS NOT NULL)
);
GO

/* =====================================================================
   2. INDEX VÀ RÀNG BUỘC ĐẶC BIỆT
   ===================================================================== */
CREATE INDEX IX_BookingRooms_Room_Dates ON dbo.BookingRooms (RoomId, CheckInDate, CheckOutDate)
    INCLUDE (Status) WHERE Status IN ('Booked', 'CheckedIn');
CREATE INDEX IX_BookingRooms_Booking ON dbo.BookingRooms (BookingId);
CREATE UNIQUE INDEX UX_Invoices_ActivePerStay ON dbo.Invoices (BookingRoomId) WHERE Status = 'Paid';
CREATE INDEX IX_Invoices_IssuedAt ON dbo.Invoices (IssuedAt) WHERE Status = 'Paid';
CREATE UNIQUE INDEX UX_HousekeepingTasks_OpenTask ON dbo.HousekeepingTasks (RoomId) WHERE Status <> 'Done';
CREATE INDEX IX_Payments_BookingRoom ON dbo.Payments (BookingRoomId) INCLUDE (SignedAmount, Type);
CREATE INDEX IX_Payments_PaidAt ON dbo.Payments (PaidAt) INCLUDE (SignedAmount, Method, Type);
CREATE INDEX IX_ServiceUsages_BookingRoom ON dbo.ServiceUsages (BookingRoomId);
CREATE INDEX IX_Bookings_Status_CreatedAt ON dbo.Bookings (Status, CreatedAt);
CREATE INDEX IX_Bookings_Customer ON dbo.Bookings (CustomerId);
CREATE INDEX IX_Customers_Phone ON dbo.Customers (Phone);
CREATE INDEX IX_Customers_FullName ON dbo.Customers (FullName);
GO

/* =====================================================================
   3. KIỂU BẢNG: danh sách phòng truyền vào sp_CreateBooking
      (C#: SqlParameter SqlDbType.Structured, TypeName = "dbo.BookingRoomInput")
   ===================================================================== */
CREATE TYPE dbo.BookingRoomInput AS TABLE (
    RoomId        INT           NOT NULL,
    CheckInDate   DATE          NOT NULL,
    CheckOutDate  DATE          NOT NULL,
    Adults        TINYINT       NOT NULL,
    Children      TINYINT       NOT NULL DEFAULT (0),
    GuestName     NVARCHAR(100) NULL,
    GuestIdNumber VARCHAR(20)   NULL
);
GO

/* =====================================================================
   4. FUNCTION
   ===================================================================== */

-- Nhân viên đang hoạt động có thuộc một trong các vai trò (danh sách cách nhau dấu phẩy) không
CREATE FUNCTION dbo.fn_HasRole (@EmployeeId INT, @Roles NVARCHAR(200))
RETURNS BIT
AS
BEGIN
    DECLARE @Result BIT = 0;
    IF EXISTS (SELECT 1
               FROM dbo.Employees e
               JOIN dbo.Roles r ON r.RoleId = e.RoleId
               WHERE e.EmployeeId = @EmployeeId
                 AND e.IsActive = 1
                 AND CHARINDEX(N',' + r.RoleName + N',', N',' + REPLACE(@Roles, N' ', N'') + N',') > 0)
        SET @Result = 1;
    RETURN @Result;
END;
GO

-- Phụ thu trả phòng trễ (mục 11). Mốc 12:00 ngày CheckOutDate, tính trên giá 1 đêm.
--   L <= 30 phút: 0 | 30 < L <= 120: CEILING((L-30)/30) x 5% | 120 < L <= 360: 15% + CEILING((L-120)/60) x 15%
--   L > 360 (sau 18:00): trả NULL = bắt buộc gia hạn thêm 1 đêm
CREATE FUNCTION dbo.fn_LateCheckoutFee (@PricePerNight DECIMAL(14,0), @CheckOutDate DATE, @ActualCheckOut DATETIME2(0))
RETURNS DECIMAL(14,0)
AS
BEGIN
    DECLARE @LateMinutes INT = DATEDIFF(MINUTE, DATEADD(HOUR, 12, CAST(@CheckOutDate AS DATETIME2(0))), @ActualCheckOut);
    DECLARE @Percent INT;

    IF @LateMinutes <= 30
        SET @Percent = 0;
    ELSE IF @LateMinutes <= 120
        SET @Percent = CEILING((@LateMinutes - 30) / 30.0) * 5;
    ELSE IF @LateMinutes <= 360
        SET @Percent = 15 + CEILING((@LateMinutes - 120) / 60.0) * 15;
    ELSE
        RETURN NULL;

    RETURN CAST(ROUND(@PricePerNight * @Percent / 100.0, 0) AS DECIMAL(14,0));
END;
GO

-- Tổng tiền đã thu ròng của một lượt lưu trú (thu - hoàn)
CREATE FUNCTION dbo.fn_StayNetPaid (@BookingRoomId INT)
RETURNS DECIMAL(14,0)
AS
BEGIN
    DECLARE @NetPaid DECIMAL(14,0);
    SELECT @NetPaid = SUM(SignedAmount) FROM dbo.Payments WHERE BookingRoomId = @BookingRoomId;
    RETURN ISNULL(@NetPaid, 0);
END;
GO

-- Cọc yêu cầu của một lượt lưu trú = ROUND(PricePerNight x Nights x DepositRate / 100)
CREATE FUNCTION dbo.fn_RequiredDeposit (@BookingRoomId INT)
RETURNS DECIMAL(14,0)
AS
BEGIN
    DECLARE @Required DECIMAL(14,0);
    SELECT @Required = CAST(ROUND(br.PricePerNight * br.Nights * b.DepositRate / 100.0, 0) AS DECIMAL(14,0))
    FROM dbo.BookingRooms br
    JOIN dbo.Bookings b ON b.BookingId = br.BookingId
    WHERE br.BookingRoomId = @BookingRoomId;
    RETURN ISNULL(@Required, 0);
END;
GO

-- Phòng trống trong khoảng [@CheckIn, @CheckOut): đang khai thác, không bảo trì, không trùng lượt Booked/CheckedIn
CREATE FUNCTION dbo.fn_AvailableRooms (@CheckIn DATE, @CheckOut DATE, @ExcludeBookingRoomId INT)
RETURNS TABLE
AS
RETURN
    SELECT r.RoomId, r.RoomNumber, r.Floor, r.Status AS RoomStatus,
           rt.RoomTypeId, rt.TypeName, rt.BasePrice, rt.MaxOccupancy, rt.BedType
    FROM dbo.Rooms r
    JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
    WHERE @CheckOut > @CheckIn
      AND r.IsActive = 1
      AND rt.IsActive = 1
      AND r.Status <> 'Maintenance'
      AND NOT EXISTS (SELECT 1
                      FROM dbo.BookingRooms br
                      WHERE br.RoomId = r.RoomId
                        AND br.Status IN ('Booked', 'CheckedIn')
                        AND br.CheckInDate < @CheckOut
                        AND br.CheckOutDate > @CheckIn
                        AND (@ExcludeBookingRoomId IS NULL OR br.BookingRoomId <> @ExcludeBookingRoomId));
GO

-- Bảng tính check-out của một lượt lưu trú tại thời điểm @CheckOutAt
-- TotalAmount dùng đúng công thức cột tính Invoices.TotalAmount
CREATE FUNCTION dbo.fn_CheckOutQuote (@BookingRoomId INT, @CheckOutAt DATETIME2(0), @Discount DECIMAL(14,0), @TaxRate DECIMAL(5,2))
RETURNS TABLE
AS
RETURN
    WITH Base AS (
        SELECT br.BookingRoomId, br.Nights, br.PricePerNight, br.CheckOutDate,
               CAST(br.PricePerNight * br.Nights AS DECIMAL(14,0)) AS RoomCharge,
               CAST(ISNULL((SELECT SUM(su.Amount) FROM dbo.ServiceUsages su
                            WHERE su.BookingRoomId = br.BookingRoomId), 0) AS DECIMAL(14,0)) AS ServiceChargeBefore,
               DATEDIFF(MINUTE, DATEADD(HOUR, 12, CAST(br.CheckOutDate AS DATETIME2(0))), @CheckOutAt) AS RawLateMinutes,
               dbo.fn_LateCheckoutFee(br.PricePerNight, br.CheckOutDate, @CheckOutAt) AS LateFee,
               CAST(ISNULL((SELECT SUM(p.SignedAmount) FROM dbo.Payments p
                            WHERE p.BookingRoomId = br.BookingRoomId), 0) AS DECIMAL(14,0)) AS NetPaid
        FROM dbo.BookingRooms br
        WHERE br.BookingRoomId = @BookingRoomId
    ), Calc AS (
        SELECT Base.*,
               CAST(Base.ServiceChargeBefore + ISNULL(Base.LateFee, 0) AS DECIMAL(14,0)) AS ServiceCharge
        FROM Base
    )
    SELECT Calc.BookingRoomId,
           Calc.Nights,
           Calc.PricePerNight,
           Calc.RoomCharge,
           Calc.ServiceChargeBefore,
           CASE WHEN Calc.RawLateMinutes > 0 THEN Calc.RawLateMinutes ELSE 0 END AS LateMinutes,
           Calc.LateFee,
           CAST(CASE WHEN Calc.LateFee IS NULL THEN 1 ELSE 0 END AS BIT) AS MustExtend,
           Calc.ServiceCharge,
           CAST(Calc.RoomCharge + Calc.ServiceCharge AS DECIMAL(14,0)) AS SubTotal,
           CAST(ROUND((Calc.RoomCharge + Calc.ServiceCharge - @Discount) * (1 + @TaxRate / 100), 0) AS DECIMAL(14,0)) AS TotalAmount,
           Calc.NetPaid,
           CAST(CAST(ROUND((Calc.RoomCharge + Calc.ServiceCharge - @Discount) * (1 + @TaxRate / 100), 0) AS DECIMAL(14,0))
                - Calc.NetPaid AS DECIMAL(14,0)) AS Balance
    FROM Calc;
GO

/* =====================================================================
   5. VIEW
   ===================================================================== */

-- Lượt lưu trú kèm phiếu, người đặt, người ở, phòng, loại phòng
CREATE VIEW dbo.vw_RoomStays
AS
SELECT br.BookingRoomId, br.BookingId, b.Status AS BookingStatus, b.DepositRate,
       b.CustomerId, c.FullName AS CustomerName, c.Phone AS CustomerPhone, c.IdNumber AS CustomerIdNumber,
       COALESCE(br.GuestName, c.FullName) AS GuestName,
       COALESCE(br.GuestIdNumber, c.IdNumber) AS GuestIdNumber,
       br.RoomId, r.RoomNumber, r.Floor, r.Status AS RoomStatus,
       rt.RoomTypeId, rt.TypeName,
       br.CheckInDate, br.CheckOutDate, br.Nights, br.Adults, br.Children, br.PricePerNight,
       br.Status AS StayStatus, br.ActualCheckIn, br.ActualCheckOut,
       br.CheckInBy, ein.FullName AS CheckInByName,
       br.CheckOutBy, eout.FullName AS CheckOutByName,
       br.CancelledAt, br.CancelReason, br.Note
FROM dbo.BookingRooms br
JOIN dbo.Bookings b   ON b.BookingId = br.BookingId
JOIN dbo.Customers c  ON c.CustomerId = b.CustomerId
JOIN dbo.Rooms r      ON r.RoomId = br.RoomId
JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
LEFT JOIN dbo.Employees ein  ON ein.EmployeeId = br.CheckInBy
LEFT JOIN dbo.Employees eout ON eout.EmployeeId = br.CheckOutBy;
GO

-- Số dư từng lượt lưu trú (EstimatedTotal tính VAT mặc định 10%, chưa trừ giảm giá)
CREATE VIEW dbo.vw_RoomStayFolio
AS
SELECT br.BookingRoomId, br.BookingId, br.RoomId, r.RoomNumber, br.Status AS StayStatus,
       br.Nights, br.PricePerNight,
       CAST(br.PricePerNight * br.Nights AS DECIMAL(14,0)) AS PlannedRoomCharge,
       CAST(ISNULL(su.ServiceCharge, 0) AS DECIMAL(14,0)) AS ServiceCharge,
       CAST(ROUND(br.PricePerNight * br.Nights * b.DepositRate / 100.0, 0) AS DECIMAL(14,0)) AS RequiredDeposit,
       CAST(ISNULL(p.DepositPaid, 0) AS DECIMAL(14,0)) AS DepositPaid,
       CAST(ISNULL(p.OtherPaid, 0) AS DECIMAL(14,0)) AS OtherPaid,
       CAST(ISNULL(p.Refunded, 0) AS DECIMAL(14,0)) AS Refunded,
       CAST(ISNULL(p.NetPaid, 0) AS DECIMAL(14,0)) AS NetPaid,
       CAST(ROUND((br.PricePerNight * br.Nights + ISNULL(su.ServiceCharge, 0)) * 1.10, 0) AS DECIMAL(14,0)) AS EstimatedTotal,
       CAST(ROUND((br.PricePerNight * br.Nights + ISNULL(su.ServiceCharge, 0)) * 1.10, 0) - ISNULL(p.NetPaid, 0) AS DECIMAL(14,0)) AS EstimatedBalance,
       CAST(CASE WHEN EXISTS (SELECT 1 FROM dbo.Invoices i
                              WHERE i.BookingRoomId = br.BookingRoomId AND i.Status = 'Paid')
                 THEN 1 ELSE 0 END AS BIT) AS HasActiveInvoice
FROM dbo.BookingRooms br
JOIN dbo.Bookings b ON b.BookingId = br.BookingId
JOIN dbo.Rooms r    ON r.RoomId = br.RoomId
OUTER APPLY (SELECT SUM(x.Amount) AS ServiceCharge
             FROM dbo.ServiceUsages x
             WHERE x.BookingRoomId = br.BookingRoomId) su
OUTER APPLY (SELECT SUM(CASE WHEN y.Type = 'Deposit' THEN y.Amount ELSE 0 END) AS DepositPaid,
                    SUM(CASE WHEN y.Type = 'Payment' THEN y.Amount ELSE 0 END) AS OtherPaid,
                    SUM(CASE WHEN y.Type = 'Refund'  THEN y.Amount ELSE 0 END) AS Refunded,
                    SUM(y.SignedAmount) AS NetPaid
             FROM dbo.Payments y
             WHERE y.BookingRoomId = br.BookingRoomId) p;
GO

-- Tổng hợp theo phiếu đặt
CREATE VIEW dbo.vw_BookingSummary
AS
SELECT b.BookingId, b.Status, b.DepositRate,
       b.CustomerId, c.FullName AS CustomerName, c.Phone AS CustomerPhone,
       b.CreatedAt, b.CreatedBy, e.FullName AS CreatedByName,
       b.ConfirmedAt, b.CancelledAt, b.CancelReason, b.Note,
       ISNULL(x.RoomCount, 0) AS RoomCount,
       ISNULL(x.BookedCount, 0) AS BookedCount,
       ISNULL(x.InHouseCount, 0) AS InHouseCount,
       ISNULL(x.CheckedOutCount, 0) AS CheckedOutCount,
       ISNULL(x.CancelledCount, 0) AS CancelledCount,
       x.FirstCheckIn, x.LastCheckOut,
       CAST(ISNULL(x.RequiredDeposit, 0) AS DECIMAL(14,0)) AS RequiredDeposit,
       CAST(ISNULL(x.NetPaid, 0) AS DECIMAL(14,0)) AS NetPaid,
       CAST(ISNULL(x.MissingDeposit, 0) AS DECIMAL(14,0)) AS MissingDeposit,
       CAST(CASE WHEN ISNULL(x.BookedCount, 0) > 0 AND ISNULL(x.MissingDeposit, 0) = 0 THEN 1 ELSE 0 END AS BIT) AS DepositEnough
FROM dbo.Bookings b
JOIN dbo.Customers c ON c.CustomerId = b.CustomerId
JOIN dbo.Employees e ON e.EmployeeId = b.CreatedBy
OUTER APPLY (
    SELECT COUNT(*) AS RoomCount,
           SUM(CASE WHEN f.StayStatus = 'Booked'     THEN 1 ELSE 0 END) AS BookedCount,
           SUM(CASE WHEN f.StayStatus = 'CheckedIn'  THEN 1 ELSE 0 END) AS InHouseCount,
           SUM(CASE WHEN f.StayStatus = 'CheckedOut' THEN 1 ELSE 0 END) AS CheckedOutCount,
           SUM(CASE WHEN f.StayStatus = 'Cancelled'  THEN 1 ELSE 0 END) AS CancelledCount,
           MIN(CASE WHEN f.StayStatus <> 'Cancelled' THEN br.CheckInDate END) AS FirstCheckIn,
           MAX(CASE WHEN f.StayStatus <> 'Cancelled' THEN br.CheckOutDate END) AS LastCheckOut,
           SUM(CASE WHEN f.StayStatus <> 'Cancelled' THEN f.RequiredDeposit ELSE 0 END) AS RequiredDeposit,
           SUM(f.NetPaid) AS NetPaid,
           SUM(CASE WHEN f.StayStatus = 'Booked' AND f.RequiredDeposit > f.NetPaid
                    THEN f.RequiredDeposit - f.NetPaid ELSE 0 END) AS MissingDeposit
    FROM dbo.vw_RoomStayFolio f
    JOIN dbo.BookingRooms br ON br.BookingRoomId = f.BookingRoomId
    WHERE f.BookingId = b.BookingId) x;
GO

-- Hóa đơn kèm thông tin khách, phòng, nhân viên, đã thu
CREATE VIEW dbo.vw_InvoiceDetail
AS
SELECT i.InvoiceId, i.InvoiceNo, i.Status, i.IssuedAt, i.IssuedBy, eis.FullName AS IssuedByName,
       i.BookingRoomId, br.BookingId, c.CustomerId, c.FullName AS CustomerName, c.Phone AS CustomerPhone,
       COALESCE(br.GuestName, c.FullName) AS GuestName,
       r.RoomNumber, rt.TypeName,
       br.CheckInDate, br.CheckOutDate, br.ActualCheckIn, br.ActualCheckOut,
       i.Nights, br.PricePerNight, i.RoomCharge, i.ServiceCharge, i.Discount, i.DiscountReason, i.TaxRate,
       CAST(i.TotalAmount - (i.RoomCharge + i.ServiceCharge - i.Discount) AS DECIMAL(14,0)) AS TaxAmount,
       i.TotalAmount,
       i.BuyerName, i.BuyerCompanyName, i.BuyerTaxCode, i.BuyerAddress,
       i.VoidedAt, i.VoidedBy, evo.FullName AS VoidedByName, i.VoidReason, i.ReplacesInvoiceId,
       CAST(ISNULL(pay.NetPaid, 0) AS DECIMAL(14,0)) AS StayNetPaid
FROM dbo.Invoices i
JOIN dbo.BookingRooms br ON br.BookingRoomId = i.BookingRoomId
JOIN dbo.Bookings b      ON b.BookingId = br.BookingId
JOIN dbo.Customers c     ON c.CustomerId = b.CustomerId
JOIN dbo.Rooms r         ON r.RoomId = br.RoomId
JOIN dbo.RoomTypes rt    ON rt.RoomTypeId = r.RoomTypeId
JOIN dbo.Employees eis   ON eis.EmployeeId = i.IssuedBy
LEFT JOIN dbo.Employees evo ON evo.EmployeeId = i.VoidedBy
OUTER APPLY (SELECT SUM(p.SignedAmount) AS NetPaid
             FROM dbo.Payments p
             WHERE p.BookingRoomId = i.BookingRoomId) pay;
GO

-- Dòng in hóa đơn: 1 dòng tiền phòng + các dòng dịch vụ
-- (hóa đơn Paid luôn khớp vì dịch vụ bị khóa sau khi lập hóa đơn)
CREATE VIEW dbo.vw_InvoiceLines
AS
SELECT i.InvoiceId,
       CAST(1 AS BIGINT) AS LineNumber,
       CAST(N'Tiền phòng ' + r.RoomNumber + N' (' + rt.TypeName + N')' AS NVARCHAR(300)) AS Description,
       CAST(i.Nights AS INT) AS Quantity,
       CAST(N'Đêm' AS NVARCHAR(20)) AS Unit,
       br.PricePerNight AS UnitPrice,
       i.RoomCharge AS Amount,
       CAST(NULL AS DATETIME2(0)) AS UsedAt
FROM dbo.Invoices i
JOIN dbo.BookingRooms br ON br.BookingRoomId = i.BookingRoomId
JOIN dbo.Rooms r         ON r.RoomId = br.RoomId
JOIN dbo.RoomTypes rt    ON rt.RoomTypeId = r.RoomTypeId
UNION ALL
SELECT i.InvoiceId,
       1 + ROW_NUMBER() OVER (PARTITION BY i.InvoiceId ORDER BY su.UsedAt, su.UsageId),
       CAST(s.ServiceName + ISNULL(N' - ' + su.Note, N'') AS NVARCHAR(300)),
       su.Quantity,
       s.Unit,
       su.UnitPrice,
       su.Amount,
       su.UsedAt
FROM dbo.Invoices i
JOIN dbo.ServiceUsages su ON su.BookingRoomId = i.BookingRoomId
JOIN dbo.Services s       ON s.ServiceId = su.ServiceId;
GO

-- Bảng kê theo phiếu đặt: tổng các hóa đơn Paid (chỉ để đối chiếu, không phải hóa đơn)
CREATE VIEW dbo.vw_BookingStatement
AS
SELECT b.BookingId, b.CustomerId, c.FullName AS CustomerName,
       COUNT(i.InvoiceId) AS InvoiceCount,
       CAST(SUM(i.RoomCharge) AS DECIMAL(14,0)) AS RoomCharge,
       CAST(SUM(i.ServiceCharge) AS DECIMAL(14,0)) AS ServiceCharge,
       CAST(SUM(i.Discount) AS DECIMAL(14,0)) AS Discount,
       CAST(SUM(i.TotalAmount - (i.RoomCharge + i.ServiceCharge - i.Discount)) AS DECIMAL(14,0)) AS TaxAmount,
       CAST(SUM(i.TotalAmount) AS DECIMAL(14,0)) AS TotalAmount
FROM dbo.Bookings b
JOIN dbo.Customers c     ON c.CustomerId = b.CustomerId
JOIN dbo.BookingRooms br ON br.BookingId = b.BookingId
JOIN dbo.Invoices i      ON i.BookingRoomId = br.BookingRoomId AND i.Status = 'Paid'
GROUP BY b.BookingId, b.CustomerId, c.FullName;
GO

-- Sơ đồ phòng: trạng thái hiện tại, khách đang ở, khách đến hôm nay
CREATE VIEW dbo.vw_RoomBoard
AS
SELECT r.RoomId, r.RoomNumber, r.Floor, rt.TypeName, rt.MaxOccupancy, rt.BasePrice,
       r.Status AS RoomStatus, r.Note, r.IsActive,
       cur.BookingRoomId AS CurrentStayId, cur.GuestName AS CurrentGuest, cur.CheckOutDate AS ExpectedCheckOut,
       arr.BookingRoomId AS ArrivalStayId, arr.GuestName AS ArrivingGuest
FROM dbo.Rooms r
JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
OUTER APPLY (SELECT TOP (1) s.BookingRoomId, s.GuestName, s.CheckOutDate
             FROM dbo.vw_RoomStays s
             WHERE s.RoomId = r.RoomId AND s.StayStatus = 'CheckedIn'
             ORDER BY s.ActualCheckIn DESC) cur
OUTER APPLY (SELECT TOP (1) s.BookingRoomId, s.GuestName
             FROM dbo.vw_RoomStays s
             WHERE s.RoomId = r.RoomId AND s.StayStatus = 'Booked'
               AND s.CheckInDate = CAST(GETDATE() AS DATE)
             ORDER BY s.BookingRoomId) arr;
GO

-- Khách đến hôm nay (lượt Booked có ngày nhận = hôm nay)
CREATE VIEW dbo.vw_TodayArrivals
AS
SELECT s.*
FROM dbo.vw_RoomStays s
WHERE s.StayStatus = 'Booked' AND s.CheckInDate = CAST(GETDATE() AS DATE);
GO

-- Khách đang ở
CREATE VIEW dbo.vw_InHouseStays
AS
SELECT s.*
FROM dbo.vw_RoomStays s
WHERE s.StayStatus = 'CheckedIn';
GO

-- Dự kiến trả hôm nay (gồm cả lượt quá hạn trả mà chưa check-out)
CREATE VIEW dbo.vw_TodayDepartures
AS
SELECT s.*
FROM dbo.vw_RoomStays s
WHERE s.StayStatus = 'CheckedIn' AND s.CheckOutDate <= CAST(GETDATE() AS DATE);
GO

-- Sổ thu: dùng cho báo cáo doanh thu thực thu (cộng SignedAmount)
CREATE VIEW dbo.vw_PaymentLedger
AS
SELECT p.PaymentId, p.PaidAt, CAST(p.PaidAt AS DATE) AS PaidDate,
       p.Type, p.Method, p.Amount, p.SignedAmount, p.ReferenceNo, p.Note,
       p.BookingRoomId, br.BookingId, r.RoomNumber, c.FullName AS CustomerName,
       p.InvoiceId, i.InvoiceNo, p.EmployeeId, e.FullName AS EmployeeName
FROM dbo.Payments p
JOIN dbo.BookingRooms br ON br.BookingRoomId = p.BookingRoomId
JOIN dbo.Bookings b      ON b.BookingId = br.BookingId
JOIN dbo.Customers c     ON c.CustomerId = b.CustomerId
JOIN dbo.Rooms r         ON r.RoomId = br.RoomId
JOIN dbo.Employees e     ON e.EmployeeId = p.EmployeeId
LEFT JOIN dbo.Invoices i ON i.InvoiceId = p.InvoiceId;
GO

-- Thống kê dịch vụ theo ngày sử dụng (lọc khoảng ngày rồi SUM ở ứng dụng/truy vấn)
CREATE VIEW dbo.vw_ServiceUsageStats
AS
SELECT s.ServiceId, s.ServiceName, s.Category, s.IsSystem,
       CAST(su.UsedAt AS DATE) AS UsedDate,
       COUNT(*) AS UsageCount,
       SUM(su.Quantity) AS TotalQuantity,
       CAST(SUM(su.Amount) AS DECIMAL(14,0)) AS TotalAmount
FROM dbo.ServiceUsages su
JOIN dbo.Services s ON s.ServiceId = su.ServiceId
GROUP BY s.ServiceId, s.ServiceName, s.Category, s.IsSystem, CAST(su.UsedAt AS DATE);
GO

-- Hàng đợi dọn phòng
CREATE VIEW dbo.vw_HousekeepingQueue
AS
SELECT t.TaskId, t.RoomId, r.RoomNumber, r.Floor, rt.TypeName, r.Status AS RoomStatus,
       t.BookingRoomId, t.Source, t.Status, t.AssignedTo, e.FullName AS AssignedToName,
       t.CreatedAt, t.StartedAt, t.Note,
       DATEDIFF(MINUTE, t.CreatedAt, SYSDATETIME()) AS WaitingMinutes
FROM dbo.HousekeepingTasks t
JOIN dbo.Rooms r      ON r.RoomId = t.RoomId
JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
LEFT JOIN dbo.Employees e ON e.EmployeeId = t.AssignedTo
WHERE t.Status <> 'Done';
GO

-- Đếm phòng đang khai thác theo trạng thái (1 dòng)
CREATE VIEW dbo.vw_RoomStatusSummary
AS
SELECT COUNT(*) AS TotalRooms,
       SUM(CASE WHEN Status = 'Available'   THEN 1 ELSE 0 END) AS AvailableRooms,
       SUM(CASE WHEN Status = 'Occupied'    THEN 1 ELSE 0 END) AS OccupiedRooms,
       SUM(CASE WHEN Status = 'Cleaning'    THEN 1 ELSE 0 END) AS CleaningRooms,
       SUM(CASE WHEN Status = 'Maintenance' THEN 1 ELSE 0 END) AS MaintenanceRooms
FROM dbo.Rooms
WHERE IsActive = 1;
GO

/* =====================================================================
   6. STORED PROCEDURE
   Mẫu chung: SET XACT_ABORT ON; chỉ mở/commit transaction khi chưa có
   transaction bên ngoài (@OwnTran), để gọi lồng nhau an toàn.
   ===================================================================== */

/* ---------- 6.1 Thủ tục nội bộ ---------- */

-- Tính lại trạng thái phiếu đặt từ trạng thái các phòng và tiền cọc (mục I.2)
CREATE PROCEDURE dbo.sp_RefreshBookingStatus
    @BookingId INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Active INT, @Booked INT, @InHouse INT, @CheckedOut INT, @Missing DECIMAL(14,0);

    SELECT @Active     = SUM(CASE WHEN f.StayStatus <> 'Cancelled' THEN 1 ELSE 0 END),
           @Booked     = SUM(CASE WHEN f.StayStatus = 'Booked' THEN 1 ELSE 0 END),
           @InHouse    = SUM(CASE WHEN f.StayStatus = 'CheckedIn' THEN 1 ELSE 0 END),
           @CheckedOut = SUM(CASE WHEN f.StayStatus = 'CheckedOut' THEN 1 ELSE 0 END),
           @Missing    = SUM(CASE WHEN f.StayStatus = 'Booked' AND f.RequiredDeposit > f.NetPaid
                                  THEN f.RequiredDeposit - f.NetPaid ELSE 0 END)
    FROM dbo.vw_RoomStayFolio f
    WHERE f.BookingId = @BookingId;

    DECLARE @NewStatus VARCHAR(20) =
        CASE WHEN ISNULL(@Active, 0) = 0 THEN 'Cancelled'
             WHEN @InHouse > 0 OR (@CheckedOut > 0 AND @Booked > 0) THEN 'CheckedIn'
             WHEN @CheckedOut = @Active THEN 'CheckedOut'
             WHEN ISNULL(@Missing, 0) = 0 THEN 'Confirmed'
             ELSE 'Pending' END;

    UPDATE dbo.Bookings
    SET Status      = @NewStatus,
        ConfirmedAt = CASE WHEN @NewStatus IN ('Confirmed', 'CheckedIn', 'CheckedOut') AND ConfirmedAt IS NULL
                           THEN SYSDATETIME() ELSE ConfirmedAt END,
        CancelledAt = CASE WHEN @NewStatus = 'Cancelled' AND CancelledAt IS NULL
                           THEN SYSDATETIME() ELSE CancelledAt END
    WHERE BookingId = @BookingId;
END;
GO

-- Kiểm tra được phép thêm/sửa/xóa dịch vụ của lượt lưu trú hay không (mục 10)
CREATE PROCEDURE dbo.usp_CheckServiceEditable
    @BookingRoomId INT,
    @EmployeeId    INT,
    @UsedAt        DATETIME2(0) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Status VARCHAR(20), @CheckInAt DATETIME2(0), @CheckOutAt DATETIME2(0);

    SELECT @Status = Status, @CheckInAt = ActualCheckIn, @CheckOutAt = ActualCheckOut
    FROM dbo.BookingRooms
    WHERE BookingRoomId = @BookingRoomId;

    IF @Status IS NULL
        THROW 50002, N'Không tìm thấy lượt lưu trú.', 1;

    IF @Status = 'CheckedIn'
    BEGIN
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
            THROW 50001, N'Bạn không có quyền ghi dịch vụ.', 1;
        IF @UsedAt IS NOT NULL AND (@UsedAt < @CheckInAt OR @UsedAt > DATEADD(MINUTE, 1, SYSDATETIME()))
            THROW 50007, N'Thời điểm sử dụng phải nằm trong thời gian lưu trú và không ở tương lai.', 1;
    END
    ELSE IF @Status = 'CheckedOut'
    BEGIN
        IF EXISTS (SELECT 1 FROM dbo.Invoices WHERE BookingRoomId = @BookingRoomId AND Status = 'Paid')
            THROW 50003, N'Phòng đã có hóa đơn nên dịch vụ bị khóa. Muốn sửa phải void hóa đơn trước.', 1;
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager') = 0
            THROW 50001, N'Chỉ Admin/Manager được sửa dịch vụ sau khi void hóa đơn.', 1;
        IF @UsedAt IS NOT NULL AND (@UsedAt < @CheckInAt OR @UsedAt > @CheckOutAt)
            THROW 50007, N'Thời điểm sử dụng phải nằm trong thời gian lưu trú.', 1;
    END
    ELSE
        THROW 50003, N'Chỉ ghi dịch vụ cho phòng đang ở.', 1;
END;
GO

-- Kiểm tra giảm giá: không âm, có lý do, không vượt tổng, Receptionist tối đa 10%
CREATE PROCEDURE dbo.usp_CheckDiscount
    @Discount       DECIMAL(14,0),
    @DiscountReason NVARCHAR(200),
    @SubTotal       DECIMAL(14,0),
    @TaxRate        DECIMAL(5,2),
    @EmployeeId     INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @Discount IS NULL OR @Discount < 0
        THROW 50004, N'Giảm giá không được âm.', 1;
    IF @Discount > @SubTotal
        THROW 50004, N'Giảm giá không được vượt tổng tiền phòng và dịch vụ.', 1;
    IF @Discount > 0 AND NULLIF(LTRIM(RTRIM(@DiscountReason)), N'') IS NULL
        THROW 50004, N'Phải ghi lý do giảm giá.', 1;
    IF @Discount * 10 > @SubTotal AND dbo.fn_HasRole(@EmployeeId, N'Admin,Manager') = 0
        THROW 50001, N'Giảm giá trên 10% phải do Manager/Admin thực hiện.', 1;
    IF @TaxRate IS NULL OR @TaxRate < 0 OR @TaxRate > 100
        THROW 50004, N'Thuế suất phải từ 0 đến 100.', 1;
END;
GO

-- Chốt tiền khi lập hóa đơn: thu thêm phần thiếu, chặn nếu chưa đủ, hoàn phần dư;
-- gắn mọi khoản thu của lượt lưu trú vào hóa đơn
CREATE PROCEDURE dbo.usp_SettleStay
    @BookingRoomId   INT,
    @InvoiceId       INT,
    @PayAmount       DECIMAL(14,0),
    @PayMethod       VARCHAR(20),
    @PayReference    VARCHAR(50),
    @RefundMethod    VARCHAR(20),
    @RefundReference VARCHAR(50),
    @EmployeeId      INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Total DECIMAL(14,0), @Balance DECIMAL(14,0), @Msg NVARCHAR(400);

    SELECT @Total = TotalAmount FROM dbo.Invoices WHERE InvoiceId = @InvoiceId;
    SET @Balance = @Total - dbo.fn_StayNetPaid(@BookingRoomId);
    SET @PayAmount = ISNULL(@PayAmount, 0);

    IF @PayAmount < 0
        THROW 50006, N'Số tiền thu không hợp lệ.', 1;

    IF @PayAmount > 0
    BEGIN
        IF @PayAmount > @Balance
        BEGIN
            SET @Msg = CONCAT(N'Số tiền thu vượt số còn phải thu (',
                              FORMAT(CASE WHEN @Balance > 0 THEN @Balance ELSE 0 END, 'N0'), N' đ).');
            THROW 50006, @Msg, 1;
        END;
        IF @PayMethod IS NULL
            THROW 50004, N'Phải chọn phương thức thanh toán.', 1;

        INSERT INTO dbo.Payments (BookingRoomId, InvoiceId, Type, Amount, Method, ReferenceNo, EmployeeId, Note)
        VALUES (@BookingRoomId, @InvoiceId, 'Payment', @PayAmount, @PayMethod, @PayReference, @EmployeeId,
                N'Thu khi lập hóa đơn');
        SET @Balance = @Balance - @PayAmount;
    END;

    IF @Balance > 0
    BEGIN
        SET @Msg = CONCAT(N'Chưa thu đủ tiền hóa đơn, còn thiếu ', FORMAT(@Balance, 'N0'), N' đ.');
        THROW 50006, @Msg, 1;
    END;

    IF @Balance < 0
    BEGIN
        IF @RefundMethod IS NULL
            THROW 50004, N'Khách đã trả dư, phải chọn phương thức hoàn tiền.', 1;

        INSERT INTO dbo.Payments (BookingRoomId, InvoiceId, Type, Amount, Method, ReferenceNo, EmployeeId, Note)
        VALUES (@BookingRoomId, @InvoiceId, 'Refund', -@Balance, @RefundMethod, @RefundReference, @EmployeeId,
                N'Hoàn tiền thừa khi lập hóa đơn');
    END;

    UPDATE dbo.Payments SET InvoiceId = @InvoiceId WHERE BookingRoomId = @BookingRoomId;
END;
GO

/* ---------- 6.2 Đăng nhập ---------- */

-- Trả thông tin để C# xác minh mật khẩu BCrypt (không so sánh mật khẩu trong SQL)
CREATE PROCEDURE dbo.sp_GetEmployeeForLogin
    @Username VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.EmployeeId, e.FullName, e.Username, e.PasswordHash, e.IsActive, r.RoleName
    FROM dbo.Employees e
    JOIN dbo.Roles r ON r.RoleId = e.RoleId
    WHERE e.Username = @Username;
END;
GO

/* ---------- 6.3 Đặt phòng và cọc ---------- */

CREATE PROCEDURE dbo.sp_CreateBooking
    @CustomerId  INT,
    @DepositRate TINYINT,
    @Note        NVARCHAR(500) = NULL,
    @EmployeeId  INT,
    @Rooms       dbo.BookingRoomInput READONLY,
    @BookingId   INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Today DATE = CAST(GETDATE() AS DATE);
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
            THROW 50001, N'Bạn không có quyền đặt phòng.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE CustomerId = @CustomerId)
            THROW 50002, N'Khách hàng không tồn tại.', 1;
        IF @DepositRate IS NULL OR @DepositRate NOT IN (50, 100)
            THROW 50004, N'Tỷ lệ cọc chỉ được là 50 hoặc 100.', 1;
        IF NOT EXISTS (SELECT 1 FROM @Rooms)
            THROW 50004, N'Booking phải có ít nhất một phòng.', 1;
        IF EXISTS (SELECT 1 FROM @Rooms WHERE CheckInDate < @Today OR CheckOutDate <= CheckInDate)
            THROW 50007, N'Ngày nhận phải từ hôm nay trở đi và ngày trả phải sau ngày nhận.', 1;
        IF EXISTS (SELECT 1 FROM @Rooms WHERE Adults < 1)
            THROW 50004, N'Mỗi phòng phải có ít nhất 1 người lớn.', 1;
        IF EXISTS (SELECT 1
                   FROM @Rooms i
                   LEFT JOIN dbo.Rooms r ON r.RoomId = i.RoomId
                   LEFT JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
                   WHERE r.RoomId IS NULL OR r.IsActive = 0 OR rt.IsActive = 0 OR r.Status = 'Maintenance')
            THROW 50004, N'Có phòng không tồn tại, đã ngừng khai thác hoặc đang bảo trì.', 1;
        IF EXISTS (SELECT 1
                   FROM @Rooms i
                   JOIN dbo.Rooms r ON r.RoomId = i.RoomId
                   JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
                   WHERE i.Adults + i.Children > rt.MaxOccupancy)
            THROW 50004, N'Số khách của một phòng vượt sức chứa của loại phòng.', 1;
        IF EXISTS (SELECT 1
                   FROM (SELECT RoomId, CheckInDate, CheckOutDate,
                                ROW_NUMBER() OVER (ORDER BY RoomId, CheckInDate, CheckOutDate) AS Rn
                         FROM @Rooms) a
                   JOIN (SELECT RoomId, CheckInDate, CheckOutDate,
                                ROW_NUMBER() OVER (ORDER BY RoomId, CheckInDate, CheckOutDate) AS Rn
                         FROM @Rooms) b
                     ON a.RoomId = b.RoomId AND a.Rn < b.Rn
                    AND a.CheckInDate < b.CheckOutDate AND a.CheckOutDate > b.CheckInDate)
            THROW 50005, N'Danh sách có cùng một phòng bị đặt trùng ngày.', 1;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        IF EXISTS (SELECT 1
                   FROM @Rooms i
                   JOIN dbo.BookingRooms br WITH (UPDLOCK, HOLDLOCK)
                     ON br.RoomId = i.RoomId
                    AND br.Status IN ('Booked', 'CheckedIn')
                    AND br.CheckInDate < i.CheckOutDate
                    AND br.CheckOutDate > i.CheckInDate)
            THROW 50005, N'Có phòng đã được đặt trùng khoảng ngày.', 1;

        INSERT INTO dbo.Bookings (CustomerId, DepositRate, Note, CreatedBy)
        VALUES (@CustomerId, @DepositRate, @Note, @EmployeeId);
        SET @BookingId = SCOPE_IDENTITY();

        INSERT INTO dbo.BookingRooms (BookingId, RoomId, CheckInDate, CheckOutDate, Adults, Children,
                                      PricePerNight, GuestName, GuestIdNumber)
        SELECT @BookingId, i.RoomId, i.CheckInDate, i.CheckOutDate, i.Adults, i.Children,
               rt.BasePrice, i.GuestName, i.GuestIdNumber
        FROM @Rooms i
        JOIN dbo.Rooms r ON r.RoomId = i.RoomId
        JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId;

        EXEC dbo.sp_RefreshBookingStatus @BookingId = @BookingId;

        IF @OwnTran = 1 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Sửa lượt lưu trú chưa nhận (đổi ngày, đổi phòng, số khách).
-- @KeepPrice = 1: khách sạn đổi phòng do phòng hỏng, phòng mới cùng hạng hoặc cao hơn, giữ giá cũ, bắt buộc lý do.
-- @KeepPrice = 0: đổi sang loại phòng khác thì lấy BasePrice mới; cùng loại thì giữ giá đã chốt.
CREATE PROCEDURE dbo.sp_UpdateBookingRoom
    @BookingRoomId INT,
    @NewRoomId     INT,
    @CheckInDate   DATE,
    @CheckOutDate  DATE,
    @Adults        TINYINT,
    @Children      TINYINT = 0,
    @KeepPrice     BIT = 0,
    @Reason        NVARCHAR(200) = NULL,
    @EmployeeId    INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Today DATE = CAST(GETDATE() AS DATE);
    DECLARE @BookingId INT, @Status VARCHAR(20), @OldRoomId INT, @OldPrice DECIMAL(14,0),
            @OldTypeId INT, @OldTypePrice DECIMAL(14,0),
            @NewTypeId INT, @NewTypePrice DECIMAL(14,0), @MaxOccupancy TINYINT, @NewPrice DECIMAL(14,0);
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
            THROW 50001, N'Bạn không có quyền sửa đặt phòng.', 1;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SELECT @BookingId = br.BookingId, @Status = br.Status, @OldRoomId = br.RoomId,
               @OldPrice = br.PricePerNight, @OldTypeId = r.RoomTypeId, @OldTypePrice = rt.BasePrice
        FROM dbo.BookingRooms br WITH (UPDLOCK)
        JOIN dbo.Rooms r ON r.RoomId = br.RoomId
        JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
        WHERE br.BookingRoomId = @BookingRoomId;

        IF @BookingId IS NULL
            THROW 50002, N'Không tìm thấy lượt lưu trú.', 1;
        IF @Status <> 'Booked'
            THROW 50003, N'Chỉ sửa được phòng chưa nhận (Booked).', 1;
        IF @CheckInDate < @Today OR @CheckOutDate <= @CheckInDate
            THROW 50007, N'Ngày nhận phải từ hôm nay trở đi và ngày trả phải sau ngày nhận.', 1;
        IF @Adults IS NULL OR @Adults < 1
            THROW 50004, N'Mỗi phòng phải có ít nhất 1 người lớn.', 1;

        SELECT @NewTypeId = r.RoomTypeId, @NewTypePrice = rt.BasePrice, @MaxOccupancy = rt.MaxOccupancy
        FROM dbo.Rooms r
        JOIN dbo.RoomTypes rt ON rt.RoomTypeId = r.RoomTypeId
        WHERE r.RoomId = @NewRoomId AND r.IsActive = 1 AND rt.IsActive = 1 AND r.Status <> 'Maintenance';

        IF @NewTypeId IS NULL
            THROW 50004, N'Phòng mới không tồn tại, đã ngừng khai thác hoặc đang bảo trì.', 1;
        IF @Adults + ISNULL(@Children, 0) > @MaxOccupancy
            THROW 50004, N'Số khách vượt sức chứa của loại phòng.', 1;
        IF @KeepPrice = 1 AND (@NewRoomId = @OldRoomId OR @NewTypePrice < @OldTypePrice
                               OR NULLIF(LTRIM(RTRIM(@Reason)), N'') IS NULL)
            THROW 50004, N'Giữ giá cũ chỉ áp dụng khi khách sạn đổi sang phòng khác cùng hạng hoặc cao hơn, và phải ghi lý do.', 1;

        IF EXISTS (SELECT 1
                   FROM dbo.BookingRooms br WITH (UPDLOCK, HOLDLOCK)
                   WHERE br.RoomId = @NewRoomId
                     AND br.BookingRoomId <> @BookingRoomId
                     AND br.Status IN ('Booked', 'CheckedIn')
                     AND br.CheckInDate < @CheckOutDate
                     AND br.CheckOutDate > @CheckInDate)
            THROW 50005, N'Phòng mới đã được đặt trùng khoảng ngày.', 1;

        SET @NewPrice = CASE WHEN @KeepPrice = 1 THEN @OldPrice
                             WHEN @NewTypeId <> @OldTypeId THEN @NewTypePrice
                             ELSE @OldPrice END;

        UPDATE dbo.BookingRooms
        SET RoomId        = @NewRoomId,
            CheckInDate   = @CheckInDate,
            CheckOutDate  = @CheckOutDate,
            Adults        = @Adults,
            Children      = ISNULL(@Children, 0),
            PricePerNight = @NewPrice,
            Note          = CASE WHEN NULLIF(LTRIM(RTRIM(@Reason)), N'') IS NULL THEN Note
                                 ELSE LEFT(CONCAT(Note + N' | ', CONVERT(NVARCHAR(16), SYSDATETIME(), 120), N' ',
                                                  CASE WHEN @KeepPrice = 1 THEN N'[Đổi phòng giữ giá] ' ELSE N'[Sửa] ' END,
                                                  @Reason), 1000) END
        WHERE BookingRoomId = @BookingRoomId;

        EXEC dbo.sp_RefreshBookingStatus @BookingId = @BookingId;

        IF @OwnTran = 1 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Hủy một lượt lưu trú chưa nhận.
-- Hủy trước 14:00 ngày nhận ít nhất 48 giờ: hoàn 100% tiền đã thu; dưới 48 giờ hoặc no-show: không hoàn.
-- No-show chỉ đánh dấu được sau 12:00 ngày hôm sau ngày nhận.
CREATE PROCEDURE dbo.sp_CancelBookingRoom
    @BookingRoomId   INT,
    @Reason          NVARCHAR(200) = NULL,
    @IsNoShow        BIT = 0,
    @RefundMethod    VARCHAR(20) = NULL,
    @RefundReference VARCHAR(50) = NULL,
    @EmployeeId      INT,
    @RefundAmount    DECIMAL(14,0) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Now DATETIME2(0) = SYSDATETIME();
    DECLARE @BookingId INT, @Status VARCHAR(20), @CheckInDate DATE, @NetPaid DECIMAL(14,0);
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
            THROW 50001, N'Bạn không có quyền hủy đặt phòng.', 1;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SELECT @BookingId = BookingId, @Status = Status, @CheckInDate = CheckInDate
        FROM dbo.BookingRooms WITH (UPDLOCK)
        WHERE BookingRoomId = @BookingRoomId;

        IF @BookingId IS NULL
            THROW 50002, N'Không tìm thấy lượt lưu trú.', 1;
        IF @Status <> 'Booked'
            THROW 50003, N'Chỉ hủy được phòng chưa nhận (Booked).', 1;
        IF @IsNoShow = 1
           AND @Now < DATEADD(HOUR, 12, CAST(DATEADD(DAY, 1, @CheckInDate) AS DATETIME2(0)))
            THROW 50007, N'Chỉ đánh dấu no-show sau 12:00 ngày hôm sau ngày nhận phòng.', 1;
        IF ISNULL(@IsNoShow, 0) = 0 AND NULLIF(LTRIM(RTRIM(@Reason)), N'') IS NULL
            THROW 50004, N'Phải nhập lý do hủy.', 1;

        SET @NetPaid = dbo.fn_StayNetPaid(@BookingRoomId);
        SET @RefundAmount =
            CASE WHEN ISNULL(@IsNoShow, 0) = 0
                      AND @Now <= DATEADD(HOUR, -48, DATEADD(HOUR, 14, CAST(@CheckInDate AS DATETIME2(0))))
                      AND @NetPaid > 0
                 THEN @NetPaid ELSE 0 END;

        IF @RefundAmount > 0
        BEGIN
            IF @RefundMethod IS NULL
                THROW 50004, N'Hủy trước 48 giờ được hoàn tiền: phải chọn phương thức hoàn tiền.', 1;
            INSERT INTO dbo.Payments (BookingRoomId, Type, Amount, Method, ReferenceNo, EmployeeId, Note)
            VALUES (@BookingRoomId, 'Refund', @RefundAmount, @RefundMethod, @RefundReference, @EmployeeId,
                    N'Hoàn tiền khi hủy trước 48 giờ');
        END;

        UPDATE dbo.BookingRooms
        SET Status       = 'Cancelled',
            CancelledAt  = @Now,
            CancelReason = CASE WHEN @IsNoShow = 1 THEN N'NoShow' ELSE @Reason END
        WHERE BookingRoomId = @BookingRoomId;

        EXEC dbo.sp_RefreshBookingStatus @BookingId = @BookingId;

        UPDATE dbo.Bookings
        SET CancelReason = CASE WHEN @IsNoShow = 1 THEN N'NoShow' ELSE @Reason END
        WHERE BookingId = @BookingId AND Status = 'Cancelled' AND CancelReason IS NULL;

        IF @OwnTran = 1 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Hủy toàn bộ phiếu đặt (mọi phòng còn Booked). Không cho hủy khi đang có phòng lưu trú.
CREATE PROCEDURE dbo.sp_CancelBooking
    @BookingId       INT,
    @Reason          NVARCHAR(200),
    @RefundMethod    VARCHAR(20) = NULL,
    @RefundReference VARCHAR(50) = NULL,
    @EmployeeId      INT,
    @TotalRefund     DECIMAL(14,0) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @StayId INT, @Refund DECIMAL(14,0);
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
            THROW 50001, N'Bạn không có quyền hủy đặt phòng.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.Bookings WHERE BookingId = @BookingId)
            THROW 50002, N'Không tìm thấy phiếu đặt.', 1;
        IF NULLIF(LTRIM(RTRIM(@Reason)), N'') IS NULL
            THROW 50004, N'Phải nhập lý do hủy.', 1;
        IF EXISTS (SELECT 1 FROM dbo.BookingRooms WHERE BookingId = @BookingId AND Status = 'CheckedIn')
            THROW 50003, N'Phiếu đang có phòng lưu trú, không hủy cả phiếu được.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.BookingRooms WHERE BookingId = @BookingId AND Status = 'Booked')
            THROW 50003, N'Phiếu không còn phòng nào để hủy.', 1;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SET @TotalRefund = 0;
        SET @StayId = 0;
        WHILE 1 = 1
        BEGIN
            SELECT TOP (1) @StayId = BookingRoomId
            FROM dbo.BookingRooms
            WHERE BookingId = @BookingId AND Status = 'Booked' AND BookingRoomId > @StayId
            ORDER BY BookingRoomId;
            IF @@ROWCOUNT = 0 BREAK;

            SET @Refund = 0;
            EXEC dbo.sp_CancelBookingRoom
                 @BookingRoomId   = @StayId,
                 @Reason          = @Reason,
                 @IsNoShow        = 0,
                 @RefundMethod    = @RefundMethod,
                 @RefundReference = @RefundReference,
                 @EmployeeId      = @EmployeeId,
                 @RefundAmount    = @Refund OUTPUT;
            SET @TotalRefund = @TotalRefund + ISNULL(@Refund, 0);
        END;

        UPDATE dbo.Bookings SET CancelReason = @Reason
        WHERE BookingId = @BookingId AND Status = 'Cancelled';

        IF @OwnTran = 1 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Hủy phiếu Pending chưa từng xác nhận, quá 24 giờ, chưa đủ cọc.
-- SQL Server Express không có SQL Agent: gọi khi mở ứng dụng và khi mở màn hình booking.
-- Trả về các phiếu vừa hủy có tiền đã thu (cần nhân viên xử lý hoàn tiền thủ công).
CREATE PROCEDURE dbo.sp_ExpirePendingBookings
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Now DATETIME2(0) = SYSDATETIME();
    DECLARE @Expired TABLE (BookingId INT PRIMARY KEY);
    BEGIN TRY
        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        INSERT INTO @Expired (BookingId)
        SELECT BookingId
        FROM dbo.Bookings WITH (UPDLOCK)
        WHERE Status = 'Pending' AND ConfirmedAt IS NULL AND CreatedAt < DATEADD(HOUR, -24, @Now);

        UPDATE br
        SET Status = 'Cancelled', CancelledAt = @Now, CancelReason = N'Hết hạn giữ chỗ'
        FROM dbo.BookingRooms br
        JOIN @Expired e ON e.BookingId = br.BookingId
        WHERE br.Status = 'Booked';

        UPDATE b
        SET Status = 'Cancelled', CancelledAt = @Now,
            CancelReason = N'Hết hạn giữ chỗ (chưa đủ cọc sau 24 giờ)'
        FROM dbo.Bookings b
        JOIN @Expired e ON e.BookingId = b.BookingId;

        IF @OwnTran = 1 COMMIT TRANSACTION;

        SELECT b.BookingId, c.FullName AS CustomerName, c.Phone,
               CAST(SUM(ISNULL(p.SignedAmount, 0)) AS DECIMAL(14,0)) AS PaidAmount
        FROM @Expired e
        JOIN dbo.Bookings b ON b.BookingId = e.BookingId
        JOIN dbo.Customers c ON c.CustomerId = b.CustomerId
        JOIN dbo.BookingRooms br ON br.BookingId = b.BookingId
        LEFT JOIN dbo.Payments p ON p.BookingRoomId = br.BookingRoomId
        GROUP BY b.BookingId, c.FullName, c.Phone
        HAVING SUM(ISNULL(p.SignedAmount, 0)) > 0;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Thu cọc cho cả phiếu: lần lượt bù phần cọc còn thiếu của từng phòng (theo thứ tự phòng),
-- phần dư bù tiếp vào tiền phòng còn lại. Ghi nhiều dòng Deposit cùng ReferenceNo.
CREATE PROCEDURE dbo.sp_RecordBookingDeposit
    @BookingId   INT,
    @Amount      DECIMAL(14,0),
    @Method      VARCHAR(20),
    @ReferenceNo VARCHAR(50) = NULL,
    @EmployeeId  INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Left DECIMAL(14,0), @Need DECIMAL(14,0), @Take DECIMAL(14,0),
            @StayId INT, @Pass INT, @Capacity DECIMAL(14,0);
    DECLARE @Alloc TABLE (
        BookingRoomId  INT PRIMARY KEY,
        MissingDeposit DECIMAL(14,0) NOT NULL,
        MissingTotal   DECIMAL(14,0) NOT NULL,
        Pay            DECIMAL(14,0) NOT NULL DEFAULT (0)
    );
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
            THROW 50001, N'Bạn không có quyền thu tiền.', 1;
        IF @Amount IS NULL OR @Amount <= 0
            THROW 50006, N'Số tiền cọc phải lớn hơn 0.', 1;
        IF @Method IS NULL OR @Method NOT IN ('Cash', 'Card', 'BankTransfer', 'EWallet')
            THROW 50004, N'Phương thức thanh toán không hợp lệ.', 1;
        IF @Method IN ('BankTransfer', 'EWallet') AND NULLIF(LTRIM(RTRIM(@ReferenceNo)), '') IS NULL
            THROW 50004, N'Chuyển khoản/ví điện tử phải có mã giao dịch.', 1;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        INSERT INTO @Alloc (BookingRoomId, MissingDeposit, MissingTotal)
        SELECT f.BookingRoomId,
               CASE WHEN f.RequiredDeposit > f.NetPaid THEN f.RequiredDeposit - f.NetPaid ELSE 0 END,
               CASE WHEN f.PlannedRoomCharge > f.NetPaid THEN f.PlannedRoomCharge - f.NetPaid ELSE 0 END
        FROM dbo.vw_RoomStayFolio f
        WHERE f.BookingId = @BookingId AND f.StayStatus = 'Booked';

        IF NOT EXISTS (SELECT 1 FROM @Alloc)
            THROW 50003, N'Phiếu không có phòng nào đang chờ nhận để thu cọc.', 1;

        SELECT @Capacity = SUM(MissingTotal) FROM @Alloc;
        IF @Amount > @Capacity
            THROW 50006, N'Số tiền vượt tổng tiền phòng còn lại của các phòng chưa nhận.', 1;

        SET @Left = @Amount;
        SET @Pass = 1;
        WHILE @Pass <= 2 AND @Left > 0
        BEGIN
            SET @StayId = 0;
            WHILE @Left > 0
            BEGIN
                SELECT TOP (1) @StayId = BookingRoomId,
                               @Need = CASE WHEN @Pass = 1 THEN MissingDeposit - Pay ELSE MissingTotal - Pay END
                FROM @Alloc
                WHERE BookingRoomId > @StayId
                  AND (CASE WHEN @Pass = 1 THEN MissingDeposit - Pay ELSE MissingTotal - Pay END) > 0
                ORDER BY BookingRoomId;
                IF @@ROWCOUNT = 0 BREAK;

                SET @Take = CASE WHEN @Need < @Left THEN @Need ELSE @Left END;
                UPDATE @Alloc SET Pay = Pay + @Take WHERE BookingRoomId = @StayId;
                SET @Left = @Left - @Take;
            END;
            SET @Pass = @Pass + 1;
        END;

        INSERT INTO dbo.Payments (BookingRoomId, Type, Amount, Method, ReferenceNo, EmployeeId, Note)
        SELECT BookingRoomId, 'Deposit', Pay, @Method, @ReferenceNo, @EmployeeId,
               CONCAT(N'Cọc phiếu đặt #', @BookingId)
        FROM @Alloc
        WHERE Pay > 0;

        EXEC dbo.sp_RefreshBookingStatus @BookingId = @BookingId;

        IF @OwnTran = 1 COMMIT TRANSACTION;

        SELECT a.BookingRoomId, r.RoomNumber, a.Pay AS AllocatedAmount
        FROM @Alloc a
        JOIN dbo.BookingRooms br ON br.BookingRoomId = a.BookingRoomId
        JOIN dbo.Rooms r ON r.RoomId = br.RoomId
        WHERE a.Pay > 0
        ORDER BY a.BookingRoomId;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Ghi một khoản tiền cho một lượt lưu trú.
--   Deposit: phòng Booked (A/M/R), không vượt tiền phòng còn lại.
--   Payment: phòng CheckedIn (A/M/R); Admin/Manager được ghi điều chỉnh ở trạng thái khác nếu có ghi chú.
--   Refund : chỉ Admin/Manager, bắt buộc ghi chú, không vượt số đã thu.
-- (Thu tiền và hoàn tiền lúc check-out/lập lại hóa đơn do sp_CheckOutRoom/sp_ReissueInvoice tự làm.)
CREATE PROCEDURE dbo.sp_RecordPayment
    @BookingRoomId INT,
    @Type          VARCHAR(10),
    @Amount        DECIMAL(14,0),
    @Method        VARCHAR(20),
    @ReferenceNo   VARCHAR(50) = NULL,
    @Note          NVARCHAR(200) = NULL,
    @EmployeeId    INT,
    @PaymentId     INT = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @BookingId INT, @Status VARCHAR(20), @NetPaid DECIMAL(14,0),
            @Planned DECIMAL(14,0), @IsManager BIT, @HasNote BIT;
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
            THROW 50001, N'Bạn không có quyền thu tiền.', 1;
        IF @Type IS NULL OR @Type NOT IN ('Deposit', 'Payment', 'Refund')
            THROW 50004, N'Loại khoản tiền không hợp lệ.', 1;
        IF @Amount IS NULL OR @Amount <= 0
            THROW 50006, N'Số tiền phải lớn hơn 0.', 1;
        IF @Method IS NULL OR @Method NOT IN ('Cash', 'Card', 'BankTransfer', 'EWallet')
            THROW 50004, N'Phương thức thanh toán không hợp lệ.', 1;
        IF @Method IN ('BankTransfer', 'EWallet') AND NULLIF(LTRIM(RTRIM(@ReferenceNo)), '') IS NULL
            THROW 50004, N'Chuyển khoản/ví điện tử phải có mã giao dịch.', 1;

        SET @IsManager = dbo.fn_HasRole(@EmployeeId, N'Admin,Manager');
        SET @HasNote = CASE WHEN NULLIF(LTRIM(RTRIM(@Note)), N'') IS NULL THEN 0 ELSE 1 END;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SELECT @BookingId = BookingId, @Status = Status,
               @Planned = CAST(PricePerNight * Nights AS DECIMAL(14,0))
        FROM dbo.BookingRooms WITH (UPDLOCK)
        WHERE BookingRoomId = @BookingRoomId;

        IF @BookingId IS NULL
            THROW 50002, N'Không tìm thấy lượt lưu trú.', 1;

        SET @NetPaid = dbo.fn_StayNetPaid(@BookingRoomId);

        IF @Type = 'Deposit'
        BEGIN
            IF @Status <> 'Booked'
                THROW 50003, N'Chỉ thu cọc cho phòng chưa nhận.', 1;
            IF @NetPaid + @Amount > @Planned
                THROW 50006, N'Tiền cọc vượt tiền phòng của lượt lưu trú.', 1;
        END
        ELSE IF @Type = 'Payment'
        BEGIN
            IF @Status <> 'CheckedIn' AND NOT (@IsManager = 1 AND @HasNote = 1 AND @Status <> 'Cancelled')
                THROW 50003, N'Chỉ thu tiền cho phòng đang ở (điều chỉnh khác do Admin/Manager kèm ghi chú).', 1;
        END
        ELSE
        BEGIN
            IF @IsManager = 0
                THROW 50001, N'Chỉ Admin/Manager được ghi hoàn tiền thủ công.', 1;
            IF @HasNote = 0
                THROW 50004, N'Hoàn tiền thủ công phải có ghi chú.', 1;
            IF @Amount > @NetPaid
                THROW 50006, N'Số tiền hoàn vượt số đã thu.', 1;
        END;

        INSERT INTO dbo.Payments (BookingRoomId, Type, Amount, Method, ReferenceNo, EmployeeId, Note)
        VALUES (@BookingRoomId, @Type, @Amount, @Method, @ReferenceNo, @EmployeeId, @Note);
        SET @PaymentId = SCOPE_IDENTITY();

        EXEC dbo.sp_RefreshBookingStatus @BookingId = @BookingId;

        IF @OwnTran = 1 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

/* ---------- 6.4 Lưu trú: check-in, dịch vụ, gia hạn ---------- */

CREATE PROCEDURE dbo.sp_CheckInRoom
    @BookingRoomId INT,
    @GuestName     NVARCHAR(100) = NULL,
    @GuestIdNumber VARCHAR(20) = NULL,
    @EmployeeId    INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Today DATE = CAST(GETDATE() AS DATE);
    DECLARE @BookingId INT, @BookingStatus VARCHAR(20), @Status VARCHAR(20), @RoomId INT,
            @CheckInDate DATE, @CheckOutDate DATE, @RoomStatus VARCHAR(20), @RoomActive BIT;
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
            THROW 50001, N'Bạn không có quyền check-in.', 1;
        IF NULLIF(LTRIM(RTRIM(@GuestIdNumber)), '') IS NOT NULL AND NULLIF(LTRIM(RTRIM(@GuestName)), N'') IS NULL
            THROW 50004, N'Nhập CCCD/hộ chiếu người ở thì phải nhập tên.', 1;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SELECT @BookingId = br.BookingId, @BookingStatus = b.Status, @Status = br.Status, @RoomId = br.RoomId,
               @CheckInDate = br.CheckInDate, @CheckOutDate = br.CheckOutDate
        FROM dbo.BookingRooms br WITH (UPDLOCK)
        JOIN dbo.Bookings b ON b.BookingId = br.BookingId
        WHERE br.BookingRoomId = @BookingRoomId;

        IF @BookingId IS NULL
            THROW 50002, N'Không tìm thấy lượt lưu trú.', 1;
        IF @Status <> 'Booked'
            THROW 50003, N'Phòng này không ở trạng thái chờ nhận.', 1;
        IF @BookingStatus NOT IN ('Confirmed', 'CheckedIn')
            THROW 50003, N'Phiếu đặt chưa được xác nhận (chưa đủ cọc).', 1;
        IF dbo.fn_StayNetPaid(@BookingRoomId) < dbo.fn_RequiredDeposit(@BookingRoomId)
            THROW 50006, N'Phòng chưa đủ tiền cọc yêu cầu.', 1;
        IF @Today < @CheckInDate
            THROW 50007, N'Chưa đến ngày nhận phòng. Muốn nhận sớm hơn phải sửa booking thêm đêm.', 1;
        IF @Today >= @CheckOutDate
            THROW 50007, N'Đã qua ngày trả phòng của lượt này.', 1;

        SELECT @RoomStatus = Status, @RoomActive = IsActive
        FROM dbo.Rooms WITH (UPDLOCK)
        WHERE RoomId = @RoomId;

        IF @RoomActive = 0 OR @RoomStatus <> 'Available'
            THROW 50003, N'Phòng chưa sẵn sàng (đang có khách, đang dọn hoặc bảo trì). Đổi phòng nếu cần.', 1;

        UPDATE dbo.BookingRooms
        SET Status        = 'CheckedIn',
            ActualCheckIn = SYSDATETIME(),
            CheckInBy     = @EmployeeId,
            GuestName     = COALESCE(NULLIF(LTRIM(RTRIM(@GuestName)), N''), GuestName),
            GuestIdNumber = COALESCE(NULLIF(LTRIM(RTRIM(@GuestIdNumber)), ''), GuestIdNumber)
        WHERE BookingRoomId = @BookingRoomId;

        UPDATE dbo.Rooms SET Status = 'Occupied' WHERE RoomId = @RoomId;

        EXEC dbo.sp_RefreshBookingStatus @BookingId = @BookingId;

        IF @OwnTran = 1 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Ghi dịch vụ cho phòng đang ở (hoặc Admin/Manager sau khi void hóa đơn).
-- Cảnh báo số lượng > 20 làm ở giao diện.
CREATE PROCEDURE dbo.sp_AddServiceUsage
    @BookingRoomId INT,
    @ServiceId     INT,
    @Quantity      INT,
    @UsedAt        DATETIME2(0) = NULL,
    @Note          NVARCHAR(200) = NULL,
    @EmployeeId    INT,
    @UsageId       INT = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @Price DECIMAL(14,0), @Status VARCHAR(20), @CheckOutAt DATETIME2(0);
    BEGIN TRY
        SELECT @Status = Status, @CheckOutAt = ActualCheckOut
        FROM dbo.BookingRooms WHERE BookingRoomId = @BookingRoomId;

        SET @UsedAt = COALESCE(@UsedAt, CASE WHEN @Status = 'CheckedOut' THEN @CheckOutAt ELSE SYSDATETIME() END);

        EXEC dbo.usp_CheckServiceEditable @BookingRoomId = @BookingRoomId, @EmployeeId = @EmployeeId, @UsedAt = @UsedAt;

        IF @Quantity IS NULL OR @Quantity <= 0
            THROW 50004, N'Số lượng phải lớn hơn 0.', 1;

        SELECT @Price = Price FROM dbo.Services WHERE ServiceId = @ServiceId AND IsActive = 1 AND IsSystem = 0;
        IF @Price IS NULL
            THROW 50004, N'Dịch vụ không tồn tại, đã ngừng hoặc là dịch vụ hệ thống.', 1;

        INSERT INTO dbo.ServiceUsages (BookingRoomId, ServiceId, Quantity, UnitPrice, UsedAt, Note, CreatedBy)
        VALUES (@BookingRoomId, @ServiceId, @Quantity, @Price, @UsedAt, @Note, @EmployeeId);
        SET @UsageId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Sửa số lượng, thời điểm, ghi chú (đơn giá giữ nguyên như lúc ghi). Không sửa dịch vụ hệ thống.
CREATE PROCEDURE dbo.sp_UpdateServiceUsage
    @UsageId    INT,
    @Quantity   INT,
    @UsedAt     DATETIME2(0),
    @Note       NVARCHAR(200) = NULL,
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @BookingRoomId INT, @IsSystem BIT;
    BEGIN TRY
        SELECT @BookingRoomId = su.BookingRoomId, @IsSystem = s.IsSystem
        FROM dbo.ServiceUsages su
        JOIN dbo.Services s ON s.ServiceId = su.ServiceId
        WHERE su.UsageId = @UsageId;

        IF @BookingRoomId IS NULL
            THROW 50002, N'Không tìm thấy lượt dùng dịch vụ.', 1;
        IF @IsSystem = 1
            THROW 50003, N'Không sửa được khoản do hệ thống ghi (phụ thu).', 1;
        IF @UsedAt IS NULL
            THROW 50004, N'Phải nhập thời điểm sử dụng.', 1;

        EXEC dbo.usp_CheckServiceEditable @BookingRoomId = @BookingRoomId, @EmployeeId = @EmployeeId, @UsedAt = @UsedAt;

        IF @Quantity IS NULL OR @Quantity <= 0
            THROW 50004, N'Số lượng phải lớn hơn 0.', 1;

        UPDATE dbo.ServiceUsages
        SET Quantity = @Quantity, UsedAt = @UsedAt, Note = @Note
        WHERE UsageId = @UsageId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

CREATE PROCEDURE dbo.sp_DeleteServiceUsage
    @UsageId    INT,
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @BookingRoomId INT, @IsSystem BIT;
    BEGIN TRY
        SELECT @BookingRoomId = su.BookingRoomId, @IsSystem = s.IsSystem
        FROM dbo.ServiceUsages su
        JOIN dbo.Services s ON s.ServiceId = su.ServiceId
        WHERE su.UsageId = @UsageId;

        IF @BookingRoomId IS NULL
            THROW 50002, N'Không tìm thấy lượt dùng dịch vụ.', 1;
        IF @IsSystem = 1
            THROW 50003, N'Không xóa được khoản do hệ thống ghi (phụ thu).', 1;

        EXEC dbo.usp_CheckServiceEditable @BookingRoomId = @BookingRoomId, @EmployeeId = @EmployeeId, @UsedAt = NULL;

        DELETE FROM dbo.ServiceUsages WHERE UsageId = @UsageId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Gia hạn phòng đang ở: dời CheckOutDate, kiểm tra trùng lịch các đêm thêm, giữ giá, không đòi thêm cọc
CREATE PROCEDURE dbo.sp_ExtendStay
    @BookingRoomId   INT,
    @NewCheckOutDate DATE,
    @Reason          NVARCHAR(200) = NULL,
    @EmployeeId      INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Status VARCHAR(20), @RoomId INT, @OldCheckOut DATE;
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
            THROW 50001, N'Bạn không có quyền gia hạn.', 1;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SELECT @Status = Status, @RoomId = RoomId, @OldCheckOut = CheckOutDate
        FROM dbo.BookingRooms WITH (UPDLOCK)
        WHERE BookingRoomId = @BookingRoomId;

        IF @Status IS NULL
            THROW 50002, N'Không tìm thấy lượt lưu trú.', 1;
        IF @Status <> 'CheckedIn'
            THROW 50003, N'Chỉ gia hạn được phòng đang ở.', 1;
        IF @NewCheckOutDate IS NULL OR @NewCheckOutDate <= @OldCheckOut
            THROW 50007, N'Ngày trả mới phải sau ngày trả hiện tại.', 1;

        IF EXISTS (SELECT 1
                   FROM dbo.BookingRooms br WITH (UPDLOCK, HOLDLOCK)
                   WHERE br.RoomId = @RoomId
                     AND br.BookingRoomId <> @BookingRoomId
                     AND br.Status IN ('Booked', 'CheckedIn')
                     AND br.CheckInDate < @NewCheckOutDate
                     AND br.CheckOutDate > @OldCheckOut)
            THROW 50005, N'Các đêm gia hạn trùng lịch của khách khác. Hãy chọn ngày khác.', 1;

        UPDATE dbo.BookingRooms
        SET CheckOutDate = @NewCheckOutDate,
            Note = LEFT(CONCAT(Note + N' | ', CONVERT(NVARCHAR(16), SYSDATETIME(), 120),
                               N' [Gia hạn] ', CONVERT(NVARCHAR(10), @OldCheckOut, 103), N' -> ',
                               CONVERT(NVARCHAR(10), @NewCheckOutDate, 103),
                               CASE WHEN NULLIF(LTRIM(RTRIM(@Reason)), N'') IS NULL THEN N'' ELSE N': ' + @Reason END), 1000)
        WHERE BookingRoomId = @BookingRoomId;

        IF @OwnTran = 1 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

/* ---------- 6.5 Check-out và hóa đơn ---------- */

-- Xem trước hóa đơn khi check-out (không ghi dữ liệu). Giao diện giữ lại CheckOutAt trả về
-- và truyền nguyên giá trị đó vào sp_CheckOutRoom (hiệu lực 10 phút).
CREATE PROCEDURE dbo.sp_PreviewCheckOut
    @BookingRoomId INT,
    @CheckOutAt    DATETIME2(0) = NULL,
    @Discount      DECIMAL(14,0) = 0,
    @TaxRate       DECIMAL(5,2) = 10
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.BookingRooms WHERE BookingRoomId = @BookingRoomId AND Status = 'CheckedIn')
        THROW 50003, N'Chỉ xem trước check-out cho phòng đang ở.', 1;

    SET @CheckOutAt = ISNULL(@CheckOutAt, CAST(SYSDATETIME() AS DATETIME2(0)));

    SELECT @CheckOutAt AS CheckOutAt, q.*
    FROM dbo.fn_CheckOutQuote(@BookingRoomId, @CheckOutAt, ISNULL(@Discount, 0), ISNULL(@TaxRate, 10)) q;
END;
GO

-- Check-out một phòng: phụ thu trễ, hóa đơn Paid, thu đủ/hoàn dư, phòng Cleaning, việc dọn, trạng thái phiếu.
CREATE PROCEDURE dbo.sp_CheckOutRoom
    @BookingRoomId    INT,
    @CheckOutAt       DATETIME2(0),
    @Discount         DECIMAL(14,0) = 0,
    @DiscountReason   NVARCHAR(200) = NULL,
    @TaxRate          DECIMAL(5,2) = 10,
    @BuyerName        NVARCHAR(150) = NULL,
    @BuyerCompanyName NVARCHAR(200) = NULL,
    @BuyerTaxCode     VARCHAR(20) = NULL,
    @BuyerAddress     NVARCHAR(255) = NULL,
    @PayAmount        DECIMAL(14,0) = 0,
    @PayMethod        VARCHAR(20) = NULL,
    @PayReference     VARCHAR(50) = NULL,
    @RefundMethod     VARCHAR(20) = NULL,
    @RefundReference  VARCHAR(50) = NULL,
    @EmployeeId       INT,
    @InvoiceId        INT = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Now DATETIME2(0) = SYSDATETIME();
    DECLARE @BookingId INT, @Status VARCHAR(20), @RoomId INT, @ActualCheckIn DATETIME2(0),
            @Nights INT, @RoomCharge DECIMAL(14,0), @ServiceCharge DECIMAL(14,0), @SubTotal DECIMAL(14,0),
            @LateMinutes INT, @LateFee DECIMAL(14,0), @MustExtend BIT, @LateServiceId INT;
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
            THROW 50001, N'Bạn không có quyền check-out.', 1;
        IF @CheckOutAt IS NULL
            THROW 50007, N'Thiếu thời điểm trả phòng (lấy từ bước xem trước).', 1;
        IF @CheckOutAt > DATEADD(MINUTE, 1, @Now) OR @CheckOutAt < DATEADD(MINUTE, -10, @Now)
            THROW 50007, N'Bảng tính check-out đã quá 10 phút, hãy xem trước lại.', 1;
        IF NULLIF(LTRIM(RTRIM(@BuyerTaxCode)), '') IS NOT NULL AND NULLIF(LTRIM(RTRIM(@BuyerCompanyName)), N'') IS NULL
            THROW 50004, N'Hóa đơn có mã số thuế phải có tên đơn vị.', 1;

        SET @Discount = ISNULL(@Discount, 0);
        SET @TaxRate = ISNULL(@TaxRate, 10);

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SELECT @BookingId = BookingId, @Status = Status, @RoomId = RoomId, @ActualCheckIn = ActualCheckIn
        FROM dbo.BookingRooms WITH (UPDLOCK)
        WHERE BookingRoomId = @BookingRoomId;

        IF @BookingId IS NULL
            THROW 50002, N'Không tìm thấy lượt lưu trú.', 1;
        IF @Status <> 'CheckedIn'
            THROW 50003, N'Chỉ check-out được phòng đang ở.', 1;
        IF @CheckOutAt <= @ActualCheckIn
            THROW 50007, N'Thời điểm trả phòng phải sau thời điểm nhận phòng.', 1;

        SELECT @Nights = Nights, @RoomCharge = RoomCharge, @ServiceCharge = ServiceCharge, @SubTotal = SubTotal,
               @LateMinutes = LateMinutes, @LateFee = LateFee, @MustExtend = MustExtend
        FROM dbo.fn_CheckOutQuote(@BookingRoomId, @CheckOutAt, @Discount, @TaxRate);

        IF @MustExtend = 1
            THROW 50007, N'Trả phòng sau 18:00 ngày trả: phải gia hạn thêm 1 đêm trước khi check-out.', 1;

        EXEC dbo.usp_CheckDiscount @Discount = @Discount, @DiscountReason = @DiscountReason,
             @SubTotal = @SubTotal, @TaxRate = @TaxRate, @EmployeeId = @EmployeeId;

        IF @LateFee > 0
        BEGIN
            SELECT @LateServiceId = ServiceId
            FROM dbo.Services
            WHERE IsSystem = 1 AND ServiceName = N'Phụ thu trả phòng trễ';
            IF @LateServiceId IS NULL
                THROW 50002, N'Thiếu dịch vụ hệ thống "Phụ thu trả phòng trễ".', 1;

            INSERT INTO dbo.ServiceUsages (BookingRoomId, ServiceId, Quantity, UnitPrice, UsedAt, Note, CreatedBy)
            VALUES (@BookingRoomId, @LateServiceId, 1, @LateFee, @CheckOutAt,
                    CONCAT(N'Trễ ', @LateMinutes, N' phút'), @EmployeeId);
        END;

        INSERT INTO dbo.Invoices (BookingRoomId, Nights, RoomCharge, ServiceCharge, Discount, DiscountReason, TaxRate,
                                  BuyerName, BuyerCompanyName, BuyerTaxCode, BuyerAddress, Status, IssuedBy)
        VALUES (@BookingRoomId, @Nights, @RoomCharge, @ServiceCharge, @Discount,
                NULLIF(LTRIM(RTRIM(@DiscountReason)), N''), @TaxRate,
                NULLIF(LTRIM(RTRIM(@BuyerName)), N''), NULLIF(LTRIM(RTRIM(@BuyerCompanyName)), N''),
                NULLIF(LTRIM(RTRIM(@BuyerTaxCode)), ''), NULLIF(LTRIM(RTRIM(@BuyerAddress)), N''),
                'Paid', @EmployeeId);
        SET @InvoiceId = SCOPE_IDENTITY();

        EXEC dbo.usp_SettleStay @BookingRoomId = @BookingRoomId, @InvoiceId = @InvoiceId,
             @PayAmount = @PayAmount, @PayMethod = @PayMethod, @PayReference = @PayReference,
             @RefundMethod = @RefundMethod, @RefundReference = @RefundReference, @EmployeeId = @EmployeeId;

        UPDATE dbo.BookingRooms
        SET Status = 'CheckedOut', ActualCheckOut = @CheckOutAt, CheckOutBy = @EmployeeId
        WHERE BookingRoomId = @BookingRoomId;

        UPDATE dbo.Rooms SET Status = 'Cleaning' WHERE RoomId = @RoomId;

        INSERT INTO dbo.HousekeepingTasks (RoomId, BookingRoomId, Source)
        VALUES (@RoomId, @BookingRoomId, 'CheckOut');

        EXEC dbo.sp_RefreshBookingStatus @BookingId = @BookingId;

        IF @OwnTran = 1 COMMIT TRANSACTION;

        SELECT * FROM dbo.vw_InvoiceDetail WHERE InvoiceId = @InvoiceId;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Void hóa đơn (Admin/Manager, bắt buộc lý do). Không đụng tới tiền đã thu.
CREATE PROCEDURE dbo.sp_VoidInvoice
    @InvoiceId  INT,
    @Reason     NVARCHAR(200),
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @Status VARCHAR(10);
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager') = 0
            THROW 50001, N'Chỉ Admin/Manager được void hóa đơn.', 1;
        IF NULLIF(LTRIM(RTRIM(@Reason)), N'') IS NULL
            THROW 50004, N'Phải nhập lý do void.', 1;

        SELECT @Status = Status FROM dbo.Invoices WHERE InvoiceId = @InvoiceId;
        IF @Status IS NULL
            THROW 50002, N'Không tìm thấy hóa đơn.', 1;
        IF @Status <> 'Paid'
            THROW 50003, N'Hóa đơn đã bị void trước đó.', 1;

        UPDATE dbo.Invoices
        SET Status = 'Void', VoidedAt = SYSDATETIME(), VoidedBy = @EmployeeId, VoidReason = @Reason
        WHERE InvoiceId = @InvoiceId AND Status = 'Paid';
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Lập lại hóa đơn sau khi void: tính lại từ dữ liệu hiện tại (không tính lại phụ thu),
-- thu thêm hoặc hoàn chênh lệch, gắn ReplacesInvoiceId.
CREATE PROCEDURE dbo.sp_ReissueInvoice
    @BookingRoomId    INT,
    @Discount         DECIMAL(14,0) = 0,
    @DiscountReason   NVARCHAR(200) = NULL,
    @TaxRate          DECIMAL(5,2) = 10,
    @BuyerName        NVARCHAR(150) = NULL,
    @BuyerCompanyName NVARCHAR(200) = NULL,
    @BuyerTaxCode     VARCHAR(20) = NULL,
    @BuyerAddress     NVARCHAR(255) = NULL,
    @PayAmount        DECIMAL(14,0) = 0,
    @PayMethod        VARCHAR(20) = NULL,
    @PayReference     VARCHAR(50) = NULL,
    @RefundMethod     VARCHAR(20) = NULL,
    @RefundReference  VARCHAR(50) = NULL,
    @EmployeeId       INT,
    @InvoiceId        INT = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Status VARCHAR(20), @Nights INT, @RoomCharge DECIMAL(14,0),
            @ServiceCharge DECIMAL(14,0), @SubTotal DECIMAL(14,0), @ReplacesId INT;
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager') = 0
            THROW 50001, N'Chỉ Admin/Manager được lập lại hóa đơn.', 1;
        IF NULLIF(LTRIM(RTRIM(@BuyerTaxCode)), '') IS NOT NULL AND NULLIF(LTRIM(RTRIM(@BuyerCompanyName)), N'') IS NULL
            THROW 50004, N'Hóa đơn có mã số thuế phải có tên đơn vị.', 1;

        SET @Discount = ISNULL(@Discount, 0);
        SET @TaxRate = ISNULL(@TaxRate, 10);

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SELECT @Status = Status, @Nights = Nights,
               @RoomCharge = CAST(PricePerNight * Nights AS DECIMAL(14,0))
        FROM dbo.BookingRooms WITH (UPDLOCK)
        WHERE BookingRoomId = @BookingRoomId;

        IF @Status IS NULL
            THROW 50002, N'Không tìm thấy lượt lưu trú.', 1;
        IF @Status <> 'CheckedOut'
            THROW 50003, N'Chỉ lập lại hóa đơn cho phòng đã trả.', 1;
        IF EXISTS (SELECT 1 FROM dbo.Invoices WHERE BookingRoomId = @BookingRoomId AND Status = 'Paid')
            THROW 50003, N'Phòng đang có hóa đơn hiệu lực. Phải void trước khi lập lại.', 1;

        SELECT TOP (1) @ReplacesId = InvoiceId
        FROM dbo.Invoices
        WHERE BookingRoomId = @BookingRoomId AND Status = 'Void'
        ORDER BY InvoiceId DESC;
        IF @ReplacesId IS NULL
            THROW 50002, N'Không có hóa đơn đã void để lập lại.', 1;

        SELECT @ServiceCharge = CAST(ISNULL(SUM(Amount), 0) AS DECIMAL(14,0))
        FROM dbo.ServiceUsages
        WHERE BookingRoomId = @BookingRoomId;
        SET @SubTotal = @RoomCharge + @ServiceCharge;

        EXEC dbo.usp_CheckDiscount @Discount = @Discount, @DiscountReason = @DiscountReason,
             @SubTotal = @SubTotal, @TaxRate = @TaxRate, @EmployeeId = @EmployeeId;

        INSERT INTO dbo.Invoices (BookingRoomId, Nights, RoomCharge, ServiceCharge, Discount, DiscountReason, TaxRate,
                                  BuyerName, BuyerCompanyName, BuyerTaxCode, BuyerAddress, Status, IssuedBy,
                                  ReplacesInvoiceId)
        VALUES (@BookingRoomId, @Nights, @RoomCharge, @ServiceCharge, @Discount,
                NULLIF(LTRIM(RTRIM(@DiscountReason)), N''), @TaxRate,
                NULLIF(LTRIM(RTRIM(@BuyerName)), N''), NULLIF(LTRIM(RTRIM(@BuyerCompanyName)), N''),
                NULLIF(LTRIM(RTRIM(@BuyerTaxCode)), ''), NULLIF(LTRIM(RTRIM(@BuyerAddress)), N''),
                'Paid', @EmployeeId, @ReplacesId);
        SET @InvoiceId = SCOPE_IDENTITY();

        EXEC dbo.usp_SettleStay @BookingRoomId = @BookingRoomId, @InvoiceId = @InvoiceId,
             @PayAmount = @PayAmount, @PayMethod = @PayMethod, @PayReference = @PayReference,
             @RefundMethod = @RefundMethod, @RefundReference = @RefundReference, @EmployeeId = @EmployeeId;

        IF @OwnTran = 1 COMMIT TRANSACTION;

        SELECT * FROM dbo.vw_InvoiceDetail WHERE InvoiceId = @InvoiceId;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

/* ---------- 6.6 Phòng, bảo trì, dọn phòng ---------- */

-- Đưa phòng Available vào bảo trì; trả về các lượt Booked bị ảnh hưởng để đổi phòng
CREATE PROCEDURE dbo.sp_SetRoomMaintenance
    @RoomId     INT,
    @Note       NVARCHAR(200),
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Status VARCHAR(20), @IsActive BIT;
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager') = 0
            THROW 50001, N'Chỉ Admin/Manager được đưa phòng vào bảo trì.', 1;
        IF NULLIF(LTRIM(RTRIM(@Note)), N'') IS NULL
            THROW 50004, N'Phải ghi chú lý do bảo trì.', 1;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SELECT @Status = Status, @IsActive = IsActive FROM dbo.Rooms WITH (UPDLOCK) WHERE RoomId = @RoomId;
        IF @Status IS NULL
            THROW 50002, N'Không tìm thấy phòng.', 1;
        IF @IsActive = 0
            THROW 50003, N'Phòng đã ngừng khai thác.', 1;
        IF @Status <> 'Available'
            THROW 50003, N'Chỉ đưa vào bảo trì phòng đang Available (phòng đang dọn thì báo qua việc dọn phòng).', 1;

        UPDATE dbo.Rooms SET Status = 'Maintenance', Note = @Note WHERE RoomId = @RoomId;

        IF @OwnTran = 1 COMMIT TRANSACTION;

        SELECT s.BookingRoomId, s.BookingId, s.CustomerName, s.CustomerPhone, s.CheckInDate, s.CheckOutDate
        FROM dbo.vw_RoomStays s
        WHERE s.RoomId = @RoomId AND s.StayStatus = 'Booked' AND s.CheckOutDate > CAST(GETDATE() AS DATE)
        ORDER BY s.CheckInDate;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Kết thúc bảo trì: Maintenance -> Cleaning, tạo việc dọn (AfterMaintenance)
CREATE PROCEDURE dbo.sp_FinishMaintenance
    @RoomId     INT,
    @Note       NVARCHAR(200) = NULL,
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Status VARCHAR(20);
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager') = 0
            THROW 50001, N'Chỉ Admin/Manager được kết thúc bảo trì.', 1;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SELECT @Status = Status FROM dbo.Rooms WITH (UPDLOCK) WHERE RoomId = @RoomId;
        IF @Status IS NULL
            THROW 50002, N'Không tìm thấy phòng.', 1;
        IF @Status <> 'Maintenance'
            THROW 50003, N'Phòng không ở trạng thái bảo trì.', 1;

        UPDATE dbo.Rooms
        SET Status = 'Cleaning', Note = COALESCE(NULLIF(LTRIM(RTRIM(@Note)), N''), Note)
        WHERE RoomId = @RoomId;

        INSERT INTO dbo.HousekeepingTasks (RoomId, Source, Note)
        VALUES (@RoomId, 'AfterMaintenance', NULLIF(LTRIM(RTRIM(@Note)), N''));

        IF @OwnTran = 1 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE PROCEDURE dbo.sp_StartHousekeepingTask
    @TaskId     INT,
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @Status VARCHAR(20);
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Housekeeping,Admin,Manager') = 0
            THROW 50001, N'Bạn không có quyền nhận việc dọn phòng.', 1;

        SELECT @Status = Status FROM dbo.HousekeepingTasks WHERE TaskId = @TaskId;
        IF @Status IS NULL
            THROW 50002, N'Không tìm thấy việc dọn phòng.', 1;
        IF @Status <> 'Pending'
            THROW 50003, N'Việc này đã được nhận hoặc đã xong.', 1;

        UPDATE dbo.HousekeepingTasks
        SET Status = 'InProgress', AssignedTo = @EmployeeId, StartedAt = SYSDATETIME()
        WHERE TaskId = @TaskId AND Status = 'Pending';
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Hoàn tất việc dọn: Clean -> phòng Available; NeedMaintenance (bắt buộc ghi chú) -> phòng Maintenance
CREATE PROCEDURE dbo.sp_CompleteHousekeepingTask
    @TaskId     INT,
    @Result     VARCHAR(20),
    @Note       NVARCHAR(500) = NULL,
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @OwnTran BIT = 0, @Status VARCHAR(20), @RoomId INT, @AssignedTo INT, @RoomStatus VARCHAR(20);
    BEGIN TRY
        IF dbo.fn_HasRole(@EmployeeId, N'Housekeeping,Admin,Manager') = 0
            THROW 50001, N'Bạn không có quyền hoàn tất việc dọn phòng.', 1;
        IF @Result IS NULL OR @Result NOT IN ('Clean', 'NeedMaintenance')
            THROW 50004, N'Kết quả chỉ được là Clean hoặc NeedMaintenance.', 1;
        IF @Result = 'NeedMaintenance' AND NULLIF(LTRIM(RTRIM(@Note)), N'') IS NULL
            THROW 50004, N'Báo cần bảo trì phải ghi chú tình trạng hỏng.', 1;

        IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @OwnTran = 1; END;

        SELECT @Status = t.Status, @RoomId = t.RoomId, @AssignedTo = t.AssignedTo, @RoomStatus = r.Status
        FROM dbo.HousekeepingTasks t WITH (UPDLOCK)
        JOIN dbo.Rooms r ON r.RoomId = t.RoomId
        WHERE t.TaskId = @TaskId;

        IF @Status IS NULL
            THROW 50002, N'Không tìm thấy việc dọn phòng.', 1;
        IF @Status = 'Done'
            THROW 50003, N'Việc này đã hoàn tất.', 1;
        IF @AssignedTo IS NOT NULL AND @AssignedTo <> @EmployeeId
           AND dbo.fn_HasRole(@EmployeeId, N'Admin,Manager') = 0
            THROW 50001, N'Việc này đang do nhân viên khác thực hiện.', 1;
        IF @RoomStatus <> 'Cleaning'
            THROW 50003, N'Phòng không ở trạng thái chờ dọn.', 1;

        UPDATE dbo.HousekeepingTasks
        SET Status      = 'Done',
            Result      = @Result,
            Note        = COALESCE(NULLIF(LTRIM(RTRIM(@Note)), N''), Note),
            AssignedTo  = ISNULL(AssignedTo, @EmployeeId),
            StartedAt   = ISNULL(StartedAt, SYSDATETIME()),
            CompletedAt = SYSDATETIME()
        WHERE TaskId = @TaskId;

        UPDATE dbo.Rooms
        SET Status = CASE WHEN @Result = 'Clean' THEN 'Available' ELSE 'Maintenance' END,
            Note   = CASE WHEN @Result = 'NeedMaintenance' THEN LEFT(@Note, 200) ELSE Note END
        WHERE RoomId = @RoomId;

        IF @OwnTran = 1 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

/* ---------- 6.7 Dashboard và báo cáo ---------- */

-- Chỉ số trong ngày. Doanh thu chỉ trả về cho Admin/Manager (Receptionist nhận NULL).
CREATE PROCEDURE dbo.sp_DashboardSummary
    @Date       DATE = NULL,
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager,Receptionist') = 0
        THROW 50001, N'Bạn không có quyền xem dashboard.', 1;

    SET @Date = ISNULL(@Date, CAST(GETDATE() AS DATE));
    DECLARE @From DATETIME2(0) = CAST(@Date AS DATETIME2(0));
    DECLARE @To DATETIME2(0) = DATEADD(DAY, 1, @From);
    DECLARE @CanSeeRevenue BIT = dbo.fn_HasRole(@EmployeeId, N'Admin,Manager');

    SELECT @Date AS ReportDate,
           s.TotalRooms, s.AvailableRooms, s.OccupiedRooms, s.CleaningRooms, s.MaintenanceRooms,
           (SELECT COUNT(*) FROM dbo.BookingRooms WHERE Status = 'Booked' AND CheckInDate = @Date) AS ArrivalsToday,
           (SELECT COUNT(*) FROM dbo.BookingRooms WHERE Status = 'CheckedIn') AS InHouseRooms,
           (SELECT COUNT(*) FROM dbo.BookingRooms WHERE Status = 'CheckedIn' AND CheckOutDate <= @Date) AS DeparturesDue,
           (SELECT COUNT(*) FROM dbo.BookingRooms WHERE ActualCheckIn >= @From AND ActualCheckIn < @To) AS CheckedInToday,
           (SELECT COUNT(*) FROM dbo.BookingRooms WHERE ActualCheckOut >= @From AND ActualCheckOut < @To) AS CheckedOutToday,
           (SELECT COUNT(*) FROM dbo.Bookings WHERE CreatedAt >= @From AND CreatedAt < @To) AS NewBookingsToday,
           CAST(CASE WHEN ISNULL(s.TotalRooms, 0) - ISNULL(s.MaintenanceRooms, 0) = 0 THEN 0
                     ELSE 100.0 * ISNULL(s.OccupiedRooms, 0) / (s.TotalRooms - s.MaintenanceRooms) END AS DECIMAL(5,1)) AS OccupancyRate,
           CASE WHEN @CanSeeRevenue = 1
                THEN (SELECT CAST(ISNULL(SUM(SignedAmount), 0) AS DECIMAL(14,0))
                      FROM dbo.Payments WHERE PaidAt >= @From AND PaidAt < @To) END AS RevenueToday
    FROM dbo.vw_RoomStatusSummary s;
END;
GO

-- Báo cáo doanh thu (Admin/Manager). @GroupBy = 'DAY' hoặc 'MONTH'.
-- Kết quả 1: thực thu theo PaidAt (thu - hoàn), chia theo loại và phương thức.
-- Kết quả 2: doanh số theo hóa đơn Paid theo IssuedAt.
CREATE PROCEDURE dbo.sp_RevenueReport
    @FromDate   DATE,
    @ToDate     DATE,
    @GroupBy    VARCHAR(10) = 'DAY',
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.fn_HasRole(@EmployeeId, N'Admin,Manager') = 0
        THROW 50001, N'Chỉ Admin/Manager được xem báo cáo doanh thu.', 1;
    IF @FromDate IS NULL OR @ToDate IS NULL OR @FromDate > @ToDate
        THROW 50007, N'Khoảng ngày không hợp lệ (từ ngày phải nhỏ hơn hoặc bằng đến ngày).', 1;
    IF @GroupBy NOT IN ('DAY', 'MONTH')
        THROW 50004, N'Chỉ nhóm theo DAY hoặc MONTH.', 1;

    DECLARE @From DATETIME2(0) = CAST(@FromDate AS DATETIME2(0));
    DECLARE @To DATETIME2(0) = DATEADD(DAY, 1, CAST(@ToDate AS DATETIME2(0)));

    SELECT x.Period,
           CAST(SUM(x.SignedAmount) AS DECIMAL(14,0)) AS NetRevenue,
           CAST(SUM(CASE WHEN x.Type = 'Deposit' THEN x.Amount ELSE 0 END) AS DECIMAL(14,0)) AS Deposits,
           CAST(SUM(CASE WHEN x.Type = 'Payment' THEN x.Amount ELSE 0 END) AS DECIMAL(14,0)) AS Payments,
           CAST(SUM(CASE WHEN x.Type = 'Refund'  THEN x.Amount ELSE 0 END) AS DECIMAL(14,0)) AS Refunds,
           CAST(SUM(CASE WHEN x.Method = 'Cash'         THEN x.SignedAmount ELSE 0 END) AS DECIMAL(14,0)) AS Cash,
           CAST(SUM(CASE WHEN x.Method = 'Card'         THEN x.SignedAmount ELSE 0 END) AS DECIMAL(14,0)) AS Card,
           CAST(SUM(CASE WHEN x.Method = 'BankTransfer' THEN x.SignedAmount ELSE 0 END) AS DECIMAL(14,0)) AS BankTransfer,
           CAST(SUM(CASE WHEN x.Method = 'EWallet'      THEN x.SignedAmount ELSE 0 END) AS DECIMAL(14,0)) AS EWallet
    FROM (SELECT CASE WHEN @GroupBy = 'MONTH' THEN DATEFROMPARTS(YEAR(p.PaidAt), MONTH(p.PaidAt), 1)
                      ELSE CAST(p.PaidAt AS DATE) END AS Period,
                 p.Type, p.Method, p.Amount, p.SignedAmount
          FROM dbo.Payments p
          WHERE p.PaidAt >= @From AND p.PaidAt < @To) x
    GROUP BY x.Period
    ORDER BY x.Period;

    SELECT y.Period,
           COUNT(*) AS InvoiceCount,
           CAST(SUM(y.RoomCharge) AS DECIMAL(14,0)) AS RoomCharge,
           CAST(SUM(y.ServiceCharge) AS DECIMAL(14,0)) AS ServiceCharge,
           CAST(SUM(y.Discount) AS DECIMAL(14,0)) AS Discount,
           CAST(SUM(y.TotalAmount - (y.RoomCharge + y.ServiceCharge - y.Discount)) AS DECIMAL(14,0)) AS TaxAmount,
           CAST(SUM(y.TotalAmount) AS DECIMAL(14,0)) AS TotalAmount
    FROM (SELECT CASE WHEN @GroupBy = 'MONTH' THEN DATEFROMPARTS(YEAR(i.IssuedAt), MONTH(i.IssuedAt), 1)
                      ELSE CAST(i.IssuedAt AS DATE) END AS Period,
                 i.RoomCharge, i.ServiceCharge, i.Discount, i.TotalAmount
          FROM dbo.Invoices i
          WHERE i.Status = 'Paid' AND i.IssuedAt >= @From AND i.IssuedAt < @To) y
    GROUP BY y.Period
    ORDER BY y.Period;
END;
GO

/* =====================================================================
   7. DỮ LIỆU KHỞI TẠO (bắt buộc)
   ===================================================================== */
SET NOCOUNT ON;

INSERT INTO dbo.Roles (RoleName, Description) VALUES
(N'Admin',        N'Quản trị hệ thống, tài khoản nhân viên'),
(N'Manager',      N'Quản lý danh mục, bảo trì, duyệt giảm giá, void hóa đơn, báo cáo'),
(N'Receptionist', N'Lễ tân: khách hàng, đặt phòng, check-in/out, dịch vụ, thu tiền'),
(N'Housekeeping', N'Buồng phòng: nhận và hoàn tất việc dọn phòng');

-- Tài khoản quản trị đầu tiên. Mật khẩu: Admin@123 (BCrypt cost 11) - ĐỔI NGAY sau lần đăng nhập đầu.
INSERT INTO dbo.Employees (RoleId, FullName, Username, PasswordHash, Email)
SELECT RoleId, N'Quản trị hệ thống', 'admin',
       '$2a$11$8h1Yje05e52dDho8gcVNp.TttdCiwTXiq1g4StEoQBg8hezl1gO0m', 'admin@hotel.local'
FROM dbo.Roles WHERE RoleName = N'Admin';

-- Dịch vụ hệ thống dùng khi check-out trễ (giá thực ghi lúc check-out)
INSERT INTO dbo.Services (ServiceName, Category, Unit, Price, IsActive, IsSystem)
VALUES (N'Phụ thu trả phòng trễ', N'Phụ thu', N'Lần', 0, 1, 1);
GO

PRINT N'Đã tạo xong database HotelManagement (14 bảng). Chạy tiếp file 02 nếu cần dữ liệu mẫu.';
GO
