------------------------------------------
-- MÀN HÌNH DASHBOARD --

-- 1. Hiển thị tất cả các list mà mình được share và tự tạo
-- 2. Hiển thị tất cả các list mà mình tự tạo
-- 3. Hiển thị thông tin user
-- 4. Hiển thị Favorites List của mình

-- 1. Hiển thị tất cả các list mà mình được share và tự tạo
DECLARE @UserId INT = 2;  -- thay bằng ID của user đang đăng nhập

SELECT DISTINCT 
	l.*,
    lmp.HighestPermissionCode, -- extra
	lmp.GrantedByAccountId -- extra
FROM List l
LEFT JOIN ListMemberPermission lmp ON l.Id = lmp.ListId AND lmp.AccountId = @UserId -- vẫn dùng JOIN ĐC vì người tạo sẽ đc thêm vào ListMemberPermission
WHERE l.CreatedBy = @UserId 
	OR lmp.Id IS NOT NULL
	AND l.ListStatus = 'Active';
GO

-- 2. Hiển thị tất cả các list mà mình tự tạo
DECLARE @UserId INT = 2; 
SELECT 
    l.*
FROM List l
WHERE l.CreatedBy = @UserId
    AND l.ListStatus = 'Active';
GO

-- 3. Hiển thị thông tin user
DECLARE @UserId INT = 2;

SELECT 
    a.Id,
    a.FirstName,
	a.LastName,
	a.Avatar,
    a.Email,
	a.Company,
    a.AccountStatus
FROM Account a
WHERE a.Id = @UserId;
GO

-- 4. Hiển thị Favorites List của mình
DECLARE @UserId INT = 2;  -- thay bằng ID user đang đăng nhập

SELECT 
    l.Id AS ListId,
    l.ListName,
    l.ListTypeId,
    l.CreatedBy,
    l.CreatedAt,
    l.ListStatus,
    fl.CreateAt AS FavoritedAt
FROM FavoriteList fl
INNER JOIN List l ON fl.ListId = l.Id
WHERE fl.FavoriteListOfUser = @UserId
    AND l.ListStatus = 'Active'
ORDER BY fl.CreateAt DESC;
GO

------------------------------------------
-- MÀN HÌNH SHOW LIST TYPE ĐỂ TẠO LIST MỚI --

-- 1. Hiển thị tất cả list type để chọn
SELECT 
    Id,
    Title,
    ListTypeDescription,
    HeaderImage
FROM ListType
ORDER BY Title;
-- 2. Hiển thị tất cả templates 
SELECT 
    lt.Id,
    lt.Title,
    lt.HeaderImage,
    lt.Sumary
FROM ListTemplate lt
ORDER BY lt.Title;

------------------------------------------
-- MÀN HÌNH CHỌN LIST TYPE ĐỂ TẠO LIST MỚI --
-- 1. Hiển thị thông tin của list type mà người dùng đã chọn
DECLARE @ListTypeId INT = 3;  -- thay bằng ID người dùng chọn

SELECT 
    Id,
    Title,
    Icon,
    ListTypeDescription,
    HeaderImage
FROM ListType
WHERE Id = @ListTypeId;
GO

------------------------------------------
-- MÀN HÌNH CHỌN TEMPLATE ĐỂ TẠO LIST MỚI DỰA THEO TEMPLATE ĐÓ--
-- 1. Hiển thị tất cả template
-- 2. Hiển thị thông tin của template đó
-- 3. Hiển thị sample data cho template đó (Chưa tạo đc)

-- 1. Hiển thị tất cả templates 
SELECT 
    lt.Id,
    lt.Title,
    lt.HeaderImage,
    lt.Sumary
FROM ListTemplate lt
ORDER BY lt.Title;

-- 2. Hiển thị thông tin của template đó
DECLARE @TemplateId INT = 5;  -- thay bằng ID của template muốn xem

SELECT 
    lt.Id,
    lt.Title,
    lt.HeaderImage,
    lt.TemplateDescription,
    lt.Icon,
    lt.Color,
    lt.Sumary,
    lt.Feature,
    lt.ListTypeId,
    lt.ProviderId
FROM ListTemplate lt
WHERE lt.Id = @TemplateId;
GO






