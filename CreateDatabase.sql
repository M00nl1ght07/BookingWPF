-- Создание базы данных
CREATE DATABASE HotelBookingDB;
GO

USE HotelBookingDB;
GO

-- Пользователи системы
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    IsAdmin BIT DEFAULT 0,
    RegistrationDate DATETIME DEFAULT GETDATE(),
    BonusPoints INT DEFAULT 0
);

-- Отели
CREATE TABLE Hotels (
    HotelID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Description NVARCHAR(MAX),
    IsPopular BIT DEFAULT 0,
    BasePrice DECIMAL(10,2) NOT NULL,
    PhotoPath NVARCHAR(200)
);

-- Удобства отелей
CREATE TABLE HotelAmenities (
    HotelID INT,
    AmenityName NVARCHAR(50), -- WiFi, Парковка, Бассейн и т.д.
    PRIMARY KEY (HotelID, AmenityName),
    FOREIGN KEY (HotelID) REFERENCES Hotels(HotelID) ON DELETE CASCADE
);

-- Номера в отелях
CREATE TABLE Rooms (
    RoomID INT IDENTITY(1,1) PRIMARY KEY,
    HotelID INT,
    RoomType NVARCHAR(50) NOT NULL,
    Area DECIMAL(5,2) NOT NULL,
    Capacity INT NOT NULL,
    PricePerNight DECIMAL(10,2) NOT NULL,
    Description NVARCHAR(MAX),
    PhotoPath NVARCHAR(200),
    FOREIGN KEY (HotelID) REFERENCES Hotels(HotelID) ON DELETE CASCADE
);

-- Удобства номеров
CREATE TABLE RoomAmenities (
    RoomID INT,
    AmenityName NVARCHAR(50), -- Кондиционер, Мини-бар, Сейф и т.д.
    PRIMARY KEY (RoomID, AmenityName),
    FOREIGN KEY (RoomID) REFERENCES Rooms(RoomID) ON DELETE CASCADE
);

-- Бронирования
CREATE TABLE Bookings (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,
    RoomID INT,
    CheckInDate DATE NOT NULL,
    CheckOutDate DATE NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(20) CHECK (Status IN ('Активно', 'Отменено', 'Завершено')),
    BookingDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    FOREIGN KEY (RoomID) REFERENCES Rooms(RoomID)
);

-- Создание индексов для оптимизации запросов
CREATE INDEX IX_Hotels_IsPopular ON Hotels(IsPopular);
CREATE INDEX IX_Bookings_Status ON Bookings(Status);
CREATE INDEX IX_Bookings_UserID ON Bookings(UserID);
CREATE INDEX IX_Rooms_HotelID ON Rooms(HotelID); 