![](../../workflows/Assignment_Build_Test/badge.svg)
![](../../workflows/Deploy_Final_Assignment/badge.svg)

[**Link to running website**](https://cybotive-ewu-cscd379-2021-spring.azurewebsites.net)

[**Link to my branch on my fork**](https://github.com/Cybotive/EWU-CSCD379-2021-Spring/tree/Assignment9)

# Assignment 9 & 10

## Overview

In this assignment we are going to build "complete" the SecretSanta project. As discussed in class on Thursday, you have two weeks to complete the assignment which is worth "double the points". The final assignment is due by __midnight on Friday, June 11__. 

__Updated June 8, 2021:__ 
 - Place items in priority order
 - Allowed video in place of end to end testing
 - Moved end to end testing to extra credit
 - Extended assignment to Friday (as announced on Teams)

## Assignment

1. Configure all data to be stored in an SqLite Database using the Entity Framework. ✔❌
    - Hint: Add a Gift object to the SecretSanta.Data project
    - Hint: Update `Group` and `Assignment` to persist as a many-to-many relationship
    - All tables must have a primary key ✔❌
    - All tables mush have an unique/alternate key (other than the primary key) ✔❌

2. Provide UI functionality for a complete Secret Santa functionality. ✔❌
  This includes:
    - The ability for a user to add a list of gifts ✔❌
    - Displaying who a user's secret santa is for in each group. ✔❌
    - Viewing all gifts requested by your secret santa ✔❌

3. Ensure website is deployed by your GitHub Action and proivde URL to the deployed site in your pull request. ✔❌

4. Provide a walk through video (or do the extra credit) showing:
    - Creates a Group ✔❌
    - Creates a 3 users ✔❌
    - Assigns a minimum of two gifts to each user ✔❌
    - Assigns the secret santas ✔❌
    - Allows a user to see their secret santa and the associated gifts wanted. ✔❌

5. Remove the MockData class and replace it with sample data that gets deployed into the database following a migration when the command "DeploySampleData" is specified on the command line. ✔❌
    - Hint: See https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding and/or search on "Entity Framework Core Seed Data"

6. Add some minimal rudimentary logging using `Microsoft.Extensions.Logging`. 
    - At a minimum, the logging should be in startup.cs. ✔❌
    - Configure the Entity Framework to log to a file (located in the SecretSanta.Data assembly directory) with the category name "Database". ✔❌
    - The name of the log file should default to `db.log`. ✔❌
    - The name of the log file should be configurable using `Microsoft.Extensions.Coniguration`.
    - Hint: Use the Serilog framework with `Microsoft.Extensions.Logging` to log to a file.) ✔❌

7. Allow for the connection string (db path) for the SqLite database to be provided using `Microsoft.Extensions.Coniguration`:
    - Default to main.db in the same directory as the SecretSanta.Data assembly. ✔❌
    - Allow for an environment variable to override the default connection string ✔❌
    - Allow a connection string passed on the command line to take precidence over the environment variable. ✔❌

9. GitHub Action CI/CD Build ✔❌
    - Ensure all tests (unit test, end to end tests, etc.) are running in your GitHub Action build and include a URL to the build in your executing GitHub action in your pull request.

## Extra Credit

- Provide end to end testing showing: ✔❌
  - Creates a Group
  - Creates a 3 users
  - Assigns a minimum of two gifts to each user
  - Assigns the secret santas
  - Allows a user to see their secret santa and the associated gifts wanted.
