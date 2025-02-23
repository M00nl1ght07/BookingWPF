CREATE PROCEDURE UpdateBookingStatuses
AS
BEGIN
    -- Обновляем завершенные бронирования
    UPDATE Bookings 
    SET Status = 'Завершено'
    WHERE Status = 'Активно' 
    AND CheckOutDate < GETDATE();
END 