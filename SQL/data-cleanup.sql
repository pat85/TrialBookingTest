use TrialBookingDb;
go

truncate table PaymentAttempts;
go

delete from Bookings;
go

update TrialClasses set ConfirmedBookingCount = 0;
go