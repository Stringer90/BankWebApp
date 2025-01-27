## Description
Currently, this is an ASP.NET Core Web API for a banking web application.
It uses an SQLite database with ADO.NET connectivity.

This was originally a group project in a university course, where I did most of the API code.
This is that API copied over to a new project, but with only my code and with some improvements.
I can demonstrate our group's project during an interview, if needed.

#### Note
The API worked in the assignment, it is not expected to work now and has not been tested.
I will test and fix it some time in the future.

## Functionality
The API has the following controllers with the following functions:
#### User Controller
- Create a user
- Get a user
- Update a user's information
- Delete a user
- Check if login credentials match a user
#### Account Controller
- Create an account
- Get an account
- Update an account's balance
- Delete an account
- Withdraw from an account
- Deposit into an account
- Get the account's of a user
#### Transaction Controller
- Create a transaction
- Get the transaction of an account
- Get all transactions
