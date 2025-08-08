-- Script cập nhật ngưỡng điểm cấp thành viên
UPDATE CapThanhViens SET NguongDiem = 0 WHERE MaCapThanhVien = 1;
UPDATE CapThanhViens SET NguongDiem = 300 WHERE MaCapThanhVien = 2;
UPDATE CapThanhViens SET NguongDiem = 500 WHERE MaCapThanhVien = 3;
