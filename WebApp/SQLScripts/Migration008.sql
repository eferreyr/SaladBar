-- 20190509JY: Add additional "value" column to WeighingMeasurementImageMetadata to track detail image issues
ALTER TABLE WeighingMeasurementImageMetadata ADD value SMALLINT NOT NULL DEFAULT 0;


CREATE TABLE dbo.DuplicateImageOptions(
	id BIGINT IDENTITY(1,1) NOT NULL,
    type VARCHAR(100) NOT NULL,
	active VARCHAR(1) NOT NULL DEFAULT 'Y',
	dt_created DATETIME NOT NULL DEFAULT getdate(),
	created_by VARCHAR(100) NOT NULL DEFAULT 'system',
	dt_modified DATETIME NULL,
	modified_by VARCHAR(100) NULL,
 CONSTRAINT pk_duplicateimageoptions PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);