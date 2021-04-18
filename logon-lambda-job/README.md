# BevCapital - Logon Background Service

Job to be deployed to a Lambda triggered by Cloud Watch Events for pushing messages to an SNS Topic.

There is another version of this job using background services in a ECS Cluster + RDS MySql.
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
