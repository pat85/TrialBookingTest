# Copilot Instructions

## Project Guidelines
- Controllers must not access repositories directly; they should use application services. For payment functionality, extend and use the existing payment service rather than injecting IPaymentAttemptRepository into PaymentController.