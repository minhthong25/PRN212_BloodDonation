CREATE DATABASE BloodDonationDB;
GO

USE BloodDonationDB;
GO

-- Users Table (UserId kiểu NVARCHAR)
CREATE TABLE Users (
    UserId NVARCHAR(20) PRIMARY KEY,  -- VD: 'US0001'
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL, -- mật khẩu đã hash
    Phone NVARCHAR(20),
    Role NVARCHAR(20) NOT NULL, -- 'Admin', 'Staff', 'Donor', 'Recipient', 'User'
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

-- Blood Groups
CREATE TABLE BloodGroups (
    BloodGroupId INT IDENTITY PRIMARY KEY,
    GroupName NVARCHAR(10) NOT NULL -- A+, O-, etc.
);

-- Donor Profiles (DonorId kiểu NVARCHAR, FK tới Users.UserId)
CREATE TABLE Donors (
    DonorId NVARCHAR(20) PRIMARY KEY, 
    BloodGroupId INT NOT NULL,
    LastDonationDate DATE,
    FOREIGN KEY (DonorId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (BloodGroupId) REFERENCES BloodGroups(BloodGroupId)
);

-- Recipient Profiles (RecipientId kiểu NVARCHAR, FK tới Users.UserId)
CREATE TABLE Recipients (
    RecipientId NVARCHAR(20) PRIMARY KEY,
    MedicalCondition NVARCHAR(255),
    FOREIGN KEY (RecipientId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- Blood Inventory
CREATE TABLE BloodInventory (
    InventoryId INT IDENTITY PRIMARY KEY,
    BloodGroupId INT NOT NULL,
    Quantity INT NOT NULL,
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (BloodGroupId) REFERENCES BloodGroups(BloodGroupId)
);

-- Blood Requests
CREATE TABLE BloodRequests (
    RequestId INT IDENTITY PRIMARY KEY,
    RecipientId NVARCHAR(20) NOT NULL,
    BloodGroupId INT NOT NULL,
    Quantity INT NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- Pending, Approved, Insufficient, Rejected
    ResponseMessage NVARCHAR(255),
    RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (RecipientId) REFERENCES Recipients(RecipientId),
    FOREIGN KEY (BloodGroupId) REFERENCES BloodGroups(BloodGroupId)
);

-- Approval Logs
CREATE TABLE RequestApprovals (
    ApprovalId INT IDENTITY PRIMARY KEY,
    RequestId INT NOT NULL,
    ApproverUserId NVARCHAR(20) NOT NULL,
    ApprovalStatus NVARCHAR(20) NOT NULL, -- Approved, Rejected
    Notes NVARCHAR(255),
    ApprovalDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (RequestId) REFERENCES BloodRequests(RequestId) ON DELETE CASCADE,
    FOREIGN KEY (ApproverUserId) REFERENCES Users(UserId)
);

-- Test Results for Donors
CREATE TABLE TestResults (
    TestId INT IDENTITY PRIMARY KEY,
    DonorId NVARCHAR(20) NOT NULL,
    TestDate DATE NOT NULL,
    ResultNote NVARCHAR(255),
    FOREIGN KEY (DonorId) REFERENCES Donors(DonorId) ON DELETE CASCADE
);

-- Donation Locations
CREATE TABLE Locations (
    LocationId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255) NOT NULL
);

-- Donation Appointments
CREATE TABLE Appointments (
    AppointmentId INT IDENTITY PRIMARY KEY,
    DonorId NVARCHAR(20) NOT NULL,
    LocationId INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    IsCompleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (DonorId) REFERENCES Donors(DonorId) ON DELETE CASCADE,
    FOREIGN KEY (LocationId) REFERENCES Locations(LocationId)
);

GO
