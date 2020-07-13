-- 20190923JY: Added Seconds column into Weighings
ALTER TABLE Weighings ADD seconds VARCHAR(1) NULL;
ALTER TABLE WeighingsStaging ADD seconds VARCHAR(1) NULL;