use [master];
DROP DATABASE IF EXISTS FurnitureShop;
GO

CREATE DATABASE [FurnitureShop]
COLLATE Cyrillic_General_100_CI_AI;;
GO

use [FurnitureShop];

CREATE TABLE Category (
	ID INT IDENTITY,
	Name NVARCHAR(60) NOT NULL

	CONSTRAINT [PK_Category] PRIMARY KEY (ID),
	CONSTRAINT [UQ_Category_Name] UNIQUE (Name)
)
GO

CREATE TABLE Manufacturer (
	ID INT IDENTITY,
	Name NVARCHAR(60) NOT NULL,
	Description NVARCHAR(255),
	Contacts NVARCHAR(255) DEFAULT ('No information about contacts'),
	Image NVARCHAR(50)

	CONSTRAINT [PK_Manufacturer] PRIMARY KEY (ID),
	CONSTRAINT [UQ_Manufacturer_Name] UNIQUE (Name)
)
GO

CREATE TABLE Collection (
	ID INT IDENTITY,
	Name NVARCHAR(60) NOT NULL,
	Description NVARCHAR (255)

	CONSTRAINT [PK_Collection] PRIMARY KEY (ID),
	CONSTRAINT [UQ_Collection_Name] UNIQUE (Name)
)
GO

CREATE TABLE Furniture (
	VendorCode NVARCHAR(16),
	Name NVARCHAR(60) NOT NULL,
	Price MONEY NOT NULL,
	PriceDiscount MONEY,
	Quantity INT NOT NULL,
	CategoryID INT,
	ManufacturerID INT NOT NULL,
	CollectionID INT NOT NULL,
	Rate INT NOT NULL DEFAULT (0),
	Image NVARCHAR(40)

	CONSTRAINT [PK_Furniture] PRIMARY KEY (VendorCode),
	CONSTRAINT [UQ_Furniture_Name] UNIQUE (Name),
	CONSTRAINT [CHK_Furniture_Price] CHECK (Price > 0),
	CONSTRAINT [CHK_Furniture_PriceDiscount] CHECK (PriceDiscount > 0),
	CONSTRAINT [CHK_Furniture_Quantity] CHECK (Quantity >= 0),
	CONSTRAINT [FK_Furniture_Category] FOREIGN KEY (CategoryID) REFERENCES Category(ID) 
		ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_Furniture_Manufacturer] FOREIGN KEY (ManufacturerID) REFERENCES Manufacturer(ID) 
		ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_Furniture_Collection] FOREIGN KEY (CollectionID) REFERENCES Collection(ID) 
		ON DELETE CASCADE ON UPDATE CASCADE
)
GO

CREATE TABLE Size (
	VendorCode NVARCHAR(16),
	Type NVARCHAR(20),
	Width INT NOT NULL,
	Height INT NOT NULL,
	Depth INT NOT NULL

	CONSTRAINT [PK_Size] PRIMARY KEY (VendorCode, Type),
	CONSTRAINT [FK_Size_VendorCode] FOREIGN KEY (VendorCode) REFERENCES Furniture(VendorCode) 
		ON DELETE CASCADE ON UPDATE CASCADE
)
GO

CREATE TABLE Color (
	ID INT IDENTITY,
	Name NVARCHAR(30) NOT NULL,
	RGB NVARCHAR(11) NOT NULL

	CONSTRAINT [PK_Color] PRIMARY KEY (ID),
	CONSTRAINT [UQ_Color_Name] UNIQUE (Name),
	CONSTRAINT [UQ_Color_RGB] UNIQUE (RGB)
)
GO

CREATE TABLE FurnitureColor (
	VendorCode NVARCHAR(16),
	ColorID INT

	CONSTRAINT [PK_FurnitureColor] PRIMARY KEY (VendorCode, ColorID),
	CONSTRAINT [FK_FurnitureColor_VendorCode] FOREIGN KEY (VendorCode) REFERENCES Furniture(VendorCode)
		ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_FurnitureColor_ColorID] FOREIGN KEY (ColorID) REFERENCES Color(ID)
		ON DELETE CASCADE ON UPDATE CASCADE
)
GO

CREATE TABLE Material (
	ID INT IDENTITY,
	Name NVARCHAR(80) NOT NULL,
	Image NVARCHAR(40)

	CONSTRAINT [PK_Material] PRIMARY KEY (ID),
	CONSTRAINT [UQ_Material_Name] UNIQUE (Name)
)
GO

CREATE TABLE FurnitureMaterial (
	VendorCode NVARCHAR(16),
	MaterialID INT

	CONSTRAINT [PK_FurnitureMaterial] PRIMARY KEY (VendorCode, MaterialID),
	CONSTRAINT [FK_FurnitureMaterial_VendorCode] FOREIGN KEY (VendorCode) REFERENCES Furniture(VendorCode)
		ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_FurnitureMaterial_MaterialID] FOREIGN KEY (MaterialID) REFERENCES Material(ID)
		ON DELETE CASCADE ON UPDATE CASCADE
)
GO

CREATE TABLE GrantType (
	ID INT IDENTITY,
	Name NVARCHAR(30) NOT NULL
	
	CONSTRAINT [PK_Grant] PRIMARY KEY (ID),
	CONSTRAINT [UQ_Grant_Name] UNIQUE (Name)
)
GO

CREATE TABLE Seller (
	ID INT IDENTITY,
	Email NVARCHAR(50) NOT NULL,
	Password NVARCHAR(255) NOT NULL,
	GrantID INT NOT NULL

	CONSTRAINT [PK_Seller] PRIMARY KEY (ID),
	CONSTRAINT [UQ_Seller_Email] UNIQUE (Email),
	CONSTRAINT [FK_Seller_GrantID] FOREIGN KEY (GrantID) REFERENCES GrantType(ID)
		ON DELETE CASCADE ON UPDATE CASCADE
)
GO

CREATE TABLE Buyer (
	ID INT IDENTITY,
	Name NVARCHAR(255) NOT NULL,
	Phone NVARCHAR(20),
	Email NVARCHAR(50) NOT NULL,
	Password NVARCHAR(255) NOT NULL

	CONSTRAINT [PK_Buyer] PRIMARY KEY (ID),
	CONSTRAINT [UQ_Buyer_Email] UNIQUE (Email)
)
GO

CREATE TABLE OrderHeader (
	ID INT IDENTITY,
	Date SMALLDATETIME,
	BuyerID INT NOT NULL

	CONSTRAINT [PK_OrderHeader] PRIMARY KEY (ID),
	CONSTRAINT [FK_OrderHeader_BuyerID] FOREIGN KEY (BuyerID) REFERENCES Buyer(ID)
		ON DELETE CASCADE ON UPDATE CASCADE
)
GO

CREATE TABLE OrderDetail (
	OrderHeaderID INT,
	VendorCode NVARCHAR(16),
	Quantity INT NOT NULL,
	Price MONEY DEFAULT(0)

	CONSTRAINT [PK_OrderDetail] PRIMARY KEY (OrderHeaderID, VendorCode),
	CONSTRAINT [FK_OrderDetail_OrderHeaderID] FOREIGN KEY (OrderHeaderID) REFERENCES OrderHeader(ID)
		ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_OrderDetail_VendorCode] FOREIGN KEY (VendorCode) REFERENCES Furniture(VendorCode)
		ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [CHK_OrderDetail_Quantity] CHECK (Quantity > 0),
	CONSTRAINT [CHK_OrderDetail_Price] CHECK (Price >= 0)
)
GO



------------------------------------------------------------------------------------------
------------------------------------------ Seed ------------------------------------------
------------------------------------------------------------------------------------------



INSERT INTO dbo.Category VALUES ('Стул')
INSERT INTO dbo.Category VALUES ('Стол')
INSERT INTO dbo.Category VALUES ('Диван')
INSERT INTO dbo.Category VALUES ('Кровать')
INSERT INTO dbo.Category VALUES ('Шкаф')
INSERT INTO dbo.Category VALUES ('Полка')
GO

INSERT INTO dbo.Material VALUES ('Светлое дерево','light_wood.png')
INSERT INTO dbo.Material VALUES ('Темное дерево','dark_wood.png')
INSERT INTO dbo.Material VALUES ('Красное дерево','red_wood.png')
INSERT INTO dbo.Material VALUES ('Сталь','steel.png')
INSERT INTO dbo.Material VALUES ('Пластик','plastic.png')
INSERT INTO dbo.Material VALUES ('Мрамор','marble.png')
GO

INSERT INTO dbo.Manufacturer VALUES ('IKEA', 'Основанная в Швеции нидерландская производственно-торговая группа компаний, владелец одной из крупнейших в мире торговых сетей по продаже мебели и товаров для дома.', 'Телефон: +38-569-598-56\nАдрес: Лейден, Нидерланды','ikea.png')
INSERT INTO dbo.Manufacturer VALUES ('JYSK', 'Датская транснациональная корпорация. Штаб-квартира — в городе Орхус. Специализируется на продаже матрасов, товаров для дома, мебели и фурнитуры. Основанная в 1979 году, по состоянию на начало 2017 оперирует 2400 магазинами в 48 странах мира.', 'Телефон: +58-264-332-86\nАдрес: Brabrand, Дания', 'jysk.png')
INSERT INTO dbo.Manufacturer VALUES ('ARQDEQ', 'Киевская мебельная компания, известная своими креативными лимитированными коллекциями в мире украинского дизайна. Все изделия производятся мастерами вручную. Мебель отлично впишется в интерьеры в скандинавском стиле или лофт.', 'Телефон: +38-446-115-34\nАдрес: Украина, г. Киев', 'arqdeq.png')
INSERT INTO dbo.Manufacturer VALUES ('Oakland', 'Столярная мастерская, качественно изготовляющая деревянную мебель, дизайн которой неразрывно связан с современными тенденциями и в то же время с требованиями заказчиков. Помимо мебели, здесь также производятся стеновые панели, перила и перегородки.', 'Телефон: +38-111-222-48\nАдрес: Украина, Харьковская обл., пос. Подворки', 'oakland.png')
GO

INSERT INTO dbo.Collection VALUES ('Современный лофт', 'Мебель выполнена в современном стиле лофт с примеся скандинавских мотивов.');
INSERT INTO dbo.Collection VALUES ('Минимализм', 'Минималистичный дизайн предметов мебели поможет разгрузить пространство и эффективно разместить все свои вещи.');
INSERT INTO dbo.Collection VALUES ('Цена/Качество', 'Стильная, качественная и бюджетная мебель для молодых семей или студентов.');
GO

INSERT INTO Color VALUES ('Кофейный','214,210,196');
INSERT INTO Color VALUES ('Тёмно-коричневый','49,38,29');
INSERT INTO Color VALUES ('Лавандовый','220,219,244');
INSERT INTO Color VALUES ('Коралловый','250,114,104');
INSERT INTO Color VALUES ('Песочный','254,248,232');
GO

INSERT INTO Furniture VALUES ('11111111','Скандинавский стол', 2500, NULL, 5, 2, 3, 1, 0, 'arqdeq/scandinavskij_stol.png')
INSERT INTO Furniture VALUES ('11111112','Витрина BRW Gent 98',4500, 4300, 2, 5, 1, 2, 0, 'ikea/vitrina_brw_gent_98.png')
GO

INSERT INTO FurnitureMaterial VALUES ('11111111', 1)
INSERT INTO FurnitureMaterial VALUES ('11111111', 4)
INSERT INTO FurnitureMaterial VALUES ('11111112', 2)
INSERT INTO FurnitureMaterial VALUES ('11111112', 3)
GO

INSERT INTO FurnitureColor VALUES ('11111111', 2)
INSERT INTO FurnitureColor VALUES ('11111111', 3)
INSERT INTO FurnitureColor VALUES ('11111112', 1)
INSERT INTO FurnitureColor VALUES ('11111112', 2)
GO

INSERT INTO Size VALUES ('11111111', '', 150,230, 60)
INSERT INTO Size VALUES ('11111112', 'С закрытыми дверцами', 100,230, 60)
INSERT INTO Size VALUES ('11111112', 'С открытыми дверцами', 100,230, 120)
GO

INSERT INTO Buyer VALUES ('Цой Игорь', '+38065891654', 'ihor.tsoi@nure.ua', 'pass')
GO

INSERT INTO OrderHeader VALUES (CAST('2007-05-08 12:35:29' AS smalldatetime), 1)
INSERT INTO OrderHeader VALUES (NULL, 1)
GO

INSERT INTO OrderDetail VALUES (1, '11111111', 2, 2500)
INSERT INTO OrderDetail VALUES (1, '11111112', 1, 4700)
INSERT INTO OrderDetail (OrderHeaderID, VendorCode, Quantity) VALUES (2, '11111112', 1)
GO

INSERT INTO GrantType VALUES ('Директор')
INSERT INTO GrantType VALUES ('Продавец')
GO

INSERT INTO Seller VALUES ('some.mail@gmail.com', 'nopass', 2)
GO



/*
SELECT * FROM Category;
SELECT * FROM Material;
SELECT * FROM Manufacturer;
SELECT * FROM Collection;
SELECT * FROM Color;
SELECT * FROM Furniture;
SELECT * FROM FurnitureMaterial;
SELECT * FROM FurnitureColor;
SELECT * FROM Size;
SELECT * FROM OrderHeader;
SELECT * FROM OrderDetail;
SELECT * FROM Buyer;
SELECT * FROM GrantType;
SELECT * FROM Seller;
*/

/*
DELETE FROM Category;
DELETE FROM Material;
DELETE FROM Manufacturer;
DELETE FROM Collection;
DELETE FROM Color;
DELETE FROM Furniture;
DELETE FROM FurnitureMaterial;
DELETE FROM FurnitureColor;
DELETE FROM Size;
DELETE FROM OrderHeader;
DELETE FROM OrderDetail;
DELETE FROM Buyer;
DELETE FROM GrantType;
DELETE FROM Seller;
*/


------------------------------------------------------------------------------------------
------------------------------------------ Views -----------------------------------------
------------------------------------------------------------------------------------------




-- 1) GET ALL INFO ABOUT THE FURNITURE
--  
CREATE VIEW FurnitureAll AS (
	SELECT Furniture.*, 
		FurnitureColor.ColorID, FurnitureMaterial.MaterialID, 
		Size.Type, Size.Width, Size.Depth, Size.Height
	FROM Furniture
	LEFT JOIN FurnitureColor 
		ON Furniture.VendorCode = FurnitureColor.VendorCode
	LEFT JOIN FurnitureMaterial
		ON Furniture.VendorCode = FurnitureMaterial.VendorCode
	LEFT JOIN Size
		ON Furniture.VendorCode = Size.VendorCode
)
GO




-- 2) GET ALL ORDER INFO FOR EVERY USER
-- SELECT * FROM OrdersAll
CREATE VIEW OrdersAll AS (
	SELECT Buyer.ID as BuyerID, Buyer.Name,
		OrderHeader.ID as OrderHeaderID, OrderHeader.Date,
		OrderDetail.VendorCode,Furniture.Name AS FurnitureName, OrderDetail.Quantity, OrderDetail.Price
	FROM Buyer
	INNER JOIN OrderHeader
		ON Buyer.ID = OrderHeader.BuyerID
	LEFT JOIN OrderDetail
		ON OrderHeader.ID = OrderDetail.OrderHeaderID
	LEFT JOIN Furniture
		ON OrderDetail.VendorCode = Furniture.VendorCode
)
GO


-- 3) GET ALL FURNITURE KEYWORDS FOR SEARCHING ALGO
-- SELECT * FROM SearchFurniture
CREATE VIEW SearchFurniture AS (
	SELECT Furniture.VendorCode, Furniture.Name as Name, Category.Name as Category, Manufacturer.Name as Manufacturer, Collection.Name as Collection
	FROM Furniture
	INNER JOIN Category
		ON Category.ID = Furniture.CategoryID
	INNER JOIN Manufacturer
		ON Manufacturer.ID = Furniture.ManufacturerID
	INNER JOIN Collection
		ON Collection.ID = Furniture.CollectionID
)
GO





------------------------------------------------------------------------------------------
------------------------------------------ UDF -------------------------------------------
------------------------------------------------------------------------------------------

-- ALL ORDER_HEADERS BY @user_id
/*
 1. return filtered orderHeaders by userId
*/
-- SELECT * FROM get_all_ohs(2)
CREATE FUNCTION get_all_ohs (@userID INT)
RETURNS TABLE
AS
RETURN (
	SELECT * 
	FROM OrderHeader
	WHERE OrderHeader.BuyerID = @userID
)
GO




-- PENDING ORDER_HEADER WITH ORDER_DETAILS BY @user_id
/*
 1. get_all_ohs (userId) 
 2. filter by max date
 3. inner join orderDetails
*/
-- SELECT * FROM get_pending_oh_od(2)
CREATE FUNCTION get_pending_oh_od(@userId int)
RETURNS TABLE 
AS
RETURN
	SELECT ID, VendorCode, Quantity
	FROM get_all_ohs(@userId) oh
	LEFT JOIN OrderDetail
		ON oh.ID = OrderDetail.OrderHeaderID
	WHERE Date IS NULL
GO





-- EVALUATE DEGREE OF COINCIDENCE (@name, @category, @manufacturer, @collection, @query) [FOR SEARCH ALGO]
/*
 1. compare the whole @query to keywords (+2x)
 2. loop:
     for each @word_query count matches through keywords (+1x)
*/
-- SELECT dbo.evaluate_degree_of_c('Скандинавский стол', 'Стол', 'IKEA', 'Весенний бриз', 'IKEA')
CREATE FUNCTION dbo.evaluate_degree_of_c(@name NVARCHAR(60), @category NVARCHAR(60), @manufacturer NVARCHAR(60), @collection NVARCHAR(60), @query NVARCHAR(60))
RETURNS INT
AS 
BEGIN
	DECLARE @degree INT = 0;
	-- 1
	SET @degree += IIF (@name LIKE @query, 2, 0) + 
		IIF (@category LIKE @query, 2, 0) +
		IIF (@manufacturer LIKE @query, 2, 0) +
		IIF (@collection LIKE @query, 2, 0);
	--
	--
	DECLARE @query_word_degree TABLE(
		word NVARCHAR(60) NOT NULL,
		degree INT NOT NULL
	)
	-- init
	INSERT INTO @query_word_degree (word, degree)
	SELECT value, 0
	FROM STRING_SPLIT(@query,' ')
	WHERE value <> ''
	-- 2
	UPDATE @query_word_degree
	SET degree += (IIF (@name LIKE ('%'+word+'%'), 1, 0) + 
		IIF (@category LIKE ('%'+word+'%'), 1, 0) +
		IIF (@manufacturer LIKE ('%'+word+'%'), 1, 0) +
		IIF (@collection LIKE ('%'+word+'%'), 1, 0))
	--
	SET @degree += COALESCE((SELECT SUM(degree) FROM @query_word_degree),0)
	RETURN @degree;
END;
GO






-- GET MATHCES BY @query [FOR SEARCH ALGO]
/*
 1. use [SearchFurniture] view;
 2. evaluate degree_of_c for every record;
*/
-- SELECT * FROM get_matches('ikea')
CREATE FUNCTION dbo.get_matches(@query NVARCHAR(40))
RETURNS TABLE
AS 
RETURN (
	SELECT sf.VendorCode, [dbo].[evaluate_degree_of_c](sf.Name, sf.Category, sf.Manufacturer, sf.Collection, @query) as Degree
	FROM [SearchFurniture] as sf
)
GO





-- SMART SEARCH BY @query [SEARCH ALGO]
/*
 1. get matches using [get_matches(@query)]
 2. inner join with [FurnitureAll]
*/
-- SELECT * FROM dbo.smart_search('витрина стол BRW GENT 98') ORDER BY Degree DESC
CREATE FUNCTION smart_search(@query NVARCHAR(40))
RETURNS TABLE
AS
RETURN (
	SELECT FurnitureAll.*, m.Degree
	FROM [dbo].[get_matches](@query) m
	INNER JOIN FurnitureAll
		ON m.VendorCode = FurnitureAll.VendorCode
	WHERE m.Degree > 0
)
GO




------------------------------------------------------------------------------------------
------------------------------------ Stored procedures -----------------------------------
------------------------------------------------------------------------------------------




-- USER REGISTRATION (@name, @phone, @email, @password) 
--											: @success(return code: 0 --> ok, 1 --> fail)
/*
 1. create transaction
 2. insert into [Buyer], get [ID] (select @ID = SCOPE_IDENTITY())
 2. initialize the order header
*/
/*
	DECLARE @res INT
	EXEC @res = register_buyer 'name', '1231231', 'iqq@wqwqwk.lq', ''
	PRINT @res
*/
CREATE PROCEDURE register_buyer
	@name NVARCHAR(255), @phone NVARCHAR(20) = NULL,
	@email NVARCHAR(50), @password NVARCHAR(255)
AS
	IF ((@name IS NULL) OR (@email IS NULL) OR (@password IS NULL) 
		OR (LEN(@name) < 1) OR (LEN(@password) < 4)
		OR (@email NOT LIKE '_%@__%.__%')
		OR (SELECT COUNT(ID) FROM Buyer WHERE Email LIKE @email) <> 0)
		-- the input data is invalid
		RETURN 1
	--
	SET XACT_ABORT ON
	BEGIN TRANSACTION
		INSERT INTO Buyer
			VALUES (@name, @phone, @email, @password)
		--
		INSERT INTO OrderHeader
			VALUES (NULL, SCOPE_IDENTITY())
	COMMIT TRANSACTION
	SET XACT_ABORT OFF
	RETURN 0;
GO



-- ADD TO CART (@id, @vendor_code) : @success (0 --> ok, 1--> no such vendor_code or user)
/*
 1. validate input (check if exists)
 2. either update or insert order detail
*/
/*
 DECLARE @res INT
 EXEC @res = add_to_cart 1, NULL
 PRINT @res
*/
CREATE PROCEDURE add_to_cart
	@id INT, @vendor_code NVARCHAR(16)
AS
	IF (EXISTS(SELECT 1 FROM Furniture WHERE VendorCode LIKE @vendor_code) AND
			EXISTS(SELECT 1 FROM Buyer WHERE ID = @id))
	BEGIN
		--
		DECLARE @oh_id INT;
		SELECT TOP 1 @oh_id =  ID FROM get_all_ohs(@id) WHERE Date IS NULL
		--
		IF (EXISTS(SELECT 1 FROM get_pending_oh_od(@id) WHERE VendorCode LIKE @vendor_code))
		BEGIN
			--
			UPDATE OrdersAll
			SET Quantity += 1
			WHERE BuyerID = @id AND OrderHeaderID = @oh_id AND VendorCode = @vendor_code
		END;
		ELSE
		BEGIN
			--
			INSERT INTO OrderDetail
			VALUES(@oh_id, @vendor_code, 1, 0)
		END;
		--
		RETURN 0;
		--
	END;
	ELSE
		RETURN 1;
GO



-- CONFIRM PURCHASE (@id) : result set, @return_code (0 --> ok, 1 --> no such user or empty OH, 2 --> not enough in the stock (see the result set))
/*
	1. check if user exists and order details exist
	2. compare quantity in ods to quantity in stock
	3. either list those goods which aren't available
	4. or update qty in stock
	5. fix prices in oh
	6. refresh date in pending oh and create new oh for user
*/
/*
 DECLARE @res INT
 EXEC confirm_purchase 1,@res OUTPUT
 PRINT @res
*/
CREATE PROCEDURE confirm_purchase
	@id INT,
	@return_code INT OUTPUT
AS
	DECLARE @oh_id INT;
	SELECT TOP 1 @oh_id =  ID FROM get_all_ohs(@id) WHERE Date IS NULL
	--
	IF (EXISTS(SELECT 1 FROM Buyer WHERE ID = @id)) AND 
		(EXISTS(SELECT 1 FROM OrderDetail WHERE OrderHeaderID = @oh_id))
	BEGIN
		IF (0 = ANY(
			SELECT IIF(oh_od.Quantity <= Furniture.Quantity, 1,0) available
			FROM get_pending_oh_od(@id) oh_od
			INNER JOIN Furniture
				ON oh_od.VendorCode = Furniture.VendorCode))
		BEGIN
			-- some of goods aren't available in stock
			SELECT oh_od.VendorCode
			FROM get_pending_oh_od(@id) oh_od
			INNER JOIN Furniture
				ON oh_od.VendorCode = Furniture.VendorCode
			WHERE oh_od.Quantity > Furniture.Quantity
			--
			SET @return_code = 2
			RETURN;
		END
		ELSE 
		BEGIN
			-- all goods are available
			-- 1. fix prices
			UPDATE OrdersAll
			SET Price = COALESCE(Furniture.PriceDiscount, Furniture.Price)
			FROM OrdersAll
			INNER JOIN Furniture
				ON OrdersAll.VendorCode = Furniture.VendorCode
			WHERE OrderHeaderID = @oh_id
			-- 2. update qty in stock
			UPDATE Furniture
			SET Quantity -= oh_od.Quantity
			FROM get_pending_oh_od(@id) oh_od
			INNER JOIN Furniture
				ON oh_od.VendorCode = Furniture.VendorCode
			-- 3. update current OH date
			UPDATE OrderHeader
			SET Date = GETDATE()
			WHERE ID = @oh_id
			-- 4. create new oh for user
			INSERT INTO OrderHeader
			VALUES(NULL, @id)
			--
			SET @return_code = 0
			RETURN;
		END;
	END;
	ELSE
		SET @return_code = 1
		RETURN;