# Aabaas BD

### Hotel Booking and Recommendation System

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4)
![C Sharp](https://img.shields.io/badge/Language-C%23-239120?logo=csharp)
![SQL Server](https://img.shields.io/badge/Database-SQL_Server-CC2927)

## About the Project

**Aabaas BD** is a web-based hotel booking system developed using ASP.NET Core MVC. The platform allows users to search for hotels across Bangladesh, view hotel details, receive personalized recommendations, complete bookings, and manage their booking history.

An administrative panel is included for managing hotels, packages, and customer bookings.

## Project Objectives

* Simplify the hotel-search and booking process
* Provide personalized hotel recommendations
* Maintain hotel, package, user, and booking information
* Apply a structured three-layer software architecture
* Implement practical ASP.NET Core MVC concepts

## Main Features

### User Features

* Registration, login, and logout
* Browse available hotels
* Search hotels by city
* Filter by price, room type, and star rating
* View hotel and package details
* Receive personalized recommendations
* Book hotels with check-in and check-out dates
* Automatically calculate booking costs
* View booking history and details
* Cancel eligible future bookings
* Receive booking and cancellation notifications

### Admin Features

* Access the administrative dashboard
* Add, update, and delete hotels
* Add, update, and delete hot packages
* Search and filter hotel records
* View all customer bookings
* Monitor booking information and status

## Recommendation System

The recommendation system analyses a user's previous bookings using:

* Preferred city
* Preferred room type
* Preferred hotel star rating

Matching hotels and packages are displayed under **Recommended for You**. New users are shown currently active popular deals.

## Technologies

| Category         | Technology                        |
| ---------------- | --------------------------------- |
| Language         | C#                                |
| Framework        | ASP.NET Core MVC, .NET 10         |
| Frontend         | Razor Views, HTML, CSS, Bootstrap |
| Database         | Microsoft SQL Server              |
| ORM              | Entity Framework Core             |
| Mapping          | AutoMapper                        |
| Email            | MailKit and SMTP                  |
| State Management | ASP.NET Core Session              |
| Version Control  | Git and GitHub                    |

## Architecture

The project follows a three-layer architecture:

| Layer                | Project | Responsibility                                                            |
| -------------------- | ------- | ------------------------------------------------------------------------- |
| Presentation Layer   | `App`   | Controllers, Razor views, sessions, and user interface                    |
| Business Logic Layer | `BLL`   | Services, DTOs, validations, recommendations, and business rules          |
| Data Access Layer    | `DAL`   | Entity Framework context, entities, repositories, and database operations |

## Database Entities

* `User`
* `Hotel`
* `Booking`
* `HotPackage`

## Academic Information

| Information    | Details                                                                          |
| -------------- | -------------------------------------------------------------------------------- |
| Project Type   | ASP.NET Core Web MVC Academic Project                                            |
| Course Teacher | [Tanvir Ahmed](https://www.aiub.edu/faculty-list/faculty-profile?q=tanvir.ahmed) |
| Designation    | Assistant Professor                                                              |
| Department     | Department of Computer Science                                                   |
| Faculty        | Faculty of Science and Technology                                                |
| Institution    | American International University-Bangladesh                                     |
