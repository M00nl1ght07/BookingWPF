-- Users
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    PhoneNumber NVARCHAR(20),
    IsAdmin BIT DEFAULT 0
)

-- Hotels
CREATE TABLE Hotels (
    HotelID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Address NVARCHAR(255),
    City NVARCHAR(100),
    Rating DECIMAL(3,2),
    ImageUrl NVARCHAR(255)
)

-- Rooms
CREATE TABLE Rooms (
    RoomID INT PRIMARY KEY IDENTITY(1,1),
    HotelID INT FOREIGN KEY REFERENCES Hotels(HotelID),
    RoomNumber NVARCHAR(20),
    RoomType NVARCHAR(50),
    Price DECIMAL(10,2),
    Capacity INT,
    Description NVARCHAR(MAX),
    ImageUrl NVARCHAR(255)
)

-- Bookings
CREATE TABLE Bookings (
    BookingID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    RoomID INT FOREIGN KEY REFERENCES Rooms(RoomID),
    CheckInDate DATE,
    CheckOutDate DATE,
    TotalPrice DECIMAL(10,2),
    Status NVARCHAR(20)
)