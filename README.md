https://yaman-store-htbme5b5frefghag.indonesiacentral-01.azurewebsites.net/

Overview

This project represents the front-end of an e-commerce platform, built using Blazor Server (.NET 8).
The client application communicates directly with the ECommerce Backend API to display products, manage the shopping cart, handle user authentication, and process orders.
The architecture is designed to be clean, scalable, and easy to maintain.
________________________________________
Architecture Summary

1- Pages Layer

Contains all UI pages such as Products, Product Details, Cart, Login, and Orders.
These pages interact with the Services layer to retrieve and send data to the backend API.

2- Services Layer

This layer handles all communication with the API using HttpClient.
It includes services such as ProductService, CartService, AuthService, and OrderService.
Responsibilities include:
•	Sending requests to the API
•	Managing JWT tokens
•	Handling CRUD operations for the front-end

3- Data Layer

Contains the data models used to represent API responses (Product, Category, Order, CartItem, etc.).
These models are used to bind data inside Blazor components.
4- Authentication Layer
Manages user authentication, token storage, and session handling.
Implements JWT-based authentication and role-based access.
5- Shared Components
Contains reusable UI components such as navigation bars, layouts, headers, and shared UI elements.
________________________________________
Authentication and Authorization

The project supports:

•	JWT Authentication
•	Access Token & Refresh Token
•	Role-Based Authorization (Admin, User)
________________________________________
Technologies Used

•	Blazor Server (.NET 8)
•	C#
•	ASP.NET Core
•	RESTful API integration with HttpClient
•	JWT Authentication
•	Dependency Injection
•	State Management (Server-Side)
•	Bootstrap / CSS
•	LINQ and asynchronous operations (async/await)
