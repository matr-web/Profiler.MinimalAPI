CREATE TABLE Roles 
(
Id INT IDENTITY(1,1) PRIMARY KEY,
RoleName VARCHAR(32) NOT NULL
);

CREATE TABLE Users 
(
Id INT IDENTITY(1,1) PRIMARY KEY,
FirstName VARCHAR(32) NOT NULL,
LastName VARCHAR(32) NOT NULL,
RoleId int NOT NULL,
FOREIGN KEY(RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
);

CREATE TABLE Addresses
(
UserId INT PRIMARY KEY,
Country VARCHAR(32) NOT NULL,
City VARCHAR(32) NOT NULL,
AddressLine1 VARCHAR(32) NOT NULL,
AddressLine2 VARCHAR(32),
ZipCode VARCHAR(32) NOT NULL
FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

INSERT INTO Roles VALUES('User');
INSERT INTO Roles VALUES('Administrator');

INSERT INTO Users (FirstName, LastName, RoleId)
VALUES 
('John', 'Doe', 1),
('Jane', 'Smith', 2),
('Alice', 'Johnson', 1),
('Bob', 'Brown', 2),
('Charlie', 'Davis', 1),
('Emily', 'Wilson', 2),
('Frank', 'Miller', 1),
('Grace', 'Taylor', 2),
('Henry', 'Anderson', 1);

INSERT INTO Addresses (UserId, Country, City, AddressLine1, AddressLine2, ZipCode)
VALUES 
(1, 'United States', 'New York', '123 Main St', NULL, '10001'),
(2, 'Canada', 'Toronto', '456 Queen St', 'Apt 12', 'M5H 2N2'),
(3, 'Germany', 'Berlin', '78 Alexanderplatz', NULL, '10178'),
(4, 'Japan', 'Tokyo', '1-2-3 Shibuya', 'Building A', '150-0002'),
(5, 'Australia', 'Sydney', '25 George St', NULL, '2000'),
(6, 'United Kingdom', 'London', '10 Downing St', NULL, 'SW1A 2AA'),
(7, 'France', 'Paris', '5 Avenue Anatole', 'Floor 2', '75007'),
(8, 'Italy', 'Rome', 'Via del Corso', NULL, '00186'),
(9, 'Poland', 'Warsaw', 'Ulica Jana Kiliñskiego 15', NULL, '01-376');


