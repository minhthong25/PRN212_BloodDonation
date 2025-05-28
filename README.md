# PRN212_BloodDonation

Data Base

	-- Step 1: Create Database
	CREATE DATABASE BloodDonationDB;
	GO

	-- Step 2: Use the Database
	USE BloodDonationDB;
	GO

	-- Step 3: Create Tables

	-- Users Table (cốt lõi)
	CREATE TABLE Users (
		UserId INT IDENTITY PRIMARY KEY,
		FullName NVARCHAR(100),
		Email NVARCHAR(100) UNIQUE,
		Phone NVARCHAR(20),
		CreatedAt DATETIME DEFAULT GETDATE()
	);

	-- Blood Groups
	CREATE TABLE BloodGroups (
		BloodGroupId INT IDENTITY PRIMARY KEY,
		GroupName NVARCHAR(10) -- A+, O-, etc.
	);
	
	-- Donor Profiles
	CREATE TABLE Donors (
		DonorId INT PRIMARY KEY, -- same as UserId
		BloodGroupId INT,
		LastDonationDate DATE,
		FOREIGN KEY (DonorId) REFERENCES Users(UserId),
		FOREIGN KEY (BloodGroupId) REFERENCES BloodGroups(BloodGroupId)
	);

	-- Recipient Profiles
	CREATE TABLE Recipients (
		RecipientId INT PRIMARY KEY, -- same as UserId
		MedicalCondition NVARCHAR(255),
		FOREIGN KEY (RecipientId) REFERENCES Users(UserId)
	);

	-- Blood Inventory
	CREATE TABLE BloodInventory (
		InventoryId INT IDENTITY PRIMARY KEY,
		BloodGroupId INT,
		Quantity INT,
		UpdatedAt DATETIME DEFAULT GETDATE(),
		FOREIGN KEY (BloodGroupId) REFERENCES BloodGroups(BloodGroupId)
	);

	-- Blood Requests
	CREATE TABLE BloodRequests (
		RequestId INT IDENTITY PRIMARY KEY,
		RecipientId INT,
		BloodGroupId INT,
		Quantity INT,
		Status NVARCHAR(50), -- Pending, Approved, Insufficient, Rejected
		ResponseMessage NVARCHAR(255),
		RequestDate DATETIME DEFAULT GETDATE(),
		FOREIGN KEY (RecipientId) REFERENCES Recipients(RecipientId),
		FOREIGN KEY (BloodGroupId) REFERENCES BloodGroups(BloodGroupId)
	);

	-- Staff Role (Users with staff permissions)
	CREATE TABLE Staffs (
		StaffId INT PRIMARY KEY, -- same as UserId
		FOREIGN KEY (StaffId) REFERENCES Users(UserId)
	);

	-- Approval Logs
	CREATE TABLE RequestApprovals (
		ApprovalId INT IDENTITY PRIMARY KEY,
		RequestId INT,
		StaffId INT,
		ApprovalStatus NVARCHAR(20), -- Approved, Rejected
		Notes NVARCHAR(255),
		ApprovalDate DATETIME DEFAULT GETDATE(),
		FOREIGN KEY (RequestId) REFERENCES BloodRequests(RequestId),
		FOREIGN KEY (StaffId) REFERENCES Staffs(StaffId)
);


	-- Test Results for Donors
	CREATE TABLE TestResults (
		TestId INT IDENTITY PRIMARY KEY,
		DonorId INT,
		TestDate DATE,
		ResultNote NVARCHAR(255),
		FOREIGN KEY (DonorId) REFERENCES Donors(DonorId)
	);

	-- Donation Locations
	CREATE TABLE Locations (
		LocationId INT IDENTITY PRIMARY KEY,
		Name NVARCHAR(100),
		Address NVARCHAR(255)
	);

	-- Donation Appointments
	CREATE TABLE Appointments (
		AppointmentId INT IDENTITY PRIMARY KEY,
		DonorId INT,
		LocationId INT,
		AppointmentDate DATETIME,
		IsCompleted BIT DEFAULT 0,
		FOREIGN KEY (DonorId) REFERENCES Donors(DonorId),
		FOREIGN KEY (LocationId) REFERENCES Locations(LocationId)
	);

	-- Recovery Reminders
	CREATE TABLE RecoveryReminders (
		ReminderId INT IDENTITY PRIMARY KEY,
		DonorId INT,
		NextEligibleDate DATE,
		Notified BIT DEFAULT 0,
		FOREIGN KEY (DonorId) REFERENCES Donors(DonorId)
	);
	GO
