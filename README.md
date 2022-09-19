# tandem-be-challenge
- This repository is a submission for the BE challenge provided by Tandem.
- It is an ASP.NET Core Web APIs project.
- It uses Cosmos DB to store the data.
---
## Prerequisites to Run Solution
- [Visual Studio](https://visualstudio.microsoft.com/vs/community/)
- A Cosmos DB server running on your local machine or [Azure](https://azure.microsoft.com/en-us/products/cosmos-db/).
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) running on your local machine if you want to run on Docker.
 ---
## How to Run Solution?
- Clone this repository.
- Open the cloned directory in Visual Studio.
- Edit the `appsettings.json` file.  Replace the properties `CosmosDb` section values with your Cosmos DB configurations.
- Run the solution with the Run button provided by Visual Studio

---
## How to Run on Docker?
- Go to the `TandemBEProject` directory where the Dockerfile is located
- Create a Docker Image with the following command
```
docker build -t tandem_be_cosmos_image
```
- Run the Docker Image on a container with the following command
```
docker run -p 8080:80 --name tandem_be_cosmos_container tandem_be_cosmos_image
```
- Once the solution is running, you can access it with the `http://localhost:8080/` endpoint.

---
## Provided APIs

#### Create a user
```
curl --location --request POST 'https://localhost:7054/api/users' \
--header 'Content-Type: application/json' \
--data-raw '{
  "firstName": "FirstName",
  "middleName": "MiddleName",
  "lastName": "LastName",
  "phoneNumber": "+9876543210",
  "emailAddress": "the_email_address@domain.com"
}'
```

#### Get a user by email
```
curl --location --request GET 'https://localhost:7054/api/users?email=the_email_address@domain.com'
```
#### Update a user
```
curl --location --request PUT 'https://localhost:7054/api/users' \
--header 'Content-Type: application/json' \
--data-raw '{
  "firstName": "FirstName",
  "middleName": "UpdatedMiddleName",
  "lastName": "LastName",
  "phoneNumber": "+9876543210",
  "emailAddress": "the_email_address@domain.com"
}'
```

#### Swagger Documentation
```
/swagger/index.html
```
#### Health Check
```
/health
```