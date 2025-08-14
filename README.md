-- CHANGELOG --

### 2025-08-14
- Update:  Unit Test 
+ Test for : Account, Workspace, WorkspaceMember, ListType, List, FavoriteList
---------------
### 2025-08-05
- Updated:
+ Update Entities: Workspace (IsPersonal) 
+ Add Entities: RecentLists
+ Add SP/Trigger: sp_AddToRecentList, trg_CreateWorkspaceForNewAccount

### 2025-08-04
- Updated: 
+ Write UnitTest: Account, Workspace

### 2025-08-03
- Updated: 
+ Add Entities: Format view (FormatRuleType, ListViewFormatRule, RowStyles), ListColumnLookupMap
+ Add data: DataType column - LookUp, Relaion, KeySetting for its

### 2025-07-31
- Updated: 
+ Add data: ListCommentRow
+ Write a query for the UI to retrieve data from List Row, List Comment

### 2025-07-29
- Updated:
+ Add data: ListDynamicCol, ListCellValue
+ Write a query for the UI to retrieve data from List, Dynamic Columns, and System Columns

### 2025-07-28
- Updated: 
// Auto-create a default "All Items" list view (type: List)
// If the list type is Board, Gallery, Calendar, or Form -- also create a view matching the list type
+ Add Store procedure: GetNextDisplayOrderForListView, CreatedList, CreateListViewForList, CreateDefaultListViewsForList, CreateDynamicColumnsForList, UpdateDisplayOrderAfterListViewDeletion
+ Add Trigger too
+ Add data: SystemColumn, SystemColumnSettingValue
+ Add data (NOT DONE): KeySetting, DataTypeSettingKey 
+ Write a query for the UI to retrieve data from List, Dynamic Columns, and System Columns

### 2025-07-27
- Created: SystemColumn, SystemColumnSettingValue 
- Updated: 
+ Added `CanRename` to SystemColumn (allow renaming Title column)

### 2025-07-25
- Created: List, ListDynamicColumn, SystemDataType
- Created: ListTemplate, TemplateView, TemplateColumn,TemplateColumnSettingValue, TemplateSampleRow, TemplateSampleCell, TemplateViewSetting
- Insert: ListTemplate, TemplateProvider, List, ListMemberPermission, FavoriteList
