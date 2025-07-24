use master
IF DB_ID('MsList') IS NULL
BEGIN
    CREATE DATABASE MsList;
END
GO

USE MsList;
GO

-- Drop tables in reverse dependency order
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ShareLinkUserAccess') DROP TABLE ShareLinkUserAccess;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ShareLink') DROP TABLE ShareLink;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListMemberPermission') DROP TABLE ListMemberPermission;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Activity') DROP TABLE Activity;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Trash') DROP TABLE Trash;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListRowComment') DROP TABLE ListRowComment;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'FileAttachment') DROP TABLE FileAttachment;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'FavoriteList') DROP TABLE FavoriteList;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListColumnSettingValue') DROP TABLE ListColumnSettingValue;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListCellValue') DROP TABLE ListCellValue;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListRow') DROP TABLE ListRow;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListColumnSettingObject') DROP TABLE ListColumnSettingObject;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListViewSettingValue') DROP TABLE ListViewSettingValue;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ChangeLog') DROP TABLE ChangeLog; -- Added for potential issue
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Notifications') DROP TABLE Notifications; -- Added for potential issue
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListDynamicColumn') DROP TABLE ListDynamicColumn;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListView') DROP TABLE ListView;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TemplateSampleCell') DROP TABLE TemplateSampleCell;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TemplateSampleRow') DROP TABLE TemplateSampleRow;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TemplateViewSetting') DROP TABLE TemplateViewSetting;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TemplateColumn') DROP TABLE TemplateColumn;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TemplateView') DROP TABLE TemplateView;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'List') DROP TABLE List;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListTemplate') DROP TABLE ListTemplate;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DataTypeSettingKey') DROP TABLE DataTypeSettingKey;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ViewTypeSettingKey') DROP TABLE ViewTypeSettingKey;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'KeySetting') DROP TABLE KeySetting;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ViewSettingKey') DROP TABLE ViewSettingKey;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Permission') DROP TABLE Permission;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'SystemDataType') DROP TABLE SystemDataType;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ViewType') DROP TABLE ViewType;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ListType') DROP TABLE ListType;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'WorkspaceMember') DROP TABLE WorkspaceMember;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TemplateProvider') DROP TABLE TemplateProvider;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Workspace') DROP TABLE Workspace;
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Account') DROP TABLE Account;
GO
-- Create tables in dependency order
CREATE TABLE Account (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	Avatar NVARCHAR(255),
    FirstName NVARCHAR(255),
    LastName NVARCHAR(255),
    DateBirth DATE,
    Email NVARCHAR(255),
	Company NVARCHAR(255),
	AccountStatus NVARCHAR(50) DEFAULT 'Active' ,
    AccountPassword NVARCHAR(255)
);

-------- WORKSPACE --------
CREATE TABLE Workspace (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    WorkspaceName NVARCHAR(255)
);

CREATE TABLE WorkspaceMember (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    WorkspaceId INT FOREIGN KEY REFERENCES Workspace(Id),
    AccountId INT FOREIGN KEY REFERENCES Account(Id),
    JoinedAt DATETIME NOT NULL DEFAULT GETDATE(),
	MemberStatus NVARCHAR(50) DEFAULT 'Active' ,
    UpdateAt DATETIME NOT NULL DEFAULT GETDATE()
);
---------------------------

---------------------- SYSTEM SETTING ---------------------

-- 3 quyền truy cập 
CREATE TABLE Permission (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PermissionName NVARCHAR(100),
    PermissionCode NVARCHAR(50) NOT NULL, --Owner, Contributor, Reader
	PermissionDescription NVARCHAR(255),
	Icon NVARCHAR(255)
);

-- Bảng lưu dạng của view
CREATE TABLE ViewType (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,        -- có 5 loại list, form, kanban, gallery và calendar
    HeaderImage NVARCHAR(255),           -- URL hoặc tên file ảnh
    Icon NVARCHAR(100),                  -- Tên icon hoặc path
    ViewTypeDescription NVARCHAR(500)            -- Mô tả loại view của
);
-- Bảng ListType
CREATE TABLE ListType (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255),  -- Ví dụ: 'List', 'Form', 'Gallery', 'Calendar', 'Board'
    Icon NVARCHAR(100),
    ListTypeDescription NVARCHAR(MAX),
    HeaderImage NVARCHAR(500)
);
-- Bảng ViewSetting 
CREATE TABLE ViewSettingKey (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SettingKey NVARCHAR(100) NOT NULL,   -- VD: 'StartDate', 'EndDate', 'IsPublic'
    ValueType NVARCHAR(50) NOT NULL      -- VD: 'number', 'boolean', 'datetime', 'string'
);
-- bảng chứa viewtype dùng setting nào
CREATE TABLE ViewTypeSettingKey(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ViewTypeId INT FOREIGN KEY REFERENCES ViewType(Id),
    ViewSettingKeyId INT FOREIGN KEY REFERENCES ViewSettingKey(Id)
);

-- các loại data cho cột (loại data mà CellValue được phép trả về)
CREATE TABLE SystemDataType (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Icon NVARCHAR(100),              -- tên icon hoặc đường dẫn
    DataTypeDescription NVARCHAR(500),       -- mô tả
    CoverImg NVARCHAR(255),          -- ảnh bìa (có thể là URL)
    DisplayName NVARCHAR(100) NOT NULL, -- VD: 'Single Text', 'Choice'
    DataTypeValue NVARCHAR(50) NOT NULL  -- VD: 'Text', 'Number', 'Boolean'
);;
-- key setting cho column 
CREATE TABLE KeySetting (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Icon NVARCHAR(100),                  -- icon hoặc tên biểu tượng
    KeyName NVARCHAR(100) NOT NULL,      -- tên setting
    ValueType NVARCHAR(50) NOT NULL,     -- 'text', 'number', 'datetime', etc.
    IsDefaultValue BIT DEFAULT 0,        -- true nếu là giá trị mặc định
	ValueOfDefault NVARCHAR(255),         -- giá trị mặc định nếu có
	IsShareLinkSetting BIT DEFAULT 0    -- true nếu dùng cho share link
);
-- xác định loại col nào thì có setting gì
CREATE TABLE DataTypeSettingKey (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SystemDataTypeId INT FOREIGN KEY REFERENCES SystemDataType(Id),
    KeySettingId INT FOREIGN KEY REFERENCES KeySetting(Id)
);

----------------- TEMPLATE-------------------
CREATE TABLE TemplateProvider (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProviderName NVARCHAR(255)
);
-- Bảng ListTemplate
CREATE TABLE ListTemplate (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255),
    HeaderImage NVARCHAR(500),
    TemplateDescription NVARCHAR(MAX),
    Icon NVARCHAR(100),
    Color NVARCHAR(50) DEFAULT '#28A745',
    Sumary NVARCHAR(MAX),
    Feature NVARCHAR(MAX),
	ListTypeId INT NOT NULL REFERENCES ListType(Id),
    ProviderId INT NOT NULL REFERENCES ListTemplate(Id)
);

-- Bảng TemplateView
CREATE TABLE TemplateView (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ListTemplateId INT NOT NULL REFERENCES ListTemplate(Id),
    ViewTypeId INT NOT NULL,
    ViewName NVARCHAR(255),
    DisplayOrder INT
);

-- Bảng TemplateColumn
CREATE TABLE TemplateColumn (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SystemDataTypeId INT NOT NULL REFERENCES SystemDataType(Id),
    ListTemplateId INT NOT NULL REFERENCES ListTemplate(Id),
    ColumnName NVARCHAR(255),
    ColumnDescription NVARCHAR(MAX),
    DisplayOrder INT,
    IsVisible BIT
);

-- Bảng TemplateViewSetting
CREATE TABLE TemplateViewSetting (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TemplateViewId INT NOT NULL REFERENCES TemplateView(Id),
    ViewTypeSettingId INT NOT NULL REFERENCES ViewTypeSettingKey(Id),
    GroupByColumnId INT NULL REFERENCES TemplateColumn(Id),
    RawValue NVARCHAR(MAX)
);

-- Bảng TemplateSampleRow
CREATE TABLE TemplateSampleRow (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ListTemplateId INT NOT NULL REFERENCES ListTemplate(Id),
	DisplayOrder INT,
);

-- Bảng TemplateSampleCell
CREATE TABLE TemplateSampleCell (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TemplateColumnId INT NOT NULL REFERENCES TemplateColumn(Id),
    TemplateSampleRowId INT NOT NULL REFERENCES TemplateSampleRow(Id),
    CellValue NVARCHAR(MAX)
);

------------------
CREATE TABLE List (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ListTypeId INT NOT NULL REFERENCES ListType(Id),             -- FK đến ListType
	ListTemplateId INT REFERENCES ListTemplate(Id),
    ListName NVARCHAR(100) NOT NULL,
    Icon NVARCHAR(100),
    Color NVARCHAR(50),
    CreatedBy INT NOT NULL,              -- FK đến User hoặc Account
    CreatedAt DATETIME DEFAULT GETDATE(),
    ListStatus NVARCHAR(50) DEFAULT 'Active'                  -- 'Active', 'Archived', etc.
);



--- 1 list có thể tạo nhiều view
--- cần trigger auto tạo 1 view tên "Tất cả khoản mục"
CREATE TABLE ListView (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	ListId INT NOT NULL REFERENCES List(Id),                     -- FK đến bảng List
    CreatedBy INT NOT NULL REFERENCES Account(Id),                  -- FK đến bảng Account/User
    ViewTypeId INT NOT NULL  REFERENCES ViewType(Id),   
	ViewName NVARCHAR(255),
    DisplayOrder INT NOT NULL DEFAULT 0                        -- Số thứ tự hiển thị
);

CREATE TABLE ListDynamicColumn (
    Id INT IDENTITY(1,1) PRIMARY KEY,                              
    ListId INT NOT NULL REFERENCES List(Id),                       
    SystemDataTypeId INT NOT NULL REFERENCES SystemDataType(Id),  -- nếu dạng choice thì key setting có hỏi là multi choice hoặc ko,
    ColumnName NVARCHAR(100) NOT NULL,                             -- Tên của cột hiển thị trên UI
    ColumnDescription NVARCHAR(255),                                     -- Mô tả ngắn về cột
    DisplayOrder INT NOT NULL DEFAULT 0,                                    -- Thứ tự hiển thị trong danh sách
	IsSystemColumn BIT NOT NULL DEFAULT 0,							--  Cột system thì user ko thể thay đổi đc 
    IsVisible BIT NOT NULL DEFAULT 1,                                       -- 1: Hiển thị | 0: Ẩn khỏi view
    CreatedBy INT NOT NULL REFERENCES Account(Id),                -- Ai tạo ra cột này
    CreatedAt DATETIME DEFAULT GETDATE()                           -- Ngày giờ tạo
);

-- lưu các giá trị cho cột có dạng là choice
CREATE TABLE ListColumnSettingObject (
    Id INT PRIMARY KEY IDENTITY,
    ListDynamicColumnId INT FOREIGN KEY REFERENCES ListDynamicColumn(Id),
    DisplayName NVARCHAR(255),    -- Tên hiển thị 
    DisplayColor NVARCHAR(20) NOT NULL DEFAULT '#28A745', -- Màu mặc định nếu không chọn
    DisplayOrder INT NOT NULL DEFAULT 0          -- Thứ tự hiển thị trong dropdown
)

-- nên đổi thêm value
CREATE TABLE ListViewSettingValue (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ListViewId INT FOREIGN KEY REFERENCES ListView(Id),
    ViewTypeSettingKeyId INT FOREIGN KEY REFERENCES ViewTypeSettingKey(Id),
    GroupByColumnId INT FOREIGN KEY REFERENCES ListDynamicColumn(Id),
    RawValue NVARCHAR(255)
);

CREATE TABLE ListRow (
    Id INT IDENTITY(1,1) PRIMARY KEY,               -- Khóa chính tự tăng
    ListId INT NOT NULL REFERENCES List(Id),        -- FK đến danh sách chứa dòng này
    DisplayOrder INT NOT NULL DEFAULT 0,                     -- Thứ tự hiển thị (sắp xếp row)
    ModifiedAt DATETIME,                            -- Thời điểm chỉnh sửa gần nhất
    CreatedBy INT NOT NULL REFERENCES Account(Id),  -- Ai tạo dòng này
    CreatedAt DATETIME DEFAULT GETDATE(),           -- Thời điểm tạo
    ListRowStatus NVARCHAR(50) DEFAULT 'Active'            -- Trạng thái: Active, Archived, Deleted,...
);

-- lưu giá trị của row tại 1 col
CREATE TABLE ListCellValue (
    Id INT IDENTITY(1,1) PRIMARY KEY,                     -- Khóa chính tự tăng
    ListRowId INT NOT NULL REFERENCES ListRow(Id),        -- FK đến dòng chứa giá trị
    ListColumnId INT NOT NULL REFERENCES ListDynamicColumn(Id), -- FK đến cột động tương ứng
    CellValue NVARCHAR(MAX),                                  -- Giá trị nhập (text, number, json,...)
    CreatedBy INT NOT NULL REFERENCES Account(Id),        -- Ai tạo giá trị này
    CreatedAt DATETIME DEFAULT GETDATE()                  -- Thời gian tạo
);

-- giá trị lưu cho setting của col 
CREATE TABLE ListColumnSettingValue (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ColumnId INT FOREIGN KEY REFERENCES ListDynamicColumn(Id),
    DataTypeSettingKey INT FOREIGN KEY REFERENCES DataTypeSettingKey(Id),
    KeyValue NVARCHAR(255),
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE FavoriteList (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ListId INT FOREIGN KEY REFERENCES List(Id),
    FavoriteListOfUser INT FOREIGN KEY REFERENCES Account(Id),
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE FileAttachment (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ListRowId INT FOREIGN KEY REFERENCES ListRow(Id),
    FileAttachmentName NVARCHAR(255),
    FileUrl NVARCHAR(500),
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE ListRowComment (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ListRowId INT FOREIGN KEY REFERENCES ListRow(Id),
    Content NVARCHAR(MAX),
    CreatedBy INT FOREIGN KEY REFERENCES Account(Id),
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateAt DATETIME NOT NULL DEFAULT GETDATE()
);


CREATE TABLE Trash (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	EntityType NVARCHAR(50), -- 'List', 'ListItem', 'FileAttachment'
    EntityId INT, -- ID of the deleted entity
    UserDeleteId INT FOREIGN KEY REFERENCES Account(Id),
	DeletedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Activity (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ListId INT FOREIGN KEY REFERENCES List(Id),
    ListRowId INT FOREIGN KEY REFERENCES ListRow(Id),
    ListCommentId INT FOREIGN KEY REFERENCES ListRowComment(Id),
    ActionType NVARCHAR(100),
    Note NVARCHAR(MAX),
    CreatedBy INT FOREIGN KEY REFERENCES Account(Id),
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE ListMemberPermission (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ListId INT FOREIGN KEY REFERENCES List(Id),
    AccountId INT FOREIGN KEY REFERENCES Account(Id), -- Người được nhận quyền
    HighestPermissionId INT FOREIGN KEY REFERENCES Permission(Id),
	HighestPermissionCode NVARCHAR(50),
    GrantedByAccountId INT FOREIGN KEY REFERENCES Account(Id),
    Note NVARCHAR(MAX),
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE ShareLink (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ListId INT FOREIGN KEY REFERENCES List(Id),
    TargetUrl NVARCHAR(500),
    IsPublic BIT,
    PermissionId INT FOREIGN KEY REFERENCES Permission(Id),
    ExpirationDate DATETIME,
	LinkStatus NVARCHAR(50) DEFAULT 'Active', 
    IsLoginRequired BIT,
    LinkPassword NVARCHAR(255),
    CreatedBy INT FOREIGN KEY REFERENCES Account(Id),
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE ShareLinkUserAccess (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ShareLinkId INT NOT NULL FOREIGN KEY REFERENCES ShareLink(Id),
    AccountId INT NULL FOREIGN KEY REFERENCES Account(Id),
	Email NVARCHAR(255) NOT NULL,
);
GO