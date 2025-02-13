# Home Assignment: Software Engineer Intern - ATM System

# Dor Sror

### Overview

This project implements the server-side of a simple ATM system using C# and ASP<nolink>.NET Core, and is deployed on AWS. The system provides three key functionalities:
- **Get Balance**: Retrieve the current balance of an account.
- **Withdraw Money**: Withdraw a specified amount from an account.
- **Deposit Money**: Deposit a specified amount into an account.

The application is designed as a RESTful API and stores account data in-memory. The project is deployed on AWS, allowing remote access to the API.

### Links
 - **GitHub Repository:** https://github.com/DorSror/ATM-System
 - **AWS Deployment URL:** http://atm-system-env.eba-nmpteb2z.us-east-1.elasticbeanstalk.com
 - **AWS Deployment URL - SwaggerUI:** http://atm-system-env.eba-nmpteb2z.us-east-1.elasticbeanstalk.com/swagger/index.html
 - **API Documentation:** Included in the repository.

### Features

- RESTful API for interacting with user accounts.
- In-memory storage using SortedDictionary (equivalent of HashMap in C#).
- Input validation to prevent incorrect transactions.
- Error handling for invalid accounts or insufficient funds.
- Cloud deployment for remote access.

## Implementation Details

- **C# and ASP<nolink>.NET Core** were chosen for their scalability and performance, and for past experience to create an ASP<nolink>.NET Core Web API.
- **Account** class is used to represent an account that can be used by the ATM system. The class holds a static **HashSet** that is used to ensure that each newly created account has its own unique identifier. The class implements the **`IDisposable`** interface to note that an instance can be disposed of manually.
- **APIConstants** class is used to store constant strings that are used in the API endpoints.
- **SortedDictionary** was used for in-memory data storage to map a unique identifier (account number) to its corresponding account.
- **Model validation** ensures that only valid transactions are processed.
- **Exception handling** prevents system crashes due to invalid inputs.

## Deployment on AWS

The server-side is hosted on AWS using Elastic Beanstalk.

The choice for which cloud platform should I use for the home assignment was the main challenge of the assignment due to my unfamiliarity with cloud platforms. Because of this, I have tried a few cloud platforms, namely GCP (Google Cloud Platform), Heroku, Azure and AWS.
I chose to use AWS mainly for the opportunity to learn the basics of a commonly used cloud platform, but also for its wide support and documentation.

The deployment steps were as follows:

1. **Set up AWS Elastic Beanstalk**
2. **Configure environment variables and set up IAM roles and permissions**
3. **Deploy the application from Visual Studio Code (Publish to AWS)**

### Challenges Faced

Choosing the right cloud platform was a major challenge due to my initial lack of familiarity with cloud hosting. I explored Heroku, Google Cloud, and AWS before deciding on AWS.
Despite following documentation and community forums, I encountered persistent issues with GCP that I could not resolve. These included difficulties in configuring the necessary permissions, setting up the correct service roles, and integrating the deployment pipeline from GitHub. After multiple attempts, I found AWS to be more straightforward, with clearer documentation and tools that aligned better with the project needs. This experience allowed me to learn the basics of AWS, including Elastic Beanstalk.

## API Endpoints

#### 1. **Get balance**
**Endpoint:** `GET /account/{account_number}/balance` 
**Description:** Retrieves the current balance of the specified account.
**Possible Responses:**
- `200 OK` - Returns the account balance.
- `400 Bad Request` - Occurs when `account_number` is not an unsigned integer.
- `404 Not Found` - Occurs when the given `account_number` has no corresponding account.

#### 2. **Withdraw money**

**Endpoint:** `POST /accounts/{account_number}/withdraw`
**Body:** Contains a single value that should be a decimal number, e.g. `350.12`.
**Description:** Deducts the specified amount from the account balance. An assumption has been made - withdraw will fail if the transaction will cause the balance to be overdrawn (i.e. negative).
**Possible Responses:**
- `200 OK` - Operation has succeeded and funds were withdrawn.
- `400 Bad Request` - Occurs when `account_number` is not an unsigned integer or when the specified amount is not a non-negative decimal number.
- `404 Not Found` - Occurs when the given `account_number` has no corresponding account.

#### 3. **Deposit money**

**Endpoint:** `POST /accounts/{account_number}/deposit`
**Body:** Contains a single value that should be a decimal number, e.g. `350.12`.
**Description:** Adds the specified amount to the account balance.
**Possible Responses:**
- `200 OK` - Operation has succeeded and funds were deposited.
- `400 Bad Request` - Occurs when `account_number` is not an unsigned integer or when the specified amount is not a non-negative decimal number.
- `404 Not Found` - Occurs when the given `account_number` has no corresponding account.

### Extra Endpoints

For the purpose of testing and data modification, I chose to add a few extra endpoints.

#### 4. **Create New Account**
**Endpoint:** `POST /accounts/newAccount`
**Body:** Contains a single value that should be a decimal number, e.g. `350.12`.
**Description:** Create a new account with a specified balance. Note that we allow negative balances upon creation for testing purposes, and in the case where the balance wasn't specified the default balance upon creation is zero.
**Possible Responses:**
- `200 OK` - Operation has succeeded and a new account has been made.
- `400 Bad Request` - Occurs when the specified amount is not a decimal number.

#### 5. **Get All Accounts**
**Endpoint:** `GET /accounts/allAccounts`
**Description:** Returns the account number and balance of all accounts currently stored.
**Possible Responses:**
- `200 OK` - Returns a json containing each account's unique identifier (account number) and its corresponding balance.

#### 6. **Delete Account**
**Endpoint:** `DELETE /accounts/{account_number}/delete`
**Description:** Deletes an account with the specified account number.
**Possible Responses:**
- `200 OK` - Operation has succeeded and the corresponding account was deleted.
- `400 Bad Request` - Occurs when `account_number` is not an unsigned integer.
- `404 Not Found` - Occurs when the given `account_number` has no corresponding account.

#### 7. **Get All Accounts**
**Endpoint:** `DELETE /accounts/dropAllAccounts`
**Description:** Deletes all accounts from the database.
**Possible Responses:**
- `200 OK` - All accounts have been deleted.

## How to Run Locally
1. Clone the repository:
```sh
git clone https://github.com/DorSror/ATM-System
```
2. Navigate to the project directory:
```sh
cd ATM-System
```
3. Run the application:
```sh
dotnet run
```

You can also open the project with **Visual Studio** or **Visual Studio Code** and run it from there.

## Executing API Calls

You can use many tools to execute API calls, I will explain two ways - by using the built-in **Swagger UI** and by using **cURL**.

#### Executing API Calls With Swagger

The **Swagger UI** is a very easy tool to work with, as you get a simple web page with all the API endpoints, and can input values and execute the API calls very easily.
As mentioned above, here is the URL: http://atm-system-env.eba-nmpteb2z.us-east-1.elasticbeanstalk.com/swagger/index.html

#### Executing API Calls With cURL

**cURL** can also be easily used to execute the API calls. Here is a list of cURL commands that can be ran from CLIs, in order corresponding to their listing above:
```sh
curl -X GET http://atm-system-env.eba-nmpteb2z.us-east-1.elasticbeanstalk.com/accounts/{account_number}/balance

curl -X POST http://atm-system-env.eba-nmpteb2z.us-east-1.elasticbeanstalk.com/accounts/{account_number}/withdraw -H "Content-Type: application/json" -d "{balance}"

curl -X POST http://atm-system-env.eba-nmpteb2z.us-east-1.elasticbeanstalk.com/accounts/{account_number}/deposit -H "Content-Type: application/json" -d "{balance}"

curl -X POST http://atm-system-env.eba-nmpteb2z.us-east-1.elasticbeanstalk.com/accounts/newAccount -H "Content-Type: application/json" -d "{balance}"

curl -X GET http://atm-system-env.eba-nmpteb2z.us-east-1.elasticbeanstalk.com/accounts/allAccounts

curl -X DELETE http://atm-system-env.eba-nmpteb2z.us-east-1.elasticbeanstalk.com/accounts/{account_number}/delete

curl -X DELETE http://atm-system-env.eba-nmpteb2z.us-east-1.elasticbeanstalk.com/accounts/dropAllAccounts
```

Note that in each line, `{account_number}` is used to denote the unique identifier (account number) of an account, and `{balance}` is used to denote the desired balance to be withdrawn/deposited/set. Change both accordingly.

### Contact
For any issues, feel free to contact me by mail.
