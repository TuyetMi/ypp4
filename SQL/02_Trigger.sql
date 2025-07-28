USE MsList;
GO
-- Xóa trigger nếu đã tồn tại để tránh lỗi khi chạy lại
IF OBJECT_ID('trg_AdjustListViewDisplayOrder', 'TR') IS NOT NULL
    DROP TRIGGER trg_AdjustListViewDisplayOrder;
GO
IF OBJECT_ID('trg_CreateDefaultListView', 'TR') IS NOT NULL
    DROP TRIGGER trg_CreateDefaultListView;
GO
IF OBJECT_ID('trg_AssignOwnerToListCreator', 'TR') IS NOT NULL
    DROP TRIGGER trg_AssignDefaultListPermission;
GO
IF OBJECT_ID('TRG_FavoriteList_PermissionCheck', 'TR') IS NOT NULL
    DROP TRIGGER TRG_FavoriteList_PermissionCheck;
GO


-- tự động tăng thứ tự display order cho list view mới trong 1 list 
CREATE TRIGGER trg_AdjustListViewDisplayOrder
ON ListView
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Tạo bảng tạm để lưu các ListId bị ảnh hưởng
    DECLARE @AffectedLists TABLE (ListId INT);

    -- Lấy các ListId từ inserted hoặc updated
    INSERT INTO @AffectedLists (ListId)
    SELECT ListId
    FROM inserted
    UNION
    SELECT ListId
    FROM deleted;

    -- Cập nhật DisplayOrder cho từng ListId bị ảnh hưởng
    WITH RankedListViews AS (
        SELECT 
            Id,
            ListId,
            ROW_NUMBER() OVER (PARTITION BY ListId ORDER BY DisplayOrder, Id) - 1 AS NewDisplayOrder
        FROM ListView
        WHERE ListId IN (SELECT ListId FROM @AffectedLists)
    )
    UPDATE ListView
    SET DisplayOrder = rlv.NewDisplayOrder
    FROM ListView lv
    INNER JOIN RankedListViews rlv ON lv.Id = rlv.Id
    WHERE lv.DisplayOrder != rlv.NewDisplayOrder;
END;
GO

-- tự động tạo 1 list view dạng list - default là "All Items"
-- nếu list mới dạng "board, galary, calendar, form" thì tạo thêm 1 list view có cùng dạng vs list type của list mới
CREATE TRIGGER trg_CreateDefaultListView
ON List
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Tạo ListView mặc định "All Items" với ViewType là 'List' và DisplayOrder = 0
    DECLARE @ListViewTypeId INT;
    SELECT @ListViewTypeId = Id FROM ViewType WHERE Title = 'List';

    IF @ListViewTypeId IS NOT NULL
    BEGIN
        INSERT INTO ListView (ListId, CreatedBy, ViewTypeId, ViewName, DisplayOrder)
        SELECT 
            i.Id,
            i.CreatedBy,
            @ListViewTypeId,
            'All Items',
            0
        FROM inserted i;
    END
    ELSE
    BEGIN
        RAISERROR ('ViewType "List" not found in ViewType table.', 16, 1);
    END

    -- Tạo ListView bổ sung nếu ListType không phải là 'List' với DisplayOrder = 1
    INSERT INTO ListView (ListId, CreatedBy, ViewTypeId, ViewName, DisplayOrder)
    SELECT 
        i.Id,
        i.CreatedBy,
        vt.Id,
        lt.Title,
        1
    FROM inserted i
    INNER JOIN ListType lt ON i.ListTypeId = lt.Id
    INNER JOIN ViewType vt ON lt.Title = vt.Title
    WHERE lt.Title != 'List' AND vt.Id IS NOT NULL;
END;
GO

-- tự động gán quyền 'OWNER' cho người mới tạo list đó
CREATE TRIGGER trg_AssignOwnerToListCreator
ON List
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @OwnerPermissionId INT;
    DECLARE @OwnerPermissionCode NVARCHAR(50);

    -- Lấy thông tin quyền OWNER
    SELECT 
        @OwnerPermissionId = Id,
        @OwnerPermissionCode = PermissionCode
    FROM Permission 
    WHERE PermissionCode = 'OWNER';

    IF @OwnerPermissionId IS NULL
    BEGIN
        RAISERROR('Permission "OWNER" not found in Permission table.', 16, 1);
        RETURN;
    END

    -- Thêm vào bảng ListMemberPermission
    INSERT INTO ListMemberPermission (
        ListId,
        AccountId,
        HighestPermissionId,
        HighestPermissionCode,
        GrantedByAccountId,
        Note,
        CreateAt,
        UpdateAt
    )
    SELECT 
        i.Id,
        i.CreatedBy,
        @OwnerPermissionId,
        @OwnerPermissionCode,
        i.CreatedBy,
        N'List creator',
        GETDATE(),
        GETDATE()
    FROM inserted i;
END;
GO

-- Create a trigger to enforce the constraint
CREATE TRIGGER TRG_FavoriteList_PermissionCheck
ON FavoriteList
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if inserted/updated rows have corresponding permissions in ListMemberPermission
    IF EXISTS (
        SELECT i.ListId, i.FavoriteListOfUser
        FROM inserted i
        LEFT JOIN ListMemberPermission lmp
            ON lmp.ListId = i.ListId
            AND lmp.AccountId = i.FavoriteListOfUser
        WHERE lmp.ListId IS NULL
    )
    BEGIN
        RAISERROR ('Cannot add list to FavoriteList. User must have permissions for the list in ListMemberPermission.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;
END;
GO