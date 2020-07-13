-- 20190610JY: Add Training table and additional column into TrainingSets and TrainingInstances to allow and keep track of additional training sets
ALTER TABLE TrainingSets ADD training_id INT NOT NULL DEFAULT 0;
ALTER TABLE TrainingInstances ADD training_id INT NOT NULL DEFAULT 0;

CREATE TABLE dbo.Trainings(
	id INT IDENTITY(1,1) NOT NULL,
    name VARCHAR(100) NOT NULL,
	active VARCHAR(1) NOT NULL DEFAULT 'Y',
	dt_created DATETIME NOT NULL DEFAULT getdate(),
	created_by VARCHAR(100) NOT NULL DEFAULT 'system',
	dt_modified DATETIME NULL,
	modified_by VARCHAR(100) NULL,
 CONSTRAINT pk_trainings PRIMARY KEY CLUSTERED 
 (
	id ASC
 )
);