-- Создаем задание для автоматического обновления статусов
USE msdb;
GO

-- Создаем job
EXEC dbo.sp_add_job
    @job_name = N'UpdateBookingStatuses_Job';

-- Создаем шаг задания
EXEC dbo.sp_add_jobstep
    @job_name = N'UpdateBookingStatuses_Job',
    @step_name = N'Update Statuses',
    @subsystem = N'TSQL',
    @command = N'EXEC HotelBookingDB.dbo.UpdateBookingStatuses',
    @database_name = N'HotelBookingDB';

-- Создаем расписание (каждый час)
EXEC dbo.sp_add_schedule
    @schedule_name = N'HourlySchedule',
    @freq_type = 4,
    @freq_interval = 1,
    @freq_subday_type = 8,
    @freq_subday_interval = 1;

-- Привязываем расписание к заданию
EXEC dbo.sp_attach_schedule
    @job_name = N'UpdateBookingStatuses_Job',
    @schedule_name = N'HourlySchedule';

-- Включаем задание
EXEC dbo.sp_add_jobserver
    @job_name = N'UpdateBookingStatuses_Job'; 