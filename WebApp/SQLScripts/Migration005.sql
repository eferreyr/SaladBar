-- 20190122JY: Added supporting tables for data entry training
USE SaladBarWeb

CREATE TABLE dbo.TrainingSets(
	id bigint IDENTITY(1,1) NOT NULL,
	randomized_student_id bigint NOT NULL,
	active varchar(1) NOT NULL DEFAULT 'Y',
	dt_created datetime NOT NULL DEFAULT getdate(),
	created_by varchar(100) NOT NULL DEFAULT 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_trainingsets PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.TrainingSolutions(
	id bigint IDENTITY(1,1) NOT NULL,
	weighing_id bigint NOT NULL,
    training_weighing_measurement_id bigint NOT NULL,
	active varchar(1) NOT NULL DEFAULT 'Y',
	dt_created datetime NOT NULL DEFAULT getdate(),
	created_by varchar(100) NOT NULL DEFAULT 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_trainingsolutions PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.TrainingInstances(
	id bigint IDENTITY(1,1) NOT NULL,
	research_team_member_id bigint NOT NULL,
	training_weighing_measurement_ids varchar(200) NULL,
	completed varchar(1) NOT NULL DEFAULT 'N',
	dt_created datetime NOT NULL DEFAULT getdate(),
	created_by varchar(100) NOT NULL DEFAULT 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_traininginstances PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.TrainingWeighingMeasurements(
	id bigint IDENTITY(1,1) NOT NULL,
	weighing_id bigint NOT NULL,
	image_type_id int NOT NULL,
	weigh_station_type_id int NOT NULL,
	weight smallint NOT NULL,
	notes varchar(1000) NULL,
	tiebreaker varchar(1) NULL,
	active varchar(1) NOT NULL DEFAULT 'Y',
	dt_created datetime NOT NULL DEFAULT getdate(),
	created_by varchar(100) NOT NULL DEFAULT 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_trainingweighingmeasurements PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.TrainingWeighingMeasurementTrays(
	id bigint IDENTITY(1,1) NOT NULL,
	training_weighing_measurement_id bigint NOT NULL,
	intervention_day_tray_type_id bigint NOT NULL,
	quantity smallint NOT NULL,
	dt_created datetime NOT NULL DEFAULT getdate(),
	created_by varchar(100) NOT NULL DEFAULT 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_trainingweighingmeasurementtrays PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.TrainingWeighingMeasurementTracking(
	id bigint IDENTITY(1,1) NOT NULL,
	training_weighing_measurement_id bigint NOT NULL,
	info varchar(max) NOT NULL,
	dt_created datetime NOT NULL DEFAULT getdate(),
	created_by varchar(100) NOT NULL DEFAULT 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_trainingweighingmeasurementtracking PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.TrainingWeighingMeasurementMenuItems(
	id bigint IDENTITY(1,1) NOT NULL,
	training_weighing_measurement_id bigint NOT NULL,
	menu_item_id bigint NOT NULL,
	selected varchar(1) NOT NULL DEFAULT 'N',
	quantity smallint NULL,
	dt_created datetime NOT NULL DEFAULT getdate(),
	created_by varchar(100) NOT NULL DEFAULT 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_trainingweighingmeasurementmenuitems PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.TrainingWeighingMeasurementImageMetadata(
	id bigint IDENTITY(1,1) NOT NULL,
	training_weighing_measurement_id bigint NOT NULL,
	image_metadata_id int NOT NULL,
	selected varchar(1) NOT NULL DEFAULT 'N',
	dt_created datetime NOT NULL DEFAULT getdate(),
	created_by varchar(100) NOT NULL DEFAULT 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_trainingweighingmeasurementimagemetadata PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.TrainingWeighingMeasurmentGlobalInfoItems(
	id bigint IDENTITY(1,1) NOT NULL,
	randomized_student_id bigint NOT NULL,
	global_info_item_id bigint NOT NULL,
	value varchar(max) NULL,
	dt_created datetime NOT NULL DEFAULT getdate(),
	created_by varchar(100) NOT NULL DEFAULT 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_trainingweighingmeasurementglobalinfoitems PRIMARY KEY CLUSTERED
 (
	id ASC
 )
);

ALTER TABLE dbo.TrainingSets  WITH CHECK ADD  CONSTRAINT fk_trainingsets_randomized_student_id_randomizedstudents_id FOREIGN KEY(randomized_student_id)
REFERENCES dbo.RandomizedStudents (id)
ALTER TABLE dbo.TrainingSets CHECK CONSTRAINT fk_trainingsets_randomized_student_id_randomizedstudents_id

ALTER TABLE dbo.TrainingInstances  WITH CHECK ADD  CONSTRAINT fk_traininginstances_research_team_member_id_researchteammembers_id FOREIGN KEY(research_team_member_id)
REFERENCES dbo.ResearchTeamMembers (id)
ALTER TABLE dbo.TrainingInstances CHECK CONSTRAINT fk_traininginstances_research_team_member_id_researchteammembers_id

ALTER TABLE dbo.TrainingWeighingMeasurements  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurements_image_type_id_imagetypes_id FOREIGN KEY(image_type_id)
REFERENCES dbo.ImageTypes (id)
ALTER TABLE dbo.TrainingWeighingMeasurements CHECK CONSTRAINT fk_trainingweighingmeasurements_image_type_id_imagetypes_id

ALTER TABLE dbo.TrainingWeighingMeasurements  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurements_weigh_station_type_id_weighstationtypes_id FOREIGN KEY(weigh_station_type_id)
REFERENCES dbo.WeighStationTypes (id)
ALTER TABLE dbo.TrainingWeighingMeasurements CHECK CONSTRAINT fk_trainingweighingmeasurements_weigh_station_type_id_weighstationtypes_id

ALTER TABLE dbo.TrainingWeighingMeasurements  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurements_weighing_id_weighings_id FOREIGN KEY(weighing_id)
REFERENCES dbo.Weighings (id)
ALTER TABLE dbo.TrainingWeighingMeasurements CHECK CONSTRAINT fk_trainingweighingmeasurements_weighing_id_weighings_id

ALTER TABLE dbo.TrainingWeighingMeasurementTrays  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurementtrays_intervention_day_tray_type_id_interventiondaytraytypes_id FOREIGN KEY(intervention_day_tray_type_id)
REFERENCES dbo.InterventionDayTrayTypes (id)
ALTER TABLE dbo.TrainingWeighingMeasurementTrays CHECK CONSTRAINT fk_trainingweighingmeasurementtrays_intervention_day_tray_type_id_interventiondaytraytypes_id

ALTER TABLE dbo.TrainingWeighingMeasurementTrays  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurementtrays_training_weighing_measurement_id_trainingweighingmeasurements_id FOREIGN KEY(training_weighing_measurement_id)
REFERENCES dbo.TrainingWeighingMeasurements (id)
ALTER TABLE dbo.TrainingWeighingMeasurementTrays CHECK CONSTRAINT fk_trainingweighingmeasurementtrays_training_weighing_measurement_id_trainingweighingmeasurements_id

ALTER TABLE dbo.TrainingWeighingMeasurementTracking  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurementtracking_training_weighing_measurement_id_trainingweighingmeasurements_id FOREIGN KEY(training_weighing_measurement_id)
REFERENCES dbo.TrainingWeighingMeasurements (id)
ALTER TABLE dbo.TrainingWeighingMeasurementTracking CHECK CONSTRAINT fk_trainingweighingmeasurementtracking_training_weighing_measurement_id_trainingweighingmeasurements_id

ALTER TABLE dbo.TrainingWeighingMeasurementMenuItems  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurementmenuitems_menu_item_id_menuitems_id FOREIGN KEY(menu_item_id)
REFERENCES dbo.MenuItems (id)
ALTER TABLE dbo.TrainingWeighingMeasurementMenuItems CHECK CONSTRAINT fk_trainingweighingmeasurementmenuitems_menu_item_id_menuitems_id

ALTER TABLE dbo.TrainingWeighingMeasurementMenuItems  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurementmenuitems_training_weighing_measurement_id_trainingweighingmeasurements_id FOREIGN KEY(training_weighing_measurement_id)
REFERENCES dbo.TrainingWeighingMeasurements (id)
ALTER TABLE dbo.TrainingWeighingMeasurementMenuItems CHECK CONSTRAINT fk_trainingweighingmeasurementmenuitems_training_weighing_measurement_id_trainingweighingmeasurements_id

ALTER TABLE dbo.TrainingWeighingMeasurementImageMetadata  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurementimagemetadata_image_metadata_id_imagemetadata_id FOREIGN KEY(image_metadata_id)
REFERENCES dbo.ImageMetadata (id)
ALTER TABLE dbo.TrainingWeighingMeasurementImageMetadata CHECK CONSTRAINT fk_trainingweighingmeasurementimagemetadata_image_metadata_id_imagemetadata_id

ALTER TABLE dbo.TrainingWeighingMeasurementImageMetadata  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurementimagemetadata_training_weighing_measurement_id_trainingweighingmeasurements_id FOREIGN KEY(training_weighing_measurement_id)
REFERENCES dbo.TrainingWeighingMeasurements (id)
ALTER TABLE dbo.TrainingWeighingMeasurementImageMetadata CHECK CONSTRAINT fk_trainingweighingmeasurementimagemetadata_training_weighing_measurement_id_trainingweighingmeasurements_id

ALTER TABLE dbo.TrainingWeighingMeasurmentGlobalInfoItems  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurementglobalinfoitems_global_info_item_id_globalinfoitems_id FOREIGN KEY(global_info_item_id)
REFERENCES dbo.GlobalInfoItems (id)
ALTER TABLE dbo.TrainingWeighingMeasurmentGlobalInfoItems CHECK CONSTRAINT fk_trainingweighingmeasurementglobalinfoitems_global_info_item_id_globalinfoitems_id

ALTER TABLE dbo.TrainingWeighingMeasurmentGlobalInfoItems  WITH CHECK ADD  CONSTRAINT fk_trainingweighingmeasurementglobalinfoitems_randomized_student_id_randomizedstudents_id FOREIGN KEY(randomized_student_id)
REFERENCES dbo.RandomizedStudents (id)
ALTER TABLE dbo.TrainingWeighingMeasurmentGlobalInfoItems CHECK CONSTRAINT fk_trainingweighingmeasurementglobalinfoitems_randomized_student_id_randomizedstudents_id