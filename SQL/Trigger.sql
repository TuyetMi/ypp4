CREATE TRIGGER trg_ListView_AutoDisplayOrder
ON ListView
INSTEAD OF INSERT
AS
BEGIN
    INSERT INTO ListView (ListId, CreatedBy, ViewTypeId, ViewName, DisplayOrder)
    SELECT
        i.ListId,
        i.CreatedBy,
        i.ViewTypeId,
        i.ViewName,
        ISNULL(
            (SELECT MAX(DisplayOrder) + 1 FROM ListView WHERE ListId = i.ListId),
            0
        )
    FROM inserted i;
END;


CREATE TRIGGER trg_AfterInsert_List
ON List
AFTER INSERT
AS
BEGIN
    -- ViewTypeId của dạng 'List' để tạo view 'All Items'
    DECLARE @ListViewTypeId INT;

    SELECT @ListViewTypeId = Id FROM ViewType WHERE Title = 'List';

    -- Insert View 'All Items' cho mỗi List mới
    INSERT INTO ListView (ListId, CreatedBy, ViewTypeId, ViewName)
    SELECT 
        i.Id,
        i.CreatedBy,
        @ListViewTypeId,
        N'All Items'
    FROM inserted i;

    -- Insert View cùng loại với List (VD: Board, Form, ...)
    INSERT INTO ListView (ListId, CreatedBy, ViewTypeId, ViewName)
    SELECT 
        i.Id,
        i.CreatedBy,
        i.ViewTypeId,
        vt.Title
    FROM inserted i
    JOIN ViewType vt ON vt.Id = i.ViewTypeId;
END;
