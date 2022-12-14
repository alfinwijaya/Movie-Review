# Movie-Review
A Simple Web API using .Net Core and MongoDB

## Background
Making an CRUD web based API for the movie review section and store it to the NoSQL database (in this case I'm using MongoDB) through a MongoDB Driver on C#.
A movie can has many reviews, so the idea is to store the reviews on the same collection(table) as the movie collection. To achieve this, we can store the 
reviews as array of embedded document and every embedded document has an UserId field that refers to the user master data collection.

## Requirement
[1] MongoDB Server hosted locally => "https://www.mongodb.com/try/download/community"

[2] .NET Core

## How to Use

[1] Create a new database and collection(table) on your localhost MongoDB server. There are 2 attached .json files as a dummy data.
    This is the example of how to add the data to the User collection from the MongoDB Shell and the result.
    ![image](https://user-images.githubusercontent.com/77500112/207492067-9ac088ee-4481-4c51-9ccc-30cc69b795c3.png)
    ![image](https://user-images.githubusercontent.com/77500112/207495709-e5985381-a941-4449-97af-ed14d69d6260.png)

[2] Make sure your database and collection name match the one on the appsetting file.

[3] Launch the swagger on the browser "https://localhost:7084/swagger/index.html". You can change the port as you want on the appsetting file.
    ![image](https://user-images.githubusercontent.com/77500112/207492447-18abe856-1870-44a7-b2e3-dcc81970c55f.png)

[4] That's it, You are good to go! Any feedback is welcome.
