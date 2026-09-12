#Trial Booking Web Test
A minimal trial class booking system as a Proof of Concept.

Setup instruction: https://github.com/pat85/TrialBookingTest/blob/main/setup-instruction.docx

However, to make things easy, I alerady setup a sample in a server: https://app.dds.co.id/trialbooking

## Required Technical Scenario: Last-Seat Race

### Problem

A trial class has a maximum capacity of 4 confirmed students. A booking remains in `PendingPayment` while the parent proceeds through the payment step.

The system must handle the case where two users are simultaneously attempting to pay for the last available seat:

1. User A selects the last available seat and proceeds to payment.
2. User B selects the same class and proceeds to payment.
3. User B completes payment first and is confirmed.
4. User A subsequently completes payment.

The system must ensure that **only one booking is confirmed for the final seat**.

### Approach

The application uses a SQL Server transaction with a row-level update lock on the relevant `TrialClass` record when completing a payment.

The critical section is:

1. Begin a database transaction.
2. Acquire an update lock (`UPDLOCK`) on the `TrialClass` row.
3. Re-check the current `ConfirmedBookingCount` against `TrialClassCapacity`.
4. Process the mock payment.
5. If payment succeeds, mark the booking as `Confirmed` and increment `ConfirmedBookingCount`.
6. Save the changes and commit the transaction.
7. If the class is already full, the booking is not confirmed.

The `TrialClass` row acts as the synchronization point for competing booking confirmations for the same class.

For example, if a class has capacity 4 and currently has 3 confirmed students:

```text
User A                         User B
------                         ------
Begin transaction             Begin transaction
Lock TrialClass               Wait for TrialClass lock
3 confirmed / 4 capacity
Payment succeeds
Confirm booking
Increment count to 4
Commit
                              Acquire lock
                              4 confirmed / 4 capacity
                              Class is full
                              Booking not confirmed
```

Therefore, even when both users started payment while the class appeared to have one seat available, only the transaction that obtains the lock first can consume that seat.

### Why this approach

The seat check and the update that consumes the seat must be treated as one concurrency-sensitive operation.

An application-level check such as:

```csharp
if (trialClass.ConfirmedBookingCount < trialClass.TrialClassCapacity)
{
    // confirm booking
}
```

is not sufficient on its own because two concurrent requests can both observe the same available seat before either request updates the database.

Using SQL Server locking places the concurrency guarantee at the shared resource itself. This also works if multiple application instances are running, unlike an in-process C# `lock`.

---

## Backend Design Requirements

### Data Model

The application uses five main entities:

* `Parent`
* `Student`
* `TrialClass`
* `Booking`
* `PaymentAttempt`

All entities use GUID primary keys.

The relationships are:

```text
Parent
  │
  └──< Student
          │
          └──< Booking >── TrialClass
                    │
                    └──< PaymentAttempt
```

`TrialClass` contains the class capacity and the current number of confirmed bookings:

```text
TrialClass
----------
TrialClassId
TrialClassTitle
TrialClassCapacity
ConfirmedBookingCount
...
```

`Booking` represents a student's booking for a particular trial class and contains its current status.

`PaymentAttempt` records the result of each payment attempt independently from the booking itself.

### Key Backend Operations

The main booking operations are implemented in `BookingService`:

#### Submit booking

`SubmitBookingAsync(studentId, trialClassId)`

This validates the selected student and class, checks for an existing booking, and creates a booking with:

```text
PendingPayment
```

At this stage the student does not consume a confirmed seat.

#### Complete payment

`CompletePaymentAsync(bookingId)`

This is the concurrency-sensitive operation.

It:

* validates the booking;
* obtains a database lock on the relevant trial class;
* checks current capacity;
* executes the mock payment;
* records a `PaymentAttempt`;
* confirms the booking and increments the confirmed count only after successful payment.

#### Cancel booking

`CancelBookingAsync(bookingId)`

This changes an existing booking to `Cancelled`.

#### Retrieve booking/roster information

The service also provides methods for retrieving individual booking details and the booking list used to display the roster.

### Booking Statuses

The application currently uses the following booking states:

| Status           | Meaning                                                                 |
| ---------------- | ----------------------------------------------------------------------- |
| `PendingPayment` | Booking has been created but payment has not completed                  |
| `Confirmed`      | Payment succeeded and the student occupies a confirmed seat             |
| `PaymentFailed`  | Payment attempt failed; the student is not part of the confirmed roster |
| `Cancelled`      | Booking has been cancelled                                              |

The important invariant is:

> Only a booking with `Confirmed` status contributes to the confirmed roster and consumes a seat.

### Preventing Duplicate Bookings

The application checks for an existing booking for the same student and trial class before creating a new booking.

This provides a normal application-level validation and allows the UI to return a meaningful duplicate-booking response.

The database should additionally enforce a unique constraint/index on:

```text
(StudentId, TrialClassId)
```

This provides the final guarantee against duplicate bookings if two requests arrive concurrently.

### Payment Failure

Payment is represented separately through `PaymentAttempt`.

When the mock payment fails:

```text
Booking:
PendingPayment → PaymentFailed

PaymentAttempt:
               → Failed
```

The booking is not added to the confirmed roster and `ConfirmedBookingCount` is not incremented.

This keeps payment failure separate from seat allocation.

### Competing for the Last Seat

The confirmed seat count is checked inside a SQL Server transaction while holding an update lock on the relevant `TrialClass` row.

Consequently, concurrent confirmation attempts for the same class are serialized at the point where the seat is consumed.

If the first transaction confirms the final seat, the next transaction observes the updated count and cannot confirm another booking.

### Responsibility by Layer

| Concern                                      | Layer                                               |
| -------------------------------------------- | --------------------------------------------------- |
| Display available classes / basic user input | UI                                                  |
| Validate selected student/class              | Backend service                                     |
| Booking state transitions                    | `BookingService`                                    |
| Payment result handling                      | Payment service + `BookingService`                  |
| Last-seat concurrency                        | Database transaction / SQL Server locking           |
| Duplicate booking guarantee                  | Application validation + database unique constraint |
| Referential integrity                        | Database foreign keys                               |
| Confirmed roster                             | Backend/database                                    |
| Payment attempt history                      | Database                                            |
| Background jobs                              | Not required for this take-home                     |

The application intentionally keeps the implementation small and focuses on the invariants required for reliable trial booking.

## What I Deliberately Cut

To keep the implementation focused on the core reliability requirements within the take-home timebox, I deliberately excluded several features that would be expected in a production-grade booking system but are not necessary to demonstrate the required booking flow.

1. Real Payment Integration: The payment flow uses a mock payment service rather than integrating with a real payment provider.
2. Authentication and Authorization: I did not implement a full authentication/authorization system for parents, teachers, or administrators. The application uses synthetic users and focuses on the booking and roster behavior.
3. Full Scheduling Management: Trial classes are provided through seed data. I did not implement an administrative interface for creating, editing, rescheduling, or cancelling classes.
4. Advanced Booking Features: I deliberately excluded features such as waitlists, recurring classes, rescheduling, refunds, coupon codes, and regular enrollment because they are outside the scope of the requested trial-booking slice.

---

## What I Would Monitor After Release

If this system were deployed to production, I would monitor both **business-level metrics** and **technical/reliability metrics**.

### Booking Metrics

* Number of trial booking attempts
* Booking confirmation rate
* Payment success/failure rate
* Number of cancelled bookings
* Number of duplicate booking attempts
* Number of attempts to book already-full classes
* Number of classes reaching full capacity

These metrics would help identify whether users are successfully completing the booking flow and whether particular classes or time slots have unusual demand.

### Concurrency and Data Integrity

I would specifically monitor:

* Attempts to confirm bookings when a class is already full
* Database constraint violations related to duplicate bookings
* Transaction/deadlock errors
* Unexpected differences between `ConfirmedBookingCount` and the actual number of confirmed bookings

The last item is particularly important because `ConfirmedBookingCount` is maintained as a denormalized value for efficient seat availability checks.

### Payment Reliability

For a real payment provider, I would monitor:

* Payment provider success/failure rates
* Payment timeout/error rates
* Payment attempts without a corresponding booking state transition
* Confirmed bookings without a successful payment
* Successful payments without a confirmed booking

The goal would be to detect inconsistencies between the payment provider and the booking database.

### Application and Infrastructure

I would also monitor standard operational metrics such as:

* API response time
* HTTP error rates
* Database query latency
* Database connection failures
* Transaction/deadlock frequency
* Application exceptions
* CPU and memory utilization

Alerts would be configured for failures that could prevent parents from completing bookings or cause incorrect roster data.

---

## What I Would Do Next With More Time

If more development time were available, I would prioritize improvements in the following order.

1. Strengthen the Payment Workflow: The current payment integration is intentionally mocked. The next step would be integrating a real payment provider with an idempotent payment workflow and webhook handling.
2. Add Comprehensive Concurrency Tests: users competing for one or more remaining seats, concurrent duplicate booking attempts, payment failures
3. Add Authentication and Authorization: I would introduce authentication for parents and role-based authorization for administrative/teacher functionality. Parents should only be able to access their own children's bookings, while teachers/administrators should have access to the appropriate class roster information.
4. Add Observability: I would add structured logging, metrics, and tracing around:

* Booking creation
* Payment attempts
* Booking confirmation
* Seat allocation failures
* Concurrency conflicts
* Payment-provider interactions

This would make production failures and booking/payment inconsistencies easier to diagnose.

5. Improve the User Experience: finally, I would improve the UI around the reliability scenarios. For example, if a user reaches payment but another user has already taken the final seat, the UI should clearly explain that the class is no longer available rather than presenting a generic payment error.
