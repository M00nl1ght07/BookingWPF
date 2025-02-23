CREATE PROCEDURE CheckRoomAvailability
    @RoomID int,
    @CheckIn date,
    @CheckOut date
AS
BEGIN
    -- Сначала обновляем статусы
    EXEC UpdateBookingStatuses;

    -- Проверяем доступность
    SELECT COUNT(*)
    FROM Bookings
    WHERE RoomID = @RoomID
    AND Status = 'Активно'
    AND NOT (CheckOutDate <= @CheckIn OR CheckInDate >= @CheckOut);
END 