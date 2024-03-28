# Loan-API

# Contents
* [About](#About)
* [Technologies Used](#Technologies Used)
* [Installation](#Installation)
* [Features](#Features)
* [Testing](#Testing)



## About

The Loan-API stores information about users and loans in two separate databases using Microsoft Entity Framework.
This is a Loan-API that allows users to register, view their information, and apply for loans. The API has the following features:
- User registration
- View user information
- Apply for different types of loans (Quick loan, Car loan, Instalment)
- View own loans
- Update and delete loan applications (only if the status is "Processing")

- The admin - 'Accountant' type user is able to block other users from creating new loans.
- Accountant can view, update or delete any user loans.
- Accountant can also unblock user to request new loans.

Nlog is used for logging errors and warnings.

## Technologies Used
* NET 5
* MSSQL Server
* NLog
* FluentValidation
* JWT Authentication and Authorization
* BCrypt
* Entity Framework Core
* XUnit

## Testing

Unit tests are used during testing (XUnit).

First of all we should use "generateaccountant" to create Accountant user that will add default accountant user and returns token,
Tokens used in this API is "Bearer Access Token".


