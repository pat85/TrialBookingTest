USE [TrialBookingDb];
GO

SET NOCOUNT ON;
GO

/* ============================================================
   PARENTS
   ============================================================ */

INSERT INTO Parents
(
    ParentId,
    ParentName,
    ParentEmail,
    CreatedOn
)
VALUES
(
    '7F3A91C2-5D84-4B17-A6E9-2C0D8F41B735',
    'Andi Pratama',
    'andi.pratama@example.com',
    '2026-08-01T08:15:00'
),
(
    'B28E4D76-91C3-47FA-8A52-6D17C903E4B1',
    'Siti Rahmawati',
    'siti.rahmawati@example.com',
    '2026-08-02T09:30:00'
),
(
    'C6419A53-E7B2-4D08-BF36-91AC527DE840',
    'Michael Tan',
    'michael.tan@example.com',
    '2026-08-03T10:45:00'
),
(
    '4D8B27F1-A693-45CE-92B7-3F80D16A5C49',
    'Rina Wijaya',
    'rina.wijaya@example.com',
    '2026-08-04T11:20:00'
);
GO


/* ============================================================
   STUDENTS
   ============================================================ */

INSERT INTO Students
(
    StudentId,
    ParentId,
    StudentName,
    CreatedOn
)
VALUES
-- Andi's children
(
    'A9137E42-6B5C-4D81-9F03-C72815EA64B9',
    '7F3A91C2-5D84-4B17-A6E9-2C0D8F41B735',
    'Dimas Pratama',
    '2026-08-01T08:20:00'
),
(
    'E57B219C-84F6-43A0-BD71-9C35E8A6240F',
    '7F3A91C2-5D84-4B17-A6E9-2C0D8F41B735',
    'Nadia Pratama',
    '2026-08-01T08:22:00'
),

-- Siti's children
(
    '38C6F1A9-7D24-4BE5-90B3-62E8A4175DCF',
    'B28E4D76-91C3-47FA-8A52-6D17C903E4B1',
    'Fajar Nugraha',
    '2026-08-02T09:35:00'
),
(
    'D7429B15-E638-4AC1-85F0-3B91C67EA204',
    'B28E4D76-91C3-47FA-8A52-6D17C903E4B1',
    'Alya Nugraha',
    '2026-08-02T09:37:00'
),

-- Michael's children
(
    '6F18D3A7-C925-4E60-B841-72AC5D9E103B',
    'C6419A53-E7B2-4D08-BF36-91AC527DE840',
    'Kevin Tan',
    '2026-08-03T10:50:00'
),
(
    'B4936E28-15D7-4FA2-9C81-E0673A5BD942',
    'C6419A53-E7B2-4D08-BF36-91AC527DE840',
    'Sophie Tan',
    '2026-08-03T10:52:00'
),

-- Rina's child
(
    '91E5C472-3A08-4DB6-BF19-6842D7AC530E',
    '4D8B27F1-A693-45CE-92B7-3F80D16A5C49',
    'Bima Ramdani',
    '2026-08-04T11:25:00'
);
GO


/* ============================================================
   TRIAL CLASSES
   ============================================================ */

INSERT INTO TrialClasses
(
    TrialClassId,
    TrialClassTitle,
    TrialClassStartDate,
    TrialClassEndDate,
    TrialClassCapacity,
    CreatedOn
)
VALUES

-- Plenty of availability
(
    '5A72D9C1-4E36-48BF-A805-91C7E2D6430F',
    'Introduction to Space Science',
    '2026-09-15T09:00:00',
    '2026-09-15T10:00:00',
    4,
    '2026-08-15T08:00:00'
),

-- Another available class
(
    'E3816B47-92DA-4C05-AF73-18D5B9E624C0',
    'Fun with Fractions',
    '2026-09-16T10:00:00',
    '2026-09-16T11:00:00',
    4,
    '2026-08-15T08:05:00'
),

-- Intended to eventually demonstrate a 3/4 class
(
    'C9472F15-6A83-4DE1-B509-72E6AC38D4F1',
    'Physics: Forces and Motion',
    '2026-09-17T13:00:00',
    '2026-09-17T14:00:00',
    4,
    '2026-08-15T08:10:00'
),

-- Another class
(
    '7D20A5E9-B461-4FC8-93D7-1A58CE6240BF',
    'Mathematics Problem Solving',
    '2026-09-18T15:00:00',
    '2026-09-18T16:00:00',
    4,
    '2026-08-15T08:15:00'
),

(
    'F6381C72-9B05-4AE3-87D4-52C6E1A903BF',
    'Advanced Science Explorers',
    '2026-09-19T09:00:00',
    '2026-09-19T10:30:00',
    4,
    '2026-08-15T08:20:00'
);
GO