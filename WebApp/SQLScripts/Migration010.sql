-- 20190805JY: Added seed value into Devices for laptops
update Devices set seed = (id * 1000000), dt_modified = GETDATE(), modified_by = 'tsungyen@asu.edu' where seed = 0