# BevCapital - Logon Microservice

Microservice to be deployed to Lambda serveless + DynamoDB.

There is another version of this microsevice using ECS Cluster + RDS MySql.
Use:
/logon-api

### All parts of the this project

- https://github.com/esilean/bevcapital-mslogon
- https://github.com/esilean/bevcapital-msstocks
- https://github.com/esilean/bevcapital-jobstockprice

## AWS - All services deployed using AWS Cloudformation

- AWS ECS - Cluster EC2
- AWS Application Load Balancer
- AWS RDS - MySql
- AWS ElastiCache - Redis
- AWS CodePipeline
- AWS CodeBuild
- AWS CodeDeploy - Deploy CloudFormation
- AWS ECR
- AWS X-Ray
- AWS CloudWatch - Logs
- AWS SQS
- AWS SNS
- AWS Lambda
- AWS DynamoDB

## Techs

- ASP.NET Core 3.1
- Entity Framework Core 3.1
- .NET Core Native DI
- AutoMapper
- Circuit Breaker and Retry Patterns
- MediatR
- Swagger UI
- Serilog

## Architecture

- Event Sourcing
- Clean Architecture
- SOLID and Clean Code
- DDD
- CQRS
- Unit of Work
- Repository

## Devs

- Leandro Bevilaqua - https://github.com/esilean
