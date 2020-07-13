-- 20181128JY: Added FirstName and LastName column into AspNetUser
ALTER TABLE AspNetUsers ADD FirstName VARCHAR(50) NULL, LastName VARCHAR(50) NULL;