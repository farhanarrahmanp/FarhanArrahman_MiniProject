-- DDL

CREATE DATABASE DbAppPresensi

CREATE TABLE Kantor (
    Id INT NOT NULL,
    CONSTRAINT PK_Kantor PRIMARY KEY (Id),
    Wilayah VARCHAR (30) NOT NULL
);

CREATE TABLE Divisi (
    Id INT NOT NULL,
    CONSTRAINT PK_Divisi PRIMARY KEY (Id),
    Nama VARCHAR (12) NOT NULL,
    CONSTRAINT U_Nama_Divisi UNIQUE (Nama)
);

CREATE TABLE Pegawai (
    Id VARCHAR (15) NOT NULL,
    CONSTRAINT PK_Pegawai PRIMARY KEY (Id),
    Nama VARCHAR (50) NOT NULL,
    TmptLahir TEXT NOT NULL,
    TglLahir DATE NOT NULL,
    Gender CHAR(1) NOT NULL CHECK (Gender IN ('L', 'P')),
    Agama VARCHAR (8) NOT NULL,
    Alamat TEXT NOT NULL,
    Email VARCHAR (40) NOT NULL,
    CONSTRAINT U_Email_Pegawai UNIQUE (Email),
    NoHp VARCHAR (15) NOT NULL,
    CONSTRAINT U_NoHp_Pegawai UNIQUE (NoHp),
    Foto VARCHAR (25) NOT NULL,
    CONSTRAINT U_Foto_Pegawai UNIQUE (Foto),
    DivisiId INT NOT NULL,
    CONSTRAINT FK_Pegawai_Divisi_DivisiId FOREIGN KEY (DivisiId) REFERENCES Divisi (Id),
    KantorId INT NOT NULL,
    CONSTRAINT FK_Pegawai_Kantor_KantorId FOREIGN KEY (KantorId) REFERENCES Kantor (Id)
);

CREATE TABLE Manajer (
    Id INT NOT NULL,
    CONSTRAINT PK_Manajer PRIMARY KEY (Id),
    Level CHAR (1) NOT NULL,
    PegawaiId VARCHAR (15) NOT NULL,
    CONSTRAINT FK_Manajer_Pegawai_PegawaiId FOREIGN KEY (PegawaiId) REFERENCES Pegawai (Id)
);

CREATE TABLE Presensi (
    Id INT NOT NULL,
    CONSTRAINT PK_Presensi PRIMARY KEY (Id),
    TglPresen DATE NOT NULL,
    WktMasuk TIME NOT NULL,
    WktKeluar TIME NOT NULL,
    StatMasuk VARCHAR (5) NOT NULL CHECK (StatMasuk IN ('Sudah', 'Belum', 'Tidak')),
    StatKeluar VARCHAR (5) NOT NULL CHECK (StatKeluar IN ('Sudah', 'Belum')),
    WktTelat TIME NOT NULL,
    PegawaiId VARCHAR (15) NOT NULL,
    CONSTRAINT FK_Presensi_Pegawai_PegawaiId FOREIGN KEY (PegawaiId) REFERENCES Pegawai (Id)
);

CREATE TABLE Absensi (
    Id INT NOT NULL,
    CONSTRAINT PK_Absensi PRIMARY KEY (Id),
    TglMulai TIME,
    TglSelesai TIME,
    Stat VARCHAR (5) CHECK (Stat IN ('Cuti', 'Sakit')),
    Ket TEXT,
    PegawaiId VARCHAR (15) NOT NULL,
    CONSTRAINT FK_Absensi_Pegawai_PegawaiId FOREIGN KEY (PegawaiId) REFERENCES Pegawai (Id)
);

-- Create Table Akun

CREATE TABLE Akun(
    Id INT NOT NULL,
    CONSTRAINT PK_Akun PRIMARY KEY(Id),
    PegawaiEmail VARCHAR(40) NOT NULL,
    CONSTRAINT 
        FK_Akun_Pegawai_PegawaiEmail 
    FOREIGN KEY(PegawaiEmail) 
    REFERENCES Pegawai(Email),
    PegawaiNoHp VARCHAR(15) NOT NULL,
    CONSTRAINT 
        FK_Akun_Pegawai_PegawaiNoHp 
    FOREIGN KEY(PegawaiNoHp) 
    REFERENCES Pegawai(NoHp),
    Pw VARCHAR(32) NOT NULL
);

-- Create Table Posisi

CREATE TABLE Posisi(
    Id INT NOT NULL,
    CONSTRAINT PK_Posisi PRIMARY KEY(Id),
    Nama VARCHAR(12) NOT NULL
);

ALTER TABLE 
    Posisi 
ALTER COLUMN 
    Nama VARCHAR(48) NOT NULL;

ALTER TABLE 
    Posisi 
ADD 
    DivisiId INT;

ALTER TABLE 
    Posisi 
ADD CONSTRAINT 
    FK_Posisi_Divisi_DivisiId 
FOREIGN KEY
    (DivisiId) 
REFERENCES 
    Divisi(Id);

-- Drop Table Manajer

IF EXISTS 
    (SELECT * FROM sys.objects 
WHERE 
    object_id = OBJECT_ID(N'[dbo].[Manajer]') 
AND 
    type in (N'U'))
DROP TABLE 
    [dbo].[Manajer]
GO

-- Modify Table Pegawai

ALTER TABLE 
    Pegawai 
DROP CONSTRAINT 
    FK_Pegawai_Divisi_DivisiId;
GO

sp_rename 'Pegawai.DivisiId', 'PosisiId', 'COLUMN';

ALTER TABLE 
    Pegawai 
ADD CONSTRAINT 
    FK_Pegawai_Posisi_PosisiId 
FOREIGN KEY
    (PosisiId) 
REFERENCES 
    Posisi(Id);

-- DML

-- Insert Values Kantor

sp_rename 'Kantor.Wilayah', 'Alamat', 'COLUMN';

ALTER TABLE 
    Kantor 
ALTER COLUMN 
    Alamat TEXT;

INSERT INTO 
    Kantor(Id, Alamat)
VALUES
    (110001, 'Jl. Jend. Sudirman Kav. 524, Jakarta 52920'),
    (110002, 'Jl. M.H. Thamrin No. 501, Jakarta 50310'),
    (110003, 'Jl. MT. Haryono Kav. 550-551, Jakarta 52770');

-- Insert Values Posisi

INSERT INTO 
    Posisi
VALUES
    (160001, 'President Director', NULL),
    (160002, 'Head of IT', NULL),
    (160003, 'Head of Division', 040001),
    (160004, 'Head of Division', 040002),
    (160005, 'Head of Division', 040003),
    (160006, 'Head of Division', 040004),
    (160007, 'IT Strategist', 040001),
    (160008, 'IT Planner', 040001),
    (160009, 'Programmer / Application Developer', 040002),
    (160010, 'System Analyst', 040002),
    (160011, 'Network Specialist', 040003),
    (160012, 'Hardware Specialist', 040003),
    (160013, 'System Administrator', 040003),
    (160014, 'Security Specialist', 040003),
    (160015, 'IT Staff', 040004),
    (160016, 'Help Desk Staff', 040004);

-- Insert Values Divisi

ALTER TABLE 
    Divisi 
ALTER COLUMN 
    Nama VARCHAR(36);

INSERT INTO 
    Divisi
VALUES
    (040001, 'IT Strategy & Planning'),
    (040002, 'IT Application & Development'),
    (040003, 'IT Network & Infrastructure'),
    (040004, 'IT Operation');

-- Insert Values Pegawai

ALTER TABLE Pegawai ALTER COLUMN Agama VARCHAR(16);

INSERT INTO Pegawai
VALUES
    ('161GW000001', 
    'Yogi Triyono', 
    'Kediri', '1986-05-22', 
    'L', 
    'Islam', 
    'Jl. Mangga Besar VI, Jawa Timur 10256', 
    'yogitryono@gmail.com', 
    '080047249654', 
    'yogitryono.jpeg', 
    160001,
    110001),
    ('161GW000002', 
    'Bayu Setianto', 
    'Ponorogo', '1986-11-25', 
    'L', 
    'Islam', 
    'Jl. Jati 20, Jawa Tengah 10123', 
    'bayusa@gmail.com', 
    '080093513649', 
    'bayusa.png', 
    160002,
    110001),
    ('161GW000003', 
    'Michael Naidu', 
    'Jakarta', '1988-01-08', 
    'L', 
    'Katolik', 
    'Jl. Zainul Arifin, Medan 70153', 
    'michidu@gmail.com', 
    '080084527351', 
    'michidu.jpeg', 
    160003,
    110001),
    ('161GW000004', 
    'Jefri Williams', 
    'Bandung', '1987-03-29', 
    'L', 
    'Protestan', 
    'Jl. Puyuh Dalam No. 243, Jawa Barat 92043', 
    'jefwil@gmail.com', 
    '080056127057', 
    'jefwil.jpg', 
    160004,
    110001),
    ('161GW000005', 
    'Muhammad Fauzi', 
    'Semarang', '1990-05-01', 
    'L', 
    'Islam', 
    'Jl. Merpati No. 14, Jawa Barat 16115', 
    'muhfauzi@gmail.com', 
    '080058205614', 
    'muhfauzi.png', 
    160005,
    110002),
    ('161GW000006', 
    'Diaz Ayu', 
    'Trenggalek', '1993-06-17', 
    'P', 
    'Islam', 
    'Jl. Gunawarman, Semarang 10217', 
    'diazayu@gmail.com', 
    '080006532865', 
    'diazayu.jpeg', 
    160006,
    110003);

-- Insert Values Akun

INSERT INTO Akun
VALUES
    (010001, 'yogitryono@gmail.com', 
    '080047249654', 'Y06130no'),
    (010002, 'bayusa@gmail.com', 
    '080093513649', 'B@y035A'),
    (010003, 'michidu@gmail.com', 
    '080084527351', 'M1c1du'),
    (010004, 'jefwil@gmail.com', 
    '080056127057', 'J3fvv1L'),
    (010005, 'muhfauzi@gmail.com', 
    '080058205614', 'M03hf@u21'),
    (010006, 'diazayu@gmail.com', 
    '080006532865', 'D1@z4yu');

---- Fitur Register

INSERT INTO Pegawai
VALUES
    ('161GW000007', 
    'Sekar Safitri', 
    'Palembang', '1990-12-04', 
    'P', 
    'Islam', 
    'Jl. Mawar No. 70, Jawa Barat 90172', 
    'sekarfitri@gmail.com', 
    '080064829915', 
    'sekarfitri.jpg', 
    160007,
    110001);

INSERT INTO Akun
VALUES
    (010007, 'sekarfitri@gmail.com', 
    '080064829915', '53k412f1TR1');

---- Fitur Login

SELECT 
    PegawaiEmail, PegawaiNoHp 
FROM
    Akun
WHERE
    (PegawaiEmail = 'sekarfitri@gmail.com' 
OR 
    PegawaiNoHp = '') 
AND
    Pw = '53k412f1TR1';

---- Fitur Lupa Password

UPDATE 
    Akun 
SET 
    Pw = 'S3K412F17' 
WHERE 
    (PegawaiEmail = 'sekarfitri@gmail.com' 
OR 
    PegawaiNoHp = '080064829915');

---- Fitur Menampilkan Data Pegawai

SELECT 
    Pe.Email, 
    Pe.Alamat, 
    Pe.Foto, 
    Pe.Nama, 
    Po.Nama 'PosisiNama', 
    Di.Nama 'DivisiNama', 
    Ka.Alamat 'KantorAlamat'
FROM 
    Pegawai Pe 
JOIN 
    Posisi Po 
ON 
    Pe.PosisiId = Po.Id
JOIN 
    Kantor Ka 
ON 
    Pe.KantorId = Ka.Id 
JOIN 
    Divisi Di 
ON 
    Po.DivisiId = Di.Id;
