-- Ensure you're working with the correct database (eventEase)
USE eventEase;

-- Drop existing tables if they exist (optional but ensures a fresh start)
DROP TABLE IF EXISTS Bookings;
DROP TABLE IF EXISTS EventInfo;
DROP TABLE IF EXISTS Venues;
DROP TABLE IF EXISTS EventType;
-- TABLE CREATION for Venues
CREATE TABLE Venues (
    VenueID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    VenueName VARCHAR(250) NOT NULL,
    Location VARCHAR(500) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl VARCHAR(500) DEFAULT 'https://via.placeholder.com/500x300' NOT NULL
);

--QUESTION 3 EventType Table
CREATE TABLE EventType (
	EventTypeID INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
	Name VARCHAR (100) NOT NULL
);

-- EVENT TABLE
CREATE TABLE EventInfo (
    EventInfoID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    EventName VARCHAR(250) NOT NULL,
    EventDate DATETIME NOT NULL,
    Description TEXT NOT NULL,
    VenueID INT NOT NULL,  -- Link to Venue
	EventTypeID INT NOT NULL,
    FOREIGN KEY (VenueID) REFERENCES Venues(VenueID) ON DELETE NO ACTION,  -- Prevent deletion of Venue if Event exists
	FOREIGN KEY (EventTypeID) REFERENCES EventType (EventTypeID) ON DELETE NO ACTION --step 3B
);

-- BOOKING TABLE (Simplified)
CREATE TABLE Bookings (
    BookingID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    EventInfoID INT NOT NULL,  -- Link to Event
    VenueID INT NOT NULL,  -- Link to Venue
    BookingDate DATETIME DEFAULT GETDATE(),  -- Timestamp of booking
    FOREIGN KEY (EventInfoID) REFERENCES EventInfo(EventInfoID) ON DELETE CASCADE,
    FOREIGN KEY (VenueID) REFERENCES Venues(VenueID) ON DELETE CASCADE,
	CONSTRAINT UQ_Venue_EventInfo UNIQUE (VenueID, EventInfoID)
);

--ensure no bookings overlaps for the same venue
CREATE UNIQUE INDEX UQ_Venues_Bookins ON Bookings (VenueID, BookingDate);

INSERT INTO Venues (VenueName, Location, Capacity, ImageUrl)
VALUES 
('Grand Hall Convention Center', '123 Main St, Springfield', 500, 'https://example.com/images/grandhall.jpg'),
('Oceanview Auditorium', '456 Beachside Blvd, Miami', 100, 'https://example.com/images/oceanview.jpg'),
('Mountain Peak Arena', '789 Highland Rd, Denver', 300, 'https://example.com/images/mountainpeak.jpg');


INSERT INTO EventType (Name)
VALUES 
('Conference'),
('wedding'),
('Naming'),
('Birthday'),
('Concert');

INSERT INTO EventInfo (EventName, EventDate, Description, VenueID, EventTypeID) --Q3 c
VALUES 
('Spring Jazz Festival', '2025-06-15 19:00:00', 'A night of smooth jazz with top performers from around the world.', 1, 1),
('InnovateTech 2025', '2025-08-10 09:00:00', 'A full-day conference featuring the latest in tech innovation.', 2, 2),
('Modern Art Showcase', '2025-09-05 17:30:00', 'An exhibition of modern and abstract art by emerging artists.', 3, 3);

INSERT INTO Bookings (EventInfoID, VenueID)
VALUES 
(1, 1),  -- Spring Jazz Festival at Grand Hall
(2, 2),  -- InnovateTech at Oceanview
(3, 3);  -- Art Showcase at Mountain Peak




-- CHECK TABLES to verify everything is inserted correctly
SELECT * FROM Venues;
SELECT * FROM EventInfo;
SELECT * FROM Bookings;
SELECT TOP 1 * FROM EventType;
SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'EventInfo'

