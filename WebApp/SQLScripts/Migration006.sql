-- 20190125JY: Updating ResearchTeamMembers information and adding additional members
USE SaladBarWeb

update ResearchTeamMembers set email = 'cmmorri3@asu.edu', modified_by = 'tsungyen@asu.edu', dt_modified = GETDATE() where email = 'Carina.Morris@asu.edu'
update ResearchTeamMembers set email = 'hmuise@asu.edu', modified_by = 'tsungyen@asu.edu', dt_modified = GETDATE() where email = 'Hannah.Muise@asu.edu'
update ResearchTeamMembers set email = 'joneill3@asu.edu', modified_by = 'tsungyen@asu.edu', dt_modified = GETDATE(), active = 'N' where email = 'enika246@gmail.com'
update ResearchTeamMembers set email = 'cmthunbe@asu.edu', modified_by = 'tsungyen@asu.edu', dt_modified = GETDATE() where email = 'Carly.Thunberg@asu.edu'


insert into ResearchTeamMembers (email, first_name, last_name, created_by)
values
('zamarque@asu.edu', '', '', 'tsungyen@asu.edu'),
('pmgibso1@asu.edu', '', '', 'tsungyen@asu.edu'),
('kkwood1@asu.edu', '', '', 'tsungyen@asu.edu'),
('lnakaima@asu.edu', '', '', 'tsungyen@asu.edu'),
('cvthalhe@asu.edu', '', '', 'tsungyen@asu.edu'),
('hankster@asu.edu', '', '', 'tsungyen@asu.edu'),
('tmchung@asu.edu', '', '', 'tsungyen@asu.edu'),
('vnapolit@asu.edu', '', '', 'tsungyen@asu.edu'),
('mjmarcot@asu.edu', '', '', 'tsungyen@asu.edu'),
('ljkieffe@asu.edu', '', '', 'tsungyen@asu.edu'),
('mkgriff1@asu.edu', '', '', 'tsungyen@asu.edu'),
('egonza64@asu.edu', '', '', 'tsungyen@asu.edu'),
('aekenne2@asu.edu', '', '', 'tsungyen@asu.edu'),
('clewis26@asu.edu', '', '', 'tsungyen@asu.edu'),
('udoan1@asu.edu', '', '', 'tsungyen@asu.edu'),
('kfdiaz@asu.edu', '', '', 'tsungyen@asu.edu'),
('test@asu.edu', 'Test', 'Account', 'tsungyen@asu.edu'),
('rabrice@asu.edu', '', '', 'tsungyen@asu.edu'),
('chaifley@asu.edu', '', '', 'tsungyen@asu.edu'),
('keromero@asu.edu', '', '', 'tsungyen@asu.edu'),
('dijimen1@asu.edu', '', '', 'tsungyen@asu.edu'),
('rvemuri1@asu.edu', '', '', 'tsungyen@asu.edu'),
('htuomist@asu.edu', '', '', 'tsungyen@asu.edu'),
('jdnieto@asu.edu', '', '', 'tsungyen@asu.edu'),
('adeleon5@asu.edu', '', '', 'tsungyen@asu.edu'),
('rweight@asu.edu', '', '', 'tsungyen@asu.edu'),
('cmmorr18@asu.edu', '', '', 'tsungyen@asu.edu');