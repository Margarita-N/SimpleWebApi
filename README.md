# SimpleWebApi

To test an SQL Server should be available, since the data is stored in a SQL Server database.
In the appsettings file provide a connection string with the name of the database(it doesn't need to be created prior)
For ease of use the signing key of the JWT token is provided in the appsettings
To create database and the tables necessary open package manager console and apply migrations by running 'update-database' command or simply run the application

To test authentication an admin user is seeded when the database is created with the credentials:
Email: admin@root
Password: 12345678
To generate a valid token make a GET request in /token with the credentials provided

For the transactions endpoints a dummy entity has been created. This entity contains an Account field, a Receiving account field and an Amount field.
All the transaction endpoints are protected with authentication, and the Create endpoint is role filtered so that only users that have the role 'Admin' are allowed to make changes.
Before making a request in said endpoints set the Authentication header to include the token generated from the /token endpoint

To create a transaction make a POST request in /transaction with a body that contains account, receivingAccount and amount fields. These fields are validated.
To retrieve the created transaction make a GET request in /transaction/{id} where the id is the value returned by the previous create response
To retrieve all the transactions created make a GET request in /transactions
