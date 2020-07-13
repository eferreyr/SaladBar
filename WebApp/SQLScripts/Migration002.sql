-- 20181211JY: Added GlobalInfoItems and WeighingMeasurmentGlobalInfoItems
CREATE TABLE dbo.GlobalInfoItems(
	id BIGINT IDENTITY(1,1) NOT NULL,
    type VARCHAR(100) NOT NULL,
	name VARCHAR(100) NOT NULL,
	active VARCHAR(1) NOT NULL DEFAULT 'Y',
	dt_created DATETIME NOT NULL DEFAULT getdate(),
	created_by VARCHAR(100) NOT NULL DEFAULT 'system',
	dt_modified DATETIME NULL,
	modified_by VARCHAR(100) NULL,
 CONSTRAINT pk_globalinfoitems PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.WeighingMeasurmentGlobalInfoItems(
	id BIGINT IDENTITY(1,1) NOT NULL,
	randomized_student_id BIGINT NOT NULL,
	global_info_item_id BIGINT NOT NULL,
    value VARCHAR(max) null,
	dt_created DATETIME NOT NULL DEFAULT getdate(),
	created_by VARCHAR(100) NOT NULL DEFAULT 'system',
	dt_modified DATETIME NULL,
	modified_by VARCHAR(100) NULL,
 CONSTRAINT pk_weighingmeasurementglobalinfoitems PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

ALTER TABLE dbo.WeighingMeasurmentGlobalInfoItems WITH CHECK ADD CONSTRAINT fk_weighingmeasurementglobalinfoitems_randomized_student_id_randomizedstudents_id FOREIGN KEY(randomized_student_id)
REFERENCES dbo.RandomizedStudents (id);
ALTER TABLE dbo.WeighingMeasurmentGlobalInfoItems CHECK CONSTRAINT fk_weighingmeasurementglobalinfoitems_randomized_student_id_randomizedstudents_id;

ALTER TABLE dbo.WeighingMeasurmentGlobalInfoItems WITH CHECK ADD CONSTRAINT fk_weighingmeasurementglobalinfoitems_global_info_item_id_globalinfoitems_id FOREIGN KEY(global_info_item_id)
REFERENCES dbo.GlobalInfoItems (id);
ALTER TABLE dbo.WeighingMeasurmentGlobalInfoItems CHECK CONSTRAINT fk_weighingmeasurementglobalinfoitems_global_info_item_id_globalinfoitems_id;