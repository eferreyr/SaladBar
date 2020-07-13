-- 20190107JY: Added missing fields to Weighings and WeighingsStaging (empty and multiple)
ALTER TABLE Weighings ADD empty VARCHAR(1) NULL, multiple VARCHAR(1) NULL;
ALTER TABLE WeighingsStaging ADD empty VARCHAR(1) NULL, multiple VARCHAR(1) NULL;