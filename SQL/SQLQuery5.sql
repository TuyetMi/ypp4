
SELECT 
    lv.Id AS ListViewId,
    lv.ViewName,
    lv.DisplayOrder,
    vt.Title AS ViewType,
    l.ListName,
    l.Id AS ListId,
    l.Color
FROM ListView lv
JOIN ViewType vt ON lv.ViewTypeId = vt.Id
JOIN List l ON lv.ListId = l.Id
WHERE lv.CreatedBy = 1
ORDER BY l.Id, lv.DisplayOrder;

-- Lấy các list view của list có id = 7
SELECT 
    lv.Id AS ListViewId,
    lv.ViewName,
    lv.DisplayOrder,
    lv.CreatedBy,
	l.Id
FROM ListView lv
JOIN List l ON lv.ListId = l.Id
WHERE lv.ListId = 7
ORDER BY lv.DisplayOrder;

-- Lấy các ListView có loại view là "Board" và do user Id = 3 tạo.
SELECT
	lv.Id,
	lv.ViewName,
	lv.CreatedBy,
	lv.ViewTypeId as ViewType,
	vt.Title 
FROM ListView lv 
JOIN ViewType vt ON lv.ViewTypeId = vt.Id
WHERE vt.Title = 'Board' and lv.CreatedBy = 3
ORDER BY lv.ListId

-- Lấy tất cả các List do user có Id = 5 tạo, kèm số lượng ListView của từng List.
SELECT 
	l.Id,
	l.CreatedBy,
	COUNT(vl.I
FROM List l
JOIN ListView lv

