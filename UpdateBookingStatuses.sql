-- Сначала удаляем существующую процедуру, если она есть
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpdateBookingStatuses')
    DROP PROCEDURE UpdateBookingStatuses
GO

-- Создаем новую процедуру
CREATE PROCEDURE UpdateBookingStatuses
AS
BEGIN
    -- Используем GETDATE() для получения текущей даты с сервера
    DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);

    -- Обновляем завершенные бронирования
    UPDATE Bookings 
    SET Status = 'Завершено'
    WHERE Status = 'Активно' 
    AND CheckOutDate <= @CurrentDate;
END
GO 