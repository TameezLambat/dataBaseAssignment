-- Ensure you're working with the correct database (eventEase)
USE eventEase;

-- Drop existing tables if they exist (optional but ensures a fresh start)
DROP TABLE IF EXISTS Bookings;
DROP TABLE IF EXISTS EventInfo;
DROP TABLE IF EXISTS Venues;

-- TABLE CREATION for Venues
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
    FOREIGN KEY (VenueID) REFERENCES Venues(VenueID) ON DELETE CASCADE,
	CONSTRAINT UQ_Venue_EventInfo UNIQUE (VenueID, EventInfoID)
);

--ensure no bookings overlaps for the same venue
CREATE UNIQUE INDEX UQ_Venues_Bookins ON Bookings (VenueID, BookingDate);


-- CHECK TABLES to verify everything is inserted correctly
SELECT * FROM Venues;
SELECT * FROM EventInfo;
SELECT * FROM Bookings;


