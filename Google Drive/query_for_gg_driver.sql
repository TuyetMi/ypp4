
----- Screen: Account -----
-- 1. Hiển thị thông tin account infomation
DECLARE @UserId INT = 2;
SELECT
	a.UserName,
	a.UserImg,
	a.Email
FROM Account a
WHERE a.UserId = @UserId
go

------ Screen: Setting -----
-- 1. Hiển thị các select cảu người dùng trong cài đặt chung
DECLARE @UserId INT = 2;
SELECT 
	ast.SettingKey,
	ast.SettingValue
FROM SettingUser su
JOIN AppSetting ast ON su.SettingId = ast.SettingId
JOIN Account a on su.UserId = a.UserId
WHERE a.UserId = @UserId
go

-- Main Screen --
-- 1. hiển thị recomment folder
-- 1. Các hành động mới nhất của user (1 dòng / object)
WITH RecentActions AS (
    SELECT
        ar.ObjectId,
        ar.ObjectTypeId,
        ar.ActionDateTime,
        ROW_NUMBER() OVER (PARTITION BY ar.ObjectId, ar.ObjectTypeId ORDER BY ar.ActionDateTime DESC) AS rn
    FROM ActionRecent ar
    WHERE ar.UserId = 3
),
FilteredRecent AS (
    SELECT *
    FROM RecentActions
    WHERE rn = 1
),

-- 2. CTE đệ quy: duyệt ngược từ folder của file lên đến gốc
FolderHierarchy AS (
    -- Start from file
    SELECT
        uf.FileId AS FileId,
        f.FolderId AS FolderId,
        f.ParentId,
        0 AS Level
    FROM UserFile uf
    JOIN FilteredRecent fr ON fr.ObjectTypeId = 2 AND fr.ObjectId = uf.FileId
    JOIN Folder f ON f.FolderId = uf.FolderId

    UNION ALL

    -- Recurse upward through parent folders
    SELECT
        fh.FileId,
        f.FolderId AS FolderId,
        f.ParentId,
        fh.Level + 1
    FROM FolderHierarchy fh
    JOIN Folder f ON f.FolderId = fh.ParentId
),

-- 3. Kiểm tra folder nào trong cây user được share
SharedFolders AS (
    SELECT
        fh.FileId,
        fh.FolderId,
        fh.Level,
        s.ShareId AS ShareId
    FROM FolderHierarchy fh
    JOIN Share s ON s.ObjectTypeId = 1 AND s.ObjectId = fh.FolderId
    JOIN SharedUser su ON su.ShareId = s.ShareId AND su.UserId = 2
),

-- 4. Lấy folder cao nhất user được share
TopSharedFolderPerFile AS (
    SELECT FileId, FolderId
    FROM (
        SELECT *,
            ROW_NUMBER() OVER (PARTITION BY FileId ORDER BY Level DESC) AS rn
        FROM SharedFolders
    ) AS ranked
    WHERE rn = 1
),

-- 5. Gom folder từ 2 nguồn: folder trực tiếp và folder từ file
RecommendedFolders AS (
    -- Folder được tương tác trực tiếp
    SELECT fr.ObjectId AS FolderId, fr.ActionDateTime
    FROM FilteredRecent fr
    JOIN Share s ON s.ObjectId = fr.ObjectId AND s.ObjectTypeId = 1
    JOIN SharedUser su ON su.ShareId = s.ShareId AND su.UserId = 2
    WHERE fr.ObjectTypeId = 1

    UNION

    -- Folder cao nhất được share từ file
    SELECT tsf.FolderId, fr.ActionDateTime
    FROM FilteredRecent fr
    JOIN TopSharedFolderPerFile tsf ON fr.ObjectTypeId = 2 AND fr.ObjectId = tsf.FileId
)

-- 6. Trả về 5 folder gần nhất
SELECT DISTINCT TOP 5
    f.FolderId AS FolderId,
    f.FolderName,
    f.FolderPath,
    rf.ActionDateTime
FROM RecommendedFolders rf
JOIN Folder f ON f.FolderId = rf.FolderId
ORDER BY rf.ActionDateTime DESC;


