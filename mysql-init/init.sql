-- Create the database
CREATE DATABASE IF NOT EXISTS taskmanagementdb;
USE taskmanagementdb;

-- Create User table
CREATE TABLE IF NOT EXISTS Users (
    Id CHAR(36) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    IsManager BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL
);

-- Create Project table
CREATE TABLE IF NOT EXISTS Projects (
    Id CHAR(36) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT NULL,
    OwnerId CHAR(36) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (OwnerId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Create ProjectMember table (junction table for many-to-many relationship between Users and Projects)
CREATE TABLE IF NOT EXISTS ProjectMembers (
    Id CHAR(36) PRIMARY KEY,
    ProjectId CHAR(36) NOT NULL,
    UserId CHAR(36) NOT NULL,
    JoinedAt DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    UNIQUE KEY (ProjectId, UserId) -- Ensure a user can't be added to the same project twice
);

-- Create WorkItem table
CREATE TABLE IF NOT EXISTS WorkItems (
    Id CHAR(36) PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Description TEXT NULL,
    DueDate DATETIME NOT NULL,
    Status ENUM('Pending', 'InProgress', 'Completed') NOT NULL DEFAULT 'Pending',
    Priority ENUM('Low', 'Medium', 'High') NOT NULL,
    ProjectId CHAR(36) NOT NULL,
    CreatedBy CHAR(36) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Create WorkItemComment table
CREATE TABLE IF NOT EXISTS WorkItemComments (
    Id CHAR(36) PRIMARY KEY,
    Content TEXT NOT NULL,
    AuthorId CHAR(36) NOT NULL,
    WorkItemId CHAR(36) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (AuthorId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (WorkItemId) REFERENCES WorkItems(Id) ON DELETE CASCADE
);

-- Create WorkItemHistory table
CREATE TABLE IF NOT EXISTS WorkItemHistories (
    Id CHAR(36) PRIMARY KEY,
    Action TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    ModifiedBy CHAR(36) NOT NULL,
    WorkItemId CHAR(36) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (ModifiedBy) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (WorkItemId) REFERENCES WorkItems(Id) ON DELETE CASCADE
);

-- Create indexes for better performance
CREATE INDEX idx_workitems_project ON WorkItems(ProjectId);
CREATE INDEX idx_workitems_status ON WorkItems(Status);
CREATE INDEX idx_workitemcomments_workitem ON WorkItemComments(WorkItemId);
CREATE INDEX idx_WorkItemHistories_workitem ON WorkItemHistories(WorkItemId);
CREATE INDEX idx_projectmembers_user ON ProjectMembers(UserId);
CREATE INDEX idx_projectmembers_project ON ProjectMembers(ProjectId);


-- Insert administrator user with fixed GUID
INSERT INTO Users (Id, Name, Email, IsManager, CreatedAt)
VALUES (
    '11111111-1111-1111-1111-111111111111', 
    'Admin User', 
    'admin@test.com', 
    TRUE, 
    UTC_TIMESTAMP()
);

-- Insert normal user with fixed GUID
INSERT INTO Users (Id, Name, Email, IsManager, CreatedAt)
VALUES (
    '22222222-2222-2222-2222-222222222222', 
    'Regular User', 
    'user@test.com', 
    FALSE, 
    UTC_TIMESTAMP()
);
