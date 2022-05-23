# UserManagement


<h1 align="left"><u>Run The Project</u></h1>

You will need the following tools:

 * VS Code
 * .Net Core 6 or later
 * Docker Desktop
 
<p align="left">
 1 -  Clone the repository
</p>
<p align="left">
 2- Docker/Docker Desktop should be running.
</p>
<p align="left">
 3- At the root directory which include docker-compose.yml files, run below command:
</p>

```
docker-compose build

docker-compose up -d
```
<p align="left">
 4- You can access to services with below links :
</p>

```
http://localhost:5225 -> Management API
http://localhost:5224 -> UserPortal API
```

Project Architecture

![alt text](https://github.com/0zkan/UserManagement/blob/main/image/architect.png)


Next Features:
 * API Gateway
 * Identity API
