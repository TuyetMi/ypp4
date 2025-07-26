USE MsList
GO

CREATE PROCEDURE sp_AdjustListViewDisplayOrder
    @ListId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @ListId IS NULL RETURN;

    -- Update DisplayOrder cho 1 List cụ thể
    WITH RankedListViews AS (
        SELECT 
            Id,
            ROW_NUMBER() OVER (ORDER BY DisplayOrder, Id) - 1 AS NewDisplayOrder
        FROM ListView
        WHERE ListId = @ListId
    )
    UPDATE lv
    SET DisplayOrder = rlv.NewDisplayOrder
    FROM ListView lv
    INNER JOIN RankedListViews rlv ON lv.Id = rlv.Id
    WHERE lv.DisplayOrder != rlv.NewDisplayOrder;
END;
GO

CREATE PROCEDURE sp_CreateDefaultListView
    @ListId INT,
    @CreatedBy INT,
    @ListTypeId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Tạo view mặc định 'All Items'
    DECLARE @ListViewType_List INT;
    SELECT @ListViewType_List = Id FROM ViewType WHERE Title = 'List';

    IF @ListViewType_List IS NOT NULL
    BEGIN
        INSERT INTO ListView (ListId, CreatedBy, ViewTypeId, ViewName, DisplayOrder)
        VALUES (@ListId, @CreatedBy, @ListViewType_List, N'All Items', 0);
    END

    -- Tạo thêm view nếu list là loại khác 'List'
    DECLARE @ListTypeTitle NVARCHAR(100), @OtherViewTypeId INT;

    SELECT @ListTypeTitle = Title FROM ListType WHERE Id = @ListTypeId;

    IF @ListTypeTitle IS NOT NULL AND @ListTypeTitle != 'List'
    BEGIN
        SELECT @OtherViewTypeId = Id FROM ViewType WHERE Title = @ListTypeTitle;

        IF @OtherViewTypeId IS NOT NULL
        BEGIN
            INSERT INTO ListView (ListId, CreatedBy, ViewTypeId, ViewName, DisplayOrder)
            VALUES (@ListId, @CreatedBy, @OtherViewTypeId, @ListTypeTitle, 1);
        END
    END
END;
GO

