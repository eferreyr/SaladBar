-- 20181226JY: Added DataEntryLocks
CREATE TABLE dbo.DataEntryLocks(
	id BIGINT IDENTITY(1,1) NOT NULL,
	asp_net_user_id NVARCHAR(450) NOT NULL,
	intervention_day_id BIGINT NOT NULL,
	locked VARCHAR(1) NOT NULL DEFAULT 'Y',
	dt_created DATETIME NOT NULL DEFAULT getdate(),
	created_by VARCHAR(100) NOT NULL DEFAULT 'system',
	dt_modified DATETIME NULL,
	modified_by VARCHAR(100) NULL,
 CONSTRAINT pk_dataentrylocks PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);

ALTER TABLE dbo.DataEntryLocks WITH CHECK ADD CONSTRAINT fk_dataentrylocks_asp_net_user_id_aspnetusers_id FOREIGN KEY(asp_net_user_id)
REFERENCES dbo.AspNetUsers (id);
ALTER TABLE dbo.DataEntryLocks CHECK CONSTRAINT fk_dataentrylocks_asp_net_user_id_aspnetusers_id;

ALTER TABLE dbo.DataEntryLocks WITH CHECK ADD CONSTRAINT fk_dataentrylocks_intervention_day_id_interventiondays_id FOREIGN KEY(intervention_day_id)
REFERENCES dbo.InterventionDays (id);
ALTER TABLE dbo.DataEntryLocks CHECK CONSTRAINT fk_dataentrylocks_intervention_day_id_interventiondays_id;