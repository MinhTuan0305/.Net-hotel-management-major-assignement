@echo off
chcp 65001 >nul
setlocal EnableExtensions

rem =====================================================================
rem  LÀM MỚI DATABASE HotelManagement: chạy 01 (tạo lại DB) -> 03 (kiểm thử) -> 02 (dữ liệu mẫu)
rem ---------------------------------------------------------------------
rem  Cách dùng:
rem    reset_db.bat                    hỏi tên server (Enter = "."), hỏi xác nhận
rem    reset_db.bat .                  SQL Server bản mặc định (Developer)
rem    reset_db.bat .\SQLEXPRESS       SQL Server Express
rem    reset_db.bat . /y               bỏ qua bước xác nhận
rem
rem  Tham số sqlcmd:
rem    -E  đăng nhập bằng tài khoản Windows      -b  dừng khi có lỗi SQL
rem    -C  tin chứng chỉ của server (cần với sqlcmd mới)
rem    -I  bật QUOTED_IDENTIFIER (bắt buộc: file 01 có filtered index, thiếu -I thì file 02 lỗi 1934)
rem    -f 65001  đọc/ghi UTF-8 để giữ tiếng Việt
rem
rem  File 03 không báo lỗi khi có ca FAIL (chỉ in bảng), nên script tự tìm dòng FAIL trong kết quả.
rem =====================================================================

set "SERVER=%~1"
set "EXITCODE=0"
set "ASK=1"
if /i "%~2"=="/y" set "ASK=0"
if "%SERVER%"=="" (
    set /p "SERVER=Tên SQL Server (Enter = .): "
)
rem Bỏ dấu cách gõ thừa (tên server không có dấu cách).
if defined SERVER set "SERVER=%SERVER: =%"
if "%SERVER%"=="" set "SERVER=."

set "DIR=%~dp0"
set "LOG=%TEMP%\HotelManagement_KiemThu.log"

where sqlcmd >nul 2>&1
if errorlevel 1 (
    echo [LỖI] Không tìm thấy sqlcmd. Cài "SQL Server Command Line Utilities" rồi chạy lại.
    goto :fail
)

echo.
echo Server  : %SERVER%
echo Thư mục : %DIR%
if "%ASK%"=="0" goto :run
echo.
echo CẢNH BÁO: file 01 sẽ XÓA database HotelManagement và tạo lại. Dữ liệu hiện có sẽ mất hết.
echo Đóng Visual Studio/SSMS đang mở DB này không bắt buộc (script tự ngắt kết nối), nhưng nên lưu việc đang làm.
set "OK="
set /p "OK=Gõ Y để tiếp tục: "
if /i not "%OK%"=="Y" (
    echo Đã hủy.
    goto :end
)

:run
echo.
echo [1/3] Tạo lại database (01_HotelManagement_TaoDB.sql)...
sqlcmd -S "%SERVER%" -E -C -b -I -f 65001 -i "%DIR%01_HotelManagement_TaoDB.sql"
if errorlevel 1 (
    echo [LỖI] File 01 chạy không thành công. Xem thông báo phía trên.
    goto :fail
)

echo.
echo [2/3] Kiểm thử nghiệp vụ (03_HotelManagement_KiemThu.sql)...
sqlcmd -S "%SERVER%" -E -C -b -I -f 65001 -W -i "%DIR%03_HotelManagement_KiemThu.sql" -o "%LOG%"
set "RC=%errorlevel%"
type "%LOG%"
if not "%RC%"=="0" (
    echo [LỖI] File 03 chạy không thành công. Xem thông báo phía trên.
    goto :fail
)
findstr /R /C:"^ *[0-9][0-9]* *FAIL " "%LOG%" >nul
if not errorlevel 1 (
    echo.
    echo [LỖI] Có ca kiểm thử FAIL:
    findstr /R /C:"^ *[0-9][0-9]* *FAIL " "%LOG%"
    echo Sửa database\01 cho hết FAIL rồi chạy lại. Chưa nạp dữ liệu mẫu.
    goto :fail
)
echo Kiểm thử: tất cả PASS.

echo.
echo [3/3] Nạp dữ liệu mẫu (02_HotelManagement_DuLieuMau.sql)...
sqlcmd -S "%SERVER%" -E -C -b -I -f 65001 -i "%DIR%02_HotelManagement_DuLieuMau.sql"
if errorlevel 1 (
    echo [LỖI] File 02 chạy không thành công. Xem thông báo phía trên.
    goto :fail
)

echo.
echo XONG. Database HotelManagement đã được làm mới trên server %SERVER%.
echo Dữ liệu mẫu tính theo ngày hôm nay; các màn hình "hôm nay" chỉ đúng trong ngày.
set "EXITCODE=0"
goto :end

:fail
set "EXITCODE=1"

:end
rem Mở bằng nhấp đúp (không có tham số) thì dừng lại để kịp đọc kết quả.
if "%~1"=="" pause
exit /b %EXITCODE%
