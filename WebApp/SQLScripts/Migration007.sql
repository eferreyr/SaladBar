-- 20190213JY: Made AuditsStaging's batchId not nullable and add the missing seed column for Devices table
ALTER TABLE AuditsStaging ALTER COLUMN batch_id BIGINT NOT NULL;
ALTER TABLE Devices ADD seed BIGINT NOT NULL DEFAULT 0;