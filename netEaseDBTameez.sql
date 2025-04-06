---DATABASE CREATION
USE master
IF EXISTS (SELECT * FROM sys.databases WHERE name ='NetEaseDB')
DROP DATABASE NetEaseDB
CREATE DATABASE NetEaseDB

USE NetEaseDB

--TABLE CREATION
CREATE TABLE Venues (
    VenueID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    VenueName VARCHAR(250) NOT NULL,
    Location VARCHAR(500) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl VARCHAR(500) DEFAULT 'https://via.placeholder.com/500x300' NOT NULL
);

-- EVENT TABLE
CREATE TABLE EventInfo (
    EventInfoID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    EventName VARCHAR(250) NOT NULL,
    EventDate DATETIME NOT NULL,
    Description TEXT NOT NULL,
    VenueID INT NOT NULL,  -- Link to Venue
    FOREIGN KEY (VenueID) REFERENCES Venues(VenueID) ON DELETE NO ACTION  -- Prevent deletion of Venue if Event exists
);



-- BOOKING TABLE (Simplified)
CREATE TABLE Bookings (
    BookingID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    EventInfoID INT NOT NULL,  -- Link to Event
    VenueID INT NOT NULL,  -- Link to Venue
    BookingDate DATETIME DEFAULT GETDATE(),  -- Timestamp of booking
    FOREIGN KEY (EventInfoID) REFERENCES EventInfo(EventInfoID) ON DELETE CASCADE,
    FOREIGN KEY (VenueID) REFERENCES Venues(VenueID) ON DELETE CASCADE
);


-- SAMPLE DATA INSERTION

INSERT INTO Venues (VenueName, Location, Capacity, ImageUrl)
VALUES 
('Grand Arena', '123 Main Street, City', 5000, 'https://example.com/grandarena.jpg'),
('Central Park', '56 Street, City', 300, 'https://via.placeholder.com/500x300'),
('Skyline Hall', '789 Broadway, Metropolis', 1200, 'https://example.com/skylinehall.jpg');

-- Sample Event Data
INSERT INTO EventInfo (EventName, EventDate, Description, VenueID)
VALUES 
('Music Fest', '2025-06-15 18:00:00', 'A live music festival featuring top bands.', 1),
('Tech Expo', '2025-09-10 09:00:00', 'A showcase of the latest in technology.', 2),
('Charity Gala', '2025-04-22 19:30:00', 'A gala event to raise funds for charity.', 3);

-- Sample Booking Data
INSERT INTO Bookings (EventInfoID, VenueID)
VALUES
(1, 1),  -- Booking for Music Fest at Grand Arena
(2, 2),  -- Booking for Tech Expo at Central Park
(3, 3);  -- Booking for Charity Gala at Skyline Hall
-- CHECK TABLES

SELECT * FROM Venues;
SELECT * FROM EventInfo;
SELECT * FROM Bookings;

-- CLEANUP TABLES
DROP TABLE IF EXISTS Bookings;
DROP TABLE IF EXISTS EventInfo;
DROP TABLE IF EXISTS Venues;