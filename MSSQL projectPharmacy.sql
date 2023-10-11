create database projectPharmacy;

use projectPharmacy;

CREATE TABLE Drugs (
    DrugID INT PRIMARY KEY,
    DrugName NVARCHAR(200) NOT NULL,
    MainForm NVARCHAR(100) NOT NULL,
    PharmaForm NVARCHAR(200) NOT NULL,
    Dosage NVARCHAR(100) NOT NULL,
    CurtForm NVARCHAR(200) NOT NULL,
    Packing INT NOT NULL,
    Manufacturer NVARCHAR(200) NOT NULL,
    Producer NVARCHAR(100) NOT NULL,
    MNN NVARCHAR(300) NOT NULL,
    FTG NVARCHAR(300) NOT NULL
);

CREATE TABLE Instructions (
    InstructionID INT PRIMARY KEY,
    InstructionName NVARCHAR(300) NOT NULL,
    InstructionDescription NTEXT NOT NULL
);

CREATE TABLE InstructionsDrugs (
	ID INT PRIMARY KEY,
    InstructionID INT NOT NULL,
    DrugID INT NOT NULL,
    FOREIGN KEY (InstructionID) REFERENCES Instructions(InstructionID) ON DELETE CASCADE,
    FOREIGN KEY (DrugID) REFERENCES Drugs(DrugID) ON DELETE CASCADE
);