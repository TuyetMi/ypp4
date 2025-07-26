------------------------------------------
-- DASHBOARD SCREEN --

-- 1. Display all lists that the logged-in user created or were shared with them
-- 2. Display all lists that the logged-in user created
-- 3. Display information of the logged-in user
-- 4. Display favorite lists of the logged-in user

-- 1. Display all lists that the logged-in user created or were shared with them
DECLARE @UserId INT = 2;  -- Replace with the ID of the logged-in user

SELECT DISTINCT 
    l.*,
    lmp.HighestPermissionCode, -- Extra
    lmp.GrantedByAccountId -- Extra
FROM List l
LEFT JOIN ListMemberPermission lmp ON l.Id = lmp.ListId AND lmp.AccountId = @UserId -- JOIN is still used because the creator will be added to ListMemberPermission
WHERE l.CreatedBy = @UserId 
    OR lmp.Id IS NOT NULL
    AND l.ListStatus = 'Active';
GO

-- 2. Display all lists that the logged-in user created
DECLARE @UserId INT = 2; 
SELECT 
    l.*
FROM List l
WHERE l.CreatedBy = @UserId
    AND l.ListStatus = 'Active';
GO

-- 3. Display information of the logged-in user
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

-- 4. Display favorite lists of the logged-in user
DECLARE @UserId INT = 2;  -- Replace with the ID of the logged-in user

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
-- LIST TYPE SELECTION SCREEN FOR CREATING A NEW LIST --

-- 1. Display all list types for selection
SELECT 
    Id,
    Title,
    ListTypeDescription,
    HeaderImage
FROM ListType
ORDER BY Title;

-- 2. Display all templates 
SELECT 
    lt.Id,
    lt.Title,
    lt.HeaderImage,
    lt.Sumary
FROM ListTemplate lt
ORDER BY lt.Title;

------------------------------------------
-- LIST TYPE SELECTION SCREEN FOR CREATING A NEW LIST --
-- 1. Display information of the list type selected by the user
DECLARE @ListTypeId INT = 3;  -- Replace with the ID selected by the user

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
-- TEMPLATE SELECTION SCREEN FOR CREATING A NEW LIST BASED ON A TEMPLATE --
-- 1. Display all templates
-- 2. Display information of the selected template
-- 3. Display sample data for the selected template (Not yet implemented)

-- 1. Display all templates 
SELECT 
    lt.Id,
    lt.Title,
    lt.HeaderImage,
    lt.Sumary
FROM ListTemplate lt
ORDER BY lt.Title;

-- 2. Display information of the selected template
DECLARE @TemplateId INT = 5;  -- Replace with the ID of the template to view

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

----------------------------------------
-- LIST SCREEN 
-- 1. Display List information 
-- 2. Display all list views of the selected list
-- 3. Display information of SystemColumn
-- 4. Display avatars of users with access to the list 

-- 1. Display List information
DECLARE @UserId INT = 2;     -- ID of the logged-in user
DECLARE @ListId INT = 2;     -- ID of the selected list

SELECT 
    l.Id,
    l.ListName,
    l.ListTypeId,
    l.CreatedBy,
    l.CreatedAt,
    l.ListStatus,
    l.Icon,
    l.Color,
    lmp.HighestPermissionCode,
    lmp.GrantedByAccountId
FROM List l
LEFT JOIN ListMemberPermission lmp 
    ON l.Id = lmp.ListId AND lmp.AccountId = @UserId
WHERE l.Id = @ListId
    AND (
        l.CreatedBy = @UserId
        OR lmp.Id IS NOT NULL
    )
    AND l.ListStatus = 'Active';

-- 2. Display all list views of the selected list
DECLARE @ListId INT = 2;     -- ID of the selected list

SELECT
    lv.Id,
    lv.ListId,
    lv.ViewName,
    lv.DisplayOrder
FROM ListView lv
WHERE lv.ListId = @ListId
ORDER BY lv.DisplayOrder;