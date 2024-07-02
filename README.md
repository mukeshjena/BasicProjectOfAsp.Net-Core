# PracticeForRevision

![Project Logo](path_to_logo_image)

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Architecture](#architecture)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Logging Service](#logging-service)
- [Export Service](#export-service)
- [Contributing](#contributing)
- [Contact](#contact)

## Introduction

PracticeForRevision is a web application built with ASP.NET Core that allows users to manage products and process payments. This project showcases various skills, including CRUD operations, PayPal payment integration, data export to Excel, and logging actions and errors for tracking purposes.

## Features

- **Product Management:** Create, read, update, and delete products.
- **Pagination:** View products with pagination for better usability.
- **PayPal Integration:** Process payments securely via PayPal.
- **Excel Export:** Export product and payment data to an Excel file with a timestamp.
- **Logging:** Automatically log all actions and errors to a file and database.

## Architecture

The application follows a layered architecture:

1. **Controllers:** Handle HTTP requests and responses.
2. **Services:** Contain business logic and interact with repositories.
3. **Repositories:** Manage data access to the database.
4. **Models:** Define data structures.
5. **Views:** Render the user interface.

## Technologies Used

- **ASP.NET Core**: Framework for building web applications.
- **Entity Framework Core**: ORM for data access.
- **PayPal API**: Integration for payment processing.
- **EPPlus**: Library for Excel export.
- **Bootstrap**: Front-end framework for responsive design.
- **SQL Server**: Database management system.

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. Clone the repository:

   ```sh
   git clone https://github.com/yourusername/PracticeForRevision.git
   cd PracticeForRevision
   ```

2. Configure the database connection in `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
   }
   ```

3. Update the PayPal settings in `appsettings.json`:

   ```json
   "PayPal": {
     "ClientId": "your_paypal_client_id",
     "ClientSecret": "your_paypal_client_secret"
   }
   ```

4. Run the following commands to set up the database:

   ```sh
   dotnet ef database update
   ```

5. Build and run the application:

   ```sh
   dotnet run
   ```

## Usage

### Product Management

- **View Products:** Navigate to the `/Products` page to view all products with pagination.
- **Create Product:** Click the "Create New" button to add a new product.
- **Edit Product:** Click the "Edit" button on a product to update its details.
- **Delete Product:** Click the "Delete" button to remove a product.

### Payment Processing

- **Process Payment:** Click the "Buy" button on a product to initiate the PayPal payment process.

### Export Data

- **Export to Excel:** Click the "Export" button to download product and payment data as an Excel file.

## Logging Service

The logging service captures all actions and errors with timestamps. The logs are saved to both a file and the database for comprehensive tracking.

## Export Service

The export service generates an Excel file containing data from the products and payment details tables. Each export is saved with a timestamp in the filename.

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes.

## Contact

For any questions or feedback, please contact:

- **Mukesh Jena**
- [LinkedIn](https://www.linkedin.com/in/mukeshjena)
- [Email](mailto:muk3shjena@gmail.com)

--
