<h1 align="center" id="title">RecipeHub-Backend</h1>

<p id="description">The project is the backend side of our El Proyecte Grande exercise - RecipeHub</p>

  
## 📋 Table of Content

* [Features](#features)
* [Installation Steps](#installation-steps)
* [Contribution Guidelines](#contribution-guidelines)

  
## Features

Here're some of the project's best features:

*   **User Registration and Authentication:** Allow users to create accounts log in and manage their profiles.
*   **Recipe Management:** Enable users to create edit and delete their recipes.
*   **Ingredient Database:** Maintain a database of ingredients allowing users to search and select from a wide variety.
*   **Search and Filter:** Implement search and filter options to help users find recipes quickly.
*   **API Integration:** Connect with external APIs to fetch additional ingredient data.

## Installation Steps:

<p>1. Clone the repository</p>

```
git clone https://github.com/yourusername/recipehub-backend.git
```

<p>2. Install Dependencies</p>


Use `dotnet restore` if necessary.


<p>3. Set Up the Database</p>


You'll need a PostgreSQL database for this project. If you haven't already set up PostgreSQL you can download it from here.  Create a new database and configure the connection in the appsettings.json file. The structure might look like this:
```json
{   
    "ConnectionStrings": {     
    "PGSQLDb": "Host=your_db_host; Database=your_db_name; Username=your_db_user; Password=your_db_password; TrustServerCertificate=True;"   }    
}
```

<p>4. Run the Application</p>


Use `dotnet run` to start the server


## Contribution Guidelines:

1\. **Fork the Repository:** Click the "Fork" button on the top of this repository.  
2\. **Create a Branch:** Create a new branch for your contribution.  
3\. **Make Changes:** Make your changes add new features or fix bugs.  
4\. **Create a Pull Request:** Create a pull request while describing your changes.