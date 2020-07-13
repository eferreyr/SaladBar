-- Scaffolding code for PowerShell
-- Scaffold-DbContext "Server=.;Database=SaladBarWeb;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir DBModels -Context AppDbContext -Force

-- Code to drop tables
-- Drop  TABLE dbo.Menus;
-- Drop  TABLE dbo.MenuItemTypes;
-- Drop  TABLE dbo.MenuItems;
-- Drop  TABLE dbo.ImageMetadata;
-- Drop  TABLE dbo.ImageTypes;
-- Drop  TABLE dbo.TrayTypes;
-- Drop  TABLE dbo.InterventionDayTrayTypes;
-- Drop  TABLE dbo.WeighingMeasurements;
-- Drop  TABLE dbo.WeighingMeasurementMenuItems;
-- Drop  TABLE dbo.WeighingMeasurementImageMetadata;
-- Drop  TABLE dbo.WeighingMeasurementTrays;

USE SaladBarWeb;

CREATE TABLE dbo.Menus(
	id bigint IDENTITY(1,1) NOT NULL,
	intervention_day_id bigint NOT NULL,
	name varchar(100) not null,
	active varchar(1) NOT NULL default 'Y',
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_menus PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.MenuItemTypes(
	id int IDENTITY(1,1) NOT NULL,
	type varchar(100) not null,
	active varchar(1) NOT NULL default 'Y',
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_menuitemtypes PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.MenuItems(
	id bigint IDENTITY(1,1) NOT NULL,
	menu_id bigint NOT NULL,
    menu_item_type_id int NOT NULL,
	name varchar(100) not null,
	quantifiable varchar(1) NOT NULL default 'N',
	active varchar(1) NOT NULL default 'Y',
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_menuitems PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.ImageMetadata(
	id int IDENTITY(1,1) NOT NULL,
	name varchar(100) not null,
	active varchar(1) NOT NULL default 'Y',
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_imagemetadata PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.ImageTypes(
	id int IDENTITY(1,1) NOT NULL,
	type varchar(100) not null,
	active varchar(1) NOT NULL default 'Y',
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_imagetypes PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.TrayTypes(
	id int IDENTITY(1,1) NOT NULL,
	type varchar(100) not null,
	active varchar(1) NOT NULL default 'Y',
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_traytypes PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.InterventionDayTrayTypes(
	id bigint IDENTITY(1,1) NOT NULL,
	tray_type_id int not null,
	intervention_day_id bigint NOT NULL,
	active varchar(1) NOT NULL default 'Y',
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_interventiondaytraytypes PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.WeighingMeasurements(
	id bigint IDENTITY(1,1) NOT NULL,
	weighing_id bigint NOT NULL,
	image_type_id int not null,
	weigh_station_type_id int not null, -- Scan Type
	weight smallint not null,
	notes varchar(1000) null,
	tiebreaker varchar(1) null, -- 'Y' or 'N'
	active varchar(1) NOT NULL default 'Y',
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_weighingmeasurements PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.WeighingMeasurementMenuItems(
	id bigint IDENTITY(1,1) NOT NULL,
	weighing_measurement_id bigint NOT NULL,
	menu_item_id bigint not null,
	selected varchar(1) not null default 'N',
    quantity smallint null,
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_weighingmeasurementmenuitems PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.WeighingMeasurementImageMetadata(
	id bigint IDENTITY(1,1) NOT NULL,
	weighing_measurement_id bigint NOT NULL,
	image_metadata_id int not null,
	selected varchar(1) not null default 'N',
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_weighingmeasurementimagemetadata PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.WeighingMeasurementTrays(
	id bigint IDENTITY(1,1) NOT NULL,
	weighing_measurement_id bigint NOT NULL,
	intervention_day_tray_type_id bigint not null,
	quantity smallint not null,
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_weighingmeasurementtrays PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

CREATE TABLE dbo.WeighingMeasurementTracking(
	id bigint IDENTITY(1,1) NOT NULL,
	weighing_measurement_id bigint NOT NULL,
	info varchar(max) not null,
	dt_created datetime NOT NULL default getdate(),
	created_by varchar(100) NOT NULL default 'system',
	dt_modified datetime NULL,
	modified_by varchar(100) NULL,
 CONSTRAINT pk_weighingmeasurementtracking PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

ALTER TABLE dbo.Menus WITH CHECK ADD CONSTRAINT fk_menus_intervention_day_id_interventiondays_id FOREIGN KEY(intervention_day_id)
REFERENCES dbo.InterventionDays (id);
ALTER TABLE dbo.Menus CHECK CONSTRAINT fk_menus_intervention_day_id_interventiondays_id;

ALTER TABLE dbo.MenuItems WITH CHECK ADD CONSTRAINT fk_menuitems_menu_id_menus_id FOREIGN KEY(menu_id)
REFERENCES dbo.Menus (id);
ALTER TABLE dbo.MenuItems CHECK CONSTRAINT fk_menuitems_menu_id_menus_id;

ALTER TABLE dbo.MenuItems WITH CHECK ADD CONSTRAINT fk_menuitems_menu_item_type_id_menuitemtypes_id FOREIGN KEY(menu_item_type_id)
REFERENCES dbo.MenuItemTypes (id);
ALTER TABLE dbo.MenuItems CHECK CONSTRAINT fk_menuitems_menu_item_type_id_menuitemtypes_id;

ALTER TABLE dbo.InterventionDayTrayTypes WITH CHECK ADD CONSTRAINT fk_interventiondaytraytypes_tray_type_id_traytypes_id FOREIGN KEY(tray_type_id)
REFERENCES dbo.TrayTypes (id);
ALTER TABLE dbo.InterventionDayTrayTypes CHECK CONSTRAINT fk_interventiondaytraytypes_tray_type_id_traytypes_id;

ALTER TABLE dbo.InterventionDayTrayTypes WITH CHECK ADD CONSTRAINT fk_menuitems_intervention_day_id_interventiondays_id FOREIGN KEY(intervention_day_id)
REFERENCES dbo.InterventionDays (id);
ALTER TABLE dbo.InterventionDayTrayTypes CHECK CONSTRAINT fk_menuitems_intervention_day_id_interventiondays_id;

ALTER TABLE dbo.WeighingMeasurements WITH CHECK ADD CONSTRAINT fk_weighingmeasurements_weighing_id_weighings_id FOREIGN KEY(weighing_id)
REFERENCES dbo.Weighings (id);
ALTER TABLE dbo.WeighingMeasurements CHECK CONSTRAINT fk_weighingmeasurements_weighing_id_weighings_id;

ALTER TABLE dbo.WeighingMeasurements WITH CHECK ADD CONSTRAINT fk_weighingmeasurements_image_type_id_imagetypes_id FOREIGN KEY(image_type_id)
REFERENCES dbo.ImageTypes (id);
ALTER TABLE dbo.WeighingMeasurements CHECK CONSTRAINT fk_weighingmeasurements_image_type_id_imagetypes_id;

ALTER TABLE dbo.WeighingMeasurements WITH CHECK ADD CONSTRAINT fk_weighingmeasurements_weigh_station_type_id_weighstationtypes_id FOREIGN KEY(weigh_station_type_id)
REFERENCES dbo.WeighStationTypes (id);
ALTER TABLE dbo.WeighingMeasurements CHECK CONSTRAINT fk_weighingmeasurements_weigh_station_type_id_weighstationtypes_id;

ALTER TABLE dbo.WeighingMeasurementMenuItems WITH CHECK ADD CONSTRAINT fk_weighingmeasurementmenuitems_weighing_measurement_id_weighingmeasurements_id FOREIGN KEY(weighing_measurement_id)
REFERENCES dbo.WeighingMeasurements (id);
ALTER TABLE dbo.WeighingMeasurementMenuItems CHECK CONSTRAINT fk_weighingmeasurementmenuitems_weighing_measurement_id_weighingmeasurements_id;

ALTER TABLE dbo.WeighingMeasurementMenuItems WITH CHECK ADD CONSTRAINT fk_weighingmeasurementmenuitems_menu_item_id_menuitems_id FOREIGN KEY(menu_item_id)
REFERENCES dbo.MenuItems (id);
ALTER TABLE dbo.WeighingMeasurementMenuItems CHECK CONSTRAINT fk_weighingmeasurementmenuitems_menu_item_id_menuitems_id;

ALTER TABLE dbo.WeighingMeasurementImageMetadata WITH CHECK ADD CONSTRAINT fk_weighingmeasurementimagemetadata_weighing_measurement_id_weighingmeasurements_id FOREIGN KEY(weighing_measurement_id)
REFERENCES dbo.WeighingMeasurements (id);
ALTER TABLE dbo.WeighingMeasurementImageMetadata CHECK CONSTRAINT fk_weighingmeasurementimagemetadata_weighing_measurement_id_weighingmeasurements_id;

ALTER TABLE dbo.WeighingMeasurementImageMetadata WITH CHECK ADD CONSTRAINT fk_weighingmeasurementimagemetadata_image_metadata_id_imagemetadata_id FOREIGN KEY(image_metadata_id)
REFERENCES dbo.ImageMetadata (id);
ALTER TABLE dbo.WeighingMeasurementImageMetadata CHECK CONSTRAINT fk_weighingmeasurementimagemetadata_image_metadata_id_imagemetadata_id;

ALTER TABLE dbo.WeighingMeasurementTrays WITH CHECK ADD CONSTRAINT fk_weighingmeasurementtrays_weighing_measurement_id_weighingmeasurements_id FOREIGN KEY(weighing_measurement_id)
REFERENCES dbo.WeighingMeasurements (id);
ALTER TABLE dbo.WeighingMeasurementTrays CHECK CONSTRAINT fk_weighingmeasurementtrays_weighing_measurement_id_weighingmeasurements_id;

ALTER TABLE dbo.WeighingMeasurementTrays WITH CHECK ADD CONSTRAINT fk_weighingmeasurementtrays_intervention_day_tray_type_id_interventiondaytraytypes_id FOREIGN KEY(intervention_day_tray_type_id)
REFERENCES dbo.InterventionDayTrayTypes (id);
ALTER TABLE dbo.WeighingMeasurementTrays CHECK CONSTRAINT fk_weighingmeasurementtrays_intervention_day_tray_type_id_interventiondaytraytypes_id;

ALTER TABLE dbo.WeighingMeasurementTracking WITH CHECK ADD CONSTRAINT fk_weighingmeasurementtracking_weighing_measurement_id_weighingmeasurements_id FOREIGN KEY(weighing_measurement_id)
REFERENCES dbo.WeighingMeasurements (id);
ALTER TABLE dbo.WeighingMeasurementTracking CHECK CONSTRAINT fk_weighingmeasurementtracking_weighing_measurement_id_weighingmeasurements_id;