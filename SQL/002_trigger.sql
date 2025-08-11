USE MsList;
GO
--
-- tự tạo workspace cá nhân "My Lists"
CREATE OR ALTER TRIGGER trg_CreateWorkspaceForNewAccount
ON Account
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    SET IDENTITY_INSERT Workspace ON;

    INSERT INTO Workspace (Id, WorkspaceName, CreatedBy, IsPersonal, CreatedAt, UpdatedAt)
    SELECT 
        Id,
        'My Lists',
        Id,
        1,
        GETDATE(),
        GETDATE()
    FROM inserted;

    SET IDENTITY_INSERT Workspace OFF;
END;
GO

-- Trigger for when a new list is created
--CREATE TRIGGER TR_List_Insert
--ON List
--AFTER INSERT
--AS
--BEGIN
--    SET NOCOUNT ON;

--    -- Insert into RecentList when a user creates a new list
--    INSERT INTO RecentList (AccountId, ListId, LastAccessedAt)
--    SELECT 
--        i.CreatedBy,
--        i.Id,
--        GETDATE()
--    FROM inserted i
--    WHERE NOT EXISTS (
--        SELECT 1 
--        FROM RecentList rl 
--        WHERE rl.AccountId = i.CreatedBy 
--        AND rl.ListId = i.Id
--    );
--END;
--GO

-- tự động gán quyền 'OWNER' cho người mới tạo list đó
CREATE OR ALTER TRIGGER trg_AssignOwnerToListCreator
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
    FROM ListPermission 
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
        CreatedAt,
        UpdatedAt
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

-- Trigger for when a user is granted permission to a list
CREATE OR ALTER TRIGGER trg_ListMemberPermission_Insert
ON ListMemberPermission
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert or update RecentList when a user is granted permission
    MERGE INTO RecentList AS target
    USING (
        SELECT 
            i.AccountId,
            i.ListId,
            GETDATE() AS LastAccessedAt
        FROM inserted i
    ) AS source
    ON target.AccountId = source.AccountId 
    AND target.ListId = source.ListId
    WHEN MATCHED THEN
        UPDATE SET LastAccessedAt = source.LastAccessedAt
    WHEN NOT MATCHED THEN
        INSERT (AccountId, ListId, LastAccessedAt)
        VALUES (source.AccountId, source.ListId, source.LastAccessedAt);
END;
GO

-- Auto-create a default "All Items" list view (type: List)
-- If the list type is Board, Gallery, Calendar, or Form, 
-- also create a view matching the list type
CREATE OR ALTER TRIGGER trg_CreateDefaultListView
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
        INSERT INTO ListView (ListId, CreatedBy, ViewTypeId, ViewName)
        SELECT 
            i.Id,
            i.CreatedBy,
            @ListViewTypeId,
            'All Items'
            
        FROM inserted i;
    END
    ELSE
    BEGIN
        RAISERROR ('ViewType "List" not found in ViewType table.', 16, 1);
    END

    -- Tạo ListView bổ sung nếu ListType không phải là 'List' với DisplayOrder = 1
    INSERT INTO ListView (ListId, CreatedBy, ViewTypeId, ViewName)
    SELECT 
        i.Id,
        i.CreatedBy,
        vt.Id,
        lt.Title
       
    FROM inserted i
    INNER JOIN ListType lt ON i.ListTypeId = lt.Id
    INNER JOIN ViewType vt ON lt.Title = vt.Title
    WHERE lt.Title != 'List' AND vt.Id IS NOT NULL;
END;
GO

-- Create a trigger to enforce the constraint
CREATE OR ALTER TRIGGER trg_FavoriteList_PermissionCheck
ON FavoriteList
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if inserted/updated rows have corresponding permissions in ListMemberPermission
    IF EXISTS (
        SELECT i.ListId, i.AccountId
        FROM inserted i
        LEFT JOIN ListMemberPermission lmp
            ON lmp.ListId = i.ListId
            AND lmp.AccountId = i.AccountId
        WHERE lmp.ListId IS NULL
    )
    BEGIN
        RAISERROR ('Cannot add list to FavoriteList. User must have permissions for the list in ListMemberPermission.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;
END;
GO

-- trigger tự động tạo Dynamic Column từ System Column
CREATE OR ALTER TRIGGER trg_CreateDynamicColumnsOnListInsert
ON List
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare variables
    DECLARE @ListId INT;
    DECLARE @SystemColumnId INT;
    DECLARE @SystemDataTypeId INT;
    DECLARE @ColumnName NVARCHAR(100);
    DECLARE @DisplayOrder INT;
    DECLARE @CreatedBy INT;
    DECLARE @DataTypeSettingKeyId INT;
    DECLARE @KeyValue NVARCHAR(255);

    -- Cursor to iterate through inserted List records
    DECLARE list_cursor CURSOR FOR
    SELECT Id, CreatedBy FROM inserted;

    OPEN list_cursor;
    FETCH NEXT FROM list_cursor INTO @ListId, @CreatedBy;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Cursor to iterate through SystemColumn records
        DECLARE system_column_cursor CURSOR FOR
        SELECT Id, SystemDataTypeId, ColumnName
        FROM SystemColumn;

        OPEN system_column_cursor;
        FETCH NEXT FROM system_column_cursor INTO @SystemColumnId, @SystemDataTypeId, @ColumnName ;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Insert into ListDynamicColumn
            INSERT INTO ListDynamicColumn (
                ListId,
                SystemDataTypeId,
                SystemColumnId,
                ColumnName, 
                IsSystemColumn,
                IsVisible,
                CreatedBy,
                CreatedAt
            )
            VALUES (
                @ListId,
                @SystemDataTypeId,
                @SystemColumnId,
                @ColumnName,
                1, -- IsSystemColumn = 1 since it's copied from SystemColumn
                CASE WHEN @ColumnName = 'Title' THEN 1 ELSE 0 END, -- IsVisible = 1 for 'Title', 0 for others
                @CreatedBy,
                GETDATE()
            );

            -- Get the newly inserted ListDynamicColumn Id
            DECLARE @NewDynamicColumnId INT;
            SET @NewDynamicColumnId = SCOPE_IDENTITY();

            -- Cursor to iterate through SystemColumnSettingValue for the current SystemColumn
            DECLARE setting_cursor CURSOR FOR
            SELECT DataTypeSettingKeyId, KeyValue
            FROM SystemColumnSettingValue
            WHERE SystemColumnId = @SystemColumnId;

            OPEN setting_cursor;
            FETCH NEXT FROM setting_cursor INTO @DataTypeSettingKeyId, @KeyValue;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                -- Insert into DynamicColumnSettingValue
                INSERT INTO DynamicColumnSettingValue (
                    DynamicColumnId,
                    DataTypeSettingKeyId,
                    KeyValue,
                    CreatedAt,
                    UpdatedAt
                )
                VALUES (
                    @NewDynamicColumnId,
                    @DataTypeSettingKeyId,
                    @KeyValue,
                    GETDATE(),
                    GETDATE()
                );

                FETCH NEXT FROM setting_cursor INTO @DataTypeSettingKeyId, @KeyValue;
            END;

            CLOSE setting_cursor;
            DEALLOCATE setting_cursor;

            FETCH NEXT FROM system_column_cursor INTO @SystemColumnId, @SystemDataTypeId, @ColumnName;
        END;

        CLOSE system_column_cursor;
        DEALLOCATE system_column_cursor;

        FETCH NEXT FROM list_cursor INTO @ListId, @CreatedBy;
    END;

    CLOSE list_cursor;
    DEALLOCATE list_cursor;
END;
go

-- Trigger to sync ListDynamicColumn to ListViewColumn for "All List" ListView
CREATE OR ALTER TRIGGER trg_ListDynamicColumn_AfterInsert
ON ListDynamicColumn
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ListId INT, @ListDynamicColumnId INT, @IsVisible BIT;

    -- Cursor to handle multiple inserted rows
    DECLARE column_cursor CURSOR FOR
    SELECT Id, ListId, IsVisible FROM inserted;

    OPEN column_cursor;
    FETCH NEXT FROM column_cursor INTO @ListDynamicColumnId, @ListId, @IsVisible;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Insert new ListDynamicColumn into ListViewColumn for "All List" ListView
        INSERT INTO ListViewColumn (ListViewId, ListDynamicColumnId, DisplayOrder, IsVisible)
        SELECT 
            lv.Id,
            @ListDynamicColumnId,
            (SELECT ISNULL(MAX(DisplayOrder), 0) + 1 FROM ListViewColumn WHERE ListViewId = lv.Id),
            @IsVisible
        FROM ListView lv
        WHERE lv.ListId = @ListId AND lv.IsSystem = 1 AND lv.ViewName = 'All List';

        -- Update DisplayOrder to prioritize visible columns
        DECLARE @ListViewId INT;
        SELECT @ListViewId = Id FROM ListView WHERE ListId = @ListId AND IsSystem = 1 AND ViewName = 'All List';

        IF @ListViewId IS NOT NULL
        BEGIN
            EXEC dbo.UpdateListViewColumnDisplayOrder @ListViewId;
        END;

        FETCH NEXT FROM column_cursor INTO @ListDynamicColumnId, @ListId, @IsVisible;
    END;

    CLOSE column_cursor;
    DEALLOCATE column_cursor;
END;
GO

-- Function to calculate DisplayOrder for ListViewColumn
CREATE OR ALTER FUNCTION dbo.CalculateListViewColumnDisplayOrder
(
    @ListViewId INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        Id,
        ListViewId,
        ListDynamicColumnId,
        ROW_NUMBER() OVER (ORDER BY IsVisible DESC, ListDynamicColumnId) AS DisplayOrder,
        IsVisible
    FROM ListViewColumn
    WHERE ListViewId = @ListViewId
);
GO

-- Procedure to update DisplayOrder in ListViewColumn
CREATE OR ALTER PROCEDURE dbo.UpdateListViewColumnDisplayOrder
    @ListViewId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE lvc
    SET DisplayOrder = calc.DisplayOrder
    FROM ListViewColumn lvc
    INNER JOIN dbo.CalculateListViewColumnDisplayOrder(@ListViewId) calc
        ON lvc.Id = calc.Id
    WHERE lvc.ListViewId = @ListViewId;
END;
GO
